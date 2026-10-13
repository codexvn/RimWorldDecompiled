using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002186 RID: 8582
	public static class MechClusterUtility
	{
		// Token: 0x0600BB21 RID: 47905 RVA: 0x00361178 File Offset: 0x0035F378
		public static IntVec3 FindClusterPosition(Map map, MechClusterSketch sketch, int maxTries = 100, float spawnCloseToColonyChance = 0f)
		{
			IntVec3 result = IntVec3.Invalid;
			float num = float.MinValue;
			if (Rand.Chance(spawnCloseToColonyChance))
			{
				int num2 = 0;
				IntVec3 intVec;
				while (num2 < 20 && DropCellFinder.TryFindRaidDropCenterClose(out intVec, map, true, true, false, 40))
				{
					float clusterPositionScore = MechClusterUtility.GetClusterPositionScore(intVec, map, sketch);
					if (clusterPositionScore >= 100f || Mathf.Approximately(clusterPositionScore, 100f))
					{
						return intVec;
					}
					if (clusterPositionScore > num)
					{
						result = intVec;
						num = clusterPositionScore;
					}
					num2++;
				}
			}
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < maxTries; i++)
			{
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 x) => x.Standable(map)));
				}
				IntVec3 intVec2 = CellFinderLoose.RandomCellWith(validator, map, 1000);
				if (intVec2.IsValid)
				{
					IntVec3 intVec3 = RCellFinder.FindSiegePositionFrom(intVec2, map, false, false, null, true);
					if (intVec3.IsValid)
					{
						float clusterPositionScore2 = MechClusterUtility.GetClusterPositionScore(intVec3, map, sketch);
						if (clusterPositionScore2 >= 100f || Mathf.Approximately(clusterPositionScore2, 100f))
						{
							return intVec3;
						}
						if (clusterPositionScore2 > num)
						{
							result = intVec2;
							num = clusterPositionScore2;
						}
					}
				}
			}
			if (!result.IsValid)
			{
				return CellFinder.RandomCell(map);
			}
			return result;
		}

		// Token: 0x0600BB22 RID: 47906 RVA: 0x003612B8 File Offset: 0x0035F4B8
		public static float GetClusterPositionScore(IntVec3 center, Map map, MechClusterSketch sketch)
		{
			MechClusterUtility.<>c__DisplayClass9_0 CS$<>8__locals1;
			CS$<>8__locals1.map = map;
			if (sketch.buildingsSketch.AnyThingOutOfBounds(CS$<>8__locals1.map, center, Sketch.SpawnPosType.Unchanged))
			{
				return -100f;
			}
			if (sketch.pawns != null)
			{
				for (int i = 0; i < sketch.pawns.Count; i++)
				{
					if (!(sketch.pawns[i].position + center).InBounds(CS$<>8__locals1.map))
					{
						return -100f;
					}
				}
			}
			CS$<>8__locals1.colonyBuildings = CS$<>8__locals1.map.listerBuildings.allBuildingsColonist;
			CS$<>8__locals1.colonists = CS$<>8__locals1.map.mapPawns.FreeColonistsAndPrisonersSpawned;
			int num = 0;
			CS$<>8__locals1.fogged = 0;
			CS$<>8__locals1.roofed = 0;
			CS$<>8__locals1.indoors = 0;
			CS$<>8__locals1.tooCloseToColony = false;
			List<SketchEntity> entities = sketch.buildingsSketch.Entities;
			for (int j = 0; j < entities.Count; j++)
			{
				if (entities[j].IsSpawningBlocked(center, CS$<>8__locals1.map, null, false))
				{
					num++;
				}
				if (!MechClusterUtility.<GetClusterPositionScore>g__CheckCell|9_0(entities[j].pos + center, ref CS$<>8__locals1))
				{
					return -100f;
				}
			}
			if (sketch.pawns != null)
			{
				for (int k = 0; k < sketch.pawns.Count; k++)
				{
					if (!(sketch.pawns[k].position + center).Standable(CS$<>8__locals1.map))
					{
						num++;
					}
					if (!MechClusterUtility.<GetClusterPositionScore>g__CheckCell|9_0(sketch.pawns[k].position + center, ref CS$<>8__locals1))
					{
						return -100f;
					}
				}
			}
			int num2 = sketch.buildingsSketch.Entities.Count + ((sketch.pawns != null) ? sketch.pawns.Count : 0);
			float num3 = (float)num / (float)num2;
			float num4 = (float)CS$<>8__locals1.fogged / (float)num2;
			float a = (float)CS$<>8__locals1.roofed / (float)num2;
			float b = (float)CS$<>8__locals1.indoors / (float)num2;
			return 100f * (1f - num3) * (1f - Mathf.Max(a, b)) * (1f - num4) * (CS$<>8__locals1.tooCloseToColony ? 0.5f : 1f);
		}

		// Token: 0x0600BB23 RID: 47907 RVA: 0x003614E8 File Offset: 0x0035F6E8
		public static List<Thing> SpawnCluster(IntVec3 center, Map map, MechClusterSketch sketch, bool dropInPods = true, bool canAssaultColony = false, string questTag = null)
		{
			MechClusterUtility.<>c__DisplayClass10_0 CS$<>8__locals1 = new MechClusterUtility.<>c__DisplayClass10_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.sketch = sketch;
			CS$<>8__locals1.spawnedThings = new List<Thing>();
			if (Faction.OfMechanoids == null)
			{
				Log.Warning("Could not spawn mech cluster, no world mech faction found.");
				return CS$<>8__locals1.spawnedThings;
			}
			foreach (IntVec3 a in CS$<>8__locals1.sketch.buildingsSketch.OccupiedRect)
			{
				IntVec3 c = a + center;
				if (c.InBounds(CS$<>8__locals1.map))
				{
					List<Thing> thingList = c.GetThingList(CS$<>8__locals1.map);
					Thing thing = null;
					foreach (Thing thing2 in thingList)
					{
						if (thing2.def.IsBlueprint)
						{
							thing = thing2;
							break;
						}
					}
					if (thing != null)
					{
						thing.Destroy(DestroyMode.Vanish);
					}
				}
			}
			CS$<>8__locals1.spawnMode = (dropInPods ? Sketch.SpawnMode.TransportPod : Sketch.SpawnMode.Normal);
			CS$<>8__locals1.sketch.buildingsSketch.Spawn(CS$<>8__locals1.map, center, Faction.OfMechanoids, Sketch.SpawnPosType.Unchanged, CS$<>8__locals1.spawnMode, false, false, false, CS$<>8__locals1.spawnedThings, CS$<>8__locals1.sketch.startDormant, false, new Func<SketchEntity, IntVec3, bool>(CS$<>8__locals1.<SpawnCluster>g__CanSpawnThing|0), delegate(IntVec3 spot, SketchEntity entity)
			{
				SketchThing sketchThing = entity as SketchThing;
				if (sketchThing == null || sketchThing.def == ThingDefOf.Wall || sketchThing.def == ThingDefOf.Barricade)
				{
					return;
				}
				entity.SpawnNear(spot, CS$<>8__locals1.map, 12f, Faction.OfMechanoids, CS$<>8__locals1.spawnMode, false, false, CS$<>8__locals1.spawnedThings, CS$<>8__locals1.sketch.startDormant, new Func<SketchEntity, IntVec3, bool>(base.<SpawnCluster>g__CanSpawnThing|0), null);
			}, null);
			float defendRadius = Mathf.Sqrt((float)(CS$<>8__locals1.sketch.buildingsSketch.OccupiedSize.x * CS$<>8__locals1.sketch.buildingsSketch.OccupiedSize.x + CS$<>8__locals1.sketch.buildingsSketch.OccupiedSize.z * CS$<>8__locals1.sketch.buildingsSketch.OccupiedSize.z)) / 2f + 6f;
			LordJob_MechanoidDefendBase lordJob_MechanoidDefendBase;
			if (CS$<>8__locals1.sketch.startDormant)
			{
				lordJob_MechanoidDefendBase = new LordJob_SleepThenMechanoidsDefend(CS$<>8__locals1.spawnedThings, Faction.OfMechanoids, defendRadius, center, canAssaultColony, true);
			}
			else
			{
				lordJob_MechanoidDefendBase = new LordJob_MechanoidsDefend(CS$<>8__locals1.spawnedThings, Faction.OfMechanoids, defendRadius, center, canAssaultColony, true);
			}
			Lord lord = LordMaker.MakeNewLord(Faction.OfMechanoids, lordJob_MechanoidDefendBase, CS$<>8__locals1.map, null);
			QuestUtility.AddQuestTag(lord, questTag);
			bool flag = Rand.Chance(0.6f);
			float randomInRange = MechClusterUtility.InitiationDelay.RandomInRange;
			int num = (int)(MechClusterUtility.MechAssemblerInitialDelayDays.RandomInRange * 60000f);
			for (int i = 0; i < CS$<>8__locals1.spawnedThings.Count; i++)
			{
				Thing thing3 = CS$<>8__locals1.spawnedThings[i];
				CompSpawnerPawn compSpawnerPawn = thing3.TryGetComp<CompSpawnerPawn>();
				if (compSpawnerPawn != null)
				{
					compSpawnerPawn.CalculateNextPawnSpawnTick((float)num);
				}
				if (thing3.TryGetComp<CompProjectileInterceptor>() != null)
				{
					lordJob_MechanoidDefendBase.AddThingToNotifyOnDefeat(thing3);
				}
				if (flag)
				{
					CompInitiatable compInitiatable = thing3.TryGetComp<CompInitiatable>();
					if (compInitiatable != null)
					{
						compInitiatable.initiationDelayTicksOverride = (int)(60000f * randomInRange);
					}
				}
				Building building = thing3 as Building;
				if (building != null && MechClusterUtility.IsBuildingThreat(building))
				{
					lord.AddBuilding(building);
				}
				thing3.SetFaction(Faction.OfMechanoids, null);
			}
			if (!CS$<>8__locals1.sketch.pawns.NullOrEmpty<MechClusterSketch.Mech>())
			{
				foreach (MechClusterSketch.Mech mech in CS$<>8__locals1.sketch.pawns)
				{
					IntVec3 intVec = mech.position + center;
					if (!intVec.Standable(CS$<>8__locals1.map))
					{
						IntVec3 root = intVec;
						Map map2 = CS$<>8__locals1.map;
						int squareRadius = 12;
						Predicate<IntVec3> validator;
						if ((validator = CS$<>8__locals1.<>9__2) == null)
						{
							validator = (CS$<>8__locals1.<>9__2 = ((IntVec3 x) => x.Standable(CS$<>8__locals1.map)));
						}
						if (!CellFinder.TryFindRandomCellNear(root, map2, squareRadius, validator, out intVec, -1))
						{
							continue;
						}
					}
					Pawn pawn = PawnGenerator.GeneratePawn(mech.kindDef, Faction.OfMechanoids, null);
					CompCanBeDormant compCanBeDormant = pawn.TryGetComp<CompCanBeDormant>();
					if (compCanBeDormant != null)
					{
						if (CS$<>8__locals1.sketch.startDormant)
						{
							compCanBeDormant.ToSleep();
						}
						else
						{
							compCanBeDormant.WakeUp();
						}
					}
					lord.AddPawn(pawn);
					CS$<>8__locals1.spawnedThings.Add(pawn);
					if (dropInPods)
					{
						ActiveTransporterInfo activeTransporterInfo = new ActiveTransporterInfo();
						activeTransporterInfo.innerContainer.TryAdd(pawn, 1, true);
						activeTransporterInfo.openDelay = 60;
						activeTransporterInfo.leaveSlag = false;
						activeTransporterInfo.despawnPodBeforeSpawningThing = true;
						activeTransporterInfo.spawnWipeMode = new WipeMode?(WipeMode.Vanish);
						DropPodUtility.MakeDropPodAt(intVec, CS$<>8__locals1.map, activeTransporterInfo, Faction.OfMechanoids);
					}
					else
					{
						GenSpawn.Spawn(pawn, intVec, CS$<>8__locals1.map, WipeMode.Vanish);
					}
				}
			}
			foreach (Thing thing4 in CS$<>8__locals1.spawnedThings)
			{
				if (!CS$<>8__locals1.sketch.startDormant)
				{
					CompWakeUpDormant compWakeUpDormant = thing4.TryGetComp<CompWakeUpDormant>();
					if (compWakeUpDormant != null)
					{
						compWakeUpDormant.Activate(null, true, true, false);
					}
				}
			}
			return CS$<>8__locals1.spawnedThings;
		}

		// Token: 0x0600BB24 RID: 47908 RVA: 0x00361A00 File Offset: 0x0035FC00
		private static bool IsBuildingThreat(Thing b)
		{
			CompPawnSpawnOnWakeup compPawnSpawnOnWakeup = b.TryGetComp<CompPawnSpawnOnWakeup>();
			if (compPawnSpawnOnWakeup != null && compPawnSpawnOnWakeup.CanSpawn)
			{
				return true;
			}
			CompSpawnerPawn compSpawnerPawn = b.TryGetComp<CompSpawnerPawn>();
			return (compSpawnerPawn != null && compSpawnerPawn.pawnsLeftToSpawn != 0) || b.def.building.IsTurret || b.TryGetComp<CompCauseGameCondition>() != null;
		}

		// Token: 0x0600BB25 RID: 47909 RVA: 0x00361A54 File Offset: 0x0035FC54
		public static bool AnyThreatBuilding(List<Thing> things)
		{
			for (int i = 0; i < things.Count; i++)
			{
				Thing thing = things[i];
				if (thing is Building && !thing.Destroyed && MechClusterUtility.IsBuildingThreat(thing))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600BB26 RID: 47910 RVA: 0x00361A98 File Offset: 0x0035FC98
		public static bool PartOfActiveMechCluster(Thing t)
		{
			foreach (Lord lord in t.MapHeld.lordManager.lords)
			{
				LordJob_MechanoidDefendBase lordJob_MechanoidDefendBase = lord.LordJob as LordJob_MechanoidDefendBase;
				if (lordJob_MechanoidDefendBase != null && lordJob_MechanoidDefendBase.things.Contains(t))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600BB28 RID: 47912 RVA: 0x00361B3C File Offset: 0x0035FD3C
		[CompilerGenerated]
		internal static bool <GetClusterPositionScore>g__CheckCell|9_0(IntVec3 x, ref MechClusterUtility.<>c__DisplayClass9_0 A_1)
		{
			if (x.Fogged(A_1.map))
			{
				int num = A_1.fogged;
				A_1.fogged = num + 1;
			}
			if (x.GetRoof(A_1.map) == RoofDefOf.RoofRockThick)
			{
				return false;
			}
			if (x.Roofed(A_1.map))
			{
				int num = A_1.roofed;
				A_1.roofed = num + 1;
			}
			if (x.GetRoom(A_1.map) != null && !x.GetRoom(A_1.map).PsychologicallyOutdoors)
			{
				int num = A_1.indoors;
				A_1.indoors = num + 1;
			}
			using (List<Thing>.Enumerator enumerator = x.GetThingList(A_1.map).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.preventSkyfallersLandingOn)
					{
						return false;
					}
				}
			}
			if (!A_1.tooCloseToColony)
			{
				for (int i = 0; i < A_1.colonyBuildings.Count; i++)
				{
					if (x.InHorDistOf(A_1.colonyBuildings[i].Position, 5f))
					{
						A_1.tooCloseToColony = true;
						break;
					}
				}
				if (!A_1.tooCloseToColony)
				{
					for (int j = 0; j < A_1.colonists.Count; j++)
					{
						if (x.InHorDistOf(A_1.colonists[j].Position, 5f))
						{
							A_1.tooCloseToColony = true;
							break;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x040080ED RID: 33005
		private const int CloseToColonyRadius = 40;

		// Token: 0x040080EE RID: 33006
		private const int MinDistanceToColony = 5;

		// Token: 0x040080EF RID: 33007
		private const float RetrySpawnEntityRadius = 12f;

		// Token: 0x040080F0 RID: 33008
		private const float MaxClusterPositionScore = 100f;

		// Token: 0x040080F1 RID: 33009
		private const float InitiationChance = 0.6f;

		// Token: 0x040080F2 RID: 33010
		private static readonly FloatRange InitiationDelay = new FloatRange(0.1f, 15f);

		// Token: 0x040080F3 RID: 33011
		private static readonly FloatRange MechAssemblerInitialDelayDays = new FloatRange(0.5f, 1.5f);

		// Token: 0x040080F4 RID: 33012
		public const string DefeatedSignal = "MechClusterDefeated";
	}
}
