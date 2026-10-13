using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020020B0 RID: 8368
	public static class SpaceGenUtility
	{
		// Token: 0x0600B729 RID: 46889 RVA: 0x0034B5DC File Offset: 0x003497DC
		public static void ChunksSetSparsley(Chunks chunks, FloatRange coverageRange)
		{
			IntVec3 item = chunks.rect.ContractedBy(1).Cells.RandomElement<IntVec3>();
			HashSet<IntVec3> hashSet = new HashSet<IntVec3>
			{
				item
			};
			Stack<IntVec3> stack = new Stack<IntVec3>();
			stack.Push(item);
			int num = Mathf.RoundToInt(coverageRange.RandomInRange * (float)chunks.rect.Area);
			IntVec3 intVec;
			while (stack.TryPop(out intVec) && num > 0)
			{
				num--;
				chunks.SetUsed(intVec, true);
				int num2 = Rand.Range(0, 3);
				for (int i = 0; i < 4; i++)
				{
					IntVec3 intVec2 = intVec + GenAdj.CardinalDirections[(i + num2) % 4];
					if (chunks.Contains(intVec2) && hashSet.Add(intVec2))
					{
						stack.Push(intVec2);
					}
				}
			}
		}

		// Token: 0x0600B72A RID: 46890 RVA: 0x0034B6A8 File Offset: 0x003498A8
		public static void ScatterExtrusions(Map map, Chunks chunks, IntRange? countRange = null, IntRange? lengthRange = null, IntRange? widthRange = null, IntRange? gapRange = null, List<CellRect> spaces = null)
		{
			SpaceGenUtility.<>c__DisplayClass5_0 CS$<>8__locals1 = new SpaceGenUtility.<>c__DisplayClass5_0();
			IntRange intRange = countRange ?? SpaceGenUtility.ExtrusionCountRange;
			IntRange intRange2 = lengthRange ?? SpaceGenUtility.ExtrusionLengthRange;
			IntRange intRange3 = widthRange ?? SpaceGenUtility.ExtrusionWidthRange;
			IntRange intRange4 = gapRange ?? SpaceGenUtility.ExtrusionGapRange;
			CS$<>8__locals1.used = new HashSet<IntVec3>();
			int randomInRange = intRange.RandomInRange;
			CellRect otherRect = map.BoundsRect(6);
			int num = 0;
			ValueTuple<IntVec3, Rot4> valueTuple;
			while (num < randomInRange && chunks.TryGetFreeEdge(out valueTuple, true, 1, new Predicate<IntVec3>(CS$<>8__locals1.<ScatterExtrusions>g__Validator|0)))
			{
				CellRect cellRect = chunks[valueTuple.Item1];
				Rot4 item = valueTuple.Item2;
				CS$<>8__locals1.used.Add(valueTuple.Item1);
				CS$<>8__locals1.used.Add(valueTuple.Item1 + item.FacingCell);
				Rot4 rot = item.Rotated(RotationDirection.Clockwise);
				IntVec3 centerCellOnEdge = cellRect.GetCenterCellOnEdge(item);
				int i = chunks.regionSize / 2 - 4;
				CellRect cellRect2 = CellRect.FromLimits(centerCellOnEdge - rot.FacingCell * i - item.FacingCell * 10, centerCellOnEdge + rot.FacingCell * i);
				IntVec3 intVec = centerCellOnEdge - rot.FacingCell * i - item.FacingCell * 10;
				int num2 = 0;
				while (cellRect2.Contains(intVec))
				{
					int num3 = Mathf.Max(intRange3.RandomInRange, 0);
					int num4 = Mathf.Max(intRange2.RandomInRange, 1);
					if (num4 == num2)
					{
						num4 = Mathf.Max(intRange2.RandomInRange, 1);
					}
					IntVec3 second = intVec + rot.FacingCell * num3 + item.FacingCell * (num4 + 10);
					CellRect item2 = CellRect.FromLimits(intVec, second).ClipInsideRect(otherRect);
					foreach (IntVec3 c in item2.Cells)
					{
						map.terrainGrid.SetTerrain(c, TerrainDefOf.OrbitalPlatform);
					}
					if (spaces != null)
					{
						spaces.Add(item2);
					}
					intVec += rot.FacingCell * (intRange4.RandomInRange + num3 + 1);
				}
				num++;
			}
		}

		// Token: 0x0600B72B RID: 46891 RVA: 0x0034B960 File Offset: 0x00349B60
		public static bool NoIntersection(CellRect rect, List<CellRect> spaces, IntRange holeRange)
		{
			rect = rect.ExpandedBy(holeRange.max * 2);
			foreach (CellRect cellRect in spaces)
			{
				if (cellRect.Overlaps(rect))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B72C RID: 46892 RVA: 0x0034B9CC File Offset: 0x00349BCC
		public static void GenerateHole(Map map, CellRect rect)
		{
			foreach (IntVec3 c in rect.Cells)
			{
				List<Thing> thingList = c.GetThingList(map);
				for (int i = thingList.Count - 1; i >= 0; i--)
				{
					thingList[i].Destroy(DestroyMode.Vanish);
				}
				map.terrainGrid.SetTerrain(c, TerrainDefOf.Space);
				map.roofGrid.SetRoof(c, null);
			}
		}

		// Token: 0x0600B72D RID: 46893 RVA: 0x0034BA5C File Offset: 0x00349C5C
		public static void DamageEdges(Map map, Chunks chunks, int maximum, ref int cells)
		{
			int num = 4;
			while (cells > maximum && num-- > 0)
			{
				SpaceGenUtility.GenerateEdgeCut(map, chunks, chunks.RandomUsedChunk, SpaceGenUtility.EdgeHoleHalfLength, SpaceGenUtility.EdgeHoleDepth, ref cells, null, null, null, null, 1f, 20);
			}
		}

		// Token: 0x0600B72E RID: 46894 RVA: 0x0034BAB0 File Offset: 0x00349CB0
		public static void GenerateEdgeCut(Map map, Chunks chunks, IntVec3 chunk, IntRange halfLengthRange, IntRange depthRange, ref int cells, Predicate<CellRect> validator = null, List<CellRect> spaces = null, IntRange? lengthRange = null, IntRange? offsetRange = null, float extendChance = 1f, int borderPadding = 20)
		{
			ValueTuple<IntVec3, Rot4> valueTuple;
			if (!chunks.TryGetFreeEdge(out valueTuple, false, 1, null))
			{
				return;
			}
			CellRect cellRect = chunks[chunk];
			Rot4 item = valueTuple.Item2;
			Rot4 rot = item.Rotated(RotationDirection.Clockwise);
			IntVec3 a = cellRect.GetCellsOnEdge(item).RandomElement<IntVec3>();
			IntVec3 a2 = a + rot.FacingCell * halfLengthRange.RandomInRange;
			IntVec3 a3 = a - rot.FacingCell * halfLengthRange.RandomInRange;
			IntVec3 b = -item.FacingCell * depthRange.RandomInRange;
			IntVec3 b2 = -item.FacingCell * borderPadding;
			CellRect cellRect2 = CellRect.FromLimits(a2 - b2, a3 + b).ClipInsideMap(map);
			if (validator != null && !validator(cellRect2))
			{
				return;
			}
			SpaceGenUtility.GenerateHole(map, cellRect2);
			IntRange lengthRange2 = lengthRange ?? SpaceGenUtility.HoleLumpLengthRange;
			IntRange offsetRange2 = offsetRange ?? SpaceGenUtility.HoleLumpOffsetRange;
			cells -= MapGenUtility.DoRectEdgeLumps(map, TerrainDefOf.Space, ref cellRect2, lengthRange2, offsetRange2, null);
			cells -= cellRect2.Area;
			if (Rand.Chance(extendChance))
			{
				SpaceGenUtility.GenerateExtraDamage(map, ref cellRect2, SpaceGenUtility.LargeHoleRange, ref cells, validator, spaces);
			}
			if (spaces != null)
			{
				spaces.Add(cellRect2);
			}
		}

		// Token: 0x0600B72F RID: 46895 RVA: 0x0034BC1C File Offset: 0x00349E1C
		public static void GenerateExtraDamage(Map map, ref CellRect rect, IntRange sizeRange, ref int cells, Predicate<CellRect> validator = null, List<CellRect> spaces = null)
		{
			if (rect.IsEmpty)
			{
				return;
			}
			IntVec3 intVec = rect.Corners.MaxBy((IntVec3 c) => c.DistanceToEdge(map));
			Rot4 rot;
			Rot4 rot2;
			if (rect.Width >= rect.Height)
			{
				rot = ((intVec.z >= rect.CenterCell.z) ? Rot4.North : Rot4.South);
				rot2 = ((intVec.x >= rect.CenterCell.x) ? Rot4.West : Rot4.East);
			}
			else
			{
				rot = ((intVec.x >= rect.CenterCell.x) ? Rot4.East : Rot4.West);
				rot2 = ((intVec.z >= rect.CenterCell.z) ? Rot4.South : Rot4.North);
			}
			IntVec3 first = intVec + rot2.FacingCell * sizeRange.RandomInRange;
			IntVec3 second = intVec + rot.FacingCell * sizeRange.RandomInRange;
			CellRect cellRect = CellRect.FromLimits(first, second).ClipInsideMap(map);
			if (validator != null && !validator(cellRect.ExpandedBy(SpaceGenUtility.HoleLumpOffsetRange.max)))
			{
				return;
			}
			SpaceGenUtility.GenerateHole(map, cellRect);
			cells -= MapGenUtility.DoRectEdgeLumps(map, TerrainDefOf.Space, ref cellRect, SpaceGenUtility.HoleLumpLengthRange, SpaceGenUtility.HoleLumpOffsetRange, null);
			cells -= cellRect.Area;
			if (spaces != null)
			{
				spaces.Add(cellRect);
			}
		}

		// Token: 0x0600B730 RID: 46896 RVA: 0x0034BD9C File Offset: 0x00349F9C
		public static void DamageHoles(Chunks chunks, Map map, int maximum, ref int cells, IntRange? sizeRange = null, FloatRange? per100Range = null, float extendChance = 1f, Predicate<CellRect> validator = null, List<CellRect> spaces = null)
		{
			IntRange intRange = sizeRange ?? SpaceGenUtility.HoleSizeRange;
			FloatRange floatRange = per100Range ?? SpaceGenUtility.HolesPerHundredCellsRange;
			int num = Mathf.RoundToInt(floatRange.RandomInRange * (float)cells / 100f);
			int num2 = 0;
			Predicate<CellRect> <>9__0;
			while (num2 < num && cells > maximum)
			{
				IntVec2 intVec = new IntVec2(intRange.RandomInRange, intRange.RandomInRange);
				IntVec2 size = intVec;
				Predicate<CellRect> validator2;
				if ((validator2 = <>9__0) == null)
				{
					validator2 = (<>9__0 = delegate(CellRect r)
					{
						Predicate<CellRect> validator4 = validator;
						return validator4 == null || validator4(r.ExpandedBy(SpaceGenUtility.HoleLumpOffsetRange.max));
					});
				}
				CellRect cellRect;
				if (!SpaceGenUtility.TryGetRect(chunks, size, out cellRect, validator2))
				{
					break;
				}
				SpaceGenUtility.GenerateHole(map, cellRect);
				cells -= MapGenUtility.DoRectEdgeLumps(map, TerrainDefOf.Space, ref cellRect, SpaceGenUtility.HoleLumpLengthRange, SpaceGenUtility.HoleLumpOffsetRange, null);
				cells -= cellRect.Area;
				if (spaces != null)
				{
					spaces.Add(cellRect);
				}
				num2++;
			}
			int num3 = Mathf.RoundToInt(floatRange.RandomInRange * (float)cells / 100f);
			int num4 = 0;
			Predicate<CellRect> <>9__1;
			while (num4 < num3 && cells > maximum)
			{
				IntVec2 intVec2 = new IntVec2(SpaceGenUtility.LargeHoleRange.RandomInRange, SpaceGenUtility.LargeHoleRange.RandomInRange);
				IntVec2 size2 = intVec2;
				Predicate<CellRect> validator3;
				if ((validator3 = <>9__1) == null)
				{
					validator3 = (<>9__1 = delegate(CellRect r)
					{
						Predicate<CellRect> validator4 = validator;
						return validator4 == null || validator4(r.ExpandedBy(SpaceGenUtility.HoleLumpOffsetRange.max));
					});
				}
				CellRect cellRect2;
				if (!SpaceGenUtility.TryGetRect(chunks, size2, out cellRect2, validator3))
				{
					break;
				}
				SpaceGenUtility.GenerateHole(map, cellRect2);
				cells -= MapGenUtility.DoRectEdgeLumps(map, TerrainDefOf.Space, ref cellRect2, SpaceGenUtility.HoleLumpLengthRange, SpaceGenUtility.HoleLumpOffsetRange, null);
				cells -= cellRect2.Area;
				if (Rand.Chance(extendChance))
				{
					SpaceGenUtility.GenerateExtraDamage(map, ref cellRect2, SpaceGenUtility.LargeHoleRange, ref cells, validator, spaces);
				}
				if (spaces != null)
				{
					spaces.Add(cellRect2);
				}
				num4++;
			}
		}

		// Token: 0x0600B731 RID: 46897 RVA: 0x0034BF84 File Offset: 0x0034A184
		public static bool TryGetRect(Chunks chunks, IntVec2 size, out CellRect rect, Predicate<CellRect> validator = null)
		{
			foreach (CellRect cellRect in chunks.EnumeratedRects.InRandomOrder(null))
			{
				if (cellRect.Size.x >= size.x && cellRect.Size.z >= size.z && cellRect.TryFindRandomInnerRect(size, out rect, validator))
				{
					return true;
				}
			}
			rect = default(CellRect);
			return false;
		}

		// Token: 0x0600B732 RID: 46898 RVA: 0x0034C014 File Offset: 0x0034A214
		public static void ScatterPrefabs(Map map, Chunks chunks, Faction faction, List<PrefabParms> prefabs, Predicate<CellRect> validator = null, FloatRange? prefabsPer100K = null)
		{
			SpaceGenUtility.<>c__DisplayClass21_0 CS$<>8__locals1 = new SpaceGenUtility.<>c__DisplayClass21_0();
			CS$<>8__locals1.validator = validator;
			CS$<>8__locals1.map = map;
			FloatRange floatRange = prefabsPer100K ?? SpaceGenUtility.PrefabsPerHundredCellsRange;
			float num = Mathf.Round((float)chunks.ApproximateArea / 100f * floatRange.RandomInRange);
			Rot4 random = Rot4.Random;
			int num2 = 0;
			while ((float)num2 < num)
			{
				SpaceGenUtility.<>c__DisplayClass21_1 CS$<>8__locals2 = new SpaceGenUtility.<>c__DisplayClass21_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.prefab = prefabs.RandomElementByWeight((PrefabParms w) => w.weight);
				int num3 = Mathf.Max(CS$<>8__locals2.prefab.def.size.x, CS$<>8__locals2.prefab.def.size.z);
				IntVec2 size = new IntVec2(num3, num3);
				CS$<>8__locals2.lRot = random;
				CellRect cellRect;
				if (SpaceGenUtility.TryGetRect(chunks, size, out cellRect, new Predicate<CellRect>(CS$<>8__locals2.<ScatterPrefabs>g__Validator|2)))
				{
					PrefabUtility.SpawnPrefab(CS$<>8__locals2.prefab.def, CS$<>8__locals2.CS$<>8__locals1.map, cellRect.CenterCell, random, faction, null, null, new Action<Thing>(SpaceGenUtility.<ScatterPrefabs>g__OnSpawned|21_1), false);
				}
				random = Rot4.Random;
				num2++;
			}
		}

		// Token: 0x0600B733 RID: 46899 RVA: 0x0034C160 File Offset: 0x0034A360
		public static void GenerateRing(Map map, CellRect rect, TerrainDef terrain, int distance, int width, float ringWidth = 4.9f, float ringExtraOffset = 0.5f, float ringExtraChance = 0f)
		{
			IntVec3 centerCell = rect.CenterCell;
			Vector3 b = centerCell.ToVector3();
			int num = distance + width;
			Vector3 a = new Vector3((float)(rect.minX - num) - ringWidth, 0f, (float)centerCell.z);
			Vector3 a2 = new Vector3((float)(rect.minX - num) - ringWidth - ringExtraOffset, 0f, (float)centerCell.z);
			Vector3 a3 = new Vector3((float)(rect.minX - num), 0f, (float)centerCell.z);
			Vector3 a4 = new Vector3((float)(rect.minX - num) + ringExtraOffset, 0f, (float)centerCell.z);
			float sqrMagnitude = (a - b).sqrMagnitude;
			float sqrMagnitude2 = (a3 - b).sqrMagnitude;
			float sqrMagnitude3 = (a2 - b).sqrMagnitude;
			float sqrMagnitude4 = (a4 - b).sqrMagnitude;
			foreach (IntVec3 intVec in map.AllCells)
			{
				float sqrMagnitude5 = (intVec - map.Center).SqrMagnitude;
				if (intVec.GetTerrain(map) == TerrainDefOf.Space && ((sqrMagnitude5 >= sqrMagnitude2 && sqrMagnitude5 <= sqrMagnitude) || (Rand.Chance(ringExtraChance) && sqrMagnitude5 >= sqrMagnitude4 && sqrMagnitude5 <= sqrMagnitude3)))
				{
					map.terrainGrid.SetTerrain(intVec, terrain);
				}
			}
		}

		// Token: 0x0600B734 RID: 46900 RVA: 0x0034C2D8 File Offset: 0x0034A4D8
		public static CellRect GenerateConnectedPlatform(Map map, TerrainDef terrain, CellRect platformRect, IntRange widthRange, IntRange heightRange, Rot4 dir, int distance = 14, float lumpChance = 0.2f, int minHeight = 2, IntRange? connectorHeightRange = null, IntRange? connectorGapRange = null, IntRange? lumpOffsetRange = null, IntRange? lumpLengthRange = null)
		{
			Rot4 rot = dir.Rotated(RotationDirection.Clockwise);
			IntVec3 corner = platformRect.GetCorner(dir, true);
			int i = platformRect.GetSideLength(dir) - heightRange.max;
			IntVec3 intVec = CellRect.FromLimits(corner - rot.FacingCell * i, corner - rot.FacingCell * 10).RandomCell + dir.FacingCell * distance;
			IntVec3 second = intVec - rot.FacingCell * heightRange.RandomInRange + dir.FacingCell * widthRange.RandomInRange;
			CellRect cellRect = CellRect.FromLimits(intVec, second).ClipInsideMap(map);
			SpaceGenUtility.SpawnWalkways(map, terrain, cellRect, dir.Opposite, platformRect.GetSideLength(rot) / 2 + distance, lumpChance, minHeight, connectorHeightRange, connectorGapRange, lumpOffsetRange, lumpLengthRange);
			foreach (IntVec3 c in cellRect)
			{
				if (c.GetTerrain(map) == TerrainDefOf.Space)
				{
					map.terrainGrid.SetTerrain(c, terrain);
				}
			}
			foreach (IntVec3 loc in cellRect.ContractedBy(2).Corners)
			{
				GenSpawn.Spawn(ThingDefOf.AncientShipBeacon, loc, map, WipeMode.Vanish);
			}
			MapGenUtility.DoRectEdgeLumps(map, terrain, ref cellRect, SpaceGenUtility.DefaultPadBorderLumpLengthRange, SpaceGenUtility.DefaultPadBorderLumpOffsetRange, null);
			return cellRect;
		}

		// Token: 0x0600B735 RID: 46901 RVA: 0x0034C488 File Offset: 0x0034A688
		public static void SpawnWalkways(Map map, TerrainDef terrain, CellRect rect, Rot4 dir, int maxDistance, float lumpChance = 0.2f, int minHeight = 2, IntRange? connectorHeightRange = null, IntRange? connectorGapRange = null, IntRange? lumpOffsetRange = null, IntRange? lumpLengthRange = null)
		{
			IntVec3 facingCell = dir.Rotated(RotationDirection.Counterclockwise).FacingCell;
			IntRange intRange = connectorHeightRange ?? SpaceGenUtility.DefaultConnectorHeightRange;
			IntRange intRange2 = connectorGapRange ?? SpaceGenUtility.DefaultConnectorGapRange;
			IntRange intRange3 = lumpOffsetRange ?? SpaceGenUtility.DefaultLumpOffsetRange;
			IntRange intRange4 = lumpLengthRange ?? SpaceGenUtility.DefaultLumpLengthRange;
			CellRect cellRect = rect.GetEdgeRect(dir).ContractedBy(dir.IsVertical ? intRange.max : 0, dir.IsHorizontal ? intRange.max : 0);
			IntVec3 intVec = cellRect.GetCorner(dir, true) + facingCell;
			while (cellRect.Contains(intVec))
			{
				int randomInRange = intRange.RandomInRange;
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				IntVec3 intVec2 = intVec;
				while (CellRect.FromLimits(intVec, intVec + dir.FacingCell * (maxDistance + 2)).Contains(intVec2))
				{
					if (num <= 0 && Rand.Chance(lumpChance))
					{
						num = intRange4.RandomInRange;
						num2 = intRange3.RandomInRange;
						if (randomInRange + num2 < minHeight)
						{
							num2 = 0;
						}
						if (num2 > num3)
						{
							num3 = num2;
						}
					}
					bool flag = false;
					for (int i = -num2; i < randomInRange + num2; i++)
					{
						IntVec3 c = intVec2 + facingCell * i;
						if (c.InBounds(map) && c.GetTerrain(map) == TerrainDefOf.Space)
						{
							map.terrainGrid.SetTerrain(c, terrain);
							flag = true;
						}
					}
					if (!flag)
					{
						break;
					}
					if (num-- <= 0)
					{
						num2 = 0;
					}
					intVec2 += dir.FacingCell;
				}
				intVec += facingCell * (randomInRange + intRange2.RandomInRange + num3);
			}
		}

		// Token: 0x0600B737 RID: 46903 RVA: 0x0034C780 File Offset: 0x0034A980
		[CompilerGenerated]
		internal static void <ScatterPrefabs>g__OnSpawned|21_1(Thing thing)
		{
			CompPowerBattery compPowerBattery;
			if (thing.TryGetComp(out compPowerBattery))
			{
				compPowerBattery.SetStoredEnergyPct(1f);
			}
		}

		// Token: 0x04007D9B RID: 32155
		private static readonly IntRange ExtrusionCountRange = new IntRange(4, 8);

		// Token: 0x04007D9C RID: 32156
		private static readonly IntRange ExtrusionLengthRange = new IntRange(8, 15);

		// Token: 0x04007D9D RID: 32157
		private static readonly IntRange ExtrusionWidthRange = new IntRange(-1, 3);

		// Token: 0x04007D9E RID: 32158
		private static readonly IntRange ExtrusionGapRange = new IntRange(0, 2);

		// Token: 0x04007D9F RID: 32159
		private static readonly IntRange EdgeHoleHalfLength = new IntRange(3, 5);

		// Token: 0x04007DA0 RID: 32160
		private static readonly IntRange EdgeHoleDepth = new IntRange(5, 10);

		// Token: 0x04007DA1 RID: 32161
		private static readonly IntRange LargeHoleRange = new IntRange(4, 6);

		// Token: 0x04007DA2 RID: 32162
		private static readonly IntRange HoleLumpLengthRange = new IntRange(3, 5);

		// Token: 0x04007DA3 RID: 32163
		private static readonly IntRange HoleLumpOffsetRange = new IntRange(-1, 2);

		// Token: 0x04007DA4 RID: 32164
		private static readonly IntRange HoleSizeRange = new IntRange(6, 12);

		// Token: 0x04007DA5 RID: 32165
		private static readonly FloatRange HolesPerHundredCellsRange = new FloatRange(0.05f, 0.05f);

		// Token: 0x04007DA6 RID: 32166
		private static readonly FloatRange PrefabsPerHundredCellsRange = new FloatRange(0.01f, 0.15f);

		// Token: 0x04007DA7 RID: 32167
		private static readonly IntRange DefaultPadBorderLumpLengthRange = new IntRange(6, 10);

		// Token: 0x04007DA8 RID: 32168
		private static readonly IntRange DefaultPadBorderLumpOffsetRange = new IntRange(0, 0);

		// Token: 0x04007DA9 RID: 32169
		private static readonly IntRange DefaultConnectorHeightRange = new IntRange(3, 3);

		// Token: 0x04007DAA RID: 32170
		private static readonly IntRange DefaultConnectorGapRange = new IntRange(2, 3);

		// Token: 0x04007DAB RID: 32171
		private static readonly IntRange DefaultLumpOffsetRange = new IntRange(0, 0);

		// Token: 0x04007DAC RID: 32172
		private static readonly IntRange DefaultLumpLengthRange = new IntRange(3, 6);
	}
}
