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
            Log.Message("CheckComp");
            compPoisonable = weapon.TryGetComp<CompPoisonable>();
            HediffDef h = dinfo.Def.hediff;
            float appliedAmount = dinfo.Amount;
            //lessons the poison by same amount toxic buildup does
            appliedAmount *= 0.028758334f;
            //multiplies by toxic sensitivity
            appliedAmount *= pawn.GetStatValue(StatDefOf.ToxicSensitivity);
            Log.Message("TEST");
            if (compPoisonable.Props.applyToStruckPart)
            {
                //private static void hediffApplicationComparisons(Pawn p, HediffDef h, FloatRange hediffFactor, BodyPartRecord targetPart)
                BodyPartRecord targetPart = dinfo.HitPart;
                
                
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
            /** Log.Message("ApplySpecialEffectsToPart");
             if (dinfo.HitPart.depth == BodyPartDepth.Inside)
             {
                 List<BodyPartRecord> list = new List<BodyPartRecord>();
                 for (BodyPartRecord bodyPartRecord = dinfo.HitPart; bodyPartRecord != null; bodyPartRecord = bodyPartRecord.parent)
                 {
                     list.Add(bodyPartRecord);
                     if (bodyPartRecord.depth == BodyPartDepth.Outside)
                     {
                         break;
                     }
                 }
                 float num = (float)(list.Count - 1) + 0.5f;
                 for (int i = 0; i < list.Count; i++)
                 {
                     DamageInfo dinfo2 = dinfo;
                     dinfo2.SetHitPart(list[i]);
                     FinalizeAndAddInjury(pawn, totalDamage / num * ((i == 0) ? 0.5f : 1f), dinfo2, result);
                 }
                 return;
             }
             Log.Message("AfterFirstIf");
             int num2 = ((def.cutExtraTargetsCurve != null) ? GenMath.RoundRandom(def.cutExtraTargetsCurve.Evaluate(Rand.Value)) : 0);
             List<BodyPartRecord> list2 = null;
             if (num2 != 0)
             {
                 IEnumerable<BodyPartRecord> enumerable = dinfo.HitPart.GetDirectChildParts();
                 if (dinfo.HitPart.parent != null)
                 {
                     enumerable = enumerable.Concat(dinfo.HitPart.parent);
                     if (dinfo.HitPart.parent.parent != null)
                     {
                         enumerable = enumerable.Concat(dinfo.HitPart.parent.GetDirectChildParts());
                     }
                 }
                 list2 = (from x in enumerable.Except(dinfo.HitPart).InRandomOrder().Take(num2)
                          where !x.def.conceptual
                          select x).ToList();
             }
             else
             {
                 list2 = new List<BodyPartRecord>();
             }
             Log.Message("AfterFirstElse");
             list2.Add(dinfo.HitPart);
             float num3 = totalDamage * (1f + def.cutCleaveBonus) / ((float)list2.Count + def.cutCleaveBonus);
             if (num2 == 0)
             {
                 num3 = ReduceDamageToPreserveOutsideParts(num3, dinfo, pawn);
             }
             Log.Message("BeforeForLoop");
             for (int j = 0; j < list2.Count; j++)
             {
                 DamageInfo dinfo3 = dinfo;
                 Log.Message("TESTONE");
                 dinfo3.SetHitPart(list2[j]);
                 Log.Message("TestTwo");
                 FinalizeAndAddInjury(pawn, num3, dinfo3, result);
                 Log.Message("TestThree");
             }
             Log.Message("CodeEnds");
            **/
        }
    }

}
