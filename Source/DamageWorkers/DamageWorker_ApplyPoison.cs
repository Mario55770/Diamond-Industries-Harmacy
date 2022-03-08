// Verse.DamageWorker
using RimWorld;
using Verse;
namespace DI_Harmacy
{

    public class DamageWorker_ApplyPoison : DamageWorker_AddInjury
    {
        protected override BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
        {

            return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, BodyPartDepth.Outside);
        }

        protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageResult result)
        {
            //get the variables
            CompPoisonable compPoisonable = null;
            Pawn p = (dinfo.Instigator as Pawn);
            ThingWithComps weapon = p.equipment.Primary;
            compPoisonable = weapon.TryGetComp<CompPoisonable>();
            CompPawnPoisonTracker poisonTracker = p.GetComp<CompPawnPoisonTracker>();
            if (poisonTracker == null || !poisonTracker.applyPoisonActive)
            {
                return;
            }
            //if comp is null or can't be used. Makes a bunch of code from elsewhere redundant, which should be removed.
            if (compPoisonable == null || compPoisonable.CanBeUsed == false)
            {
                return;
            }
            //Uses poison.
            compPoisonable.UsedOnce();
            //gets from original comp to help cut down redundant code. Probably slightly worse for performance...
            HediffDef hediffToApply = compPoisonable.hediffToApply;
            //should be lumped in next if statement but its more readable
            //Ends the method if the hediff is null
            if (hediffToApply == null)
            { return; }
            //gets the amount applied if the damage info has a different amount than default this is important
            float appliedAmount = dinfo.Amount;
            //lessons the poison by same amount toxic buildup does
            appliedAmount *= 0.028758334f;
            //multiplies by the setting.
            appliedAmount *= DIHSettings.instance.poisonMultiplier;
            //multiplies by toxic sensitivity
            appliedAmount *= pawn.GetStatValue(StatDefOf.ToxicSensitivity);
            //gets hit part
            //Applies to struck part if possible and told to do so. As a comptablity measure if its null it just defaults to whole body handling.
            if (compPoisonable.applyToStruckPart)
            {
                BodyPartRecord targetPart = dinfo.HitPart;
                if (targetPart != null)
                {
                    //borrowed from the hediff handler from diamond shield. Checks if pawns dead first and the part is there
                    if (pawn.health.Dead || pawn.health.hediffSet.PartIsMissing(targetPart)) //If pawn dead or part missing..
                        return; //Abort.
                    bool found = false;
                    foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
                    {
                        if (hediff.def != hediffToApply || hediff.Part != targetPart)
                            continue;
                        found = true;
                        hediff.Severity += appliedAmount;
                    }
                    if (!found)
                    {
                        hediffToApply.initialSeverity = appliedAmount;
                        pawn.health.AddHediff(hediffToApply, targetPart);
                    }
                }
            }
            else
            {
                HealthUtility.AdjustSeverity(pawn, hediffToApply, appliedAmount);
            }
            Find.HistoryEventsManager.RecordEvent(new HistoryEvent(DIH_HistoryDefs.DIH_UsedPoisonWeapon, p.Named(HistoryEventArgsNames.Doer)));

        }
    }

}
