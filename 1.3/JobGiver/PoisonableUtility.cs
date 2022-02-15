// RimWorld.ReloadableUtility
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
namespace DI_Harmacy
{
    public static class PoisonableUtility
{
		//formerlyCompReloadable. Will definitely have to be modified as the comp will be on weapons not apparel but is being left for a test.
	public static CompPoisonable FindSomeReloadableComponent(Pawn pawn, bool allowForcedReload)
	{
			var pawnsWeapon =pawn.equipment.Primary;
			CompPoisonable compPoisonable = pawnsWeapon.TryGetComp<CompPoisonable>();
			if (compPoisonable != null&&compPoisonable.NeedsReload(allowForcedReload))
			{
				return compPoisonable;
			}
				/**if (pawn.apparel == null)
		{
			return null;
		}
		List<Apparel> wornApparel = pawn.apparel.WornApparel;
		for (int i = 0; i < wornApparel.Count; i++)
		{
			CompPoisonable compPoisonable = wornApparel[i].TryGetComp<CompPoisonable>();
			if (compPoisonable != null && compPoisonable.NeedsReload(allowForcedReload))
			{
				return compPoisonable;
			}
		}**/
		return null;
	}

	public static List<Thing> FindEnoughAmmo(Pawn pawn, IntVec3 rootCell, CompPoisonable comp, bool forceReload)
	{
		if (comp == null)
		{
			return null;
		}
		IntRange desiredQuantity = new IntRange(comp.MinAmmoNeeded(forceReload), comp.MaxAmmoNeeded(forceReload));
			comp.updatePoisons()
			ThingDef ammoToUse=pawn.GetComp<CompPawnPoisonTracker>().pawn_InventoryStockTracker.GetDesiredThingForGroup(DIH_PoisonStockGroups.DIH_PoisonStockGroup);
			if (ammoToUse != null)
				return RefuelWorkGiverUtility.FindEnoughReservableThings(pawn, rootCell, desiredQuantity, (Thing t) => t.def == ammoToUse);//comp.AmmoDef);
			else
				return null;
	}

	public static IEnumerable<Pair<CompPoisonable, Thing>> FindPotentiallyReloadableGear(Pawn pawn, List<Thing> potentialAmmo)
	{
			var pawnsWeapon = pawn.equipment.Primary;
			CompPoisonable comp = pawnsWeapon.TryGetComp<CompPoisonable>();
			if (comp == null)
			{
				yield break;
			}
			
			/**if (comp?.AmmoDef == null)
			{
				continue;
			}**/
			for (int j = 0; j < potentialAmmo.Count; j++)
			{
				Thing thing = potentialAmmo[j];
				if (thing.def == comp.Props.ammoDef)
				{
					yield return new Pair<CompPoisonable, Thing>(comp, thing);
				}
			}
			/**if (pawn.apparel == null)
			{
				yield break;
			}
			List<Apparel> worn = pawn.apparel.WornApparel;
			for (int i = 0; i < worn.Count; i++)
			{
				CompPoisonable comp = worn[i].TryGetComp<CompPoisonable>();
				if (comp?.AmmoDef == null)
				{
					continue;
				}
				for (int j = 0; j < potentialAmmo.Count; j++)
				{
					Thing thing = potentialAmmo[j];
					if (thing.def == comp.Props.ammoDef)
					{
						yield return new Pair<CompPoisonable, Thing>(comp, thing);
					}
				}
			
		}**/
	}

	public static Pawn WearerOf(CompPoisonable comp)
	{
			
			return ((comp.ParentHolder as Pawn_EquipmentTracker)?.pawn);
			//return (Thing.ParentHolder is Pawn pawn);
		//return (comp.ParentHolder as Pawn_ApparelTracker)?.pawn;
	}

	public static int TotalChargesFromQueuedJobs(Pawn pawn, ThingWithComps gear)
	{
		CompPoisonable compPoisonable = gear.TryGetComp<CompPoisonable>();
		int num = 0;
		if (compPoisonable != null && pawn != null)
		{
			foreach (Job item in pawn.jobs.AllJobs())
			{
				Verb verbToUse = item.verbToUse;
				if (verbToUse != null)// && compPoisonable == verbToUse.ReloadableCompSource)
				{
					num++;
				}
			}
			return num;
		}
		return num;
	}

	public static bool CanUseConsideringQueuedJobs(Pawn pawn, ThingWithComps gear, bool showMessage = true)
	{
		CompPoisonable compReloadable = gear.TryGetComp<CompPoisonable>();
		if (compReloadable == null)
		{
			return true;
		}
		string text = null;
		if (!Event.current.shift)
		{
			if (!compReloadable.CanBeUsed)
			{
				text = compReloadable.DisabledReason(compReloadable.MinAmmoNeeded(allowForcedReload: false), compReloadable.MaxAmmoNeeded(allowForcedReload: false));
			}
		}
		else if (TotalChargesFromQueuedJobs(pawn, gear) + 1 > compReloadable.RemainingCharges)
		{
			text = compReloadable.DisabledReason(compReloadable.MaxAmmoAmount(), compReloadable.MaxAmmoAmount());
		}
		if (text != null)
		{
			if (showMessage)
			{
				Messages.Message(text, pawn, MessageTypeDefOf.RejectInput, historical: false);
			}
			return false;
		}
		return true;
	}
}
}