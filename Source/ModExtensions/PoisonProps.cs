using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace DI_Harmacy
{
    public class PoisonProps:DefModExtension
    {
        public HediffDef poisonInflicts = null;
        public int ammoCountPerCharge = 20;
        public int maxCharges = 5;
    }
}
