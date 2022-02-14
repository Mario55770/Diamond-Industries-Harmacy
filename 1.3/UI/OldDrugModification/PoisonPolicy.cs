// RimWorld.DrugPolicy
using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
namespace DI_Harmacy
{
	public class PoisonPolicy : IExposable, ILoadReferenceable
	{
		public int uniqueId;

		public string label;

		public PoisonPolicyDef sourceDef;

		private List<PoisonPolicyEntry> entriesInt;

		public int Count => entriesInt.Count;

		public PoisonPolicyEntry this[int index]
		{
			get
			{
				return entriesInt[index];
			}
			set
			{
				entriesInt[index] = value;
			}
		}

		public PoisonPolicyEntry this[ThingDef drugDef]
		{
			get
			{
				for (int i = 0; i < entriesInt.Count; i++)
				{
					if (entriesInt[i].poison == drugDef)
					{
						return entriesInt[i];
					}
				}
				throw new ArgumentException();
			}
		}

		public PoisonPolicy()
		{
		}

		public PoisonPolicy(int uniqueId, string label)
		{
			this.uniqueId = uniqueId;
			this.label = label;
			InitializeIfNeeded();
		}

		public void InitializeIfNeeded(bool overwriteExisting = true)
		{
			if (overwriteExisting)
			{
				if (entriesInt != null)
				{
					return;
				}
				entriesInt = new List<PoisonPolicyEntry>();
			}
			List<ThingDef> thingDefs = DefDatabase<ThingDef>.AllDefsListForReading;
			int i;
			for (i = 0; i < thingDefs.Count; i++)
			{
				if (thingDefs[i].category == ThingCategory.Item && thingDefs[i].HasModExtension<PoisonProps>() && (overwriteExisting || !entriesInt.Any((PoisonPolicyEntry x) => x.poison == thingDefs[i])))
				{
					Log.Message(thingDefs[i].ToString());
					PoisonPolicyEntry poisonPolicyEntry = new PoisonPolicyEntry();
					poisonPolicyEntry.poison = thingDefs[i];
					poisonPolicyEntry.allowedForAddiction = true;
					entriesInt.Add(poisonPolicyEntry);
				}
			}
//			entriesInt.SortBy((PoisonPolicyEntry e) => e.poison.GetCompProperties<CompProperties_Drug>().listOrder);
		}

		public void ExposeData()
		{
			Scribe_Values.Look(ref uniqueId, "uniqueId", 0);
			Scribe_Values.Look(ref label, "label");
			Scribe_Collections.Look(ref entriesInt, "drugs", LookMode.Deep);
			Scribe_Defs.Look(ref sourceDef, "sourceDef");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && entriesInt != null)
			{
				if (entriesInt.RemoveAll((PoisonPolicyEntry x) => x == null || x.poison == null) != 0)
				{
					Log.Error("Some DrugPolicyEntries were null after loading.");
				}
				InitializeIfNeeded(overwriteExisting: false);
			}
		}

		public string GetUniqueLoadID()
		{
			return "DrugPolicy_" + label + uniqueId;
		}
	}
}