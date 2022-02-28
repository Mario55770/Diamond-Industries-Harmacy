// RimWorld.PawnColumnWorker_MedicalCare
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;
namespace DI_Harmacy
{

    public class PawnColumnWorker_Poison : PawnColumnWorker
    {
        public override void DoHeader(Rect rect, PawnTable table)
        {
            MouseoverSounds.DoRegion(rect);
            base.DoHeader(rect, table);
        }

        public override int GetMinWidth(PawnTable table)
        {
            return Mathf.Max(base.GetMinWidth(table), 28);
        }

        public override int GetMaxWidth(PawnTable table)
        {
            return Mathf.Min(base.GetMaxWidth(table), GetMinWidth(table));
        }

        public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
        {
            PoisonUIUtility.MedicalCareSelectButton(rect, pawn);
        }


    }
}