using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using RimWorld;
using Verse.Sound;

namespace DI_Harmacy
{
    // Token: 0x020011C5 RID: 4549
    [StaticConstructorOnStartup]
    public class CompPoisonable : ThingComp, IVerbOwner
    {
        public CompProperties_Poison Props
        {
            get
            {
                return (CompProperties_Poison)this.props;
            }
        }



        // Token: 0x17001327 RID: 4903
        // (get) Token: 0x06006EAA RID: 28330 RVA: 0x002563CF File Offset: 0x002545CF
        public int RemainingCharges
        {
            get
            {
                return this.remainingCharges;
            }
        }

        // Token: 0x17001328 RID: 4904
        // (get) Token: 0x06006EAB RID: 28331 RVA: 0x002563D7 File Offset: 0x002545D7
        public int MaxCharges
        {
            get
            {
                return this.Props.maxCharges;
            }
        }

        // Token: 0x17001329 RID: 4905
        // (get) Token: 0x06006EAC RID: 28332 RVA: 0x002563E4 File Offset: 0x002545E4
        public ThingDef AmmoDef
        {
            get
            {
                return this.Props.ammoDef;
            }
        }

        // Token: 0x1700132A RID: 4906
        // (get) Token: 0x06006EAD RID: 28333 RVA: 0x002563F1 File Offset: 0x002545F1
        public bool CanBeUsed
        {
            get
            {
                return this.remainingCharges > 0;
            }
        }

        public Pawn Weilder
        {
            get
            {
                return PoisonableUtility.WeaponHolder(this);
            }
        }
        public List<VerbProperties> VerbProperties
        {
            get
            {
                return this.parent.def.Verbs;
            }
        }

        // Token: 0x1700132D RID: 4909
        // (get) Token: 0x06006EB0 RID: 28336 RVA: 0x0009C9D9 File Offset: 0x0009ABD9
        public List<Tool> Tools
        {
            get
            {
                return this.parent.def.tools;
            }
        }

        // Token: 0x1700132E RID: 4910
        // (get) Token: 0x06006EB1 RID: 28337 RVA: 0x001AE522 File Offset: 0x001AC722
        public ImplementOwnerTypeDef ImplementOwnerTypeDef
        {
            get
            {
                return ImplementOwnerTypeDefOf.NativeVerb;
            }
        }

        // Token: 0x1700132F RID: 4911
        // (get) Token: 0x06006EB2 RID: 28338 RVA: 0x00256404 File Offset: 0x00254604
        public Thing ConstantCaster
        {
            get
            {
                return this.Weilder;
            }
        }

        // Token: 0x06006EB3 RID: 28339 RVA: 0x0025640C File Offset: 0x0025460C
        public string UniqueVerbOwnerID()
        {
            return "Reloadable_" + this.parent.ThingID;
        }

        // Token: 0x06006EB4 RID: 28340 RVA: 0x00256423 File Offset: 0x00254623
        public bool VerbsStillUsableBy(Pawn p)
        {
            return this.Weilder == p;
        }

        // Token: 0x17001330 RID: 4912
        // (get) Token: 0x06006EB5 RID: 28341 RVA: 0x0025642E File Offset: 0x0025462E
        public VerbTracker VerbTracker
        {
            get
            {
                if (this.verbTracker == null)
                {
                    this.verbTracker = new VerbTracker(this);
                }
                return this.verbTracker;
            }
        }

        // Token: 0x17001331 RID: 4913
        // (get) Token: 0x06006EB6 RID: 28342 RVA: 0x0025644A File Offset: 0x0025464A
        public string LabelRemaining
        {
            get
            {
                return string.Format("{0} / {1}", this.RemainingCharges, this.MaxCharges);
            }
        }

        // Token: 0x17001332 RID: 4914
        // (get) Token: 0x06006EB7 RID: 28343 RVA: 0x0025646C File Offset: 0x0025466C
        public List<Verb> AllVerbs
        {
            get
            {
                return this.VerbTracker.AllVerbs;
            }
        }

        // Token: 0x06006EB8 RID: 28344 RVA: 0x00256479 File Offset: 0x00254679
        public override void PostPostMake()
        {
            base.PostPostMake();
            this.remainingCharges = this.MaxCharges;
        }

        // Token: 0x06006EB9 RID: 28345 RVA: 0x0025648D File Offset: 0x0025468D
        public override string CompInspectStringExtra()
        {
            return "ChargesRemaining".Translate(this.Props.ChargeNounArgument) + ": " + this.LabelRemaining;
        }

        // Token: 0x06006EBA RID: 28346 RVA: 0x002564BE File Offset: 0x002546BE
        public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
        {
            IEnumerable<StatDrawEntry> enumerable = base.SpecialDisplayStats();
            if (enumerable != null)
            {
                foreach (StatDrawEntry statDrawEntry in enumerable)
                {
                    yield return statDrawEntry;
                }
                IEnumerator<StatDrawEntry> enumerator = null;
            }
            yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_ReloadChargesRemaining_Name".Translate(this.Props.ChargeNounArgument), this.LabelRemaining, "Stat_Thing_ReloadChargesRemaining_Desc".Translate(this.Props.ChargeNounArgument), 2749, null, null, false);
            //these two were here but detected as never reached?
            //yield break;
            //yield break;
        }

        // Token: 0x06006EBB RID: 28347 RVA: 0x002564D0 File Offset: 0x002546D0
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<int>(ref this.remainingCharges, "remainingCharges", -999, false);
            Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
            {
                this
            });
            if (Scribe.mode == LoadSaveMode.PostLoadInit && this.remainingCharges == -999)
            {
                this.remainingCharges = this.MaxCharges;
            }
        }

        //        public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
        public override void CompTick()
        {
            UsedOnce();
            CompGetGizmosExtra();
             
        }
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
           // Log.Message("TEST");
            if (Find.Selector.SingleSelectedThing == this.parent)
            {
                yield return new GizmoPoisonAmmoStatus
                {
                    poisonable = this
                };

            }

           /** Log.Message("Should print if this ever runs exist");
            foreach (Gizmo gizmo in base.CompGetWornGizmosExtra())
            {

                yield return gizmo;
            }
            IEnumerator<Gizmo> enumerator = null;
            bool drafted = this.Weilder.Drafted;
            if ((drafted && !this.Props.displayGizmoWhileDrafted) || (!drafted && !this.Props.displayGizmoWhileUndrafted))
            {
                yield break;
            }
            ThingWithComps gear = this.parent;
            foreach (Verb verb in this.VerbTracker.AllVerbs)
            {
                if (verb.verbProps.hasStandardCommand)
                {
                    yield return this.CreateVerbTargetCommand(gear, verb);
                }
            }
            List<Verb>.Enumerator enumerator2 = default(List<Verb>.Enumerator);
            if (Prefs.DevMode)
            {
                yield return new Command_Action
                {
                    defaultLabel = "Debug: Reload to full",
                    action = delegate ()
                    {
                        this.remainingCharges = this.MaxCharges;
                    }
                };
            }
            yield break;
            // yield break;
           **/
        }

        // Token: 0x06006EBD RID: 28349 RVA: 0x00256544 File Offset: 0x00254744
        private Command_Poisonable CreateVerbTargetCommand(Thing gear, Verb verb)
        {
            Command_Poisonable command_poisonable = new Command_Poisonable(this);
            command_poisonable.defaultDesc = gear.def.description;
            command_poisonable.hotKey = this.Props.hotKey;
            command_poisonable.defaultLabel = verb.verbProps.label;
            command_poisonable.verb = verb;
            if (verb.verbProps.defaultProjectile != null && verb.verbProps.commandIcon == null)
            {
                command_poisonable.icon = verb.verbProps.defaultProjectile.uiIcon;
                command_poisonable.iconAngle = verb.verbProps.defaultProjectile.uiIconAngle;
                command_poisonable.iconOffset = verb.verbProps.defaultProjectile.uiIconOffset;
                command_poisonable.overrideColor = new Color?(verb.verbProps.defaultProjectile.graphicData.color);
            }
            else
            {
                command_poisonable.icon = ((verb.UIIcon != BaseContent.BadTex) ? verb.UIIcon : gear.def.uiIcon);
                command_poisonable.iconAngle = gear.def.uiIconAngle;
                command_poisonable.iconOffset = gear.def.uiIconOffset;
                command_poisonable.defaultIconColor = gear.DrawColor;
            }
            if (!this.Weilder.IsColonistPlayerControlled)
            {
                command_poisonable.Disable(null);
            }
            else if (verb.verbProps.violent && this.Weilder.WorkTagIsDisabled(WorkTags.Violent))
            {
                command_poisonable.Disable("IsIncapableOfViolenceLower".Translate(this.Weilder.LabelShort, this.Weilder).CapitalizeFirst() + ".");
            }
            else if (!this.CanBeUsed)
            {
                command_poisonable.Disable(this.DisabledReason(this.MinAmmoNeeded(false), this.MaxAmmoNeeded(false)));
            }
            return command_poisonable;
        }

        // Token: 0x06006EBE RID: 28350 RVA: 0x00256704 File Offset: 0x00254904
        public string DisabledReason(int minNeeded, int maxNeeded)
        {
            string result;
            if (this.AmmoDef == null)
            {
                result = "CommandReload_NoCharges".Translate(this.Props.ChargeNounArgument);
            }
            else
            {
                string arg;
                if (this.Props.ammoCountToRefill != 0)
                {
                    arg = this.Props.ammoCountToRefill.ToString();
                }
                else
                {
                    arg = ((minNeeded == maxNeeded) ? minNeeded.ToString() : string.Format("{0}-{1}", minNeeded, maxNeeded));
                }
                result = "CommandReload_NoAmmo".Translate(this.Props.ChargeNounArgument, this.AmmoDef.Named("AMMO"), arg.Named("COUNT"));
            }
            return result;
        }

        // Token: 0x06006EBF RID: 28351 RVA: 0x002567B4 File Offset: 0x002549B4
        public bool NeedsReload(bool allowForcedReload)
        {
            if (this.AmmoDef == null)
            {
                return false;
            }
            if (this.Props.ammoCountToRefill == 0)
            {
                return this.RemainingCharges != this.MaxCharges;
            }
            if (!allowForcedReload)
            {
                return this.remainingCharges == 0;
            }
            return this.RemainingCharges != this.MaxCharges;
        }

        // Token: 0x06006EC0 RID: 28352 RVA: 0x00256808 File Offset: 0x00254A08
        public void ReloadFrom(Thing ammo)
        {
            if (!this.NeedsReload(true))
            {
                return;
            }
            if (this.Props.ammoCountToRefill != 0)
            {
                if (ammo.stackCount < this.Props.ammoCountToRefill)
                {
                    return;
                }
                ammo.SplitOff(this.Props.ammoCountToRefill).Destroy(DestroyMode.Vanish);
                this.remainingCharges = this.MaxCharges;
            }
            else
            {
                if (ammo.stackCount < this.Props.ammoCountPerCharge)
                {
                    return;
                }
                int num = Mathf.Clamp(ammo.stackCount / this.Props.ammoCountPerCharge, 0, this.MaxCharges - this.RemainingCharges);
                ammo.SplitOff(num * this.Props.ammoCountPerCharge).Destroy(DestroyMode.Vanish);
                this.remainingCharges += num;
            }
            if (this.Props.soundReload != null)
            {
                this.Props.soundReload.PlayOneShot(new TargetInfo(this.Weilder.Position, this.Weilder.Map, false));
            }
        }

        // Token: 0x06006EC1 RID: 28353 RVA: 0x00256904 File Offset: 0x00254B04
        public int MinAmmoNeeded(bool allowForcedReload)
        {
            if (!this.NeedsReload(allowForcedReload))
            {
                return 0;
            }
            if (this.Props.ammoCountToRefill != 0)
            {
                return this.Props.ammoCountToRefill;
            }
            return this.Props.ammoCountPerCharge;
        }

        // Token: 0x06006EC2 RID: 28354 RVA: 0x00256935 File Offset: 0x00254B35
        public int MaxAmmoNeeded(bool allowForcedReload)
        {
            if (!this.NeedsReload(allowForcedReload))
            {
                return 0;
            }
            if (this.Props.ammoCountToRefill != 0)
            {
                return this.Props.ammoCountToRefill;
            }
            return this.Props.ammoCountPerCharge * (this.MaxCharges - this.RemainingCharges);
        }

        // Token: 0x06006EC3 RID: 28355 RVA: 0x00256974 File Offset: 0x00254B74
        public int MaxAmmoAmount()
        {
            if (this.AmmoDef == null)
            {
                return 0;
            }
            if (this.Props.ammoCountToRefill == 0)
            {
                return this.Props.ammoCountPerCharge * this.MaxCharges;
            }
            return this.Props.ammoCountToRefill;
        }

        // Token: 0x06006EC4 RID: 28356 RVA: 0x002569AC File Offset: 0x00254BAC
        public void UsedOnce()
        {
            if (this.remainingCharges > 0)
            {
                this.remainingCharges--;
            }

        }

        // Token: 0x04003D65 RID: 15717
        private int remainingCharges;

        // Token: 0x04003D66 RID: 15718
        private VerbTracker verbTracker;
    }
}

