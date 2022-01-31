using System;
using System.Collections.Generic;
using Verse;
using RimWorld;
namespace DI_Harmacy
{
	// Token: 0x020011C8 RID: 4552
	public class CompProperties_Poison : CompProperties
	{
		// Token: 0x17001325 RID: 4901
		// (get) Token: 0x06006EA3 RID: 28323 RVA: 0x00256342 File Offset: 0x00254542
		public NamedArgument ChargeNounArgument
		{
			get
			{
				return this.chargeNoun.Named("CHARGENOUN");
			}
		}

		// Token: 0x06006EA4 RID: 28324 RVA: 0x00256354 File Offset: 0x00254554
		public CompProperties_Poison()
		{
			this.compClass = typeof(CompPoisonable);
		}

		// Token: 0x06006EA5 RID: 28325 RVA: 0x00256394 File Offset: 0x00254594
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.ammoDef != null && this.ammoCountToRefill == 0 && this.ammoCountPerCharge == 0)
			{
				yield return "Reloadable component has ammoDef but one of ammoCountToRefill or ammoCountPerCharge must be set";
			}
			if (this.ammoCountToRefill != 0 && this.ammoCountPerCharge != 0)
			{
				yield return "Reloadable component: specify only one of ammoCountToRefill and ammoCountPerCharge";
			}
			yield break;
			//yield break;
		}

		// Token: 0x06006EA6 RID: 28326 RVA: 0x002563AB File Offset: 0x002545AB
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (!req.HasThing)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_ReloadMaxCharges_Name".Translate(this.ChargeNounArgument), this.maxCharges.ToString(), "Stat_Thing_ReloadMaxCharges_Desc".Translate(this.ChargeNounArgument), 2749, null, null, false);
			}
			if (this.ammoDef != null)
			{
				if (this.ammoCountToRefill != 0)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_ReloadRefill_Name".Translate(this.ChargeNounArgument), string.Format("{0} {1}", this.ammoCountToRefill, this.ammoDef.label), "Stat_Thing_ReloadRefill_Desc".Translate(this.ChargeNounArgument), 2749, null, null, false);
				}
				else
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_ReloadPerCharge_Name".Translate(this.ChargeNounArgument), string.Format("{0} {1}", this.ammoCountPerCharge, this.ammoDef.label), "Stat_Thing_ReloadPerCharge_Desc".Translate(this.ChargeNounArgument), 2749, null, null, false);
				}
			}
			if (this.destroyOnEmpty)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_ReloadDestroyOnEmpty_Name".Translate(this.ChargeNounArgument), "Yes".Translate(), "Stat_Thing_ReloadDestroyOnEmpty_Desc".Translate(this.ChargeNounArgument), 2749, null, null, false);
			}
			yield break;
			//yield break;
		}
		public string PoisonGizmoLabel
		{
			get
			{
				if (this.poisonGizmoLabel.NullOrEmpty())
				{
					return "Fuel".TranslateSimple();
				}
				return this.poisonGizmoLabel;
			}
		}
		public string poisonGizmoLabel;
		// Token: 0x04003D5A RID: 15706
		public int maxCharges = 1;

		// Token: 0x04003D5B RID: 15707
		public ThingDef ammoDef;

		// Token: 0x04003D5C RID: 15708
		public int ammoCountToRefill;

		// Token: 0x04003D5D RID: 15709
		public int ammoCountPerCharge;

		// Token: 0x04003D5E RID: 15710
		public bool destroyOnEmpty;

		// Token: 0x04003D5F RID: 15711
		public int baseReloadTicks = 60;

		// Token: 0x04003D60 RID: 15712
		public bool displayGizmoWhileUndrafted = true;

		// Token: 0x04003D61 RID: 15713
		public bool displayGizmoWhileDrafted = true;

		// Token: 0x04003D62 RID: 15714
		public KeyBindingDef hotKey;

		// Token: 0x04003D63 RID: 15715
		public SoundDef soundReload;

		// Token: 0x04003D64 RID: 15716
		[MustTranslate]
		public string chargeNoun = "charge";
	}
}
