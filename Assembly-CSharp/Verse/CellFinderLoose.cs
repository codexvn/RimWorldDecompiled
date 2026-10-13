using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000CDE RID: 3294
	public static class CellFinderLoose
	{
		// Token: 0x06005449 RID: 21577 RVA: 0x001B19E0 File Offset: 0x001AFBE0
		public static IntVec3 RandomCellWith(Predicate<IntVec3> validator, Map map, int maxTries = 1000)
		{
			IntVec3 result;
			CellFinderLoose.TryGetRandomCellWith(validator, map, maxTries, out result);
			return result;
		}

		// Token: 0x0600544A RID: 21578 RVA: 0x001B19FC File Offset: 0x001AFBFC
		public static bool TryGetRandomCellWith(Predicate<IntVec3> validator, Map map, int maxTries, out IntVec3 result)
		{
			for (int i = 0; i < maxTries; i++)
			{
				result = CellFinder.RandomCell(map);
				if (validator(result))
				{
					return true;
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0600544B RID: 21579 RVA: 0x001B1A40 File Offset: 0x001AFC40
		public static bool TryFindRandomNotEdgeCellWith(int minEdgeDistance, Predicate<IntVec3> validator, Map map, out IntVec3 result)
		{
			for (int i = 0; i < 1000; i++)
			{
				result = CellFinder.RandomNotEdgeCell(minEdgeDistance, map);
				if (result.IsValid && validator(result))
				{
					return true;
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0600544C RID: 21580 RVA: 0x001B1A90 File Offset: 0x001AFC90
		public static bool GetFleeExitPosition(Pawn pawn, float radius, out IntVec3 position)
		{
			return CellFinder.TryFindRandomEdgeCellNearWith(pawn.Position, radius, pawn.Map, (IntVec3 p) => GenSight.LineOfSight(pawn.Position, p, pawn.Map) && pawn.CanReach(p, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn), out position);
		}

		// Token: 0x0600544D RID: 21581 RVA: 0x001B1AD3 File Offset: 0x001AFCD3
		public static IntVec3 GetFleeDest(Pawn pawn, List<Thing> threats, float distance = 23f)
		{
			if (pawn.IsAnimal)
			{
				return CellFinderLoose.GetFleeDestAnimal(pawn, threats, distance);
			}
			return CellFinderLoose.GetFleeDestToolUser(pawn, threats, distance);
		}

		// Token: 0x0600544E RID: 21582 RVA: 0x001B1AF0 File Offset: 0x001AFCF0
		public static IntVec3 GetFleeDestAnimal(Pawn pawn, List<Thing> threats, float distance = 23f)
		{
			Vector3 normalized = (pawn.Position - threats[0].Position).ToVector3().normalized;
			float num = distance - pawn.Position.DistanceTo(threats[0].Position);
			for (float num2 = 200f; num2 <= 360f; num2 += 10f)
			{
				IntVec3 intVec = pawn.Position + (normalized.RotatedBy(Rand.Range(-num2 / 2f, num2 / 2f)) * num).ToIntVec3();
				if (CellFinderLoose.CanFleeToLocation(pawn, intVec))
				{
					return intVec;
				}
			}
			float num3 = num;
			while (num3 * 3f > num)
			{
				IntVec3 intVec2 = pawn.Position + IntVec3Utility.RandomHorizontalOffset(num3);
				if (CellFinderLoose.CanFleeToLocation(pawn, intVec2))
				{
					return intVec2;
				}
				num3 -= distance / 10f;
			}
			return pawn.Position;
		}

		// Token: 0x0600544F RID: 21583 RVA: 0x001B1BE0 File Offset: 0x001AFDE0
		public static bool CanFleeToLocation(Pawn pawn, IntVec3 location)
		{
			return location.Standable(pawn.Map) && pawn.Map.pawnDestinationReservationManager.CanReserve(location, pawn, false) && location.GetRegion(pawn.Map, RegionType.Set_Passable).type != RegionType.Portal && !location.GetTerrain(pawn.Map).dangerous && (!pawn.roping.IsRoped || location.InHorDistOf(pawn.roping.RopedToSpot, 8f)) && pawn.CanReach(location, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn);
		}

		// Token: 0x06005450 RID: 21584 RVA: 0x001B1C80 File Offset: 0x001AFE80
		public static IntVec3 GetFleeDestToolUser(Pawn pawn, List<Thing> threats, float distance = 23f)
		{
			IntVec3 bestPos = pawn.Position;
			float bestScore = -1f;
			TraverseParms traverseParms = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
			RegionTraverser.BreadthFirstTraverse(pawn.GetRegion(RegionType.Set_Passable), (Region from, Region reg) => reg.Allows(traverseParms, false), delegate(Region reg)
			{
				Danger danger = reg.DangerFor(pawn);
				Map map = pawn.Map;
				foreach (IntVec3 intVec in reg.Cells)
				{
					if (intVec.Standable(map) && !reg.IsDoorway)
					{
						if (intVec.GetTerrain(pawn.Map).dangerous)
						{
							return false;
						}
						Thing thing = null;
						float num = 0f;
						for (int i = 0; i < threats.Count; i++)
						{
							float num2 = (float)intVec.DistanceToSquared(threats[i].Position);
							if (thing == null || num2 < num)
							{
								thing = threats[i];
								num = num2;
							}
						}
						float num3 = Mathf.Sqrt(num);
						float num4 = Mathf.Pow(Mathf.Min(num3, distance), 1.2f);
						num4 *= Mathf.InverseLerp(50f, 0f, (intVec - pawn.Position).LengthHorizontal);
						if (intVec.GetRoom(map) != thing.GetRoom(RegionType.Set_All))
						{
							num4 *= 4.2f;
						}
						else if (num3 < 8f)
						{
							num4 *= 0.05f;
						}
						if (!map.pawnDestinationReservationManager.CanReserve(intVec, pawn, false))
						{
							num4 *= 0.5f;
						}
						if (danger == Danger.Deadly)
						{
							num4 *= 0.8f;
						}
						if (ModsConfig.AnomalyActive && (pawn.RaceProps.Humanlike || pawn.IsPlayerControlled) && map.gameConditionManager.MapBrightness < 0.1f && map.glowGrid.PsychGlowAt(intVec) == PsychGlow.Dark)
						{
							num4 *= 0.1f;
						}
						if (num4 > bestScore)
						{
							bestPos = intVec;
							bestScore = num4;
						}
					}
				}
				return false;
			}, 20, RegionType.Set_Passable);
			return bestPos;
		}

		// Token: 0x06005451 RID: 21585 RVA: 0x001B1D10 File Offset: 0x001AFF10
		public static IntVec3 TryFindCentralCell(Map map, int tightness, int minCellCount, Predicate<IntVec3> extraValidator = null, bool returnInvalidOnFail = false)
		{
			int debug_numStand = 0;
			int debug_numDistrict = 0;
			int debug_numTouch = 0;
			int debug_numDistrictCellCount = 0;
			int debug_numExtraValidator = 0;
			Predicate<IntVec3> validator = delegate(IntVec3 c)
			{
				if (!c.Standable(map))
				{
					int num2 = debug_numStand;
					debug_numStand = num2 + 1;
					return false;
				}
				District district = c.GetDistrict(map, RegionType.Set_Passable);
				if (district == null)
				{
					int num2 = debug_numDistrict;
					debug_numDistrict = num2 + 1;
					return false;
				}
				if (!district.TouchesMapEdge)
				{
					int num2 = debug_numTouch;
					debug_numTouch = num2 + 1;
					return false;
				}
				if (district.CellCount < minCellCount)
				{
					int num2 = debug_numDistrictCellCount;
					debug_numDistrictCellCount = num2 + 1;
					return false;
				}
				if (extraValidator != null && !extraValidator(c))
				{
					int num2 = debug_numExtraValidator;
					debug_numExtraValidator = num2 + 1;
					return false;
				}
				return true;
			};
			for (int i = tightness; i >= 1; i--)
			{
				int num = map.Size.x / i;
				IntVec3 result;
				if (CellFinderLoose.TryFindRandomNotEdgeCellWith((map.Size.x - num) / 2, validator, map, out result))
				{
					return result;
				}
			}
			if (returnInvalidOnFail)
			{
				return IntVec3.Invalid;
			}
			Log.Error(string.Concat(new string[]
			{
				"Found no good central spot. Choosing randomly. numStand=",
				debug_numStand.ToString(),
				", numDistrict=",
				debug_numDistrict.ToString(),
				", numTouch=",
				debug_numTouch.ToString(),
				", numDistrictCellCount=",
				debug_numDistrictCellCount.ToString(),
				", numExtraValidator=",
				debug_numExtraValidator.ToString()
			}));
			return CellFinderLoose.RandomCellWith((IntVec3 x) => x.Standable(map), map, 1000);
		}

		// Token: 0x06005452 RID: 21586 RVA: 0x001B1E58 File Offset: 0x001B0058
		public static bool TryFindSkyfallerCell(ThingDef skyfaller, Map map, TerrainAffordanceDef terrainAffordance, out IntVec3 cell, int minDistToEdge = 10, IntVec3 nearLoc = default(IntVec3), int nearLocMaxDist = -1, bool allowRoofedCells = true, bool allowCellsWithItems = false, bool allowCellsWithBuildings = false, bool colonyReachable = false, bool avoidColonistsIfExplosive = true, bool alwaysAvoidColonists = false, Predicate<IntVec3> extraValidator = null)
		{
			CellFinderLoose.<>c__DisplayClass9_0 CS$<>8__locals1 = new CellFinderLoose.<>c__DisplayClass9_0();
			CS$<>8__locals1.skyfaller = skyfaller;
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.terrainAffordance = terrainAffordance;
			CS$<>8__locals1.allowRoofedCells = allowRoofedCells;
			CS$<>8__locals1.allowCellsWithItems = allowCellsWithItems;
			CS$<>8__locals1.allowCellsWithBuildings = allowCellsWithBuildings;
			CS$<>8__locals1.minDistToEdge = minDistToEdge;
			CS$<>8__locals1.colonyReachable = colonyReachable;
			CS$<>8__locals1.extraValidator = extraValidator;
			CS$<>8__locals1.avoidColonists = ((avoidColonistsIfExplosive && CS$<>8__locals1.skyfaller.skyfaller.CausesExplosion) || alwaysAvoidColonists);
			if (nearLocMaxDist > 0)
			{
				return CellFinder.TryFindRandomCellNear(nearLoc, CS$<>8__locals1.map, nearLocMaxDist, new Predicate<IntVec3>(CS$<>8__locals1.<TryFindSkyfallerCell>g__Validator|0), out cell, -1);
			}
			return CellFinderLoose.TryFindRandomNotEdgeCellWith(CS$<>8__locals1.minDistToEdge, new Predicate<IntVec3>(CS$<>8__locals1.<TryFindSkyfallerCell>g__Validator|0), CS$<>8__locals1.map, out cell);
		}

		// Token: 0x06005453 RID: 21587 RVA: 0x001B1F14 File Offset: 0x001B0114
		public static IntVec3 GetFallbackDest(Pawn pawn, List<Thing> threats, float maxDistance = 40f, float minDistanceFromThreat = 0f, float minDistanceFromRoot = 0f, int maxRegions = 20, Func<IntVec3, bool> validator = null)
		{
			IntVec3 bestPos = IntVec3.Invalid;
			float bestScore = -1f;
			TraverseParms traverseParms = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
			RegionTraverser.BreadthFirstTraverse(pawn.GetRegion(RegionType.Set_Passable), (Region from, Region reg) => reg.Allows(traverseParms, false), delegate(Region reg)
			{
				reg.DangerFor(pawn);
				Map map = pawn.Map;
				foreach (IntVec3 intVec in reg.Cells)
				{
					if ((validator == null || validator(intVec)) && intVec.Standable(map))
					{
						int num = intVec.DistanceToSquared(pawn.Position);
						if ((float)num >= minDistanceFromRoot * minDistanceFromRoot && (float)num <= maxDistance * maxDistance)
						{
							Thing thing = null;
							float num2 = 0f;
							for (int i = 0; i < threats.Count; i++)
							{
								int num3 = intVec.DistanceToSquared(threats[i].Position);
								if (thing == null || (float)num3 < num2)
								{
									thing = threats[i];
									num2 = (float)num3;
								}
							}
							if (num2 >= minDistanceFromThreat * minDistanceFromThreat)
							{
								float num4 = Mathf.Sqrt(num2);
								if (!map.pawnDestinationReservationManager.CanReserve(intVec, pawn, false))
								{
									num4 *= 0.5f;
								}
								num4 += CoverUtility.TotalSurroundingCoverScore(intVec, pawn.Map);
								if (num4 > bestScore)
								{
									bestPos = intVec;
									bestScore = num4;
								}
							}
						}
					}
				}
				return false;
			}, maxRegions, RegionType.Set_Passable);
			return bestPos;
		}
	}
}
