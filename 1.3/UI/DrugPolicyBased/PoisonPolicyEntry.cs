// RimWorld.DrugPolicyEntry
using RimWorld;
using Verse;
namespace DI_Harmacy
{
	public class PoisonPolicyEntry : IExposable
	{
		public ThingDef poison;

		public bool allowedForAddiction;

		public bool allowedForJoy;

		public bool allowScheduled;

		public float daysFrequency = 1f;

		public float onlyIfMoodBelow = 1f;

		public float onlyIfJoyBelow = 1f;

		public int takeToInventory;

		public string takeToInventoryTempBuffer;

		public void CopyFrom(PoisonPolicyEntry other)
		{
			poison = other.poison;
			allowedForAddiction = other.allowedForAddiction;
			allowedForJoy = other.allowedForJoy;
			allowScheduled = other.allowScheduled;
			daysFrequency = other.daysFrequency;
			onlyIfMoodBelow = other.onlyIfMoodBelow;
			onlyIfJoyBelow = other.onlyIfJoyBelow;
			takeToInventory = other.takeToInventory;
		}

		public void ExposeData()
		{
			Scribe_Defs.Look(ref poison, "drug");
			Scribe_Values.Look(ref allowedForAddiction, "allowedForAddiction", defaultValue: false);
			Scribe_Values.Look(ref allowedForJoy, "allowedForJoy", defaultValue: false);
			Scribe_Values.Look(ref allowScheduled, "allowScheduled", defaultValue: false);
			Scribe_Values.Look(ref daysFrequency, "daysFrequency", 1f);
			Scribe_Values.Look(ref onlyIfMoodBelow, "onlyIfMoodBelow", 1f);
			Scribe_Values.Look(ref onlyIfJoyBelow, "onlyIfJoyBelow", 1f);
			Scribe_Values.Look(ref takeToInventory, "takeToInventory", 0);
		}
	}
}