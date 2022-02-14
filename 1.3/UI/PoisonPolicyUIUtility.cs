// RimWorld.DrugPolicyUIUtility
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
namespace DI_Harmacy
{
	public static class PoisonPolicyUiUtility
	{
		public const string AssigningDrugsTutorHighlightTag = "ButtonAssignPoisons";

		public static void DoAssignDrugPolicyButtons(Rect rect, Pawn pawn)
		{
			Pawn_PoisonPolicyTracker pawn_PoisonPolicyTracker = pawn.def.GetModExtension<PawnPoisonTrackerExtension>().pawn_PoisonPolicyTracker;


			int num = Mathf.FloorToInt((rect.width - 4f) * 0.714285731f);
			int num2 = Mathf.FloorToInt((rect.width - 4f) * 0.2857143f);
			float x = rect.x;
			Rect rect2 = new Rect(x, rect.y + 2f, num, rect.height - 4f);
			string text = pawn_PoisonPolicyTracker.CurrentPolicy.label;
			//string text = pawn.drugs.CurrentPolicy.label;
			if (pawn.story != null && pawn.story.traits != null)
			{
				Trait trait = pawn.story.traits.GetTrait(TraitDefOf.DrugDesire);
				if (trait != null)
				{
					text = text + " (" + trait.Label + ")";
				}
			}
			//Widgets.Dropdown(rect2, pawn, (Pawn p) => p.drugs.CurrentPolicy, Button_GenerateMenu, text.Truncate(rect2.width), null, pawn.drugs.CurrentPolicy.label, null, delegate
			//Widgets.Dropdown(rect2, pawn, (Pawn p) => pawn_PoisonPolicyTracker.CurrentPolicy, Button_GenerateMenu, text.Truncate(rect2.width), null, pawn_PoisonPolicyTracker.CurrentPolicy.label, null, delegate
			//{
				//PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.DrugPolicies, KnowledgeAmount.Total);
			//}, paintable: true);
			x += (float)num;
			x += 4f;
			Rect rect3 = new Rect(x, rect.y + 2f, num2, rect.height - 4f);
			if (Widgets.ButtonText(rect3, "AssignTabEdit".Translate()))
			{
				Find.WindowStack.Add(new Dialog_ManageDrugPolicies(pawn.drugs.CurrentPolicy));
			}
			UIHighlighter.HighlightOpportunity(rect2, "ButtonAssignDrugs");
			UIHighlighter.HighlightOpportunity(rect3, "ButtonAssignDrugs");
			x += (float)num2;
			
		}

		private static IEnumerable<Widgets.DropdownMenuElement<PoisonPolicy>> Button_GenerateMenu(Pawn pawn)
		{
			//this will need to change to poison policy
			foreach (PoisonPolicy assignedDrugs in StaticPoisonDatabase.poisonPolicyDatabase.AllPolicies)
			{
				yield return new Widgets.DropdownMenuElement<PoisonPolicy>
				{
					option = new FloatMenuOption(assignedDrugs.label, delegate
					{
						Pawn_PoisonPolicyTracker pawn_PoisonPolicyTracker = pawn.def.GetModExtension<PawnPoisonTrackerExtension>().pawn_PoisonPolicyTracker;
						pawn_PoisonPolicyTracker.CurrentPolicy = assignedDrugs;
						//pawn.drugs.CurrentPolicy = assignedDrugs;
					}),
					//payload = assignedDrugs
				};
			}
		}
	}
}