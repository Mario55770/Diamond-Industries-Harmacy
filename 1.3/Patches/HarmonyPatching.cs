using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using HarmonyLib;
namespace DI_Harmacy
{
    [StaticConstructorOnStartup]
    class HarmonyPatching
    {
        static HarmonyPatching()
        {
           // Log.Message("TEST LAUNCH ERROR");
            DoPatching();
        }
        public static void DoPatching()
        {
            var harmony = new Harmony("DI_Harmacy.PoisonMeleeVerb.patch");

            var mOriginal = AccessTools.Method(typeof(Verb_MeleeAttackDamage), "DamageInfosToApply"); // if possible use nameof() here
                                                                                                      // var mPrefix = SymbolExtensions.GetMethodInfo(() => MyPrefix());
            IEnumerable<DamageInfo> test = null;
            var mPostfix = SymbolExtensions.GetMethodInfo(() => meleeWeaponPoisonPostFix(test));
            harmony.Patch(mOriginal, null, new HarmonyMethod(mPostfix));//new HarmonyMethod(mPrefix), new HarmonyMethod(mPostfix));
            
        }
       // [HarmonyDebug]
        public static IEnumerable<DamageInfo> meleeWeaponPoisonPostFix(IEnumerable<DamageInfo> __result)
        {
           
                CompPoisonable compPoisonable = null;
            //(comp.ParentHolder as Pawn)
            //Log.Message("Var V");
                var v = __result.First();
            //Log.Message("PAwnInstagate");
                Pawn pawn = (v.Instigator as Pawn);
            //Log.Message("CheckNull");
                if (v.Weapon == null || pawn == null ||!pawn.RaceProps.Humanlike)
                {
                   // Log.Message("PawnNull");
                    return __result;
                }
            //Log.Message("Weaponnullcheck");
            
                ThingWithComps weapon = pawn.equipment.Primary;
                if (weapon == null || weapon.def == null)
                {
                    //Log.Message("WeaponNull");
                    return __result;
                }
            //Log.Message("WeaponCompare");
                if (weapon.def == v.Weapon)
                {
                    compPoisonable = weapon.TryGetComp<CompPoisonable>();
                //Log.Message("CompCheck");
                    if (compPoisonable != null && compPoisonable.CanBeUsed)
                    {
                     
                    
                  return compPoisonable.applyPoison(__result);
                   
                }
                    else
                    {
                  //      Log.Message("CompCheckedNull");
                        return __result;
                    }
                }

                //if (compPoisonable.Props.applyToStruckPart)
                //{
                //foreach (DamageInfo damageInfo in __result)
                //{
                
                return __result;
                
            
            
          }
    }
}
