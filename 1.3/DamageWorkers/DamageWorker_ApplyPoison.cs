// Verse.DamageWorker
using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
namespace DI_Harmacy
{

    public class DamageWorker_ApplyPoison : DamageWorker_AddInjury
    {
        protected override BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
        {
           // Log.Message("ChooseHitPart");
            return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, BodyPartDepth.Outside);
        }

        protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageResult result)
        {
            
            CompPoisonable compPoisonable = null;
            //(comp.ParentHolder as Pawn)
            //Log.Message("Var V");
            
            //Log.Message("PAwnInstagate");
            Pawn p = (dinfo.Instigator as Pawn);
            //Log.Message("CheckNull");
            
            ThingWithComps weapon = p.equipment.Primary;
            //Log.Message("CheckComp");
            compPoisonable = weapon.TryGetComp<CompPoisonable>();
            if (compPoisonable == null ||compPoisonable.CanBeUsed==false)
            {
                return;
            }
            compPoisonable.UsedOnce();
                HediffDef h = compPoisonable.Props.hediffToApply;
            Log.Message(h.ToString());
            float appliedAmount = dinfo.Amount;
            //lessons the poison by same amount toxic buildup does
            appliedAmount *= 0.028758334f;
            //multiplies by toxic sensitivity
            appliedAmount *= pawn.GetStatValue(StatDefOf.ToxicSensitivity);
            // Log.Message("TEST");
            BodyPartRecord targetPart = dinfo.HitPart;
            if (compPoisonable.Props.applyToStruckPart&&targetPart!=null)
            {
                //private static void hediffApplicationComparisons(Pawn p, HediffDef h, FloatRange hediffFactor, BodyPartRecord targetPart)
                
                
                
                if (pawn.health.Dead || pawn.health.hediffSet.PartIsMissing(targetPart)) //If pawn dead or part missing..
                    return; //Abort.
                bool found = false;
                foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
                {
                    if (hediff.def != h || hediff.Part != targetPart)
                        continue;
                    found = true;
                    hediff.Severity += appliedAmount;
                }
                if (!found)
                {
                    h.initialSeverity = appliedAmount;
                    pawn.health.AddHediff(h, targetPart);
                }
            }
            else
            {
                HealthUtility.AdjustSeverity(pawn, h, appliedAmount);
            }
            
        }
    }

}
