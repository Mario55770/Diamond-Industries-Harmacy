using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace DI_Harmacy
{
    public static class DIH_PoisonStockGroups
    {
        public static InventoryStockGroupDef DIH_PoisonStockGroup= (InventoryStockGroupDef)GenDefDatabase.GetDef(typeof(InventoryStockGroupDef), "DIH_PoisonStockGroup");
    }
}
