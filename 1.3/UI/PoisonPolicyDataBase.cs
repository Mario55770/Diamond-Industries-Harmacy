// RimWorld.DrugPolicyDatabase
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
namespace DI_Harmacy
{
	public class PoisonPolicyDatabase : IExposable
	{
		private List<PoisonPolicy> policies = new List<PoisonPolicy>();

		public List<PoisonPolicy> AllPolicies => policies;

		public PoisonPolicyDatabase()
		{
			GenerateStartingDrugPolicies();
		}

		public void ExposeData()
		{
			Scribe_Collections.Look(ref policies, "policies", LookMode.Deep);
		}

		public PoisonPolicy DefaultDrugPolicy()
		{
			if (policies.Count == 0)
			{
				MakeNewDrugPolicy();
			}
			return policies[0];
		}

		public void MakePolicyDefault(PoisonPolicyDef policyDef)
		{
			if (DefaultDrugPolicy().sourceDef != policyDef)
			{
				PoisonPolicy drugPolicy = policies.FirstOrDefault((PoisonPolicy x) => x.sourceDef == policyDef);
				if (drugPolicy != null)
				{
					policies.Remove(drugPolicy);
					policies.Insert(0, drugPolicy);
				}
			}
		}

		public AcceptanceReport TryDelete(PoisonPolicy policy)
		{
		/**	foreach (Pawn item in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
			{
				if (item.drugs != null && item.drugs.CurrentPolicy == policy)
				{
					return new AcceptanceReport("DrugPolicyInUse".Translate(item));
				}
			}
			foreach (Pawn item2 in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				if (item2.drugs != null && item2.drugs.CurrentPolicy == policy)
				{
					item2.drugs.CurrentPolicy = null;
				}
			}
			policies.Remove(policy);**/
			return AcceptanceReport.WasAccepted;
		}

		public PoisonPolicy MakeNewDrugPolicy()
		{
			int uniqueId = ((!policies.Any()) ? 1 : (policies.Max((PoisonPolicy o) => o.uniqueId) + 1));
			PoisonPolicy drugPolicy = new PoisonPolicy(uniqueId, "DrugPolicy".Translate() + " " + uniqueId.ToString());
			policies.Add(drugPolicy);
			return drugPolicy;
		}

		public PoisonPolicy NewDrugPolicyFromDef(PoisonPolicyDef def)
		{
			PoisonPolicy drugPolicy = MakeNewDrugPolicy();
			drugPolicy.label = def.LabelCap;
			drugPolicy.sourceDef = def;
			if (def.allowPleasureDrugs)
			{
				for (int i = 0; i < drugPolicy.Count; i++)
				{
					if (drugPolicy[i].poison.IsPleasureDrug)
					{
						drugPolicy[i].allowedForJoy = true;
					}
				}
			}
			if (def.entries != null)
			{
				for (int j = 0; j < def.entries.Count; j++)
				{
					//drugPolicy[def.entries[j].drug].CopyFrom(def.entries[j]);
				}
			}
			return drugPolicy;
		}

		private void GenerateStartingDrugPolicies()
		{
			foreach (PoisonPolicyDef allDef in DefDatabase<PoisonPolicyDef>.AllDefs)
			{
				NewDrugPolicyFromDef(allDef);
			}
		}
	}
}