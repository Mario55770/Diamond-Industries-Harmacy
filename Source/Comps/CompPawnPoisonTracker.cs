using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
namespace DI_Harmacy
{
    public class CompPawnPoisonTracker : ThingComp
    {
        public override void PostExposeData()
        {
            //Saves the poison that the pawn is currently assigned.
            Scribe_Defs.Look(ref assignedPoison, "assignedPoison");  
        }
        public override void CompTick()
        {
            
        }
        //the poison currently equipped. Initialized to null
        public ThingDef assignedPoison=null;
        public CompPropertiesPawnPoisonTracker Props => (CompPropertiesPawnPoisonTracker)this.props;

        }

    public class CompPropertiesPawnPoisonTracker : CompProperties
    {
        
        public CompPropertiesPawnPoisonTracker()
        {
            this.compClass = typeof(CompPawnPoisonTracker);
        }

        public CompPropertiesPawnPoisonTracker(Type compClass) : base(compClass)
        {
            this.compClass = compClass;
        }
    }
}

