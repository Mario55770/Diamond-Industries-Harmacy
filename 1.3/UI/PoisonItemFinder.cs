using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
namespace DI_Harmacy
{
    public class PoisonItemFinder
    {
        public static List<ThingDef> poisonItemList = null;
        public PoisonItemFinder()
        {
            IEnumerable<ThingDef> temp = fillList();
            poisonItemList=temp.ToList();
        }
        internal IEnumerable<ThingDef> fillList()
        {
            foreach (ThingDef possiblePoison in DefDatabase<ThingDef>.AllDefsListForReading)
            {       
                if(possiblePoison.HasModExtension<PoisonProps>())
                {
                    yield return possiblePoison;
                }
            }    
                yield break;
        }
        
    }
}
