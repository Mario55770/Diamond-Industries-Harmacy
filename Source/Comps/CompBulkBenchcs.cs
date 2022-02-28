using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
namespace DI_Harmacy
{
    public class CompBulkBench : ThingComp
    {
        public override void PostExposeData()
        {
            //Saves the poison that the pawn is currently assigned.
            Building_WorkTable workTable= (this.ParentHolder as Building_WorkTable);

            List<RecipeDef> allRecipes = workTable.def.AllRecipes;
            String[] recipeList=new string[allRecipes.Count];
            foreach(RecipeDef recipeDef in allRecipes)
            {
                Log.Message(recipeDef.ToString());
            }
            Scribe_Collections.Look(ref recipeList);
        }

        public ThingDef assignedPoison = null;
        public CompPropertiesBulkBench Props => (CompPropertiesBulkBench)this.props;

    }

    public class CompPropertiesBulkBench : CompProperties
    {

        public CompPropertiesBulkBench()
        {
            this.compClass = typeof(CompBulkBench);
        }

        public CompPropertiesBulkBench(Type compClass) : base(compClass)
        {
            this.compClass = compClass;
        }
    }
}

