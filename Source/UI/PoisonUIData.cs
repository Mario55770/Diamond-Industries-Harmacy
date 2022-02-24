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
    public static class PoisonUIDataList
    {
        public static List<PoisonUIData> poisonUIDataList = new List<PoisonUIData>();
        static PoisonUIDataList()
        {
            PoisonUIData p = new PoisonUIData(null, "Use No Poisons");
            poisonUIDataList.Add(p);
            p = new PoisonUIData((ThingDef)GenDefDatabase.GetDef(typeof(ThingDef), "DIH_SnakePoisonVial"), "Use Snake Venom");
            poisonUIDataList.Add(p);
            p = new PoisonUIData((ThingDef)GenDefDatabase.GetDef(typeof(ThingDef), "DIH_MidnightMurderPoisonVial"), "Use Midnight Murder");
            poisonUIDataList.Add(p);
            p = new PoisonUIData((ThingDef)GenDefDatabase.GetDef(typeof(ThingDef), "DIH_BrickKnockoutPoisonVial"), "Use Brick Knockout");
            poisonUIDataList.Add(p);

        }
       /** public static string LabelValue(ThingDef thingdef)
        {
            foreach(PoisonUIData poisonUIData in poisonUIDataList)
            {
                if(poisonUIData.thingDef == thingdef)
                {
                    return poisonUIData.optionLabel;
                }
            }
            return null;
        }
        public static PoisonUIData GetMatchingData(ThingDef thingdef)
        {
            foreach (PoisonUIData poisonUIData in poisonUIDataList)
            {
                if (poisonUIData.thingDef == thingdef)
                {
                    return poisonUIData;
                }
            }
            return null;
        }**/
    }
}