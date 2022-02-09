using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using HarmonyLib;
namespace DI_Harmacy
{
    [StaticConstructorOnStartup]
    [HarmonyDebug]
    class RangedHarmonyPatch
    {
        static ExtraDamage poisonDamage;
        static RangedHarmonyPatch()
        {
            // Log.Message("TEST LAUNCH ERROR");
            DoPatching();
        }

        public static void DoPatching()
        {
            var harmony = new Harmony("DI_Harmacy.PoisonProjectileLaunch.patch");
            var rOriginal = AccessTools.Method(typeof(Projectile), "Launch", new[] { typeof(Thing), typeof(Vector3), typeof(LocalTargetInfo), typeof(LocalTargetInfo), typeof(ProjectileHitFlags), typeof(bool), typeof(Thing), typeof(ThingDef) });
            Thing variables = null;
            Def nullDef = null;
            Projectile p = null;
            var rPostFix = SymbolExtensions.GetMethodInfo(() => rangedPoisonPostFix(variables, variables, ref p));
            harmony.Patch(rOriginal, null, new HarmonyMethod(rPostFix));
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
            CompPoisonable compPoisonable = equipment.TryGetComp<CompPoisonable>();
            if (compPoisonable == null || compPoisonable.Props.hediffToApply == null || compPoisonable.CanBeUsed == false)
                return;
           
           

            if (__instance.def.projectile.extraDamages.NullOrEmpty<ExtraDamage>())
            {
                if (__instance.def.projectile.extraDamages == null)
                {
                    
                    ExtraDamage[] extradamageArr = { poisonDamage };
                    List<ExtraDamage> extraDamages = new List<ExtraDamage>(extradamageArr);
                    __instance.def.projectile.extraDamages = extraDamages;
                }
            }
            else
            {
              
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
