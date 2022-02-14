using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace DI_Harmacy
{
    class PawnColumnWorker_PoisonPolicy : PawnColumnWorker
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
				Find.WindowStack.Add(new Dialog_ManagePoisonPolicies(null));
			}
			UIHighlighter.HighlightOpportunity(rect2, "Manage poison policies");//"ManageDrugPolicies");
			UIHighlighter.HighlightOpportunity(rect2, "ButtonAssignPoisons");//"ButtonAssignDrugs");
		}

		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Pawn_PoisonPolicyTracker pawn_PoisonPolicyTracker = pawn.def.GetModExtension<PawnPoisonTrackerExtension>().pawn_PoisonPolicyTracker;
			//Log.Message("DocEll");
			//if (pawn.drugs != null)
			if (pawn_PoisonPolicyTracker != null)
			{
				Log.Error("NotImplementedDoCell");
				PoisonPolicyUiUtility.DoAssignDrugPolicyButtons(rect, pawn);
			}
		}

		public override int GetMinWidth(PawnTable table)
		{
			//Log.Message("GetMinWidth");
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
		}

		public override int GetOptimalWidth(PawnTable table)
		{
			//Log.Message("GetOptimalWidth");
			return Mathf.Clamp(Mathf.CeilToInt(251f), GetMinWidth(table), GetMaxWidth(table));
		}

		public override int GetMinHeaderHeight(PawnTable table)
		{
			//Log.Message("GetMinHeaderHeight");
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		public override int Compare(Pawn a, Pawn b)
		{
			//Log.Message("Compare");
			return GetValueToCompare(a).CompareTo(GetValueToCompare(b));
		}

		private int GetValueToCompare(Pawn pawn)
		{
			Pawn_PoisonPolicyTracker pawn_PoisonPolicyTracker = pawn.def.GetModExtension<PawnPoisonTrackerExtension>().pawn_PoisonPolicyTracker;

			if (pawn_PoisonPolicyTracker!=null &&pawn_PoisonPolicyTracker.CurrentPolicy!=null)
            {
				Log.Error("GetValueToCompareNotImplemented");
				return pawn_PoisonPolicyTracker.CurrentPolicy.uniqueId;
            }
			//Log.Message("GetValueToCompare");
			//if (pawn.drugs != null && pawn.drugs.CurrentPolicy != null)
			//{
				//return pawn.drugs.CurrentPolicy.uniqueId;
			//}
			return int.MinValue;
		}
	}
}