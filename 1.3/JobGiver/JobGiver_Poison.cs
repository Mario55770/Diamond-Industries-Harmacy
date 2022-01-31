using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using RimWorld;
namespace DI_Harmacy
{
	// Token: 0x0200083E RID: 2110
	public class JobGiver_Poison : ThinkNode_JobGiver
	{
		// Token: 0x0600387A RID: 14458 RVA: 0x0013FF42 File Offset: 0x0013E142
		public override float GetPriority(Pawn pawn)
		{
			return 5.9f;
		}

		// Token: 0x0600387B RID: 14459 RVA: 0x0013FF4C File Offset: 0x0013E14C
		protected override Job TryGiveJob(Pawn pawn)
		{
			Log.Message("TEST");
			CompPoisonable compPoisonable = PoisonableUtility.FindSomeReloadableComponent(pawn, false);
			if (compPoisonable == null)
			{
				return null;
			}
			List<Thing> list = PoisonableUtility.FindEnoughAmmo(pawn, pawn.Position, compPoisonable, false);
			if (list == null)
			{
				return null;
			}
			return JobGiver_Poison.MakeReloadJob(compPoisonable, list);
		}

		// Token: 0x0600387C RID: 14460 RVA: 0x0013FF84 File Offset: 0x0013E184
		public static Job MakeReloadJob(CompPoisonable comp, List<Thing> chosenAmmo)
		{
			Job job = JobMaker.MakeJob(DIH_JobDefs.DIH_PoisonJob, comp.parent);
			job.targetQueueB = (from t in chosenAmmo
								select new LocalTargetInfo(t)).ToList<LocalTargetInfo>();
			job.count = chosenAmmo.Sum((Thing t) => t.stackCount);
			job.count = Math.Min(job.count, comp.MaxAmmoNeeded(true));
			return job;
		}

		// Token: 0x04001F9F RID: 8095
		private const bool forceReloadWhenLookingForWork = false;
	}
}
