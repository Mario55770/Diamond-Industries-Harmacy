// RimWorld.ReloadableUtility
using RimWorld;
using System.Collections.Generic;
using Verse;
namespace DI_Harmacy
{
    public static class PoisonableUtility
    {
        public static CompPoisonable FindSomeReloadableComponent(Pawn pawn, bool allowForcedReload)
        {
            var pawnsWeapon = pawn.equipment.Primary;

            CompPoisonable compPoisonable = pawnsWeapon.TryGetComp<CompPoisonable>();

            if (compPoisonable == null)
            {
                return null;
            }
            compPoisonable.updatePoisons(pawn);

            if (compPoisonable.NeedsReload(allowForcedReload))
            {
                return compPoisonable;
            }

            return null;
        }

        public static List<Thing> FindEnoughAmmo(Pawn pawn, IntVec3 rootCell, CompPoisonable comp, bool forceReload)
        {
            if (comp == null)
            {
                return null;
            }
            IntRange desiredQuantity = new IntRange(comp.MinAmmoNeeded(forceReload), comp.MaxAmmoNeeded(forceReload));

            return RefuelWorkGiverUtility.FindEnoughReservableThings(pawn, rootCell, desiredQuantity, (Thing t) => t.def == comp.AmmoDef);

        }

        public static Pawn WearerOf(CompPoisonable comp)
        {

            return ((comp.ParentHolder as Pawn_EquipmentTracker)?.pawn);
        }
    }
}