// RimWorld.JobGiver_Reload
using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace DI_Harmacy
{
	public class JobGiver_Poison : ThinkNode_JobGiver
	{
		private const bool forceReloadWhenLookingForWork = false;

		public override float GetPriority(Pawn pawn)
		{
			return 5.9f;
			Log.Message("GetPriority");
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Log.Message("TryGiveJob");
			CompPoisonable compPoisonable = PoisonableUtility.FindSomeReloadableComponent(pawn, allowForcedReload: false);
			if (compPoisonable == null)
			{
				return null;
			}
			List<Thing> list = PoisonableUtility.FindEnoughAmmo(pawn, pawn.Position, compPoisonable, forceReload: false);
			if (list == null)
			{
				return null;
			}
			return MakeReloadJob(compPoisonable, list);
		}

		public static Job MakeReloadJob(CompPoisonable comp, List<Thing> chosenAmmo)
		{
			Log.Message("MakeReloadJob");
			//Job job = JobMaker.MakeJob(JobDefOf.Reload, comp.parent);
			Job job = JobMaker.MakeJob(DIH_JobDefs.DIH_PoisonJob, comp.parent);
			job.targetQueueB = chosenAmmo.Select((Thing t) => new LocalTargetInfo(t)).ToList();
			job.count = chosenAmmo.Sum((Thing t) => t.stackCount);
			job.count = Math.Min(job.count, comp.MaxAmmoNeeded(allowForcedReload: true));
			return job;
		}
	}
}