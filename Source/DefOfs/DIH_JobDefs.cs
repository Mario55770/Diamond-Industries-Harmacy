using RimWorld;
using Verse;
namespace DI_Harmacy
{
    [DefOf]
    public static class DIH_JobDefs
    {
        public static JobDef DIH_PoisonJob;
        static DIH_JobDefs()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(JobDefOf));
        }
    }
}