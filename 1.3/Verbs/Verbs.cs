using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;
using DI_Harmacy;
namespace DI_Harmacy_VerbExtensions
{

    public static class Verbs
    {
        //public CompReloadable PoisonableCompSource => DirectOwner as CompPoisonable;
        public static CompPoisonable PoisonableCompSource(this IVerbOwner directOwner)
        {
           //get
            //{
                return directOwner as CompPoisonable;
                //return this.DirectOwner as CompPoisonable;
            //}
        }

       
    }
}

