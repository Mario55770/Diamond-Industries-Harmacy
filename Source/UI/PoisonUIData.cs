using System.Collections.Generic;
using Verse;
namespace DI_Harmacy
{
    public class PoisonUIData
    {
        public ThingDef thingDef;
        public string optionLabel;
        public PoisonUIData(ThingDef thing, string label)
        {
            this.thingDef = thing;
            this.optionLabel = label;
        }

    }
    [StaticConstructorOnStartup]
    public static class PoisonUIDataList
    {
        public static List<PoisonUIData> poisonUIDataList = new List<PoisonUIData>();
        static PoisonUIDataList()
        {
            PoisonUIData p = new PoisonUIData(null, "Use No Poisons");
            poisonUIDataList.Add(p);
            foreach (ThingDef thingdef in DefDatabase<ThingDef>.AllDefsListForReading)
            {
                PoisonProps poisonProp = thingdef.GetModExtension<PoisonProps>();
                if (poisonProp != null)
                {
                    if (poisonProp.assignMenuLabel != null)
                    {
                        p = new PoisonUIData(thingdef, poisonProp.assignMenuLabel);
                    }
                    else
                    {
                        p = new PoisonUIData(thingdef, "Use " + thingdef.label);
                    }
                    poisonUIDataList.Add(p);
                }
            }

        }
    }
}