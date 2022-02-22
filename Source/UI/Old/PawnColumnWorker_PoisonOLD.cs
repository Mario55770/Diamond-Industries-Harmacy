﻿// RimWorld.PawnColumnWorker_Carry
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
namespace DI_Harmacy
{
	//TODO. Consider swapping this to use PawnColumnWorker_Medicine
	public class PawnColumnWorker_PoisonOLD : PawnColumnWorker
	{
		private const int TopAreaHeight = 65;

		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if(pawn.WorkTagIsDisabled(WorkTags.Violent))
            {
				return;
            }				
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
				InventoryStockGroupDef group = DIH_PoisonStockGroups.DIH_PoisonStockGroup;
				
				Widgets.Dropdown(new Rect(x, rect.y + 2f, num2, rect.height - 4f), pawn, (Pawn p) => p.inventoryStock.GetDesiredThingForGroup(group), (Pawn p) => GenerateThingButtons(p, group), null, pawn_InventoryStockTracker.GetDesiredThingForGroup(group).uiIcon, null, null, null, paintable: true);
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
						pawn_InventoryStockTracker.SetThingForGroup(group, thing);
					}),
					payload = thing
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
				return pawn_InventoryStockTracker.GetDesiredCountForGroup(DIH_PoisonStockGroups.DIH_PoisonStockGroup); 
			}
			return int.MinValue;
		}
	}

}