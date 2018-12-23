using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using AbilityUser;


namespace AIPawnController
{
    public class CompProperties_AbilityController : CompProperties
    {

        public List<AbilityDef> Abilities = new List<AbilityDef>();
        
        public CompProperties_AbilityController()
        {
            this.compClass = typeof(CompAbilityController);
        }
    }
}
