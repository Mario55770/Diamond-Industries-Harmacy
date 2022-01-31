using System;
using UnityEngine;
using Verse;

using RimWorld;
namespace DI_Harmacy
{
	public class Command_Poisonable : Command_VerbTarget
	{
		private readonly CompPoisonable comp;

		public Color? overrideColor;

		public override string TopRightLabel => comp.LabelRemaining;

		public override Color IconDrawColor => overrideColor ?? base.IconDrawColor;

		public Command_Poisonable(CompPoisonable comp)
		{
			this.comp = comp;
		}

		public override void GizmoUpdateOnMouseover()
		{
			verb.DrawHighlight(LocalTargetInfo.Invalid);
		}

		public override bool GroupsWith(Gizmo other)
		{
			if (!base.GroupsWith(other))
			{
				return false;
			}
			Command_Poisonable command_Poisonable = other as Command_Poisonable;
			if (command_Poisonable == null)
			{
				return false;
			}
			return comp.parent.def == command_Poisonable.comp.parent.def;
		}
	}

}
