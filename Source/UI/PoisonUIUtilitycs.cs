// RimWorld.MedicalCareUtility
using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace DI_Harmacy
{
	[StaticConstructorOnStartup]
	
	public static class PoisonUIUtility
	{
		private static Texture2D[] careTextures;

		public const float CareSetterHeight = 28f;

		public const float CareSetterWidth = 140f;

		private static bool medicalCarePainting;
		static PoisonUIUtility()
        {
			Reset();
        }
		public static void Reset()
		{
			//Log.Message("Reset");
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				careTextures = new Texture2D[5];
				careTextures[0] = ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoCare");
				careTextures[1] = ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoMeds");
				careTextures[2] = ThingDefOf.MedicineHerbal.uiIcon;
				careTextures[3] = ThingDefOf.MedicineIndustrial.uiIcon;
				careTextures[4] = ThingDefOf.MedicineUltratech.uiIcon;
			});
		}
		//from DraggableResultUtility, its the same, but I had to do it here as it was originally part of an internal class
		public static bool AnyPressed(this Widgets.DraggableResult result)
		{
			//Log.Message("AnyPressed");
			if (result != Widgets.DraggableResult.Pressed)
			{
				return result == Widgets.DraggableResult.DraggedThenPressed;
			}
			return true;
		} 
		public static void MedicalCareSetter(Rect rect, ref PoisonUsagePolicy medCare)
		{
			//Log.Message("MedicalCareSetter");
			Rect rect2 = new Rect(rect.x, rect.y, rect.width / 5f, rect.height);
			for (int i = 0; i < 5; i++)
			{
				PoisonUsagePolicy mc = (PoisonUsagePolicy)i;
				Widgets.DrawHighlightIfMouseover(rect2);
				MouseoverSounds.DoRegion(rect2);
				GUI.DrawTexture(rect2, careTextures[i]);
				Widgets.DraggableResult draggableResult = Widgets.ButtonInvisibleDraggable(rect2);
				if (draggableResult == Widgets.DraggableResult.Dragged)
				{
					medicalCarePainting = true;
				}
				if ((medicalCarePainting && Mouse.IsOver(rect2) && medCare != mc) || draggableResult.AnyPressed())
				{
					medCare = mc;
					SoundDefOf.Tick_High.PlayOneShotOnCamera();
				}
				if (medCare == mc)
				{
					Widgets.DrawBox(rect2, 3);
				}
				if (Mouse.IsOver(rect2))
				{
					TooltipHandler.TipRegion(rect2, () => mc.GetLabel(), 632165 + i * 17);
				}
				rect2.x += rect2.width;
			}
			if (!Input.GetMouseButton(0))
			{
				medicalCarePainting = false;
			}
		}

		public static string GetLabel(this PoisonUsagePolicy cat)
		{
			//Translate should be readdded when I can
			//return ("PoisonUsagePolicy_" + cat);
			return ("MedicalCareCategory_" + cat).Translate();
		}

		public static bool AllowsMedicine(this PoisonUsagePolicy cat, ThingDef meds)
		{
			//Log.Message("AllowsMedicine");
			switch (cat)
			{
				case PoisonUsagePolicy.NoCare:
					return false;
				case PoisonUsagePolicy.NoMeds:
					return false;
				case PoisonUsagePolicy.HerbalOrWorse:
					return meds.GetStatValueAbstract(StatDefOf.MedicalPotency) <= ThingDefOf.MedicineHerbal.GetStatValueAbstract(StatDefOf.MedicalPotency);
				case PoisonUsagePolicy.NormalOrWorse:
					return meds.GetStatValueAbstract(StatDefOf.MedicalPotency) <= ThingDefOf.MedicineIndustrial.GetStatValueAbstract(StatDefOf.MedicalPotency);
				case PoisonUsagePolicy.Best:
					return true;
				default:
					throw new InvalidOperationException();
			}
		}

		public static void MedicalCareSelectButton(Rect rect, Pawn pawn)
		{
			//Log.Message("MedicalCareSelectButton");
			Widgets.Dropdown(rect, pawn, MedicalCareSelectButton_GetMedicalCare, MedicalCareSelectButton_GenerateMenu, null, careTextures[(uint)pawn.GetComp<CompPawnPoisonTracker>().poisonUsagePolicy], null, null, null, paintable: true);
			
			//	Widgets.Dropdown(rect, pawn, MedicalCareSelectButton_GetMedicalCare, MedicalCareSelectButton_GenerateMenu, null, careTextures[(uint)pawn.playerSettings.medCare], null, null, null, paintable: true);

		}

		private static PoisonUsagePolicy MedicalCareSelectButton_GetMedicalCare(Pawn pawn)
		{
			//Log.Message("MedicalCareSelectButtong_GetMedicalCare");
			//return pawn.playerSettings.medCare;
			return pawn.GetComp<CompPawnPoisonTracker>().poisonUsagePolicy;
		}

		private static IEnumerable<Widgets.DropdownMenuElement<PoisonUsagePolicy>> MedicalCareSelectButton_GenerateMenu(Pawn p)
		{
			//Log.Message("MedicalCareSelectButton_GenerateMenu");
			for (int i = 0; i < 5; i++)
			{
				PoisonUsagePolicy mc = (PoisonUsagePolicy)i;
				yield return new Widgets.DropdownMenuElement<PoisonUsagePolicy>
				{
					option = new FloatMenuOption(mc.GetLabel(), delegate
					{
						p.GetComp<CompPawnPoisonTracker>().poisonUsagePolicy=mc;
						//p.playerSettings.medCare = mc;
					}),
					payload = mc
				};
			}
		}
	}
}