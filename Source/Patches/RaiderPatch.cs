using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
namespace DI_Harmacy
{
    [StaticConstructorOnStartup]
    class RaiderPatch
    {
        static RaiderPatch()
        {
            DoPatching();
        }
        public static void DoPatching()
        {
            var harmony = new Harmony("DI_Harmacy.PoisonRaider.patch");

            var mOriginal = AccessTools.Method(typeof(IncidentWorker_Raid), "TryExecuteWorker"); // if possible use nameof() here
                                                                                                 // var mPrefix = SymbolExtensions.GetMethodInfo(() => MyPrefix());
            IncidentParms nullIncidentParms = null;
            var mPostfix = SymbolExtensions.GetMethodInfo(() => poisonRaiderPostFix(nullIncidentParms));
            harmony.Patch(mOriginal, null, new HarmonyMethod(mPostfix));//new HarmonyMethod(mPrefix), new HarmonyMethod(mPostfix));

        }
        public static void poisonRaiderPostFix(IncidentParms parms)
        {
            if (parms.target is Map map)
            {
                Faction faction = parms.faction;
                List<Pawn> pawns = map.mapPawns.FreeHumanlikesOfFaction(faction);

                for (int i = 0; i < pawns.Count; ++i)
                {
                    Pawn pawn = pawns[i];
                    CompPoisonable compPoisonable = pawn.equipment.Primary.GetComp<CompPoisonable>();
                    if (compPoisonable != null)
                    {
                        compPoisonable.poisonRaider(faction);
                    }
                }
            }
        }
    }
}

