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
        static DamageDef poisonDamageDef;
        static RangedHarmonyPatch()
        {
            // Log.Message("TEST LAUNCH ERROR");
            DoPatching();
        }

        public static void DoPatching()
        {
            //does the patching
            var harmony = new Harmony("DI_Harmacy.PoisonProjectileLaunch.patch");
            var rOriginal = AccessTools.Method(typeof(Projectile), "Launch", new[] { typeof(Thing), typeof(Vector3), typeof(LocalTargetInfo), typeof(LocalTargetInfo), typeof(ProjectileHitFlags), typeof(bool), typeof(Thing), typeof(ThingDef) });
            Thing variables = null;
            Projectile p = null;
            var rPostFix = SymbolExtensions.GetMethodInfo(() => rangedPoisonPostFix(variables, variables, ref p));
            harmony.Patch(rOriginal, null, new HarmonyMethod(rPostFix));
            //just caches it so it doesn't regen it every time.
            poisonDamage = new ExtraDamage
            {
                chance = 1f,
                amount = 1f
            };
            poisonDamage.armorPenetration = 1f;


            poisonDamageDef = DIH_DamageDefs.DIH_PoisonDamageBase;

            poisonDamageDef.isRanged = true;

            poisonDamage.def = poisonDamageDef;
        }

        public static void rangedPoisonPostFix(Thing launcher, Thing equipment, ref Projectile __instance)
        {
            //gets the comp
            CompPoisonable compPoisonable = equipment.TryGetComp<CompPoisonable>();
            //if null, cant be used, or hediff is null, end the method. Shouldn't really be impactful once the code is set up but hey its there now
            if (compPoisonable == null || compPoisonable.hediffToApply == null || compPoisonable.CanBeUsed == false)
                return;
            //if the list is empty or null DO NOT MAKE THIS A LOCAL VARIABLE.
            if (__instance.def.projectile.extraDamages.NullOrEmpty<ExtraDamage>())
            {
                    ExtraDamage[] extradamageArr = { poisonDamage };
                    List<ExtraDamage> extraDamages = new List<ExtraDamage>(extradamageArr);
                    __instance.def.projectile.extraDamages = extraDamages;
            }
            else
            {
                //searches through each Extra damage for one with the damage def I made earlier.
                foreach (ExtraDamage exDamage in __instance.def.projectile.extraDamages)
                {
                    //if match, end method
                    if (exDamage.def.Equals(poisonDamageDef))
                    {
                        return;
                    }
                }
                //elsewise, add the damage def.
                __instance.def.projectile.extraDamages.Add(poisonDamage);
            }


        }
    }
}