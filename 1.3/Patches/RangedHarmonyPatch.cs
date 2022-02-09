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
            var rPostFix = SymbolExtensions.GetMethodInfo(() => rangedPoisonPostFix(variables, variables,ref p));
            harmony.Patch(rOriginal, null, new HarmonyMethod(rPostFix));
        }
        // 
        public static void rangedPoisonPostFix(Thing launcher, Thing equipment, ref Projectile __instance)
        {
           CompPoisonable compPoisonable= equipment.TryGetComp<CompPoisonable>();
            if (compPoisonable == null || compPoisonable.Props.hediffToApply == null ||compPoisonable.CanBeUsed==false)
                return;
                Log.Message("RangedPoison");
            ExtraDamage poisonDamage = new ExtraDamage
            {
                chance = 1f,
                amount = 1f
            };
            poisonDamage.armorPenetration = 1f;

            //DamageInfo dInfo=new DamageInfo(DIH_DamageDefs.DIH_PoisonDamageBase, poisonDamage.amount, instigator: launcher);
            
            DamageDef poisonDamageDef = DIH_DamageDefs.DIH_PoisonDamageBase;
            //poisonDamageDef
            poisonDamageDef.isRanged = true;
            
            poisonDamage.def = poisonDamageDef;
            // Log.Message(poisonDamage.AdjustedDamageAmount().ToString());
            
            if (__instance.def.projectile.extraDamages.NullOrEmpty<ExtraDamage>())
                {
                    Log.Message("List is null or empty");
                if(__instance.def.projectile.extraDamages==null)
                    { Log.Message("List seems null");
                    ExtraDamage[] extradamageArr = { poisonDamage };
                    List<ExtraDamage> extraDamages = new List<ExtraDamage>(extradamageArr);
                    __instance.def.projectile.extraDamages = extraDamages;
                }   
                }
            else {
                Log.Message(__instance.def.projectile.extraDamages.Count().ToString());
                __instance.def.projectile.extraDamages.Add(poisonDamage);
            }
            Log.Message(__instance.def.projectile.extraDamages.Count().ToString());
            Log.Message("Added to list");
           // Log.Message(__instance.def.projectile.extraDamages.Last<ExtraDamage>().toString());
            
        }
    }
}
