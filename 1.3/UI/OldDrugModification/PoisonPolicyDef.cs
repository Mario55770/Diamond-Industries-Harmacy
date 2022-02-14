// RimWorld.DrugPolicyDef
using System.Collections.Generic;
using RimWorld;
using Verse;
namespace DI_Harmacy
{
	public class PoisonPolicyDef : Def
	{
		public bool allowPleasureDrugs;

		public List<DrugPolicyEntry> entries;
	}
}