using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000112 RID: 274
	public static class GenRadial
	{
		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000842 RID: 2114 RVA: 0x00026CB4 File Offset: 0x00024EB4
		public static float MaxRadialPatternRadius
		{
			get
			{
				float[] radialPatternRadii = GenRadial.RadialPatternRadii;
				return radialPatternRadii[radialPatternRadii.Length - 1];
			}
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x00026CC1 File Offset: 0x00024EC1
		static GenRadial()
		{
			GenRadial.SetupManualRadialPattern();
			GenRadial.SetupRadialPattern();
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x00026D04 File Offset: 0x00024F04
		private static void SetupManualRadialPattern()
		{
			GenRadial.ManualRadialPattern[0] = new IntVec3(0, 0, 0);
			GenRadial.ManualRadialPattern[1] = new IntVec3(0, 0, -1);
			GenRadial.ManualRadialPattern[2] = new IntVec3(1, 0, 0);
			GenRadial.ManualRadialPattern[3] = new IntVec3(0, 0, 1);
			GenRadial.ManualRadialPattern[4] = new IntVec3(-1, 0, 0);
			GenRadial.ManualRadialPattern[5] = new IntVec3(1, 0, -1);
			GenRadial.ManualRadialPattern[6] = new IntVec3(1, 0, 1);
			GenRadial.ManualRadialPattern[7] = new IntVec3(-1, 0, 1);
			GenRadial.ManualRadialPattern[8] = new IntVec3(-1, 0, -1);
			GenRadial.ManualRadialPattern[9] = new IntVec3(2, 0, 0);
			GenRadial.ManualRadialPattern[10] = new IntVec3(-2, 0, 0);
			GenRadial.ManualRadialPattern[11] = new IntVec3(0, 0, 2);
			GenRadial.ManualRadialPattern[12] = new IntVec3(0, 0, -2);
			GenRadial.ManualRadialPattern[13] = new IntVec3(2, 0, 1);
			GenRadial.ManualRadialPattern[14] = new IntVec3(2, 0, -1);
			GenRadial.ManualRadialPattern[15] = new IntVec3(-2, 0, 1);
			GenRadial.ManualRadialPattern[16] = new IntVec3(-2, 0, -1);
			GenRadial.ManualRadialPattern[17] = new IntVec3(-1, 0, 2);
			GenRadial.ManualRadialPattern[18] = new IntVec3(1, 0, 2);
			GenRadial.ManualRadialPattern[19] = new IntVec3(-1, 0, -2);
			GenRadial.ManualRadialPattern[20] = new IntVec3(1, 0, -2);
			GenRadial.ManualRadialPattern[21] = new IntVec3(2, 0, 2);
			GenRadial.ManualRadialPattern[22] = new IntVec3(-2, 0, -2);
			GenRadial.ManualRadialPattern[23] = new IntVec3(2, 0, -2);
			GenRadial.ManualRadialPattern[24] = new IntVec3(-2, 0, 2);
			GenRadial.ManualRadialPattern[25] = new IntVec3(3, 0, 0);
			GenRadial.ManualRadialPattern[26] = new IntVec3(0, 0, 3);
			GenRadial.ManualRadialPattern[27] = new IntVec3(-3, 0, 0);
			GenRadial.ManualRadialPattern[28] = new IntVec3(0, 0, -3);
			GenRadial.ManualRadialPattern[29] = new IntVec3(3, 0, 1);
			GenRadial.ManualRadialPattern[30] = new IntVec3(-3, 0, -1);
			GenRadial.ManualRadialPattern[31] = new IntVec3(1, 0, 3);
			GenRadial.ManualRadialPattern[32] = new IntVec3(-1, 0, -3);
			GenRadial.ManualRadialPattern[33] = new IntVec3(-3, 0, 1);
			GenRadial.ManualRadialPattern[34] = new IntVec3(3, 0, -1);
			GenRadial.ManualRadialPattern[35] = new IntVec3(-1, 0, 3);
			GenRadial.ManualRadialPattern[36] = new IntVec3(1, 0, -3);
			GenRadial.ManualRadialPattern[37] = new IntVec3(3, 0, 2);
			GenRadial.ManualRadialPattern[38] = new IntVec3(-3, 0, -2);
			GenRadial.ManualRadialPattern[39] = new IntVec3(2, 0, 3);
			GenRadial.ManualRadialPattern[40] = new IntVec3(-2, 0, -3);
			GenRadial.ManualRadialPattern[41] = new IntVec3(-3, 0, 2);
			GenRadial.ManualRadialPattern[42] = new IntVec3(3, 0, -2);
			GenRadial.ManualRadialPattern[43] = new IntVec3(-2, 0, 3);
			GenRadial.ManualRadialPattern[44] = new IntVec3(2, 0, -3);
			GenRadial.ManualRadialPattern[45] = new IntVec3(3, 0, 3);
			GenRadial.ManualRadialPattern[46] = new IntVec3(3, 0, -3);
			GenRadial.ManualRadialPattern[47] = new IntVec3(-3, 0, 3);
			GenRadial.ManualRadialPattern[48] = new IntVec3(-3, 0, -3);
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x000270F8 File Offset: 0x000252F8
		private static void SetupRadialPattern()
		{
			List<IntVec3> list = new List<IntVec3>();
			for (int i = -60; i < 60; i++)
			{
				for (int j = -60; j < 60; j++)
				{
					list.Add(new IntVec3(i, 0, j));
				}
			}
			list.Sort(delegate(IntVec3 A, IntVec3 B)
			{
				float num = (float)A.LengthHorizontalSquared;
				float num2 = (float)B.LengthHorizontalSquared;
				if (num < num2)
				{
					return -1;
				}
				if (num == num2)
				{
					return 0;
				}
				return 1;
			});
			for (int k = 0; k < 10000; k++)
			{
				GenRadial.RadialPattern[k] = list[k];
				GenRadial.RadialPatternRadii[k] = list[k].LengthHorizontal;
			}
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x00027193 File Offset: 0x00025393
		public static int NumCellsToFillForRadius_ManualRadialPattern(int radius)
		{
			if (radius == 0)
			{
				return 1;
			}
			if (radius == 1)
			{
				return 9;
			}
			if (radius == 2)
			{
				return 21;
			}
			if (radius == 3)
			{
				return 37;
			}
			Log.Error("NumSquares radius error");
			return 0;
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x000271BC File Offset: 0x000253BC
		public static int NumCellsInRadius(float radius)
		{
			if (radius >= GenRadial.MaxRadialPatternRadius)
			{
				Log.Error(string.Format("Not enough squares to get to radius {0}. Max is {1}", radius, GenRadial.MaxRadialPatternRadius));
				return 10000;
			}
			float num = radius + float.Epsilon;
			int num2 = Array.BinarySearch<float>(GenRadial.RadialPatternRadii, num);
			if (num2 < 0)
			{
				return ~num2;
			}
			for (int i = num2; i < 10000; i++)
			{
				if (GenRadial.RadialPatternRadii[i] > num)
				{
					return i;
				}
			}
			return 10000;
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x00027232 File Offset: 0x00025432
		public static float RadiusOfNumCells(int numCells)
		{
			return GenRadial.RadialPatternRadii[numCells];
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0002723B File Offset: 0x0002543B
		public static IEnumerable<IntVec3> RadialPatternInRadius(float radius)
		{
			int numSquares = GenRadial.NumCellsInRadius(radius);
			int num;
			for (int i = 0; i < numSquares; i = num + 1)
			{
				yield return GenRadial.RadialPattern[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0002724B File Offset: 0x0002544B
		public static IEnumerable<IntVec3> RadialCellsAround(IntVec3 center, float radius, bool useCenter)
		{
			int numSquares = GenRadial.NumCellsInRadius(radius);
			int num;
			for (int i = useCenter ? 0 : 1; i < numSquares; i = num + 1)
			{
				yield return GenRadial.RadialPattern[i] + center;
				num = i;
			}
			yield break;
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x00027269 File Offset: 0x00025469
		public static IEnumerable<IntVec3> RadialCellsAround(IntVec3 center, float minRadius, float maxRadius)
		{
			int numSquares = GenRadial.NumCellsInRadius(maxRadius);
			float minRadiusSquared = minRadius * minRadius;
			int num;
			for (int i = 0; i < numSquares; i = num + 1)
			{
				if ((float)GenRadial.RadialPattern[i].LengthHorizontalSquared >= minRadiusSquared)
				{
					yield return GenRadial.RadialPattern[i] + center;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x00027287 File Offset: 0x00025487
		public static IEnumerable<Thing> RadialDistinctThingsAround(IntVec3 center, Map map, float radius, bool useCenter)
		{
			int numCells = GenRadial.NumCellsInRadius(radius);
			HashSet<Thing> returnedThings = null;
			int num;
			for (int i = useCenter ? 0 : 1; i < numCells; i = num + 1)
			{
				IntVec3 c = GenRadial.RadialPattern[i] + center;
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					int j = 0;
					while (j < thingList.Count)
					{
						Thing thing = thingList[j];
						if (thing.def.size.x <= 1 && thing.def.size.z <= 1)
						{
							goto IL_ED;
						}
						if (returnedThings == null)
						{
							returnedThings = new HashSet<Thing>();
						}
						if (returnedThings.Add(thing))
						{
							goto IL_ED;
						}
						IL_104:
						num = j;
						j = num + 1;
						continue;
						IL_ED:
						yield return thing;
						goto IL_104;
					}
					thingList = null;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x000272AC File Offset: 0x000254AC
		public static void ProcessEquidistantCells(IntVec3 center, float radius, Func<List<IntVec3>, bool> processor, Map map = null)
		{
			if (GenRadial.working)
			{
				Log.Error("Nested calls to ProcessEquidistantCells() are not allowed.");
				return;
			}
			GenRadial.tmpCells.Clear();
			GenRadial.working = true;
			try
			{
				float num = -1f;
				int num2 = GenRadial.NumCellsInRadius(radius);
				for (int i = 0; i < num2; i++)
				{
					IntVec3 intVec = center + GenRadial.RadialPattern[i];
					if (map == null || intVec.InBounds(map))
					{
						float num3 = (float)intVec.DistanceToSquared(center);
						if (Mathf.Abs(num3 - num) > 0.0001f)
						{
							if (GenRadial.tmpCells.Any<IntVec3>() && processor(GenRadial.tmpCells))
							{
								return;
							}
							num = num3;
							GenRadial.tmpCells.Clear();
						}
						GenRadial.tmpCells.Add(intVec);
					}
				}
				if (GenRadial.tmpCells.Any<IntVec3>())
				{
					processor(GenRadial.tmpCells);
				}
			}
			finally
			{
				GenRadial.tmpCells.Clear();
				GenRadial.working = false;
			}
		}

		// Token: 0x04000418 RID: 1048
		public static IntVec3[] ManualRadialPattern = new IntVec3[49];

		// Token: 0x04000419 RID: 1049
		public static IntVec3[] RadialPattern = new IntVec3[10000];

		// Token: 0x0400041A RID: 1050
		public static float[] RadialPatternRadii = new float[10000];

		// Token: 0x0400041B RID: 1051
		private const int RadialPatternCount = 10000;

		// Token: 0x0400041C RID: 1052
		private static readonly List<IntVec3> tmpCells = new List<IntVec3>();

		// Token: 0x0400041D RID: 1053
		private static bool working;
	}
}
