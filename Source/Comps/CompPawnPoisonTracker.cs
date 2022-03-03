using System;
using System.Collections.Generic;
using Verse;
namespace DI_Harmacy
{
    public class CompPawnPoisonTracker : ThingComp
    {
        public override void PostExposeData()
        {
            //Saves the poison that the pawn is currently assigned.
            Scribe_Defs.Look(ref assignedPoison, "assignedPoison");
            Scribe_Values.Look(ref applyPoisonActive, "ShouldUsePoison");

        }

        public ThingDef assignedPoison = null;
        public CompPropertiesPawnPoisonTracker Props => (CompPropertiesPawnPoisonTracker)this.props;
        public bool applyPoisonActive = true;
        //adds gizmo to panel to toggle pawn if they have an assigned poison
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }
            Pawn pawn = parent as Pawn;
            if (assignedPoison != null)//pawn.IsColonistPlayerControlled && pawn.WorkTagIsDisabled(WorkTags.Violent) &&assignedPoison!=null)
            {
                Command_Toggle command_TogglePoison = new Command_Toggle();
                if (applyPoisonActive)
                {
                    command_TogglePoison.defaultLabel = "Poison will be applied";
                    command_TogglePoison.defaultDesc = "Poison is going to be used on every successful hit.";
                }
                else
                {
                    command_TogglePoison.defaultLabel = "Poison will not be applied";
                    command_TogglePoison.defaultDesc = "Poison is not being used. Useful for hunting, or against enemies with low toxic sensitivity";
                }
                //command_TogglePoison.hotKey = KeyBindingDefOf.Command_ItemForbid;
                command_TogglePoison.icon = assignedPoison.uiIcon;
                command_TogglePoison.isActive = () => applyPoisonActive;
                command_TogglePoison.toggleAction = delegate
                 {
                     if (applyPoisonActive)
                     {
                         applyPoisonActive = false;
                     }
                     else
                     {
                         applyPoisonActive = true;
                     }
                 };
                yield return command_TogglePoison;
            }
        }
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

