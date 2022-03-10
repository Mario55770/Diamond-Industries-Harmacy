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
        private static bool ideoStuff = ModsConfig.IdeologyActive;
        private static PreceptDef poisonDishonorable;
        private static PreceptDef poisonHonorable;
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
            harmony.Patch(mOriginal, null, new HarmonyMethod(mPostfix));
            if (ideoStuff)
            {

                poisonDishonorable = (PreceptDef)GenDefDatabase.GetDef(typeof(PreceptDef), "DIH_PoisonedWeaponDishonorable");
                poisonHonorable=(PreceptDef)GenDefDatabase.GetDef(typeof(PreceptDef), "DIH_PoisonedWeaponHonorable");
            }
        }
        
            public static void poisonRaiderPostFix(IncidentParms parms)
        {
            Faction faction = parms.faction;
            FactionIdeosTracker ideo = faction.ideos;
            //assuming all pawns of a raid share an ideology, this is a safe enough bet. Elsewise...assume peer pressure or something.
            //This will end if the faction ideology does not allow poisons. It will also check if the target is a map, and set that, or end it if it is not
            if(!(parms.target is Map map)|| ideoStuff && ideo.GetPrecept(poisonDishonorable) != null)
            {
                return;
            }

            List<Pawn> pawns = map.mapPawns.FreeHumanlikesSpawnedOfFaction(faction);
            bool raiderWillReroll = faction.def.techLevel.IsNeolithicOrWorse() || (ideoStuff && ideo.GetPrecept(poisonHonorable) != null);
                for (int i = 0; i < pawns.Count; ++i)
                {
                    Pawn pawn = pawns[i];
                    CompPoisonable compPoisonable = pawn.equipment.Primary.GetComp<CompPoisonable>();
                    if (compPoisonable != null)
                    {
                        compPoisonable.poisonRaider(raiderWillReroll);
                    }
                }
            
        }
    }
}

