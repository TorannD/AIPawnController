using Harmony;
using RimWorld;
using AbilityUser;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.AI;
using System.Reflection.Emit;

namespace AIPawnController
{
    [StaticConstructorOnStartup]
    internal class HarmonyPatches
    {
        private static readonly Type patchType = typeof(HarmonyPatches);

        static HarmonyPatches()
        {
            HarmonyInstance harmonyInstance = HarmonyInstance.Create(id: "rimworld.torann.aipawncontroller");
            harmonyInstance.Patch(original: AccessTools.Method(type: typeof(PawnAbility), name: "GetJob"),
                prefix: new HarmonyMethod(type: patchType, name: nameof(PawnAbility_GetJob_Prefix)));

        }

        public static bool PawnAbility_GetJob_Prefix(PawnAbility __instance, AbilityContext context, LocalTargetInfo target, ref Job __result)
        {
            Job job;
            Log.Message("target is " + target.Thing.LabelShort);
            AbilityDef abilityDef = Traverse.Create(root: __instance).Field(name: "powerdef").GetValue<AbilityDef>();
            Log.Message("ability def is " + abilityDef.defName);
            Verb_UseAbility verb = Traverse.Create(root: __instance).Field(name: "verb").GetValue<Verb_UseAbility>();            
            
            Log.Message("verb is " + verb);
            if(verb == null)
            {
                Verb_UseAbility verb_UseAbility = (Verb_UseAbility)Activator.CreateInstance(abilityDef.MainVerb.verbClass);
                verb_UseAbility.caster = __instance.Pawn;
                verb_UseAbility.Ability = __instance;
                verb_UseAbility.verbProps = abilityDef.MainVerb;
                verb = verb_UseAbility;
            }
            if(verb != null)
            {
                Log.Message("verb is no longer null");
            }
            job = abilityDef.GetJob(verb.UseAbilityProps.AbilityTargetCategory, target);
            job.playerForced = true;
            job.verbToUse = verb;
            job.count = context == AbilityContext.Player ? 1 : 0; //Count 1 for Player : 0 for AI
            if (target != null)
                if (target.Thing is Pawn pawn2)
                    job.killIncappedTarget = pawn2.Downed;
            __result = job;
            return false;
        }
    }
}
