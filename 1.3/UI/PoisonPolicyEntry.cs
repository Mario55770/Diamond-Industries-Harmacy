// RimWorld.DrugPolicyEntry
using RimWorld;
using Verse;
namespace DI_Harmacy
{
	public class PoisonPolicyEntry : IExposable
	{
		public ThingDef poison;

		public void CopyFrom(PoisonPolicyEntry other)
		{
			poison = other.poison;			
		}

		public void ExposeData()
		{
			Scribe_Defs.Look(ref poison, "poison");
			}
	}
}