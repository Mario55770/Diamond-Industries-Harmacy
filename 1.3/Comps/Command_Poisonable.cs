using System;
using UnityEngine;
using Verse;

using RimWorld;
namespace DI_Harmacy
{
	// Token: 0x020011CA RID: 4554
	public class Command_Poisonable : Command_VerbTarget
	{
		// Token: 0x06006EC9 RID: 28361 RVA: 0x00256A1C File Offset: 0x00254C1C
		public Command_Poisonable(CompPoisonable comp)
		{
			this.comp = comp;
		}

		// Token: 0x17001333 RID: 4915
		// (get) Token: 0x06006ECA RID: 28362 RVA: 0x00256A2B File Offset: 0x00254C2B
		public override string TopRightLabel
		{
			get
			{
				return this.comp.LabelRemaining;
			}
		}

		// Token: 0x17001334 RID: 4916
		// (get) Token: 0x06006ECB RID: 28363 RVA: 0x00256A38 File Offset: 0x00254C38
		public override Color IconDrawColor
		{
			get
			{
				Color? color = this.overrideColor;
				if (color == null)
				{
					return base.IconDrawColor;
				}
				return color.GetValueOrDefault();
			}
		}

		// Token: 0x06006ECC RID: 28364 RVA: 0x00256A63 File Offset: 0x00254C63
		public override void GizmoUpdateOnMouseover()
		{
			this.verb.DrawHighlight(LocalTargetInfo.Invalid);
		}

		// Token: 0x06006ECD RID: 28365 RVA: 0x00256A78 File Offset: 0x00254C78
		public override bool GroupsWith(Gizmo other)
		{
			if (!base.GroupsWith(other))
			{
				return false;
			}
			Command_Poisonable command_Reloadable = other as Command_Poisonable;
			return command_Reloadable != null && this.comp.parent.def == command_Reloadable.comp.parent.def;
		}

		// Token: 0x04003D67 RID: 15719
		private readonly CompPoisonable comp;

		// Token: 0x04003D68 RID: 15720
		public Color? overrideColor;
	}
}
