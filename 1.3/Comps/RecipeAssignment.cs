using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace DI_Harmacy
{
    [StaticConstructorOnStartup]
    public static class RecipeAssignment
    {

        static RecipeAssignment()
        {

            ThingDef recipeSource = DefDatabase<ThingDef>.GetNamed("DrugLab");

            ThingDef applyRecipesTo = DefDatabase<ThingDef>.GetNamed("DIH_BulkDrugLab");



            //applied recipes
            int a = 0;
            //total detected recipes
            int t = 0;
            List<RecipeDef> nullRecipes = new List<RecipeDef>();
            foreach (RecipeDef recipeToCopy in recipeSource.AllRecipes)
            {
                //Log.Message(recipeToCopy.defName);

                RecipeDef clonedRecipe = recipeCloneAndParse(recipeToCopy);

                if (clonedRecipe != null)
                {
                    a += 1;
                    applyRecipesTo.AllRecipes.Add(clonedRecipe);
                    applyRecipesTo.recipes.Add(clonedRecipe);
                   
                }
                else
                {
                    nullRecipes.Add(recipeToCopy);
                }
                //known to work but not usful for my application.applyRecipesTo.AllRecipes.AddRange(recipeSource.AllRecipes);
                t++;
            }
            if (a != t)
            {
                Log.Message("Created " + a + " bulk recipes for DIH Bulk Drug bench out of a detected possible " + t + ". If some bulk recipes from the default drug bench seem to be missing, try chaning load order" +
                    "if this message appears, it is not inherently an issue. We detect but cannot create a bulk version of some recipes in this version of the mod." +
                    "For instance, we detect 13 and create 6 with vanilla.");
            }
        }
        private static RecipeDef recipeCloneAndParse(RecipeDef recipeToCopy)
        {
           
            RecipeDef clonedandparsed = new RecipeDef();
          
            //if statement and try catch try to copy the work amount if it is initialized, in two ways. If neither work, its marked as null and the method stops
            float newWorkAmount = recipeToCopy.workAmount+0;
            if (newWorkAmount == -1)
            {
                try
                {
                    newWorkAmount = recipeToCopy.WorkAmountTotal(recipeToCopy.ProducedThingDef);
                    Log.Message("Managed to get work amount somewhat unexpectedly. This is a pleasant suprise, and one the mod dev would probably like to know about" +
                        " as thus far he has not yet had this code execute. This is not an issue as it means that a feature is working better than expected. "+newWorkAmount);
                }
                catch
                {
                    //Log.Message("Recipe work amount not yet accessible. Ignoring it to avoid errors");
                    return null;
                }

            }
            //as if it does not return null, this should run, outside if to prevent redundancy
           //Log.Message(newWorkAmount.ToString());
            newWorkAmount *= 4;
            
            clonedandparsed.workAmount = newWorkAmount;
            //run first time. Elsewies dont
            string newName = "";
            
                clonedandparsed.adjustedCount = recipeToCopy.adjustedCount * 4;
                clonedandparsed.defName = "DIH_BulkRecipes_" + recipeToCopy.defName + "*4";
                //duplicates the values in recipe to copy, multiplies it by four, and then adds it to the cloned and parsed recipe list.
                foreach (var item in recipeToCopy.ingredients)
                {
                    //for some ungodly reason, I have to declare it this way, instead of just getting that info in setbase. 
                    //sets the ingredient usage to be four times as expensive
                    float newBase = item.GetBaseCount();
                IngredientCount tempItem = new IngredientCount();
                tempItem.filter = item.filter;
                newBase = newBase * 4;
                tempItem.SetBaseCount(newBase);
                //Log.Message(item.ToString());
                //Log.Message(tempItem.ToString());
                //Log.Message(newBase+"");
                //Log.Message(tempItem.Equals(item).ToString());

                clonedandparsed.ingredients.Add(tempItem);
                }
                //handles products by multiplying them by four andd copying the last item for a name.
                //This works by creating a new item from scratch. 
                foreach (var item in recipeToCopy.products)
                {
                ThingDefCount prod = new ThingDefCount(item.thingDef,item.count*4);
                //Log.Message(item.count+"");
                //item.count = item.count * 4;

                newName = prod.ThingDef.label+" x"+prod.Count;
                clonedandparsed.products.Add(prod);

                }
            
            
            clonedandparsed.allowMixingIngredients = recipeToCopy.allowMixingIngredients;
            clonedandparsed.addsHediff = recipeToCopy.addsHediff;
            clonedandparsed.anesthetize = recipeToCopy.anesthetize;
            clonedandparsed.appliedOnFixedBodyPartGroups = recipeToCopy.appliedOnFixedBodyPartGroups;
            clonedandparsed.appliedOnFixedBodyParts = recipeToCopy.appliedOnFixedBodyParts;
            clonedandparsed.autoStripCorpses = recipeToCopy.autoStripCorpses;
            clonedandparsed.changesHediffLevel = recipeToCopy.changesHediffLevel;
            clonedandparsed.conceptLearned = recipeToCopy.conceptLearned;
            clonedandparsed.defaultIngredientFilter = recipeToCopy.defaultIngredientFilter;
            
            clonedandparsed.description = "Make drugs in bulk";
            clonedandparsed.effectWorking = recipeToCopy.effectWorking;
            clonedandparsed.efficiencyStat = recipeToCopy.efficiencyStat;
            clonedandparsed.fixedIngredientFilter = recipeToCopy.fixedIngredientFilter;
            clonedandparsed.forceHiddenSpecialFilters = recipeToCopy.forceHiddenSpecialFilters;
            clonedandparsed.fromIdeoBuildingPreceptOnly = recipeToCopy.fromIdeoBuildingPreceptOnly;
            
            
            
            
            clonedandparsed.interruptIfIngredientIsRotting = recipeToCopy.interruptIfIngredientIsRotting;
            //can probably change to making drugs in bulk
            clonedandparsed.jobString = recipeToCopy.jobString;
            clonedandparsed.label = recipeToCopy.label;
            clonedandparsed.memePrerequisitesAny = recipeToCopy.memePrerequisitesAny;
            clonedandparsed.minPartCount = recipeToCopy.minPartCount;
            //Unknown?
            clonedandparsed.modContentPack = recipeToCopy.modContentPack;
            //unknown?
            clonedandparsed.modExtensions = recipeToCopy.modExtensions;

            
            clonedandparsed.productHasIngredientStuff = recipeToCopy.productHasIngredientStuff;
            
            //using the last product in the list of products generate a new name
            newName = "Bulk Production of " + newName;
            clonedandparsed.label = newName;
            clonedandparsed.recipeUsers = recipeToCopy.recipeUsers;
            clonedandparsed.regenerateOnDifficultyChange = recipeToCopy.regenerateOnDifficultyChange;
            clonedandparsed.removesHediff = recipeToCopy.removesHediff;
            clonedandparsed.requiredGiverWorkType = recipeToCopy.requiredGiverWorkType;
            
            clonedandparsed.researchPrerequisite = recipeToCopy.researchPrerequisite;
            clonedandparsed.researchPrerequisites = recipeToCopy.researchPrerequisites;
            //clonedandparsed.shortHash
            clonedandparsed.skillRequirements = recipeToCopy.skillRequirements;
            clonedandparsed.soundWorking = recipeToCopy.soundWorking;
            clonedandparsed.specialProducts = recipeToCopy.specialProducts;
            clonedandparsed.targetCountAdjustment = recipeToCopy.targetCountAdjustment;
            clonedandparsed.unfinishedThingDef = recipeToCopy.unfinishedThingDef;
            clonedandparsed.useIngredientsForColor = recipeToCopy.useIngredientsForColor;

            
            
            clonedandparsed.workerClass = recipeToCopy.workerClass;
            //clonedandparsed.requiredGiverWorkType= 
            //clonedandparsed.workerClass= DefData;
            clonedandparsed.workerCounterClass = recipeToCopy.workerCounterClass;
            clonedandparsed.workSkill = recipeToCopy.workSkill;
            clonedandparsed.workSkillLearnFactor = recipeToCopy.workSkillLearnFactor;
            clonedandparsed.workSpeedStat = recipeToCopy.workSpeedStat;
            clonedandparsed.workTableEfficiencyStat = recipeToCopy.workTableEfficiencyStat;
            clonedandparsed.workTableSpeedStat = recipeToCopy.workTableSpeedStat;
            clonedandparsed.shortHash = recipeToCopy.shortHash;
            //List<RecipeDef> r = new List<RecipeDef>
            //{
            //  clonedandparsed
            //};
            //return r;
            clonedandparsed.generated = recipeToCopy.generated;
            //Log.Message(recipeToCopy.generated.ToString());
             
            return clonedandparsed;
        }
    }
}