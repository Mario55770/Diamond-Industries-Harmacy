// RimWorld.PawnColumnWorker_Carry
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
namespace DI_Harmacy
{

	public class PawnColumnWorker_Poison : PawnColumnWorker
	{
		private const int TopAreaHeight = 65;

		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			CompPawnPoisonTracker c = pawn.GetComp<CompPawnPoisonTracker>();
			if(c==null)
            {
				return;
            }
			if (c.pawn_InventoryStockTracker==null)
			{
				c.pawn_InventoryStockTracker = new Pawn_InventoryStockTracker(pawn);
				
			}
			Pawn_InventoryStockTracker pawn_InventoryStockTracker = c.pawn_InventoryStockTracker;
			//formerly pawn.inventory
			if (pawn_InventoryStockTracker != null)
			{
				float num = rect.width - 4f;
				int num2 = Mathf.FloorToInt(num * 0.333333343f);
				float x = rect.x;
				//InventoryStockGroupDef group = InventoryStockGroupDefOf.Medicine;
				InventoryStockGroupDef group = DIH_PoisonStockGroups.DIH_PoisonStockGroup;
				/**InventoryStockGroupDef group = new InventoryStockGroupDef();
					group.thingDefs = PoisonItemFinder.poisonItemList;
				group.max = 3;*/
				
				Widgets.Dropdown(new Rect(x, rect.y + 2f, num2, rect.height - 4f), pawn, (Pawn p) => p.inventoryStock.GetDesiredThingForGroup(group), (Pawn p) => GenerateThingButtons(p, group), null, pawn_InventoryStockTracker.GetDesiredThingForGroup(group).uiIcon, null, null, null, paintable: true);
				Widgets.Dropdown(new Rect(x + (float)num2 + 4f, width: Mathf.FloorToInt(num * (2f / 3f)), y: rect.y + 2f, height: rect.height - 4f), pawn, (Pawn p) => p.inventoryStock.GetDesiredCountForGroup(group), (Pawn p) => GenerateCountButtons(p, group), pawn_InventoryStockTracker.GetDesiredCountForGroup(group).ToString(), null, null, null, null, paintable: true);
			}
		}

		private IEnumerable<Widgets.DropdownMenuElement<ThingDef>> GenerateThingButtons(Pawn pawn, InventoryStockGroupDef group)
		{
			CompPawnPoisonTracker c = pawn.GetComp<CompPawnPoisonTracker>();
			Pawn_InventoryStockTracker pawn_InventoryStockTracker = c.pawn_InventoryStockTracker;
			foreach (ThingDef thing in group.thingDefs)
			{
				yield return new Widgets.DropdownMenuElement<ThingDef>
				{
					option = new FloatMenuOption(thing.LabelCap, delegate
					{
						//this line updates the pawn to change its tracker
						//pawn.def.GetModExtension<PawnPoisonTrackerExtension>().poisonToUse = thing;
						pawn_InventoryStockTracker.SetThingForGroup(group, thing);
					}),
					payload = thing
				};
			}
		}

		private IEnumerable<Widgets.DropdownMenuElement<int>> GenerateCountButtons(Pawn pawn, InventoryStockGroupDef group)
		{
			CompPawnPoisonTracker c = pawn.GetComp<CompPawnPoisonTracker>();
			Pawn_InventoryStockTracker pawn_InventoryStockTracker = c.pawn_InventoryStockTracker;
			for (int i = group.min; i <= group.max; i++)
			{
				int localI = i;
				yield return new Widgets.DropdownMenuElement<int>
				{
					option = new FloatMenuOption(i.ToString(), delegate
					{	
						pawn_InventoryStockTracker.SetCountForGroup(group, localI);
					}),
					payload = i
				};
			}
		}

		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(54f));
		}

		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(104f), GetMinWidth(table), GetMaxWidth(table));
		}

		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		public override int Compare(Pawn a, Pawn b)
		{
			return GetValueToCompare(a).CompareTo(GetValueToCompare(b));
		}

		private int GetValueToCompare(Pawn pawn)
		{
			CompPawnPoisonTracker c = pawn.GetComp<CompPawnPoisonTracker>();
			Pawn_InventoryStockTracker pawn_InventoryStockTracker = c.pawn_InventoryStockTracker;
			if (pawn_InventoryStockTracker!= null)
			{
				return pawn_InventoryStockTracker.GetDesiredCountForGroup(DIH_PoisonStockGroups.DIH_PoisonStockGroup); //InventoryStockGroupDefOf.Medicine);
			}
			return int.MinValue;
		}
	}

}