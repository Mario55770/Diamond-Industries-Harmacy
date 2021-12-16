using System;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;
namespace DI_Harmacy
{
	// Token: 0x020002D1 RID: 721
	public class NecroticInjuryHandler : Hediff_Injury
	{
		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06001385 RID: 4997 RVA: 0x0006FA98 File Offset: 0x0006DC98
		/**public override int UIGroupKey
		{
			get
			{
				int num = base.UIGroupKey;
				if (this.IsTended())
				{
					num = Gen.HashCombineInt(num, 152235495);
				}
				return num;
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06001386 RID: 4998 RVA: 0x0006FAC4 File Offset: 0x0006DCC4
		public override string LabelBase
		{
			get
			{
				HediffComp_GetsPermanent hediffComp_GetsPermanent = this.TryGetComp<HediffComp_GetsPermanent>();
				if (hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent)
				{
					if (base.Part.def.delicate && !hediffComp_GetsPermanent.Props.instantlyPermanentLabel.NullOrEmpty())
					{
						return hediffComp_GetsPermanent.Props.instantlyPermanentLabel;
					}
					if (!hediffComp_GetsPermanent.Props.permanentLabel.NullOrEmpty())
					{
						return hediffComp_GetsPermanent.Props.permanentLabel;
					}
				}
				return base.LabelBase;
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06001387 RID: 4999 RVA: 0x0006FB38 File Offset: 0x0006DD38
		public override string LabelInBrackets
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.LabelInBrackets);
				if (this.sourceHediffDef != null)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(this.sourceHediffDef.label);
				}
				else if (this.source != null)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(this.source.label);
					if (this.sourceBodyPartGroup != null)
					{
						stringBuilder.Append(" ");
						stringBuilder.Append(this.sourceBodyPartGroup.LabelShort);
					}
				}
				HediffComp_GetsPermanent hediffComp_GetsPermanent = this.TryGetComp<HediffComp_GetsPermanent>();
				if (hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent && hediffComp_GetsPermanent.PainCategory != PainCategory.Painless)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(("PainCategory_" + hediffComp_GetsPermanent.PainCategory.ToString()).Translate());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06001388 RID: 5000 RVA: 0x0006FC3F File Offset: 0x0006DE3F
		public override Color LabelColor
		{
			get
			{
				if (this.IsPermanent())
				{
					return Hediff_Injury.PermanentInjuryColor;
				}
				return Color.white;
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001389 RID: 5001 RVA: 0x0006FC54 File Offset: 0x0006DE54
		public override string SeverityLabel
		{
			get
			{
				if (this.Severity == 0f)
				{
					return null;
				}
				return this.Severity.ToString("F1");
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x0600138A RID: 5002 RVA: 0x0006FC83 File Offset: 0x0006DE83
		public override float SummaryHealthPercentImpact
		{
			get
			{
				if (this.IsPermanent() || !this.Visible)
				{
					return 0f;
				}
				return this.Severity / (75f * this.pawn.HealthScale);
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x0600138B RID: 5003 RVA: 0x0006FCB4 File Offset: 0x0006DEB4
		public override float PainOffset
		{
			get
			{
				if (this.pawn.Dead || this.pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(base.Part) || this.causesNoPain)
				{
					return 0f;
				}
				HediffComp_GetsPermanent hediffComp_GetsPermanent = this.TryGetComp<HediffComp_GetsPermanent>();
				float num;
				if (hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent)
				{
					num = this.Severity * this.def.injuryProps.averagePainPerSeverityPermanent * hediffComp_GetsPermanent.PainFactor;
				}
				else
				{
					num = this.Severity * this.def.injuryProps.painPerSeverity;
				}
				return num / this.pawn.HealthScale;
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x0600138C RID: 5004 RVA: 0x0006FD54 File Offset: 0x0006DF54
		public override float BleedRate
		{
			get
			{
				if (this.pawn.Dead)
				{
					return 0f;
				}
				if (this.BleedingStoppedDueToAge)
				{
					return 0f;
				}
				if (base.Part.def.IsSolid(base.Part, this.pawn.health.hediffSet.hediffs) || this.IsTended() || this.IsPermanent())
				{
					return 0f;
				}
				if (this.pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(base.Part))
				{
					return 0f;
				}
				float num = this.Severity * this.def.injuryProps.bleedRate;
				if (base.Part != null)
				{
					num *= base.Part.def.bleedRate;
				}
				return num;
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x0600138D RID: 5005 RVA: 0x0006FE1C File Offset: 0x0006E01C
		private int AgeTicksToStopBleeding
		{
			get
			{
				int num = 90000;
				float t = Mathf.Clamp(Mathf.InverseLerp(1f, 30f, this.Severity), 0f, 1f);
				return num + Mathf.RoundToInt(Mathf.Lerp(0f, 90000f, t));
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x0600138E RID: 5006 RVA: 0x0006FE69 File Offset: 0x0006E069
		private bool BleedingStoppedDueToAge
		{
			get
			{
				return this.ageTicks >= this.AgeTicksToStopBleeding;
			}
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x0006FE7C File Offset: 0x0006E07C
		public override void Tick()
		{
			bool bleedingStoppedDueToAge = this.BleedingStoppedDueToAge;
			base.Tick();
			bool bleedingStoppedDueToAge2 = this.BleedingStoppedDueToAge;
			if (bleedingStoppedDueToAge != bleedingStoppedDueToAge2)
			{
				this.pawn.health.Notify_HediffChanged(this);
			}
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x0006FEB0 File Offset: 0x0006E0B0
		public override void Heal(float amount)
		{
			this.Severity -= amount;
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostInjuryHeal(amount);
				}
			}
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x0006FF0C File Offset: 0x0006E10C
		public override bool TryMergeWith(Hediff other)
		{
			Hediff_Injury hediff_Injury = other as Hediff_Injury;
			return hediff_Injury != null && hediff_Injury.def == this.def && hediff_Injury.Part == base.Part && !hediff_Injury.IsTended() && !hediff_Injury.IsPermanent() && !this.IsTended() && !this.IsPermanent() && this.def.injuryProps.canMerge && base.TryMergeWith(other);
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x0006FF7C File Offset: 0x0006E17C
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (base.Part != null && base.Part.coverageAbs <= 0f && (dinfo == null || dinfo.Value.Def != DamageDefOf.SurgicalCut))
			{
				Log.Error(string.Concat(new object[]
				{
					"Added injury to ",
					base.Part.def,
					" but it should be impossible to hit it. pawn=",
					this.pawn.ToStringSafe<Pawn>(),
					" dinfo=",
					dinfo.ToStringSafe<DamageInfo?>()
				}));
			}
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x00070016 File Offset: 0x0006E216
		public override void PostRemoved()
		{
			base.PostRemoved();
			this.pawn.Drawer.renderer.WoundOverlays.ClearCache();
			PortraitsCache.SetDirty(this.pawn);
			GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x0007004F File Offset: 0x0006E24F
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error("Hediff_Injury has null part after loading.");
				this.pawn.health.hediffSet.hediffs.Remove(this);
				return;
			}
		}

		// Token: 0x04000E8F RID: 3727
		private static readonly Color PermanentInjuryColor = new Color(0.72f, 0.72f, 0.72f);
		**/
	}
}
