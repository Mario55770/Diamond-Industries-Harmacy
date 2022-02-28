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
            /**Scribe_Collections.Look(ref allRecipes,"BulkDrugLabRecipeNames");
            if (Scribe.mode==LoadSaveMode.PostLoadInit)
            {
                foreach (string recipeName in recipeList)
                {
                    RecipeDef recipeDef = (RecipeDef)GenDefDatabase.GetDefSilentFail(typeof(RecipeDef), recipeName);
                    if (recipeDef != null)
                    {
                        allRecipes.Add(recipeDef);
                    }
                }
            }**/
        }

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

