﻿using System;
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
            base.PostExposeData();
            Scribe_Values.Look(ref pawn_InventoryStockTracker, "pawn_InventoryStockTracker", null);
            
        }
        public override void CompTick()
        {
            //Log.Error("Let's error on every tick!");
        }
        public Pawn_InventoryStockTracker pawn_InventoryStockTracker;
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

