using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using UnityEngine;
using RimWorld;
using AbilityUser;

namespace AIPawnController
{
    public class CompAbilityController : ThingComp
    {

        public int abilityUseEvaluationFrequency = 120;

        public override void PostExposeData()
        {
            base.PostExposeData();
        }

        public CompProperties_AbilityController Props
        {
            get
            {
                return (CompProperties_AbilityController)this.props;
            }
        }

        public override void Initialize(CompProperties props)
        {
            Log.Message("initializing AbilityController");
            CompAbilityUser comp = this.parent.GetComp<CompAbilityUser>();
            if(comp != null)
            {
                Log.Message("adding abilities");
                comp.AddPawnAbility(AIDefOf.CC_Shrapnel);
                
                //for(int i =0; i < this.Props.Abilities.Count; i++)
                //{
                //    comp.AddPawnAbility(this.Props.Abilities[i]);
                //}
            }
            base.Initialize(props);
        }

        public Pawn ParentPawn
        {
            get
            {
                Pawn pawn = this.parent as Pawn;
                bool flag = pawn == null;
                if (flag)
                {
                    Log.Error("pawn is null");
                }
                return pawn;
            }
            set => this.parent = value;
        }        

        public override void CompTick()
        {
            if(Find.TickManager.TicksGame % this.abilityUseEvaluationFrequency == 0)
            {
                Log.Message("attempting ability use");
                AbilityDef abilityDef = AIDefOf.CC_Shrapnel;
                CompAbilityUser comp = this.parent.GetComp<CompAbilityUser>();
                bool success = false;
                PawnAbility ability = comp.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == abilityDef);
                CombatAbility_OnTarget.TryExecute(comp, abilityDef, ability, Functions.Eval.FindNearbyOtherPawn(this.parent as Pawn, 30), 0, out success);
                
            }
            base.CompTick();
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);
        }
    }
}
