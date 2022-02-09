using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using HarmonyLib;
namespace DI_Harmacy
{
    [StaticConstructorOnStartup]
    class RangedHarmonyPatch
    {
        static ExtraDamage poisonDamage;
        static RangedHarmonyPatch()
        {
            DoPatching();
        }

        public static void DoPatching()
        {
            var harmony = new Harmony("DI_Harmacy.PoisonProjectileLaunch.patch");
            var rOriginal = AccessTools.Method(typeof(Projectile), "Launch", new[] { typeof(Thing), typeof(Vector3), typeof(LocalTargetInfo), typeof(LocalTargetInfo), typeof(ProjectileHitFlags), typeof(bool), typeof(Thing), typeof(ThingDef) });
            Thing variables = null;
            Projectile p = null;
            var rPostFix = SymbolExtensions.GetMethodInfo(() => rangedPoisonPostFix(variables, variables, ref p));
            harmony.Patch(rOriginal, null, new HarmonyMethod(rPostFix));
            //Creates a damage def once so we don't keep making it.
            ExtraDamage poisonDamage = new ExtraDamage
            {
                chance = 1f,
                amount = 1f
            };
            poisonDamage.armorPenetration = 1f;
            DamageDef poisonDamageDef = DIH_DamageDefs.DIH_PoisonDamageBase;
            poisonDamageDef.isRanged = true;
            poisonDamage.def = poisonDamageDef;
        }

        public static void rangedPoisonPostFix(Thing launcher, Thing equipment, ref Projectile __instance)
        {
            //gets comp, checks if null, has no hediff, or cannot be used. Ends method if so.
            CompPoisonable compPoisonable = equipment.TryGetComp<CompPoisonable>();
            if (compPoisonable == null || compPoisonable.Props.hediffToApply == null || compPoisonable.CanBeUsed == false)
            {
                return;
            }
            //checks if list is null or empty. If so, make my own list for this.
            if (__instance.def.projectile.extraDamages.NullOrEmpty<ExtraDamage>())
            {
               //if (__instance.def.projectile.extraDamages == null)
                //{
                    
                    ExtraDamage[] extradamageArr = { poisonDamage };
                    List<ExtraDamage> extraDamages = new List<ExtraDamage>(extradamageArr);
                    __instance.def.projectile.extraDamages = extraDamages;
                //}
            }
            else
            {
                //if a extra damage with my damage def does not exist, add my own damage def. Slower than assuming it will either not exist or will have my damage def...but safer
                foreach (ExtraDamage exDamage in __instance.def.projectile.extraDamages)
                {
                    if (exDamage.def.Equals(poisonDamage.def))
                    {

                        return;
                    }
                }
                __instance.def.projectile.extraDamages.Add(poisonDamage);
            }
           

        }
    }
}
