using System;
using UnityEngine;
using Verse;
using RimWorld;
namespace DI_Harmacy
{
	// Token: 0x020011C6 RID: 4550
	[StaticConstructorOnStartup]
	public class GizmoPoisonAmmoStatus : Gizmo
	{
		// Token: 0x06006E9B RID: 28315 RVA: 0x0023CB48 File Offset: 0x0023AD48
		public GizmoPoisonAmmoStatus()
		{
			this.order = -100f;
		}

		// Token: 0x06006E9C RID: 28316 RVA: 0x0023CB5B File Offset: 0x0023AD5B
		public override float GetWidth(float maxWidth)
		{
			return 140f;
		}

		// Token: 0x06006E9D RID: 28317 RVA: 0x002560C4 File Offset: 0x002542C4
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect overRect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Find.WindowStack.ImmediateWindow(1523289473, overRect, WindowLayer.GameUI, delegate
			{
				Rect rect2;
				Rect rect = rect2 = overRect.AtZero().ContractedBy(6f);
				rect2.height = overRect.height / 2f;
				Text.Font = GameFont.Tiny;
				Widgets.Label(rect2, this.poisonable.Props.PoisonGizmoLabel);
				Rect rect3 = rect;
				rect3.yMin = overRect.height / 2f;
				float fillPercent = this.poisonable.RemainingCharges / this.poisonable.Props.maxCharges;
				Widgets.FillableBar(rect3, fillPercent, GizmoPoisonAmmoStatus.FullBarTex, GizmoPoisonAmmoStatus.EmptyBarTex, false);
				/**if (this.poisonable.Props.targetFuelLevelConfigurable)
				{
					float num = this.poisonable.TargetFuelLevel / this.poisonable.Props.fuelCapacity;
					float x = rect3.x + num * rect3.width - (float)GizmoPoisonAmmoStatus.TargetLevelArrow.width * 0.5f / 2f;
					float y = rect3.y - (float)GizmoPoisonAmmoStatus.TargetLevelArrow.height * 0.5f;
					GUI.DrawTexture(new Rect(x, y, (float)GizmoPoisonAmmoStatus.TargetLevelArrow.width * 0.5f, (float)GizmoPoisonAmmoStatus.TargetLevelArrow.height * 0.5f), GizmoPoisonAmmoStatus.TargetLevelArrow);
				}**/
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect3, this.poisonable.RemainingCharges.ToString("F0") + " / " + this.poisonable.Props.maxCharges.ToString("F0"));
				Text.Anchor = TextAnchor.UpperLeft;
			}, true, false, 1f, null);
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x04003D53 RID: 15699
		public CompPoisonable poisonable;

		// Token: 0x04003D54 RID: 15700
		private static readonly Texture2D FullBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.35f, 0.35f, 0.2f));

		// Token: 0x04003D55 RID: 15701
		private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(Color.black);

		// Token: 0x04003D56 RID: 15702
		private static readonly Texture2D TargetLevelArrow = ContentFinder<Texture2D>.Get("UI/Misc/BarInstantMarkerRotated", true);

		// Token: 0x04003D57 RID: 15703
		private const float ArrowScale = 0.5f;
	}
}
