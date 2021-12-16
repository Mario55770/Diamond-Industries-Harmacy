using System.Collections.Generic;
using Verse;
using UnityEngine;

namespace DI_Harmacy
{
        public class DIHSettings: ModSettings
        {
            public static DIHSettings instance
            public bool debugLogging = false;
            //public bool enableBulkRecipes=true;

            /// <summary>
            /// The part that writes our settings to file. Note that saving is by ref.
            /// </summary>
            public override void ExposeData()
            {
               Scribe_Values.Look(ref debugLogging, "debugLogging", false);
               
                base.ExposeData();
            }
        }

        public class QEEMod : Mod
        {
            /// <summary>
            /// A mandatory constructor which resolves the reference to our settings.
            /// </summary>
            /// <param name="content"></param>
            public QEEMod(ModContentPack content) : base(content)
            {
                QEESettings.instance = base.GetSettings<QEESettings>();
            }

            /// <summary>
            /// The (optional) GUI part to set your settings.
            /// </summary>
            /// <param name="inRect">A Unity Rect with the size of the settings window.</param>
            public override void DoSettingsWindowContents(Rect inRect)
            {
                Listing_Standard listingStandard = new Listing_Standard();
                listingStandard.Begin(inRect);
                //listingStandard.SliderLabeled("QE_MaintenanceWorkThreshold".Translate(), ref QEESettings.instance.maintWorkThresholdFloat, QEESettings.instance.maintWorkThresholdFloat.ToStringPercent(), 0.00f, 1.0f, "QE_MaintenanceWorkThresholdTooltip".Translate());
                
                listingStandard.CheckboxLabeled("QE_DebugLogging".Translate(), ref QEESettings.instance.debugLogging, "QE_DebugLoggingTooltip".Translate());
                listingStandard.End();
                base.DoSettingsWindowContents(inRect);
            }

            public static void TryLog(string message)
            {
                if (QEESettings.instance.debugLogging)
                {
                    Log.Message("QEE: " + message);
                }
            }

            /// <summary>
            /// Override SettingsCategory to show up in the list of settings.
            /// Using .Translate() is optional, but does allow for localisation.
            /// </summary>
            /// <returns>The (translated) mod name.</returns>
            public override string SettingsCategory() => "Questionable Ethics Enhanced";
        }


    }
}
}
