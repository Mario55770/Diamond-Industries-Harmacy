﻿// RimWorld.JobDriver_Reload
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;
namespace DI_Harmacy
{
    public class JobDriver_Poison : JobDriver
    {
        private const TargetIndex GearInd = TargetIndex.A;

        private const TargetIndex AmmoInd = TargetIndex.B;

        private Thing Gear => job.GetTarget(TargetIndex.A).Thing;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            //Log.Message("TryMakePreToilReservation");
            pawn.ReserveAsManyAsPossible(job.GetTargetQueue(TargetIndex.B), job);
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            //Log.Message("MakeNewToils");
            CompPoisonable comp = Gear?.TryGetComp<CompPoisonable>();
            //Log.Message("TestCompPassed");
            this.FailOn(() => comp == null);


            this.FailOn(() => comp.weilderOf != pawn);
            this.FailOn(() => !comp.NeedsReload(allowForcedReload: true));
            this.FailOnDestroyedOrNull(TargetIndex.A);
            //Log.Message("Item Destroyed");
            this.FailOnIncapable(PawnCapacityDefOf.Manipulation);
            //Log.Message("Pawn has manipulation.");
            Toil getNextIngredient = Toils_General.Label();
            //Log.Message("Get ingredient");
            yield return getNextIngredient;
            //Log.Message("YeildIngredient");
            foreach (Toil item in ReloadAsMuchAsPossible(comp))
            {
               // Log.Message("loopone");
                yield return item;
            }
            yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B);
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
            yield return Toils_Haul.StartCarryThing(TargetIndex.B, putRemainderInQueue: false, subtractNumTakenFromJobCount: true).FailOnDestroyedNullOrForbidden(TargetIndex.B);
            yield return Toils_Jump.JumpIf(getNextIngredient, () => !job.GetTargetQueue(TargetIndex.B).NullOrEmpty());
            foreach (Toil item2 in ReloadAsMuchAsPossible(comp))
            {
             //   Log.Message("loop2");
                yield return item2;
            }
            Toil toil = new Toil();
            toil.initAction = delegate
            {
                Thing carriedThing = pawn.carryTracker.CarriedThing;
                if (carriedThing != null && !carriedThing.Destroyed)
                {
                    pawn.carryTracker.TryDropCarriedThing(pawn.Position, ThingPlaceMode.Near, out var _);
                }
            };
            toil.defaultCompleteMode = ToilCompleteMode.Instant;
           // Log.Message("FINISH");
            yield return toil;
        }

        private IEnumerable<Toil> ReloadAsMuchAsPossible(CompPoisonable comp)
        {
           // Log.Message("ReloadAsMuchAsPossible");
            Toil done = Toils_General.Label();
            yield return Toils_Jump.JumpIf(done, () => pawn.carryTracker.CarriedThing == null || pawn.carryTracker.CarriedThing.stackCount < comp.MinAmmoNeeded(allowForcedReload: true));
            yield return Toils_General.Wait(comp.Props.baseReloadTicks).WithProgressBarToilDelay(TargetIndex.A);
            Toil toil = new Toil();
            toil.initAction = delegate
            {
                Thing carriedThing = pawn.carryTracker.CarriedThing;
                comp.ReloadFrom(carriedThing);
            };
            toil.defaultCompleteMode = ToilCompleteMode.Instant;
            //Log.Message("TEST RELOAD");
            yield return toil;
            yield return done;
        }
    }
}