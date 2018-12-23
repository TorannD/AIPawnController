﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;
using Verse.AI;

namespace AIPawnController
{
    public static class Functions
    {
        public static class Eval
        {
            public static bool IsRobotPawn(Pawn pawn)
            {
                bool flag_Core = pawn.RaceProps.IsMechanoid;
                bool flag_AndroidTiers = (pawn.def.defName == "Android1Tier" || pawn.def.defName == "Android2Tier" || pawn.def.defName == "Android3Tier" || pawn.def.defName == "Android4Tier" || pawn.def.defName == "Android5Tier" || pawn.def.defName == "M7Mech" || pawn.def.defName == "MicroScyther");
                bool flag_Androids = pawn.RaceProps.FleshType.defName == "ChJDroid" || pawn.def.defName == "ChjAndroid";
                bool isRobot = flag_Core || flag_AndroidTiers || flag_Androids;
                return isRobot;
            }

            public static bool IsUndead(Pawn pawn)
            {
                if (pawn != null)
                {
                    bool flag_Hediff = false;
                    if (pawn.health != null && pawn.health.hediffSet != null)
                    {
                        if (pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadHD"), false) || pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadAnimalHD"), false) || pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_LichHD"), false) || pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadStageHD"), false))
                        {
                            flag_Hediff = true;
                        }
                        Hediff hediff = null;
                        for (int i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
                        {
                            hediff = pawn.health.hediffSet.hediffs[i];
                            if (hediff.def.defName.Contains("ROM_Vamp"))
                            {
                                flag_Hediff = true;
                            }
                        }
                    }
                    bool flag_DefName = false;
                    if (pawn.def.defName == "SL_Runner" || pawn.def.defName == "SL_Peon" || pawn.def.defName == "SL_Archer" || pawn.def.defName == "SL_Hero")
                    {
                        flag_DefName = true;
                    }
                    bool flag_Trait = false;
                    if (pawn.story != null && pawn.story.traits != null)
                    {

                    }
                    bool isUndead = flag_Hediff || flag_DefName || flag_Trait;
                    return isUndead;
                }
                return false;
            }

            public static bool IsUndeadNotVamp(Pawn pawn)
            {
                if (pawn != null)
                {
                    bool flag_Hediff = false;
                    if (pawn.health != null)
                    {
                        if (pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadHD"), false) || pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadAnimalHD"), false) || pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_LichHD"), false) || pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadStageHD"), false))
                        {
                            flag_Hediff = true;
                        }
                    }
                    bool flag_DefName = false;
                    if (pawn.def.defName == "SL_Runner" || pawn.def.defName == "SL_Peon" || pawn.def.defName == "SL_Archer" || pawn.def.defName == "SL_Hero")
                    {
                        flag_DefName = true;
                    }
                    bool flag_Trait = false;
                    if (pawn.story != null && pawn.story.traits != null)
                    {

                    }
                    bool isUndead = flag_Hediff || flag_DefName || flag_Trait;
                    return isUndead;
                }
                return false;
            }

            public static Vector3 GetVector(IntVec3 from, IntVec3 to)
            {
                Vector3 heading = (to - from).ToVector3();
                float distance = heading.magnitude;
                Vector3 direction = heading / distance;
                return direction;
            }

            public static Vector3 GetVector(Vector3 from, Vector3 to)
            {
                Vector3 heading = (to - from);
                float distance = heading.magnitude;
                Vector3 direction = heading / distance;
                return direction;
            }

            public static Pawn FindNearbyOtherPawn(Pawn pawn, int radius)
            {
                List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
                List<Pawn> pawnList = new List<Pawn>();
                Pawn targetPawn = null;
                pawnList.Clear();
                for (int i = 0; i < mapPawns.Count; i++)
                {
                    targetPawn = mapPawns[i];
                    if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed && !targetPawn.Downed)
                    {
                        if (targetPawn != pawn  && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius) //&& !targetPawn.HostileTo(pawn.Faction)
                        {
                            pawnList.Add(targetPawn);
                            targetPawn = null;
                        }
                        else
                        {
                            targetPawn = null;
                        }
                    }
                }
                if (pawnList.Count > 0)
                {
                    return pawnList.RandomElement();
                }
                else
                {
                    return null;
                }
            }

            public static Pawn FindNearbyPawn(Pawn pawn, int radius)
            {
                List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
                List<Pawn> pawnList = new List<Pawn>();
                Pawn targetPawn = null;
                pawnList.Clear();
                for (int i = 0; i < mapPawns.Count; i++)
                {
                    targetPawn = mapPawns[i];
                    if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed && !targetPawn.Downed)
                    {
                        if (!targetPawn.HostileTo(pawn.Faction) && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                        {
                            pawnList.Add(targetPawn);
                            targetPawn = null;
                        }
                        else
                        {
                            targetPawn = null;
                        }
                    }
                }
                if (pawnList.Count > 0)
                {
                    return pawnList.RandomElement();
                }
                else
                {
                    return null;
                }
            }

            public static Pawn FindNearbyInjuredPawn(Pawn pawn, int radius, float minSeverity)
            {
                List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
                List<Pawn> pawnList = new List<Pawn>();
                Pawn targetPawn = null;
                pawnList.Clear();
                for (int i = 0; i < mapPawns.Count; i++)
                {
                    targetPawn = mapPawns[i];
                    if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed && !Eval.IsUndead(targetPawn))
                    {
                        if (targetPawn.IsColonist && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                        {
                            float injurySeverity = 0;
                            using (IEnumerator<BodyPartRecord> enumerator = targetPawn.health.hediffSet.GetInjuredParts().GetEnumerator())
                            {
                                while (enumerator.MoveNext())
                                {
                                    BodyPartRecord rec = enumerator.Current;
                                    IEnumerable<Hediff_Injury> arg_BB_0 = targetPawn.health.hediffSet.GetHediffs<Hediff_Injury>();
                                    Func<Hediff_Injury, bool> arg_BB_1;
                                    arg_BB_1 = ((Hediff_Injury injury) => injury.Part == rec);

                                    foreach (Hediff_Injury current in arg_BB_0.Where(arg_BB_1))
                                    {
                                        bool flag5 = current.CanHealNaturally() && !current.IsPermanent();
                                        if (flag5)
                                        {
                                            injurySeverity += current.Severity;
                                        }
                                    }
                                }
                            }
                            if (minSeverity != 0)
                            {
                                if (injurySeverity >= minSeverity)
                                {
                                    pawnList.Add(targetPawn);
                                }
                            }
                            else
                            {
                                if (injurySeverity != 0)
                                {
                                    pawnList.Add(targetPawn);
                                }
                            }
                            targetPawn = null;
                        }
                        else
                        {
                            targetPawn = null;
                        }
                    }
                }
                if (pawnList.Count > 0)
                {
                    return pawnList.RandomElement();
                }
                else
                {
                    return null;
                }
            }

            public static Pawn FindNearbyInjuredPawnOther(Pawn pawn, int radius, float minSeverity)
            {
                List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
                List<Pawn> pawnList = new List<Pawn>();
                Pawn targetPawn = null;
                pawnList.Clear();
                for (int i = 0; i < mapPawns.Count; i++)
                {
                    targetPawn = mapPawns[i];
                    if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed && !Eval.IsUndead(targetPawn))
                    {
                        if (targetPawn.IsColonist && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius && targetPawn != pawn)
                        {
                            float injurySeverity = 0;
                            using (IEnumerator<BodyPartRecord> enumerator = targetPawn.health.hediffSet.GetInjuredParts().GetEnumerator())
                            {
                                while (enumerator.MoveNext())
                                {
                                    BodyPartRecord rec = enumerator.Current;
                                    IEnumerable<Hediff_Injury> arg_BB_0 = targetPawn.health.hediffSet.GetHediffs<Hediff_Injury>();
                                    Func<Hediff_Injury, bool> arg_BB_1;
                                    arg_BB_1 = ((Hediff_Injury injury) => injury.Part == rec);

                                    foreach (Hediff_Injury current in arg_BB_0.Where(arg_BB_1))
                                    {
                                        bool flag5 = current.CanHealNaturally() && !current.IsPermanent();
                                        if (flag5)
                                        {
                                            injurySeverity += current.Severity;
                                        }
                                    }
                                }
                            }
                            if (minSeverity != 0)
                            {
                                if (injurySeverity >= minSeverity)
                                {
                                    pawnList.Add(targetPawn);
                                }
                            }
                            else
                            {
                                if (injurySeverity != 0)
                                {
                                    pawnList.Add(targetPawn);
                                }
                            }
                            targetPawn = null;
                        }
                        else
                        {
                            targetPawn = null;
                        }
                    }
                }
                if (pawnList.Count > 0)
                {
                    return pawnList.RandomElement();
                }
                else
                {
                    return null;
                }
            }

            public static Pawn FindNearbyPermanentlyInjuredPawn(Pawn pawn, int radius, float minSeverity)
            {
                List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
                List<Pawn> pawnList = new List<Pawn>();
                Pawn targetPawn = null;
                pawnList.Clear();
                for (int i = 0; i < mapPawns.Count; i++)
                {
                    targetPawn = mapPawns[i];
                    if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed && !Eval.IsUndead(targetPawn))
                    {
                        if (targetPawn.IsColonist && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                        {
                            float injurySeverity = 0;
                            using (IEnumerator<BodyPartRecord> enumerator = targetPawn.health.hediffSet.GetInjuredParts().GetEnumerator())
                            {
                                while (enumerator.MoveNext())
                                {
                                    BodyPartRecord rec = enumerator.Current;
                                    IEnumerable<Hediff_Injury> arg_BB_0 = targetPawn.health.hediffSet.GetHediffs<Hediff_Injury>();
                                    Func<Hediff_Injury, bool> arg_BB_1;
                                    arg_BB_1 = ((Hediff_Injury injury) => injury.Part == rec);

                                    foreach (Hediff_Injury current in arg_BB_0.Where(arg_BB_1))
                                    {
                                        bool flag5 = !current.CanHealNaturally() && current.IsPermanent();
                                        if (flag5)
                                        {
                                            injurySeverity += current.Severity;
                                        }
                                    }
                                }
                            }
                            if (minSeverity != 0)
                            {
                                if (injurySeverity >= minSeverity)
                                {
                                    pawnList.Add(targetPawn);
                                }
                            }
                            else
                            {
                                if (injurySeverity != 0)
                                {
                                    pawnList.Add(targetPawn);
                                }
                            }
                            targetPawn = null;
                        }
                        else
                        {
                            targetPawn = null;
                        }
                    }
                }
                if (pawnList.Count > 0)
                {
                    return pawnList.RandomElement();
                }
                else
                {
                    return null;
                }
            }

            public static Pawn FindNearbyAfflictedPawn(Pawn pawn, int radius, List<string> validAfflictionDefnames)
            {
                List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
                List<Pawn> pawnList = new List<Pawn>();
                Pawn targetPawn = null;
                pawnList.Clear();
                for (int i = 0; i < mapPawns.Count; i++)
                {
                    targetPawn = mapPawns[i];
                    if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed)
                    {
                        if (targetPawn.IsColonist && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                        {
                            using (IEnumerator<Hediff> enumerator = targetPawn.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
                            {
                                while (enumerator.MoveNext())
                                {
                                    Hediff rec = enumerator.Current;
                                    for (int j = 0; j < validAfflictionDefnames.Count; j++)
                                    {
                                        if (rec.def.defName.Contains(validAfflictionDefnames[j]))
                                        {
                                            pawnList.Add(targetPawn);
                                        }
                                    }

                                }
                            }
                            targetPawn = null;
                        }
                        else
                        {
                            targetPawn = null;
                        }
                    }
                }
                if (pawnList.Count > 0)
                {
                    return pawnList.RandomElement();
                }
                else
                {
                    return null;
                }
            }

            public static Pawn FindNearbyAddictedPawn(Pawn pawn, int radius, List<string> validAddictionDefnames)
            {
                List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
                List<Pawn> pawnList = new List<Pawn>();
                Pawn targetPawn = null;
                pawnList.Clear();
                for (int i = 0; i < mapPawns.Count; i++)
                {
                    targetPawn = mapPawns[i];
                    if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed)
                    {
                        if (targetPawn.IsColonist && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                        {
                            using (IEnumerator<Hediff_Addiction> enumerator = targetPawn.health.hediffSet.GetHediffs<Hediff_Addiction>().GetEnumerator())
                            {
                                while (enumerator.MoveNext())
                                {
                                    Hediff_Addiction rec = enumerator.Current;
                                    for (int j = 0; j < validAddictionDefnames.Count; j++)
                                    {
                                        if (rec.Chemical.defName.Contains(validAddictionDefnames[j]))
                                        {
                                            pawnList.Add(targetPawn);
                                        }
                                    }

                                }
                            }
                            targetPawn = null;
                        }
                        else
                        {
                            targetPawn = null;
                        }
                    }
                }
                if (pawnList.Count > 0)
                {
                    return pawnList.RandomElement();
                }
                else
                {
                    return null;
                }
            }

            public static Pawn FindNearbyEnemy(Pawn pawn, int radius)
            {
                return FindNearbyEnemy(pawn.Position, pawn.Map, pawn.Faction, radius, 0);
            }

            public static Pawn FindNearbyEnemy(IntVec3 position, Map map, Faction faction, float radius, float minRange)
            {
                List<Pawn> mapPawns = map.mapPawns.AllPawnsSpawned;
                List<Pawn> pawnList = new List<Pawn>();
                Pawn targetPawn = null;
                pawnList.Clear();
                for (int i = 0; i < mapPawns.Count; i++)
                {
                    targetPawn = mapPawns[i];
                    if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed && !targetPawn.Downed)
                    {
                        if (targetPawn.Position != position && targetPawn.HostileTo(faction) && (position - targetPawn.Position).LengthHorizontal <= radius && (position - targetPawn.Position).LengthHorizontal > minRange)
                        {
                            pawnList.Add(targetPawn);
                            targetPawn = null;
                        }
                        else
                        {
                            targetPawn = null;
                        }
                    }
                }
                if (pawnList.Count > 0)
                {
                    return pawnList.RandomElement();
                }
                else
                {
                    return null;
                }
            }

            public static List<Pawn> FindPawnsNearTarget(Pawn pawn, int radius, IntVec3 targetCell, bool hostile)
            {
                List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
                List<Pawn> pawnList = new List<Pawn>();
                Pawn targetPawn = null;
                pawnList.Clear();
                for (int i = 0; i < mapPawns.Count; i++)
                {
                    targetPawn = mapPawns[i];
                    if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed && !targetPawn.Downed)
                    {
                        if (targetPawn != pawn && (targetCell - targetPawn.Position).LengthHorizontal <= radius)
                        {
                            if (hostile && targetPawn.HostileTo(pawn.Faction))
                            {
                                pawnList.Add(targetPawn);
                            }
                            else if (!hostile && !targetPawn.HostileTo(pawn.Faction))
                            {
                                pawnList.Add(targetPawn);
                            }
                        }
                        targetPawn = null;
                    }
                }
                if (pawnList.Count > 0)
                {
                    return pawnList;
                }
                else
                {
                    return null;
                }
            }

            public static bool HasLoSFromTo(IntVec3 root, LocalTargetInfo targ, Thing caster, float minRange, float maxRange)
            {
                float range = (targ.Cell - root).LengthHorizontal;
                if (targ.HasThing && targ.Thing.Map != caster.Map)
                {
                    return false;
                }
                if (range <= minRange || range >= maxRange)
                {
                    return false;
                }
                CellRect cellRect = (!targ.HasThing) ? CellRect.SingleCell(targ.Cell) : targ.Thing.OccupiedRect();
                if (caster is Pawn)
                {
                    if (GenSight.LineOfSight(caster.Position, targ.Cell, caster.Map, skipFirstCell: true))
                    {
                        return true;
                    }
                    List<IntVec3> tempLeanShootSources = new List<IntVec3>();
                    ShootLeanUtility.LeanShootingSourcesFromTo(root, cellRect.ClosestCellTo(root), caster.Map, tempLeanShootSources);
                    for (int i = 0; i < tempLeanShootSources.Count; i++)
                    {
                        IntVec3 intVec = tempLeanShootSources[i];
                        if (GenSight.LineOfSight(intVec, targ.Cell, caster.Map, skipFirstCell: true))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (GenSight.LineOfSight(root, targ.Cell, caster.Map, skipFirstCell: true))
                    {
                        return true;
                    }
                }
                return false;
            }

            public static List<Thing> FindNearbyDamagedBuilding(Pawn pawn, int radius)
            {
                List<Thing> mapBuildings = pawn.Map.listerBuildingsRepairable.RepairableBuildings(pawn.Faction);
                List<Thing> buildingList = new List<Thing>();
                Building building = null;
                buildingList.Clear();
                for (int i = 0; i < mapBuildings.Count; i++)
                {
                    building = mapBuildings[i] as Building;
                    if (building != null && (building.Position - pawn.Position).LengthHorizontal <= radius && building.def.useHitPoints && building.HitPoints != building.MaxHitPoints)
                    {
                        if (pawn.Drafted && building.def.designationCategory == DesignationCategoryDefOf.Security || building.def.building.ai_combatDangerous)
                        {
                            buildingList.Add(building);
                        }
                        else if (!pawn.Drafted)
                        {
                            buildingList.Add(building);
                        }
                    }
                    building = null;
                }

                if (buildingList.Count > 0)
                {
                    return buildingList;
                }
                else
                {
                    return null;
                }
            }

            public static Thing FindNearbyDamagedThing(Pawn pawn, int radius)
            {
                List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
                List<Thing> pawnList = new List<Thing>();
                Pawn targetPawn = null;
                pawnList.Clear();
                for (int i = 0; i < mapPawns.Count; i++)
                {
                    targetPawn = mapPawns[i];
                    if (targetPawn != null && targetPawn.Spawned && !targetPawn.Dead && !targetPawn.Destroyed)
                    {
                        //Log.Message("evaluating targetpawn " + targetPawn.LabelShort);
                        //Log.Message("pawn faction is " + targetPawn.Faction);
                        //Log.Message("pawn position is " + targetPawn.Position);
                        //Log.Message("pawn is robot: " + TM_Calc.IsRobotPawn(targetPawn));
                        if (targetPawn.Faction != null && targetPawn.Faction == pawn.Faction && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius && Eval.IsRobotPawn(targetPawn))
                        {
                            float injurySeverity = 0;
                            using (IEnumerator<BodyPartRecord> enumerator = targetPawn.health.hediffSet.GetInjuredParts().GetEnumerator())
                            {
                                while (enumerator.MoveNext())
                                {
                                    BodyPartRecord rec = enumerator.Current;
                                    IEnumerable<Hediff_Injury> arg_BB_0 = targetPawn.health.hediffSet.GetHediffs<Hediff_Injury>();
                                    Func<Hediff_Injury, bool> arg_BB_1;
                                    arg_BB_1 = ((Hediff_Injury injury) => injury.Part == rec);

                                    foreach (Hediff_Injury current in arg_BB_0.Where(arg_BB_1))
                                    {
                                        bool flag5 = !current.IsPermanent();
                                        if (flag5)
                                        {
                                            injurySeverity += current.Severity;
                                        }
                                    }
                                }
                            }

                            if (injurySeverity != 0)
                            {
                                pawnList.Add(targetPawn as Thing);
                            }
                        }
                        targetPawn = null;
                    }
                }

                List<Thing> buildingList = Eval.FindNearbyDamagedBuilding(pawn, radius);
                if (buildingList != null)
                {
                    for (int i = 0; i < buildingList.Count; i++)
                    {
                        pawnList.Add(buildingList[i]);
                    }
                }

                if (pawnList.Count > 0)
                {
                    return pawnList.RandomElement();
                }
                else
                {
                    return null;
                }
            }


        }
    }
}
