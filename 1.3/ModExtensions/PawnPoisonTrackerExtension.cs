using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
namespace DI_Harmacy
{
    class PawnPoisonTrackerExtension:DefModExtension
    {
        public bool test;
        public Pawn_PoisonPolicyTracker pawn_PoisonPolicyTracker=new Pawn_PoisonPolicyTracker();
    }
}
