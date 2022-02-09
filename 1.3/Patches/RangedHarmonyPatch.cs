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
             poisonDamage = new ExtraDamage
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


            //get the vvariable one and work with it
            List<ExtraDamage> extraDamages1 = __instance.def.projectile.extraDamages;
            //if null or empty, create my own list, with my own poison damage
            if (extraDamages1.NullOrEmpty())
            {       
                    ExtraDamage[] extradamageArr = { poisonDamage };
                    List<ExtraDamage> extraDamages = new List<ExtraDamage>(extradamageArr);
                extraDamages1 = extraDamages;
                
            }
            else
            {
                //check each extra damage for my daamge def.
                foreach (ExtraDamage exDamage in extraDamages1)
                {
                    //if found, end
                    if (exDamage.def.Equals(poisonDamage.def))
                    {

                        return;
                    }
                }
                //if it hits this point, it means that I need to add my poison to the list
                extraDamages1.Add(poisonDamage);
            }
           

        }
    }
}
