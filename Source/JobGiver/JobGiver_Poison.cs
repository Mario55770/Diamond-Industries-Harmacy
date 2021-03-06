// RimWorld.JobGiver_Reload
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace DI_Harmacy
{
    public class JobGiver_Poison : ThinkNode_JobGiver
    {
        public override float GetPriority(Pawn pawn)
        {
            //Originally 5.9f
            return 6.1f;

        }

        protected override Job TryGiveJob(Pawn pawn)
        {
            CompPoisonable compPoisonable = RecoatingUtility.FindSomeReloadableComponent(pawn, allowForcedReload: false);
            if (compPoisonable == null)
            {
                return null;
            }
            List<Thing> list = RecoatingUtility.FindEnoughAmmo(pawn, pawn.Position, compPoisonable, forceReload: false);
            if (list == null)
            {
                return null;
            }
            return MakeReloadJob(compPoisonable, list);
        }

        public static Job MakeReloadJob(CompPoisonable comp, List<Thing> chosenAmmo)
        {
            Job job = JobMaker.MakeJob(DIH_JobDefs.DIH_PoisonJob, comp.parent);
            job.targetQueueB = chosenAmmo.Select((Thing t) => new LocalTargetInfo(t)).ToList();
            job.count = chosenAmmo.Sum((Thing t) => t.stackCount);
            job.count = Math.Min(job.count, comp.MaxAmmoNeeded(allowForcedReload: true));
            return job;
        }
    }
}