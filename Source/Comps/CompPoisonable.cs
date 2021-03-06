// RimWorld.CompReloadable
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;
namespace DI_Harmacy
{
    public class CompPoisonable : ThingComp
    {

        private int remainingCharges;
        public CompProperties_Poisonable Props => props as CompProperties_Poisonable;
        public bool enabled= true;
        public int RemainingCharges => remainingCharges;
        public PoisonProps poisonProps;
        public bool applyToStruckPart = false;
        public int MaxCharges => Props.maxCharges;

        public HediffDef hediffToApply;
        public ThingDef AmmoDef = null;// => Props.ammoDef;
        public bool hasBeenInitialized = true;
        public bool CanBeUsed => enabled&&remainingCharges > 0;

        public Pawn weilderOf => RecoatingUtility.WearerOf(this);

        public string LabelRemaining => $"{RemainingCharges} / {MaxCharges}";

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
                enabled = !DIHSettings.disabledWeapons.Contains(parent.def);
                
            }
        //Initializes raiders to poison state. THEORETICAL OPTOMIZATIon roll the poison and only get comp when it turns up as should poison.
        public void poisonRaider(bool factionWillReroll)
        {
            //due to considerable rework, this just checks if thing has been initialized
            if (!hasBeenInitialized ||!enabled)
            {
                return;
            }
            ThingDef thingDef = PoisonUIDataList.GetRandomPoisonThingDef();
            //checks if the compprops or the rolled poison is null, and if so, ends if the tech level is above neolethic, unless they approve of poison
            if (Props == null || !factionWillReroll&&thingDef == null)
            {
                hasBeenInitialized = false;
                return;
            }
            //rerolls the poison if ideology or tech level wish for more poison and it was null.
            if (thingDef == null)
            {
                
                thingDef = PoisonUIDataList.GetRandomPoisonThingDef();
                if (thingDef == null)
                {
                    hasBeenInitialized = false;
                    return;
                }
            }
            Props.ammoDef = thingDef;
            poisonProps = thingDef.GetModExtension<PoisonProps>();
            Props.maxCharges = poisonProps.maxCharges;
            remainingCharges = MaxCharges;
            hediffToApply = poisonProps.poisonInflicts;
            applyToStruckPart = poisonProps.applyToStruckPart;
            hasBeenInitialized = false;

        }

       

        //this updates what the weapon wants to be reloaded with. IT DOES NOT CHANGE CURRENT HEDIFF OR CHARGES. Least...it shouldn't
        public void updatePoisons(Pawn pawn)
        {

            CompPawnPoisonTracker compPawnPoisonTracker = pawn.GetComp<CompPawnPoisonTracker>();
            if (compPawnPoisonTracker == null)// || compPawnPoisonTracker.assignedPoison == null)
            {
                return;
            }
            ThingDef ammoToUse = compPawnPoisonTracker.assignedPoison;//pawn_InventoryStockTracker.GetDesiredThingForGroup(DIH_PoisonStockGroups.DIH_PoisonStockGroup);

            Props.ammoDef = ammoToUse;
            if (ammoToUse == null)
            {
                poisonProps = null;
                return;
            }
            poisonProps = ammoToUse.GetModExtension<PoisonProps>();
            if (poisonProps == null)
            {
                return;
            }

            AmmoDef = ammoToUse;

            Props.ammoCountPerCharge = poisonProps.ammoCountPerCharge;

        }


        public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
        {
            IEnumerable<StatDrawEntry> enumerable = base.SpecialDisplayStats();
            if (enumerable != null)
            {
                foreach (StatDrawEntry item in enumerable)
                {
                    yield return item;
                }
            }
            //previously statcatagorydefof.apparel
            if (hediffToApply != null && remainingCharges != 0)
            {
                yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "Stat_Thing_ReloadChargesRemaining_Name".Translate(Props.ChargeNounArgument), LabelRemaining, "Stat_Thing_ReloadChargesRemaining_Desc".Translate(Props.ChargeNounArgument), 2749);
                yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "Poison Inflicts", hediffToApply.LabelCap, "This weapon applies this hediff", 2750);
            }
        }

        //currently only used on melee damage. Made partly redundant by changes in handling.
        public IEnumerable<DamageInfo> applyPoison(IEnumerable<DamageInfo> damageInfos)
        {

            //hands back original list unchanged. Seemingly slightly faster than for loop...somehow
            foreach (DamageInfo dInfo in damageInfos)
            {
                yield return dInfo;
            }
            DamageInfo copyFrom = damageInfos.ElementAt(0);//damageInfos.ElementAt(Rand.Range(0, damageInfos.Count()));//damageInfos.RandomElement<DamageInfo>();
            DamageInfo poisonDamageInfo = new DamageInfo(DIH_DamageDefs.DIH_PoisonDamageBase, copyFrom.Amount, instigator: copyFrom.Instigator);
            yield return poisonDamageInfo;

        }

        public override void PostExposeData()
        {
            if(!enabled)
            { return; }
            base.PostExposeData();
            Scribe_Values.Look(ref remainingCharges, "remainingCharges", -999);
            Scribe_Defs.Look(ref hediffToApply, "HediffToApply");
            Scribe_Values.Look(ref applyToStruckPart, "applyToStruckPart", false);
            Scribe_Values.Look(ref hasBeenInitialized, "poisonRaider", true);
            if (Scribe.mode == LoadSaveMode.PostLoadInit && remainingCharges == -999)
            {
                remainingCharges = MaxCharges;
            }

        }

        public bool NeedsReload(bool allowForcedReload)
        {
            if (!enabled||AmmoDef == null)
            {
                return false;
            }
            if (hediffToApply != AmmoDef.GetModExtension<PoisonProps>().poisonInflicts)
            {
                return true;
            }
            if (Props.ammoCountToRefill != 0)
            {
                if (!allowForcedReload)
                {
                    return remainingCharges == 0;
                }

                return RemainingCharges != MaxCharges;
            }
            return RemainingCharges != MaxCharges;
        }

        public void ReloadFrom(Thing ammo)
        {
            if (!NeedsReload(allowForcedReload: true))
            {
                return;
            }

            HediffDef poisonInflicts = AmmoDef.GetModExtension<PoisonProps>().poisonInflicts;
            if (hediffToApply != poisonInflicts)
            {
                hediffToApply = poisonInflicts;
                remainingCharges = 0;
                applyToStruckPart = poisonProps.applyToStruckPart;
                Props.maxCharges = poisonProps.maxCharges;
            }
            if (Props.ammoCountToRefill != 0)
            {

                if (ammo.stackCount < Props.ammoCountToRefill)
                {
                    return;
                }
                ammo.SplitOff(Props.ammoCountToRefill).Destroy();
                remainingCharges = MaxCharges;
            }
            else
            {
                if (ammo.stackCount < Props.ammoCountPerCharge)
                {
                    return;
                }
                int num = Mathf.Clamp(ammo.stackCount / Props.ammoCountPerCharge, 0, MaxCharges - RemainingCharges);
                ammo.SplitOff(num * Props.ammoCountPerCharge).Destroy();
                remainingCharges += num;
            }
            if (Props.soundReload != null)
            {
                Props.soundReload.PlayOneShot(new TargetInfo(weilderOf.Position, weilderOf.Map));
            }
        }

        public int MinAmmoNeeded(bool allowForcedReload)
        {
            if (!NeedsReload(allowForcedReload))
            {
                return 0;
            }
            if (poisonProps == null)
                return 0;
            if (poisonProps.poisonInflicts != hediffToApply)
            {
                return poisonProps.ammoCountPerCharge;
            }
            if (Props.ammoCountToRefill != 0)
            {
                return Props.ammoCountToRefill;
            }
            return Props.ammoCountPerCharge;
        }

        public int MaxAmmoNeeded(bool allowForcedReload)
        {
            if (!NeedsReload(allowForcedReload))
            {
                return 0;
            }
            if (poisonProps.poisonInflicts != hediffToApply)
            {
                return poisonProps.ammoCountPerCharge * poisonProps.maxCharges;
            }
            if (Props.ammoCountToRefill != 0)
            {
                return Props.ammoCountToRefill;
            }
            return Props.ammoCountPerCharge * (MaxCharges - RemainingCharges);
        }

        public int MaxAmmoAmount()
        {
            if (AmmoDef == null)
            {
                return 0;
            }
            if (poisonProps == null)
                return 0;
            if (hediffToApply != poisonProps.poisonInflicts)
            {
                return poisonProps.ammoCountPerCharge * poisonProps.maxCharges;
            }
            if (Props.ammoCountToRefill == 0)
            {
                return Props.ammoCountPerCharge * MaxCharges;
            }
            return Props.ammoCountToRefill;
        }

        public void UsedOnce()
        {
            if (remainingCharges > 0)
            {
                remainingCharges--;
            }
        }
    }
}