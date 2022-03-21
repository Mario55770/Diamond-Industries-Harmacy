using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace DI_Harmacy
{
    public class DIHSettings : ModSettings
    {
        public static DIHSettings instance;
        public bool debugLogging = false;
        public float poisonMultiplier = 1;
        public static List<ThingDef> disabledWeapons;
        public static void initializeDisabledWeaponsList()
        {
            //Log.Error("This ran");
            disabledWeapons = new List<ThingDef>();
            //Log.Error(DefDatabase<ThingDef>.AllDefsListForReading.Count.ToString());
            foreach (ThingDef weapon in DefDatabase<ThingDef>.AllDefsListForReading)
            {
                if (weapon.IsWeapon &&weapon.weaponTags!=null)
                {
                    
                    List<string> weaponTags = weapon.weaponTags;
                    if (weaponTags!=null &&weaponTags.Contains("GrenadeEMP") || weaponTags.Contains("GrenadeDestructive") || weaponTags.Contains("EmpireGrenadeDestructive"))
                    {
                            disabledWeapons.Add(weapon);
                            //Log.Message("Disabled somethign");
                   }
                }
            }
        }
        public override void ExposeData()
        {
            DIHSettings.initializeDisabledWeaponsList();

            Scribe_Values.Look(ref debugLogging, "debugLogging", false);
            
            base.ExposeData();
        }
    }

    public class DIHMod : Mod
    {
        /// <summary>
        /// A mandatory constructor which resolves the reference to our settings.
        /// </summary>
        /// <param name="content"></param>
        public DIHMod(ModContentPack content) : base(content)
        {
            DIHSettings.instance = base.GetSettings<DIHSettings>();
           
        }

        /// <summary>
        /// The (optional) GUI part to set your settings.
        /// </summary>
        /// <param name="inRect">A Unity Rect with the size of the settings window.</param>
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label(label: "Poison Multiplier " + DIHSettings.instance.poisonMultiplier.ToString(), tooltip: "This is how much further poison should be affected by. Default value of 1. It is still multiplied by toxic sensitivity, a handful of other internal values, and for melee weapons, the strength of the hit");
            DIHSettings.instance.poisonMultiplier = Mathf.Round(listingStandard.Slider(100f * DIHSettings.instance.poisonMultiplier, 0f, 200f)) / 100f;

            //listingStandard.Slider("Poison effect multiplier", ref DIHSettings.instance.poisonMultiplier, DIHSettings.instance.poisonMultiplier.ToStringPercent(), 0.00f, 1.0f, "This is how much further poison should be affected by. Default value of 1. It is still multiplied by toxic sensitivity, a handful of other internal values, and for melee weapons, the strength of the hit");
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public static void TryLog(string message)
        {

        }

        /// <summary>
        /// Override SettingsCategory to show up in the list of settings.
        /// Using .Translate() is optional, but does allow for localisation.
        /// </summary>
        /// <returns>The (translated) mod name.</returns>
        public override string SettingsCategory() => "Diamond Industry Harmacy";
    }



}
