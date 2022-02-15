// RimWorld.CompReloadable
using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;
namespace DI_Harmacy
{ 
public class CompPoisonable : ThingComp, IVerbOwner
{
	private int remainingCharges;

	private VerbTracker verbTracker;
		

		//public CompProperties_Reloadable Props => props as CompProperties_Reloadable;
		public CompProperties_Poisonable Props => props as CompProperties_Poisonable;
	public int RemainingCharges => remainingCharges;
		
		public int MaxCharges => Props.maxCharges;
		public HediffDef hediffToApply;

	public ThingDef AmmoDef => Props.ammoDef;

	public bool CanBeUsed => remainingCharges > 0;


		//public Pawn Wearer => PoisonableUtility.WearerOf(this);
		public Pawn Wearer => PoisonableUtility.WearerOf(this);
		
		public List<VerbProperties> VerbProperties => parent.def.Verbs;

	public List<Tool> Tools => parent.def.tools;

	public ImplementOwnerTypeDef ImplementOwnerTypeDef => ImplementOwnerTypeDefOf.NativeVerb;

	public Thing ConstantCaster => Wearer;

	public VerbTracker VerbTracker
	{
		get
		{
			if (verbTracker == null)
			{
				verbTracker = new VerbTracker(this);
			}
			return verbTracker;
		}
	}

	public string LabelRemaining => $"{RemainingCharges} / {MaxCharges}";

	public List<Verb> AllVerbs => VerbTracker.AllVerbs;

		internal void updatePoisons(Pawn pawn)
		{

			ThingDef ammoToUse = pawn.GetComp<CompPawnPoisonTracker>().pawn_InventoryStockTracker.GetDesiredThingForGroup(DIH_PoisonStockGroups.DIH_PoisonStockGroup);
			if (ammoToUse == null || AmmoDef == ammoToUse)
			{
				return;
			}
			PoisonProps poisonProps = ammoToUse.GetModExtension<PoisonProps>();
			if (poisonProps == null)
			{
				return;
			}
			Props.ammoDef = ammoToUse;
			Props.ammoCountPerCharge = poisonProps.ammoCountPerCharge;
			Props.maxCharges = poisonProps.ammoCountPerCharge;
			if (hediffToApply != poisonProps.poisonInflicts)
			{
				hediffToApply = poisonProps.poisonInflicts;
				remainingCharges = 0;
				return;
			}
				if(remainingCharges>MaxCharges)
            {
				remainingCharges = MaxCharges;
            }
			
		}

		public string UniqueVerbOwnerID()
	{
		return "Reloadable_" + parent.ThingID;
	}

	public bool VerbsStillUsableBy(Pawn p)
	{
		return Wearer == p;
	}

	public override void PostPostMake()
	{
		base.PostPostMake();
		remainingCharges = MaxCharges;
	}

	public override string CompInspectStringExtra()
	{
		return "ChargesRemaining".Translate(Props.ChargeNounArgument) + ": " + LabelRemaining;
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
		yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "Stat_Thing_ReloadChargesRemaining_Name".Translate(Props.ChargeNounArgument), LabelRemaining, "Stat_Thing_ReloadChargesRemaining_Desc".Translate(Props.ChargeNounArgument), 2749);
		
		}

		//currently only used on melee damage. Made partly redundant by changes in handling.
        public IEnumerable<DamageInfo> applyPoison(IEnumerable<DamageInfo> damageInfos)
        {
			//hand the original list back unchanged.
			foreach (DamageInfo dInfo in damageInfos)
			{
				yield return dInfo;
			}
			//ends method if theres no hediff to use.
			if(hediffToApply==null)
			{ yield break; }
			DamageInfo copyFrom = damageInfos.RandomElement<DamageInfo>();
			DamageInfo poisonDamageInfo = new DamageInfo(DIH_DamageDefs.DIH_PoisonDamageBase, copyFrom.Amount, instigator: copyFrom.Instigator) ;
			yield return poisonDamageInfo;
			
		}	
		
		public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref remainingCharges, "remainingCharges", -999);
			Scribe_Values.Look(ref hediffToApply, "HediffToApply", null);
			
		Scribe_Deep.Look(ref verbTracker, "verbTracker", this);
			
		if (Scribe.mode == LoadSaveMode.PostLoadInit && remainingCharges == -999)
		{
			remainingCharges = MaxCharges;
		}
	}

	public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
	{
		foreach (Gizmo item in base.CompGetWornGizmosExtra())
		{
			yield return item;
		}
		bool drafted = Wearer.Drafted;
		if ((drafted && !Props.displayGizmoWhileDrafted) || (!drafted && !Props.displayGizmoWhileUndrafted))
		{
			yield break;
		}
		ThingWithComps gear = parent;
		foreach (Verb allVerb in VerbTracker.AllVerbs)
		{
			if (allVerb.verbProps.hasStandardCommand)
			{
				yield return CreateVerbTargetCommand(gear, allVerb);
			}
		}
		if (Prefs.DevMode)
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "Debug: Reload to full";
			command_Action.action = delegate
			{
				remainingCharges = MaxCharges;
			};
			yield return command_Action;
		}
	}

	private Command_Poisonable CreateVerbTargetCommand(Thing gear, Verb verb)
	{
		Command_Poisonable command_Poisonable = new Command_Poisonable(this);
		command_Poisonable.defaultDesc = gear.def.description;
		command_Poisonable.hotKey = Props.hotKey;
		command_Poisonable.defaultLabel = verb.verbProps.label;
		command_Poisonable.verb = verb;
		if (verb.verbProps.defaultProjectile != null && verb.verbProps.commandIcon == null)
		{
			command_Poisonable.icon = verb.verbProps.defaultProjectile.uiIcon;
			command_Poisonable.iconAngle = verb.verbProps.defaultProjectile.uiIconAngle;
			command_Poisonable.iconOffset = verb.verbProps.defaultProjectile.uiIconOffset;
			command_Poisonable.overrideColor = verb.verbProps.defaultProjectile.graphicData.color;
		}
		else
		{
			command_Poisonable.icon = ((verb.UIIcon != BaseContent.BadTex) ? verb.UIIcon : gear.def.uiIcon);
			command_Poisonable.iconAngle = gear.def.uiIconAngle;
			command_Poisonable.iconOffset = gear.def.uiIconOffset;
			command_Poisonable.defaultIconColor = gear.DrawColor;
		}
		if (!Wearer.IsColonistPlayerControlled)
		{
			command_Poisonable.Disable();
		}
		else if (verb.verbProps.violent && Wearer.WorkTagIsDisabled(WorkTags.Violent))
		{
			command_Poisonable.Disable("IsIncapableOfViolenceLower".Translate(Wearer.LabelShort, Wearer).CapitalizeFirst() + ".");
		}
		else if (!CanBeUsed)
		{
			command_Poisonable.Disable(DisabledReason(MinAmmoNeeded(allowForcedReload: false), MaxAmmoNeeded(allowForcedReload: false)));
		}
		return command_Poisonable;
	}

	public string DisabledReason(int minNeeded, int maxNeeded)
	{
		if (AmmoDef == null)
		{
			return "CommandReload_NoCharges".Translate(Props.ChargeNounArgument);
		}
		return TranslatorFormattedStringExtensions.Translate(arg3: ((Props.ammoCountToRefill == 0) ? ((minNeeded == maxNeeded) ? minNeeded.ToString() : $"{minNeeded}-{maxNeeded}") : Props.ammoCountToRefill.ToString()).Named("COUNT"), key: "CommandReload_NoAmmo", arg1: Props.ChargeNounArgument, arg2: NamedArgumentUtility.Named(AmmoDef, "AMMO"));
	}

	public bool NeedsReload(bool allowForcedReload)
	{
		if (AmmoDef == null)
		{
			return false;
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
			Props.soundReload.PlayOneShot(new TargetInfo(Wearer.Position, Wearer.Map));
		}
	}

	public int MinAmmoNeeded(bool allowForcedReload)
	{
		if (!NeedsReload(allowForcedReload))
		{
			return 0;
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
		if (Props.destroyOnEmpty && remainingCharges == 0 && !parent.Destroyed)
		{
			parent.Destroy();
		}
	}
}
}