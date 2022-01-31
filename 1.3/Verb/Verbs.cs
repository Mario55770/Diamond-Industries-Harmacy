using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;
namespace DI_Harmacy
{

    public static class NewVerbs
    {
        public static CompPoisonable PoisonableCompSource(this Verb verb)
        {
           //get
            //{
                return verb.DirectOwner as CompPoisonable;
                //return this.DirectOwner as CompPoisonable;
            //}
        }

       
    }
}

