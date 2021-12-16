using RimWorld;
using System;
using Verse;
namespace DI_Harmacy
{
    public class AdvanceNecrosis : Verse.HediffComp
    {
        int i = 0;
        public override void CompPostTick(ref float var)
        {
            if (i % 16 == 0)
            {
                
                foreach(Hediff_Injury toCheck in Pawn.health.hediffSet.GetInjuriesTendable())
                {

                    if (toCheck.ageTicks < 10 && !toCheck.def.Equals(DIH_Hediffs.DIH_NecroticTissue))
                    {
                        Log.Message(toCheck.Severity.ToString());
                        DamageInfo d = new DamageInfo();
                        toCheck.Severity /= 2;
                        d.SetAmount(toCheck.Severity);
                        d.SetHitPart(toCheck.Part);

                        Pawn.health.AddHediff(DIH_Hediffs.DIH_NecroticTissue, toCheck.Part, d);


                    }
                    else if (i % 64 == 0)
                    { 
                        if (toCheck.def.Equals(DIH_Hediffs.DIH_NecroticTissue))
                        {
                            toCheck.Severity += 0.1f; 
                        } 
                    }
                    
                }
                

                i = 1;
           }
            else
            {
                i += 1;
            }
        }
    }
}