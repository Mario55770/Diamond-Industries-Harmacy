using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
namespace DI_Harmacy
{
    [StaticConstructorOnStartup]
    class HarmonyPatching
    {
        static HarmonyPatching()
        {
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
        public static IEnumerable<DamageInfo> meleeWeaponPoisonPostFix(IEnumerable<DamageInfo> __result)
        {
            //Make a variable for the result, and gets the pawn.
            var v = __result.First();
            Pawn pawn = (v.Instigator as Pawn);
            //if instator weapon is null, pawn null, or pawn isn't human end
            if (pawn == null || !pawn.RaceProps.Humanlike||v.Weapon == null)
            {
                return __result;
            }
            CompPawnPoisonTracker poisonTracker=pawn.GetComp<CompPawnPoisonTracker>();
            if (poisonTracker==null||!poisonTracker.applyPoisonActive)
            {
                return __result;
            }
            ThingWithComps weapon = pawn.equipment.Primary;
            //if the weapon def is null, or does not match the used weapon, end
            if (weapon.def == null || !(weapon.def == v.Weapon))
            {
                return __result;
            }
            //get the comp
            CompPoisonable compPoisonable = weapon.TryGetComp<CompPoisonable>();
            //if the comp exists, and can be used, apply poison
            if (compPoisonable != null && compPoisonable.CanBeUsed && compPoisonable.hediffToApply != null)
            {
                return compPoisonable.applyPoison(__result);

            }
            //Ends anything that reached this point.
            return __result;
        }
    }
}
