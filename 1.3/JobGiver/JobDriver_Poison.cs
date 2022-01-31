using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;
namespace DI_Harmacy
{
    public class JobDriver_Poison : JobDriver
    {
        private const TargetIndex GearInd = TargetIndex.A;
        private const TargetIndex AmmoInd = TargetIndex.B;

        private Thing Gear => this.job.GetTarget(TargetIndex.A).Thing;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job);
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            JobDriver_Poison f = this;
            Thing gear = f.Gear;
            CompPoisonable comp = gear != null ? gear.TryGetComp<CompPoisonable>() : (CompPoisonable)null;
            f.FailOn<JobDriver_Poison>((Func<bool>)(() => comp == null));
            f.FailOn<JobDriver_Poison>((Func<bool>)(() => comp.Weilder != this.pawn));
            f.FailOn<JobDriver_Poison>((Func<bool>)(() => !comp.NeedsReload(true)));
            f.FailOnDestroyedOrNull<JobDriver_Poison>(TargetIndex.A);
            f.FailOnIncapable<JobDriver_Poison>(PawnCapacityDefOf.Manipulation);
            Toil getNextIngredient = Toils_General.Label();
            yield return getNextIngredient;
            foreach (Toil toil in f.ReloadAsMuchAsPossible(comp))
                yield return toil;
            yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B);
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden<Toil>(TargetIndex.B).FailOnSomeonePhysicallyInteracting<Toil>(TargetIndex.B);
            yield return Toils_Haul.StartCarryThing(TargetIndex.B, subtractNumTakenFromJobCount: true).FailOnDestroyedNullOrForbidden<Toil>(TargetIndex.B);
            yield return Toils_Jump.JumpIf(getNextIngredient, (Func<bool>)(() => !this.job.GetTargetQueue(TargetIndex.B).NullOrEmpty<LocalTargetInfo>()));
            foreach (Toil toil in f.ReloadAsMuchAsPossible(comp))
                yield return toil;
            yield return new Toil()
            {
                initAction = (Action)(() =>
                {
                    Thing carriedThing = this.pawn.carryTracker.CarriedThing;
                    if (carriedThing == null || carriedThing.Destroyed)
                        return;
                    this.pawn.carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Near, out Thing _);
                }),
                defaultCompleteMode = ToilCompleteMode.Instant
            };
        }

        private IEnumerable<Toil> ReloadAsMuchAsPossible(CompPoisonable comp)
        {
            Toil done = Toils_General.Label();
            yield return Toils_Jump.JumpIf(done, (Func<bool>)(() => this.pawn.carryTracker.CarriedThing == null || this.pawn.carryTracker.CarriedThing.stackCount < comp.MinAmmoNeeded(true)));
            yield return Toils_General.Wait(comp.Props.baseReloadTicks).WithProgressBarToilDelay(TargetIndex.A);
            yield return new Toil()
            {
                initAction = (Action)(() => comp.ReloadFrom(this.pawn.carryTracker.CarriedThing)),
                defaultCompleteMode = ToilCompleteMode.Instant
            };
            yield return done;
        }
    }
}