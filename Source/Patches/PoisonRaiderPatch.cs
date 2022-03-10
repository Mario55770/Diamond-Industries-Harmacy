using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
namespace DI_Harmacy
{
    [StaticConstructorOnStartup]

    class PoisonRaiderPatch
    {
        static PoisonRaiderPatch()
        {
            DoPatching();
        }

        public static void DoPatching()
        {
            //does the patching
            var harmony = new Harmony("DI_Harmacy.PawnGroupMakerUtility.patch");
            var rOriginal = AccessTools.Method(typeof(PawnGroupMakerUtility), "GeneratePawns");
            IEnumerable<Pawn> iEnumParamater = null;
            var rPostFix = SymbolExtensions.GetMethodInfo(() => pawnGenerationPostFix(ref iEnumParamater));
            harmony.Patch(rOriginal, null, new HarmonyMethod(rPostFix));
            }

        public static void pawnGenerationPostFix(ref IEnumerable<Pawn> __result)
        {
            foreach(Pawn pawn in __result)
            {
                CompPoisonable compPoisonable= pawn.equipment.Primary.GetComp<CompPoisonable>();
                if (compPoisonable != null)
                {
                    compPoisonable.poisonRaider();
                    Log.Message("FOreach");
                }
            }
            Log.Message("THIS RAN");
        }

    }
}