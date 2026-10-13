using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02002096 RID: 8342
	public static class MapGenCavesUtility
	{
		// Token: 0x0600B69C RID: 46748 RVA: 0x003479E8 File Offset: 0x00345BE8
		public static void GenerateCaves(Map map, BoolGrid visited, List<IntVec3> group, ModuleBase directionNoise, MapGenCavesUtility.CaveGenParms parms, Predicate<IntVec3> isRock)
		{
			Action<IntVec3> <>9__0;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (!visited[intVec] && isRock(intVec))
				{
					group.Clear();
					FloodFiller floodFiller = map.floodFiller;
					IntVec3 root = intVec;
					Action<IntVec3> processor;
					if ((processor = <>9__0) == null)
					{
						processor = (<>9__0 = delegate(IntVec3 x)
						{
							visited[x] = true;
							group.Add(x);
						});
					}
					floodFiller.FloodFill(root, isRock, processor, int.MaxValue, false, null);
					MapGenCavesUtility.Trim(group, map);
					MapGenCavesUtility.RemoveSmallDisconnectedSubGroups(group, map, parms);
					if (group.Count >= parms.minRocksToGenerateAnyTunnel)
					{
						MapGenCavesUtility.DoOpenTunnels(group, map, directionNoise, parms, isRock);
						MapGenCavesUtility.DoClosedTunnels(group, map, directionNoise, parms, isRock);
					}
				}
			}
		}

		// Token: 0x0600B69D RID: 46749 RVA: 0x00347AF8 File Offset: 0x00345CF8
		private static void DoOpenTunnels(List<IntVec3> group, Map map, ModuleBase directionNoise, MapGenCavesUtility.CaveGenParms parms, Predicate<IntVec3> isRock)
		{
			int num = GenMath.RoundRandom((float)group.Count * Rand.Range(0.9f, 1.1f) * parms.openTunnelsPer10k / 10000f);
			num = Mathf.Min(num, parms.maxOpenTunnelsPerRockGroup);
			if (num > 0)
			{
				num = Rand.RangeInclusive(1, num);
			}
			float num2 = parms.tunnelsWidthPerRockCount.Evaluate((float)group.Count);
			for (int i = 0; i < num; i++)
			{
				IntVec3 start = IntVec3.Invalid;
				float num3 = -1f;
				float dir = -1f;
				float num4 = -1f;
				for (int j = 0; j < 10; j++)
				{
					IntVec3 intVec = MapGenCavesUtility.FindRandomEdgeCellForTunnel(group, map);
					float distToCave = MapGenCavesUtility.GetDistToCave(intVec, group, map, 40f, false);
					float num6;
					float num5 = MapGenCavesUtility.FindBestInitialDir(intVec, group, out num6);
					if (!start.IsValid || distToCave > num3 || (Mathf.Approximately(distToCave, num3) && num6 > num4))
					{
						start = intVec;
						num3 = distToCave;
						dir = num5;
						num4 = num6;
					}
				}
				float width = Rand.Range(num2 * 0.8f, num2);
				MapGenCavesUtility.Dig(start, dir, width, group, map, false, directionNoise, parms, isRock, null);
			}
		}

		// Token: 0x0600B69E RID: 46750 RVA: 0x00347C0F File Offset: 0x00345E0F
		private static void Trim(List<IntVec3> group, Map map)
		{
			GenMorphology.Open(group, 6, map);
		}

		// Token: 0x0600B69F RID: 46751 RVA: 0x00347C1C File Offset: 0x00345E1C
		private static void DoClosedTunnels(List<IntVec3> group, Map map, ModuleBase directionNoise, MapGenCavesUtility.CaveGenParms parms, Predicate<IntVec3> isRock)
		{
			int num = GenMath.RoundRandom((float)group.Count * Rand.Range(0.9f, 1.1f) * parms.closedTunnelsPer10k / 10000f);
			num = Mathf.Min(num, parms.maxClosedTunnelsPerRockGroup);
			if (num > 0)
			{
				num = Rand.RangeInclusive(0, num);
			}
			float num2 = parms.tunnelsWidthPerRockCount.Evaluate((float)group.Count);
			for (int i = 0; i < num; i++)
			{
				IntVec3 start = IntVec3.Invalid;
				float num3 = -1f;
				for (int j = 0; j < 7; j++)
				{
					IntVec3 intVec = group.RandomElement<IntVec3>();
					float distToCave = MapGenCavesUtility.GetDistToCave(intVec, group, map, 30f, true);
					if (!start.IsValid || distToCave > num3)
					{
						start = intVec;
						num3 = distToCave;
					}
				}
				float width = Rand.Range(num2 * 0.8f, num2);
				MapGenCavesUtility.Dig(start, Rand.Range(0f, 360f), width, group, map, true, directionNoise, parms, isRock, null);
			}
		}

		// Token: 0x0600B6A0 RID: 46752 RVA: 0x00347D08 File Offset: 0x00345F08
		public static IntVec3 FindRandomEdgeCellForTunnel(List<IntVec3> group, Map map)
		{
			MapGenFloatGrid caves = MapGenerator.Caves;
			IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
			MapGenCavesUtility.tmpCells.Clear();
			MapGenCavesUtility.tmpGroupSet.Clear();
			MapGenCavesUtility.tmpGroupSet.AddRange(group);
			for (int i = 0; i < group.Count; i++)
			{
				if (group[i].DistanceToEdge(map) >= 3 && caves[group[i]] <= 0f)
				{
					for (int j = 0; j < 4; j++)
					{
						IntVec3 item = group[i] + cardinalDirections[j];
						if (!MapGenCavesUtility.tmpGroupSet.Contains(item))
						{
							MapGenCavesUtility.tmpCells.Add(group[i]);
							break;
						}
					}
				}
			}
			if (!MapGenCavesUtility.tmpCells.Any<IntVec3>())
			{
				Log.Warning("Could not find any valid edge cell.");
				return group.RandomElement<IntVec3>();
			}
			return MapGenCavesUtility.tmpCells.RandomElement<IntVec3>();
		}

		// Token: 0x0600B6A1 RID: 46753 RVA: 0x00347DE0 File Offset: 0x00345FE0
		public static float FindBestInitialDir(IntVec3 start, List<IntVec3> group, out float dist)
		{
			float num = (float)MapGenCavesUtility.GetDistToNonRock(start, group, IntVec3.East, 40);
			float num2 = (float)MapGenCavesUtility.GetDistToNonRock(start, group, IntVec3.West, 40);
			float num3 = (float)MapGenCavesUtility.GetDistToNonRock(start, group, IntVec3.South, 40);
			float num4 = (float)MapGenCavesUtility.GetDistToNonRock(start, group, IntVec3.North, 40);
			float num5 = (float)MapGenCavesUtility.GetDistToNonRock(start, group, IntVec3.NorthWest, 40);
			float num6 = (float)MapGenCavesUtility.GetDistToNonRock(start, group, IntVec3.NorthEast, 40);
			float num7 = (float)MapGenCavesUtility.GetDistToNonRock(start, group, IntVec3.SouthWest, 40);
			float num8 = (float)MapGenCavesUtility.GetDistToNonRock(start, group, IntVec3.SouthEast, 40);
			dist = Mathf.Max(new float[]
			{
				num,
				num2,
				num3,
				num4,
				num5,
				num6,
				num7,
				num8
			});
			return GenMath.MaxByRandomIfEqual<float>(0f, num + num8 / 2f + num6 / 2f, 45f, num8 + num3 / 2f + num / 2f, 90f, num3 + num8 / 2f + num7 / 2f, 135f, num7 + num3 / 2f + num2 / 2f, 180f, num2 + num7 / 2f + num5 / 2f, 225f, num5 + num4 / 2f + num2 / 2f, 270f, num4 + num6 / 2f + num5 / 2f, 315f, num6 + num4 / 2f + num / 2f, 0.0001f);
		}

		// Token: 0x0600B6A2 RID: 46754 RVA: 0x00347F68 File Offset: 0x00346168
		public static void Dig(IntVec3 start, float dir, float width, List<IntVec3> group, Map map, bool closed, ModuleBase directionNoise, MapGenCavesUtility.CaveGenParms parms, Predicate<IntVec3> isRock, HashSet<IntVec3> visited = null)
		{
			Vector3 vector = start.ToVector3Shifted();
			IntVec3 intVec = start;
			float num = 0f;
			MapGenFloatGrid caves = MapGenerator.Caves;
			bool flag = false;
			bool flag2 = false;
			ModuleBase moduleBase = new Perlin((double)parms.widthNoiseFrequency, 2.0, 0.5, 4, Rand.Int, QualityMode.Medium);
			if (visited == null)
			{
				visited = new HashSet<IntVec3>();
			}
			MapGenCavesUtility.tmpGroupSet.Clear();
			MapGenCavesUtility.tmpGroupSet.AddRange(group);
			int num2 = 0;
			for (;;)
			{
				if (closed)
				{
					int num3 = GenRadial.NumCellsInRadius(width / 2f + 1.5f);
					for (int i = 0; i < num3; i++)
					{
						IntVec3 intVec2 = intVec + GenRadial.RadialPattern[i];
						if (!visited.Contains(intVec2) && (!MapGenCavesUtility.tmpGroupSet.Contains(intVec2) || caves[intVec2] > 0f))
						{
							return;
						}
					}
				}
				if (num2 >= parms.allowBranchingAfterThisManyCells && width > parms.minTunnelWidth + parms.branchedTunnelWidthOffset.max)
				{
					if (!flag && Rand.Chance(parms.branchChance))
					{
						MapGenCavesUtility.DigInBestDirection(intVec, dir, new FloatRange(40f, 90f), width - parms.branchedTunnelWidthOffset.RandomInRange, group, map, closed, directionNoise, parms, isRock, visited);
						flag = true;
					}
					if (!flag2 && Rand.Chance(parms.branchChance))
					{
						MapGenCavesUtility.DigInBestDirection(intVec, dir, new FloatRange(-90f, -40f), width - parms.branchedTunnelWidthOffset.RandomInRange, group, map, closed, directionNoise, parms, isRock, visited);
						flag2 = true;
					}
				}
				float tunnelWidth = Mathf.Max(width + moduleBase.GetValue(intVec) * parms.widthNoiseAmplitude, 1f);
				bool flag3;
				MapGenCavesUtility.SetCaveAround(intVec, tunnelWidth, map, visited, out flag3, isRock);
				if (flag3)
				{
					return;
				}
				while (vector.ToIntVec3() == intVec)
				{
					vector += Vector3Utility.FromAngleFlat(dir) * 0.5f;
					num += 0.5f;
				}
				if (!MapGenCavesUtility.tmpGroupSet.Contains(vector.ToIntVec3()))
				{
					return;
				}
				IntVec3 intVec3 = new IntVec3(intVec.x, 0, vector.ToIntVec3().z);
				if (isRock(intVec3))
				{
					caves[intVec3] = Mathf.Max(caves[intVec3], width);
					visited.Add(intVec3);
				}
				intVec = vector.ToIntVec3();
				dir += (float)directionNoise.GetValue((double)(num * 60f), (double)((float)start.x * 200f), (double)((float)start.z * 200f)) * parms.directionChangeSpeed;
				width -= parms.widthOffsetPerCell;
				if (width < parms.minTunnelWidth)
				{
					return;
				}
				num2++;
			}
		}

		// Token: 0x0600B6A3 RID: 46755 RVA: 0x00348210 File Offset: 0x00346410
		private static void DigInBestDirection(IntVec3 curIntVec, float curDir, FloatRange dirOffset, float width, List<IntVec3> group, Map map, bool closed, ModuleBase directionNoise, MapGenCavesUtility.CaveGenParms parms, Predicate<IntVec3> isRock, HashSet<IntVec3> visited = null)
		{
			int num = -1;
			float dir = -1f;
			for (int i = 0; i < 6; i++)
			{
				float num2 = curDir + dirOffset.RandomInRange;
				int distToNonRock = MapGenCavesUtility.GetDistToNonRock(curIntVec, group, num2, 50);
				if (distToNonRock > num)
				{
					num = distToNonRock;
					dir = num2;
				}
			}
			if (num < 18)
			{
				return;
			}
			MapGenCavesUtility.Dig(curIntVec, dir, width, group, map, closed, directionNoise, parms, isRock, visited);
		}

		// Token: 0x0600B6A4 RID: 46756 RVA: 0x00348270 File Offset: 0x00346470
		private static void SetCaveAround(IntVec3 around, float tunnelWidth, Map map, HashSet<IntVec3> visited, out bool hitAnotherTunnel, Predicate<IntVec3> isRock)
		{
			hitAnotherTunnel = false;
			int num = GenRadial.NumCellsInRadius(tunnelWidth / 2f);
			MapGenFloatGrid caves = MapGenerator.Caves;
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = around + GenRadial.RadialPattern[i];
				if (isRock(intVec))
				{
					if (caves[intVec] > 0f && !visited.Contains(intVec))
					{
						hitAnotherTunnel = true;
					}
					caves[intVec] = Mathf.Max(caves[intVec], tunnelWidth);
					visited.Add(intVec);
				}
			}
		}

		// Token: 0x0600B6A5 RID: 46757 RVA: 0x003482F4 File Offset: 0x003464F4
		private static int GetDistToNonRock(IntVec3 from, List<IntVec3> group, IntVec3 offset, int maxDist)
		{
			MapGenCavesUtility.groupSet.Clear();
			MapGenCavesUtility.groupSet.AddRange(group);
			for (int i = 0; i <= maxDist; i++)
			{
				IntVec3 item = from + offset * i;
				if (!MapGenCavesUtility.groupSet.Contains(item))
				{
					return i;
				}
			}
			return maxDist;
		}

		// Token: 0x0600B6A6 RID: 46758 RVA: 0x00348340 File Offset: 0x00346540
		private static int GetDistToNonRock(IntVec3 from, List<IntVec3> group, float dir, int maxDist)
		{
			MapGenCavesUtility.groupSet.Clear();
			MapGenCavesUtility.groupSet.AddRange(group);
			Vector3 a = Vector3Utility.FromAngleFlat(dir);
			for (int i = 0; i <= maxDist; i++)
			{
				IntVec3 item = (from.ToVector3Shifted() + a * (float)i).ToIntVec3();
				if (!MapGenCavesUtility.groupSet.Contains(item))
				{
					return i;
				}
			}
			return maxDist;
		}

		// Token: 0x0600B6A7 RID: 46759 RVA: 0x003483A0 File Offset: 0x003465A0
		public static float GetDistToCave(IntVec3 cell, List<IntVec3> group, Map map, float maxDist, bool treatOpenSpaceAsCave)
		{
			MapGenFloatGrid caves = MapGenerator.Caves;
			MapGenCavesUtility.tmpGroupSet.Clear();
			MapGenCavesUtility.tmpGroupSet.AddRange(group);
			int num = GenRadial.NumCellsInRadius(maxDist);
			IntVec3[] radialPattern = GenRadial.RadialPattern;
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = cell + radialPattern[i];
				if ((treatOpenSpaceAsCave && !MapGenCavesUtility.tmpGroupSet.Contains(intVec)) || (intVec.InBounds(map) && caves[intVec] > 0f))
				{
					return cell.DistanceTo(intVec);
				}
			}
			return maxDist;
		}

		// Token: 0x0600B6A8 RID: 46760 RVA: 0x00348428 File Offset: 0x00346628
		private static void RemoveSmallDisconnectedSubGroups(List<IntVec3> group, Map map, MapGenCavesUtility.CaveGenParms parms)
		{
			MapGenCavesUtility.groupSet.Clear();
			MapGenCavesUtility.groupSet.AddRange(group);
			MapGenCavesUtility.groupVisited.Clear();
			for (int i = 0; i < group.Count; i++)
			{
				if (!MapGenCavesUtility.groupVisited.Contains(group[i]) && MapGenCavesUtility.groupSet.Contains(group[i]))
				{
					MapGenCavesUtility.subGroup.Clear();
					map.floodFiller.FloodFill(group[i], (IntVec3 x) => MapGenCavesUtility.groupSet.Contains(x), delegate(IntVec3 x)
					{
						MapGenCavesUtility.subGroup.Add(x);
						MapGenCavesUtility.groupVisited.Add(x);
					}, int.MaxValue, false, null);
					if (MapGenCavesUtility.subGroup.Count < parms.minRocksToGenerateAnyTunnel || (float)MapGenCavesUtility.subGroup.Count < 0.05f * (float)group.Count)
					{
						for (int j = 0; j < MapGenCavesUtility.subGroup.Count; j++)
						{
							MapGenCavesUtility.groupSet.Remove(MapGenCavesUtility.subGroup[j]);
						}
					}
				}
			}
			group.Clear();
			group.AddRange(MapGenCavesUtility.groupSet);
		}

		// Token: 0x04007D42 RID: 32066
		private static readonly List<IntVec3> tmpCells = new List<IntVec3>();

		// Token: 0x04007D43 RID: 32067
		private static readonly HashSet<IntVec3> tmpGroupSet = new HashSet<IntVec3>();

		// Token: 0x04007D44 RID: 32068
		private static readonly HashSet<IntVec3> groupSet = new HashSet<IntVec3>();

		// Token: 0x04007D45 RID: 32069
		private static readonly HashSet<IntVec3> groupVisited = new HashSet<IntVec3>();

		// Token: 0x04007D46 RID: 32070
		private static readonly List<IntVec3> subGroup = new List<IntVec3>();

		// Token: 0x02002097 RID: 8343
		public struct CaveGenParms
		{
			// Token: 0x17001BDF RID: 7135
			// (get) Token: 0x0600B6AA RID: 46762 RVA: 0x00348594 File Offset: 0x00346794
			public static MapGenCavesUtility.CaveGenParms Default
			{
				get
				{
					return new MapGenCavesUtility.CaveGenParms
					{
						openTunnelsPer10k = 5.8f,
						closedTunnelsPer10k = 2.5f,
						maxOpenTunnelsPerRockGroup = 3,
						maxClosedTunnelsPerRockGroup = 1,
						directionChangeSpeed = 8f,
						minRocksToGenerateAnyTunnel = 300,
						widthOffsetPerCell = 0.034f,
						minTunnelWidth = 1.4f,
						branchChance = 0.1f,
						widthNoiseFrequency = 0.05f,
						widthNoiseAmplitude = 0f,
						branchedTunnelWidthOffset = new FloatRange(0.2f, 0.4f),
						tunnelsWidthPerRockCount = new SimpleCurve
						{
							{
								new CurvePoint(100f, 2f),
								true
							},
							{
								new CurvePoint(300f, 4f),
								true
							},
							{
								new CurvePoint(3000f, 5.5f),
								true
							}
						}
					};
				}
			}

			// Token: 0x04007D47 RID: 32071
			public float openTunnelsPer10k;

			// Token: 0x04007D48 RID: 32072
			public float closedTunnelsPer10k;

			// Token: 0x04007D49 RID: 32073
			public int maxOpenTunnelsPerRockGroup;

			// Token: 0x04007D4A RID: 32074
			public int maxClosedTunnelsPerRockGroup;

			// Token: 0x04007D4B RID: 32075
			public float directionChangeSpeed;

			// Token: 0x04007D4C RID: 32076
			public int minRocksToGenerateAnyTunnel;

			// Token: 0x04007D4D RID: 32077
			public int allowBranchingAfterThisManyCells;

			// Token: 0x04007D4E RID: 32078
			public float widthOffsetPerCell;

			// Token: 0x04007D4F RID: 32079
			public float minTunnelWidth;

			// Token: 0x04007D50 RID: 32080
			public float branchChance;

			// Token: 0x04007D51 RID: 32081
			public float widthNoiseFrequency;

			// Token: 0x04007D52 RID: 32082
			public float widthNoiseAmplitude;

			// Token: 0x04007D53 RID: 32083
			public FloatRange branchedTunnelWidthOffset;

			// Token: 0x04007D54 RID: 32084
			public SimpleCurve tunnelsWidthPerRockCount;
		}
	}
}
