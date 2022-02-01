// RimWorld.Verb_Jump
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using DI_Harmacy_VerbExtensions;
namespace DI_Harmacy
{
	public class VerbPoison_Jump : Verb
	{
		private float cachedEffectiveRange = -1f;

		protected override float EffectiveRange
		{

			get
			{
				
				if (cachedEffectiveRange < 0f)
				{
					try
					{
						cachedEffectiveRange = base.EquipmentSource.GetStatValue(StatDefOf.JumpRange);
					}
					catch
                    {
						////Log.Error("Value unset. setting five");
						cachedEffectiveRange = 5f;
                    }
					}
				return cachedEffectiveRange;
			}
		}

		public override bool MultiSelect => true;

		protected override bool TryCastShot()
		{

			////Log.Error("TRYCASTSHOT");
			if (!ModLister.CheckRoyalty("Jumping"))
			{
				return false;
			}
			
			
			CompPoisonable poisonableCompSource = Verbs.PoisonableCompSource(base.DirectOwner);
			Pawn casterPawn = CasterPawn;
			if (casterPawn == null || poisonableCompSource == null || !poisonableCompSource.CanBeUsed)
			{
				return false;
			}
			IntVec3 cell = currentTarget.Cell;
			Map map = casterPawn.Map;
			poisonableCompSource.UsedOnce();
			PawnFlyer pawnFlyer = PawnFlyer.MakeFlyer(ThingDefOf.PawnJumper, casterPawn, cell);
			if (pawnFlyer != null)
			{
				GenSpawn.Spawn(pawnFlyer, cell, map);
				return true;
			}
			return false;
		}

		public override void OrderForceTarget(LocalTargetInfo target)
		{
		//	//Log.Error("OrderForceTarget");
			Map map = CasterPawn.Map;
			IntVec3 intVec = RCellFinder.BestOrderedGotoDestNear(target.Cell, CasterPawn, AcceptableDestination);
			Job job = JobMaker.MakeJob(JobDefOf.CastJump, intVec);
			job.verbToUse = this;
			if (CasterPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc))
			{
				FleckMaker.Static(intVec, map, FleckDefOf.FeedbackGoto);
			}
			bool AcceptableDestination(IntVec3 c)
			{
				if (ValidJumpTarget(map, c))
				{
					return CanHitTargetFrom(caster.Position, c);
				}
				return false;
			}
		}

		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			////Log.Error("ValidateTarget");
			if (caster == null)
			{
				return false;
			}
			if (!CanHitTarget(target) || !ValidJumpTarget(caster.Map, target.Cell))
			{
				return false;
			}
			if (!PoisonableUtility.CanUseConsideringQueuedJobs(CasterPawn, base.EquipmentSource))
			{
				return false;
			}
			return true;
		}

		public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
		{
		//	//Log.Error("CanHitTargetFrom");
			float num = EffectiveRange * EffectiveRange;
			IntVec3 cell = targ.Cell;
			if ((float)caster.Position.DistanceToSquared(cell) <= num)
			{
				return GenSight.LineOfSight(root, cell, caster.Map);
			}
			return false;
		}

		public override void OnGUI(LocalTargetInfo target)
		{
			////Log.Error("OnGui");
			if (CanHitTarget(target) && ValidJumpTarget(caster.Map, target.Cell))
			{
				base.OnGUI(target);
			}
			else
			{
				GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
			}
		}

		public override void DrawHighlight(LocalTargetInfo target)
		{
			////Log.Error("DrawHighlight");
			if(target.IsValid)
				////Log.Error("IsValid");
			if (ValidJumpTarget(caster.Map, target.Cell))
				//Log.Error("validjumptarget");
			if (target.IsValid && ValidJumpTarget(caster.Map, target.Cell))
			{
				
				GenDraw.DrawTargetHighlightWithLayer(target.CenterVector3, AltitudeLayer.MetaOverlays);
			}
			//Log.Error("GenDrawBefore");
			GenDraw.DrawRadiusRing(caster.Position, EffectiveRange, Color.white, (IntVec3 c) => GenSight.LineOfSight(caster.Position, c, caster.Map) && ValidJumpTarget(caster.Map, c));
			//Log.Error("GenDrawAfter");
		}

		public static bool ValidJumpTarget(Map map, IntVec3 cell)
		{
			//Log.Error("ValidJumpTarget");
			if (!cell.IsValid || !cell.InBounds(map))
			{
				return false;
			}
			if (cell.Impassable(map) || !cell.Walkable(map) || cell.Fogged(map))
			{
				return false;
			}
			Building edifice = cell.GetEdifice(map);
			Building_Door building_Door;
			if (edifice != null && (building_Door = edifice as Building_Door) != null && !building_Door.Open)
			{
				return false;
			}
			return true;
		}
	}
}