using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using RimWorld;
namespace DI_Harmacy
{
    // Token: 0x020011CB RID: 4555
    public static class PoisonableUtility
    {
        // Token: 0x06006ECE RID: 28366 RVA: 0x00256AC0 File Offset: 0x00254CC0
        public static CompPoisonable FindSomeReloadableComponent(Pawn pawn, bool allowForcedReload)
        {
            /**	if (pawn.apparel == null)
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
				}
				return null;
			**/
            if (pawn.equipment == null)
            {
                return null;
            }
            List<ThingWithComps> heldEquipment = pawn.equipment.AllEquipmentListForReading;
            for (int i = 0; i < heldEquipment.Count; i++)
            {
                CompPoisonable compPoisonable = heldEquipment[i].TryGetComp<CompPoisonable>();
                if (compPoisonable != null && compPoisonable.NeedsReload(allowForcedReload))
                {
                    return compPoisonable;
                }
            }
            return null;
        }

        // Token: 0x06006ECF RID: 28367 RVA: 0x00256B10 File Offset: 0x00254D10
        public static List<Thing> FindEnoughAmmo(Pawn pawn, IntVec3 rootCell, CompPoisonable comp, bool forceReload)
        {
            if (comp == null)
            {
                return null;
            }
            IntRange desiredQuantity = new IntRange(comp.MinAmmoNeeded(forceReload), comp.MaxAmmoNeeded(forceReload));
            return RefuelWorkGiverUtility.FindEnoughReservableThings(pawn, rootCell, desiredQuantity, (Thing t) => t.def == comp.AmmoDef);
        }

        // Token: 0x06006ED0 RID: 28368 RVA: 0x00256B67 File Offset: 0x00254D67
        public static IEnumerable<Pair<CompPoisonable, Thing>> FindPotentiallyReloadableGear(Pawn pawn, List<Thing> potentialAmmo)
        {
            /**if (pawn.apparel == null)
			{
				yield break;
			}
			List<Apparel> worn = pawn.apparel.WornApparel;
			int num;
			for (int i = 0; i < worn.Count; i = num + 1)
			{
				CompPoisonable comp = worn[i].TryGetComp<CompPoisonable>();
				CompPoisonable compPoisonable = comp;
				if (((compPoisonable != null) ? compPoisonable.AmmoDef : null) != null)
				{
					for (int j = 0; j < potentialAmmo.Count; j = num + 1)
					{
						Thing thing = potentialAmmo[j];
						if (thing.def == comp.Props.ammoDef)
						{
							yield return new Pair<CompPoisonable, Thing>(comp, thing);
						}
						num = j;
					}
					comp = null;
				}
				num = i;
			}
			yield break;
			**/
            if (pawn.equipment == null)
            {
                yield break;
            }
            List<ThingWithComps> heldEquipment = pawn.equipment.AllEquipmentListForReading;
            int num;
            for (int i = 0; i < heldEquipment.Count; i = num + 1)
            {
                CompPoisonable comp = heldEquipment[i].TryGetComp<CompPoisonable>();


                //CompPoisonable comp = worn[i].TryGetComp<CompPoisonable>();
                CompPoisonable compPoisonable = comp;
                if (((compPoisonable != null) ? compPoisonable.AmmoDef : null) != null)
                {
                    for (int j = 0; j < potentialAmmo.Count; j = num + 1)
                    {
                        Thing thing = potentialAmmo[j];
                        if (thing.def == comp.Props.ammoDef)
                        {
                            yield return new Pair<CompPoisonable, Thing>(comp, thing);
                        }
                        num = j;
                    }
                    comp = null;
                }
                num = i;
            }
            yield break;
        }

        // Token: 0x06006ED1 RID: 28369 RVA: 0x00256B80 File Offset: 0x00254D80
        //formerly 
        public static Pawn WeaponHolder(CompPoisonable comp)
        {
            Pawn_EquipmentTracker pawn_EquipmentTracker = comp.ParentHolder as Pawn_EquipmentTracker;
            if (pawn_EquipmentTracker != null)
            {
                return pawn_EquipmentTracker.pawn;
            }
            return null;
        }

        // Token: 0x06006ED2 RID: 28370 RVA: 0x00256BA4 File Offset: 0x00254DA4
        public static int TotalChargesFromQueuedJobs(Pawn pawn, ThingWithComps gear)
        {
            CompPoisonable compPoisonable = gear.TryGetComp<CompPoisonable>();
            int num = 0;
            if (compPoisonable != null && pawn != null)
            {
                foreach (Job job in pawn.jobs.AllJobs())
                {
                    Verb verbToUse = job.verbToUse;
                    
                    if (verbToUse != null && compPoisonable == verbToUse.PoisonableCompSource())
                    {
                        num++;
                    }
                }
            }
            return num;
        }

        // Token: 0x06006ED3 RID: 28371 RVA: 0x00256C14 File Offset: 0x00254E14
        public static bool CanUseConsideringQueuedJobs(Pawn pawn, ThingWithComps gear, bool showMessage = true)
        {
            CompPoisonable compPoisonable = gear.TryGetComp<CompPoisonable>();
            if (compPoisonable == null)
            {
                return true;
            }
            string text = null;
            if (!Event.current.shift)
            {
                if (!compPoisonable.CanBeUsed)
                {
                    text = compPoisonable.DisabledReason(compPoisonable.MinAmmoNeeded(false), compPoisonable.MaxAmmoNeeded(false));
                }
            }
            else if (PoisonableUtility.TotalChargesFromQueuedJobs(pawn, gear) + 1 > compPoisonable.RemainingCharges)
            {
                text = compPoisonable.DisabledReason(compPoisonable.MaxAmmoAmount(), compPoisonable.MaxAmmoAmount());
            }
            if (text != null)
            {
                if (showMessage)
                {
                    Messages.Message(text, pawn, MessageTypeDefOf.RejectInput, false);
                }
                return false;
            }
            return true;
        }
    }
}
