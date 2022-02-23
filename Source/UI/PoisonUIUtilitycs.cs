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

        //private static bool medicalCarePainting;
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

        public static void MedicalCareSelectButton(Rect rect, Pawn pawn)
        {
            if(pawn.GetComp<CompPawnPoisonTracker>() == null || pawn.WorkTagIsDisabled(WorkTags.Violent))
            {
                return;
            }
            ThingDef assignedPoison = pawn.GetComp<CompPawnPoisonTracker>().assignedPoison;
            Widgets.Dropdown(rect: rect, target: pawn, iconColor: Color.white,(Pawn p) => MedicalCareSelectButton_GetMedicalCare(pawn), (Pawn p) => MedicalCareSelectButton_GenerateMenu(pawn),buttonIcon: GetTextures(assignedPoison), paintable:true);
         }

            private static Texture2D GetTextures(ThingDef thingDef)
            {
            if(thingDef==null)
            {
               return ContentFinder<Texture2D>.Get("DIHarmacy/Things/Item/Resource/RedSlash");
               
            }
            else
            {
                return thingDef.uiIcon;
            }
           
            }

        private static ThingDef MedicalCareSelectButton_GetMedicalCare(Pawn pawn)
            {
        
                ThingDef mc = pawn.GetComp<CompPawnPoisonTracker>().assignedPoison;
            return mc;
            }
    private static IEnumerable<Widgets.DropdownMenuElement<ThingDef>> MedicalCareSelectButton_GenerateMenu(Pawn p)
            {
                foreach (PoisonUIData poisonUIData in PoisonUIDataList.poisonUIDataList)
                {
                    ThingDef mc = poisonUIData.thingDef;
                    yield return new Widgets.DropdownMenuElement<ThingDef>
                    {
                        option = new FloatMenuOption(poisonUIData.optionLabel, delegate
                        {
                            p.GetComp<CompPawnPoisonTracker>().assignedPoison = mc;
                        //p.playerSettings.medCare = mc;
                    }),
                        payload = mc
                    };
                }
            }
        }
    }