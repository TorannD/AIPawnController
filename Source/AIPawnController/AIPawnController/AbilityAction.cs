using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbilityUser;
using Verse;
using RimWorld;
using Verse.AI;
using UnityEngine;

namespace AIPawnController
{
    public static class AbilityAction
    {

    }

    public static class CombatAbility_OnTarget
    {
        public static void TryExecute(CompAbilityUser casterComp, AbilityDef abilitydef, PawnAbility ability, LocalTargetInfo target, int minRange, out bool success)
        {
            success = false;
            if (ability.CooldownTicksLeft <= 0)
            {
                Pawn caster = casterComp.Pawn;
                LocalTargetInfo jobTarget = target;
                float distanceToTarget = (jobTarget.Cell - caster.Position).LengthHorizontal;
                if (distanceToTarget > minRange && distanceToTarget < (abilitydef.MainVerb.range * .9f) && jobTarget != null && jobTarget.Thing != null && Functions.Eval.HasLoSFromTo(caster.Position, jobTarget, caster, 0, abilitydef.MainVerb.range))
                {
                    Log.Message("ability is " + ability.Def.defName + " target is " + jobTarget.Thing.LabelShort + " caster job is " + caster.CurJob);
                    Job job = ability.GetJob(AbilityContext.AI, jobTarget);
                    Log.Message("job is " + job);
                    job.endIfCantShootTargetFromCurPos = true;
                    caster.jobs.TryTakeOrderedJob(job);
                    success = true;
                }
                //a small change to test a push
            }
        }
    }
}
