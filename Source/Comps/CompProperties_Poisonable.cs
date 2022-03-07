using RimWorld;
using System.Collections.Generic;
using Verse;
namespace DI_Harmacy
{
    // Token: 0x020011C8 RID: 4552
    public class CompProperties_Poisonable : CompProperties
    {
        public int maxCharges = 1;

        public ThingDef ammoDef;

        public int ammoCountToRefill;

        public int ammoCountPerCharge;

        public int baseReloadTicks = 60;

        public SoundDef soundReload;

        [MustTranslate]
        public string chargeNoun = "charge";

        public NamedArgument ChargeNounArgument => chargeNoun.Named("CHARGENOUN");

        public CompProperties_Poisonable()
        {
            compClass = typeof(CompPoisonable);
        }
      
        public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
        {
            foreach (StatDrawEntry item in base.SpecialDisplayStats(req))
            {
                yield return item;
            }
            if (!req.HasThing && ammoCountPerCharge != 0)
            {
                //apparently all of these used to say apparel
                yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "Stat_Thing_ReloadMaxCharges_Name".Translate(ChargeNounArgument), maxCharges.ToString(), "Stat_Thing_ReloadMaxCharges_Desc".Translate(ChargeNounArgument), 2749);
            }
            if (ammoDef != null && ammoCountPerCharge != 0)
            {
                if (ammoCountToRefill != 0)
                {
                    yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "Stat_Thing_ReloadRefill_Name".Translate(ChargeNounArgument), $"{ammoCountToRefill} {ammoDef.label}", "Stat_Thing_ReloadRefill_Desc".Translate(ChargeNounArgument), 2749);
                }
                else
                {
                    yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "Stat_Thing_ReloadPerCharge_Name".Translate(ChargeNounArgument), $"{ammoCountPerCharge} {ammoDef.label}", "Stat_Thing_ReloadPerCharge_Desc".Translate(ChargeNounArgument), 2749);
                }
            }

        }
    }
}