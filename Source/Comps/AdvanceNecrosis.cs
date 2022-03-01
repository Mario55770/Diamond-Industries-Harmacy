using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
namespace DI_Harmacy
{
    public class AdvanceNecrosis : Verse.HediffComp
    {
        int i = 0;
        Random r = new Random();
        public override void CompPostTick(ref float var)
        {
            if (i % 16 == 0)
            {
                if (Pawn == null || Pawn.Dead || Pawn.health.hediffSet.GetInjuriesTendable() == null || Pawn.health.hediffSet.GetInjuriesTendable().Count() == 0)
                {
                    return;
                }
                foreach (Hediff_Injury toCheck in Pawn.health.hediffSet.GetInjuriesTendable())
                {
                    if (toCheck == null)
                        return;
                    if (toCheck.ageTicks < 10 && !toCheck.def.Equals(DIH_Hediffs.DIH_NecroticTissue))
                    {
                        //Log.Message(toCheck.Severity.ToString());
                        DamageInfo d = new DamageInfo();

                        d.SetAmount(toCheck.Severity / 2);
                        toCheck.Severity = toCheck.Severity / 2;
                        d.SetHitPart(toCheck.Part);

                        Pawn.health.AddHediff(DIH_Hediffs.DIH_NecroticTissue, toCheck.Part, d);
                    }
                }
                IEnumerable<Hediff_Injury> necroticList = (from x in Pawn.health.hediffSet.GetInjuriesTendable() where x.def.Equals(DIH_Hediffs.DIH_NecroticTissue) select x);
                if (necroticList == null || necroticList.Count() == 0)
                    return;
                Hediff_Injury necroticInjury = necroticList.RandomElement();
                if (necroticInjury == null)
                    return;

                if (necroticInjury.def.Equals(DIH_Hediffs.DIH_NecroticTissue))
                {

                    if ((float)r.NextDouble() < Pawn.health.hediffSet.GetFirstHediffOfDef(DIH_Hediffs.DIH_SnakeVenom).Severity)
                    {
                        //Log.Message(Pawn.health.hediffSet.GetFirstHediffOfDef(DIH_Hediffs.DIH_SnakeVenom).Severity.ToString());
                        necroticInjury.Severity += 0.01f;
                        Pawn.health.Notify_HediffChanged(necroticInjury);
                    }
                }
            }
            if (i > 16)
            {
                i = 1;
            }
            else
            {
                i += 1;
            }

        }
    }
}