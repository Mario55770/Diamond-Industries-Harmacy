﻿// RimWorld.PawnColumnWorker_DrugPolicy
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;
namespace DI_Harmacy
{
	public class PawnColumnWorker_PoisonPolicy : PawnColumnWorker
	{
		private const int TopAreaHeight = 65;

		public const int ManageDrugPoliciesButtonHeight = 32;

		public override void DoHeader(Rect rect, PawnTable table)
		{
			//Log.Message("DoHeader");
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
			Rect rect2 = new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f);
			if (Widgets.ButtonText(rect2, "Manage poison policies"))//"ManageDrugPolicies".Translate()))
			{
				Find.WindowStack.Add(new Dialog_ManageDrugPolicies(null));
			}
			UIHighlighter.HighlightOpportunity(rect2, "Manage poison policies");//"ManageDrugPolicies");
			UIHighlighter.HighlightOpportunity(rect2, "ButtonAssignPoisons");//"ButtonAssignDrugs");
		}

		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Log.Message("DocEll");
			if (pawn.drugs != null)
			{
				PoisonPolicyUiUtility.DoAssignDrugPolicyButtons(rect, pawn);
			}
		}

		public override int GetMinWidth(PawnTable table)
		{
			Log.Message("GetMinWidth");
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
		}

		public override int GetOptimalWidth(PawnTable table)
		{
			Log.Message("GetOptimalWidth");
			return Mathf.Clamp(Mathf.CeilToInt(251f), GetMinWidth(table), GetMaxWidth(table));
		}

		public override int GetMinHeaderHeight(PawnTable table)
		{
			Log.Message("GetMinHeaderHeight");
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		public override int Compare(Pawn a, Pawn b)
		{
			Log.Message("Compare");
			return GetValueToCompare(a).CompareTo(GetValueToCompare(b));
		}

		private int GetValueToCompare(Pawn pawn)
		{
			Log.Message("GetValueToCompare");
			if (pawn.drugs != null && pawn.drugs.CurrentPolicy != null)
			{
				return pawn.drugs.CurrentPolicy.uniqueId;
			}
			return int.MinValue;
		}
	}
}