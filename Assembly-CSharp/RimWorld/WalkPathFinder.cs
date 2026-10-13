using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014EA RID: 5354
	public static class WalkPathFinder
	{
		// Token: 0x06007FCF RID: 32719 RVA: 0x00267538 File Offset: 0x00265738
		public static bool TryFindWalkPath(Pawn pawn, IntVec3 root, out List<IntVec3> result)
		{
			WalkPathFinder.<>c__DisplayClass6_0 CS$<>8__locals1;
			CS$<>8__locals1.pawn = pawn;
			List<IntVec3> list = new List<IntVec3>
			{
				root
			};
			IntVec3 intVec = root;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec2 = IntVec3.Invalid;
				float num = -1f;
				for (int j = WalkPathFinder.StartRadialIndex; j > WalkPathFinder.EndRadialIndex; j -= 3)
				{
					IntVec3 intVec3 = intVec + GenRadial.RadialPattern[j];
					if (WalkPathFinder.<TryFindWalkPath>g__ValidCell|6_0(intVec3, intVec, ref CS$<>8__locals1))
					{
						float num2 = 10000f;
						for (int k = 0; k < list.Count; k++)
						{
							num2 += (float)(list[k] - intVec3).LengthManhattan;
						}
						float num3 = (float)(intVec3 - root).LengthManhattan;
						if (num3 > 40f)
						{
							num2 *= Mathf.InverseLerp(70f, 40f, num3);
						}
						if (list.Count >= 2)
						{
							List<IntVec3> list2 = list;
							IntVec3 a = list2[list2.Count - 1];
							List<IntVec3> list3 = list;
							float num4 = (a - list3[list3.Count - 2]).AngleFlat;
							float angleFlat = (intVec3 - intVec).AngleFlat;
							float num5;
							if (angleFlat > num4)
							{
								num5 = angleFlat - num4;
							}
							else
							{
								num4 -= 360f;
								num5 = angleFlat - num4;
							}
							if (num5 > 110f)
							{
								num2 *= 0.01f;
							}
						}
						if (list.Count >= 4 && (intVec - root).LengthManhattan < (intVec3 - root).LengthManhattan)
						{
							num2 *= 1E-05f;
						}
						if (num2 > num)
						{
							intVec2 = intVec3;
							num = num2;
						}
					}
				}
				if (num < 0f)
				{
					result = null;
					return false;
				}
				list.Add(intVec2);
				intVec = intVec2;
			}
			list.Add(root);
			result = list;
			return true;
		}

		// Token: 0x06007FD0 RID: 32720 RVA: 0x00267710 File Offset: 0x00265910
		public static void DebugFlashWalkPath(IntVec3 root, int numEntries = 8)
		{
			Map currentMap = Find.CurrentMap;
			List<IntVec3> list;
			if (!WalkPathFinder.TryFindWalkPath(currentMap.mapPawns.FreeColonistsSpawned.First<Pawn>(), root, out list))
			{
				currentMap.debugDrawer.FlashCell(root, 0.2f, "NOPATH", 50);
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				currentMap.debugDrawer.FlashCell(list[i], (float)i / (float)numEntries, i.ToString(), 50);
				if (i > 0)
				{
					currentMap.debugDrawer.FlashLine(list[i], list[i - 1], 50, SimpleColor.White);
				}
			}
		}

		// Token: 0x06007FD2 RID: 32722 RVA: 0x002677CC File Offset: 0x002659CC
		[CompilerGenerated]
		internal static bool <TryFindWalkPath>g__ValidCell|6_0(IntVec3 c, IntVec3 intVec3, ref WalkPathFinder.<>c__DisplayClass6_0 A_2)
		{
			return c.InBounds(A_2.pawn.Map) && !c.Fogged(A_2.pawn.Map) && !c.GetTerrain(A_2.pawn.Map).avoidWander && !c.Roofed(A_2.pawn.Map) && c.Standable(A_2.pawn.Map) && !c.IsForbidden(A_2.pawn) && GenSight.LineOfSight(intVec3, c, A_2.pawn.Map) && !PawnUtility.KnownDangerAt(c, A_2.pawn.Map, A_2.pawn);
		}

		// Token: 0x04005577 RID: 21879
		private const int NumPathNodes = 8;

		// Token: 0x04005578 RID: 21880
		private const float StepDistMin = 2f;

		// Token: 0x04005579 RID: 21881
		private const float StepDistMax = 14f;

		// Token: 0x0400557A RID: 21882
		private static readonly int StartRadialIndex = GenRadial.NumCellsInRadius(14f);

		// Token: 0x0400557B RID: 21883
		private static readonly int EndRadialIndex = GenRadial.NumCellsInRadius(2f);

		// Token: 0x0400557C RID: 21884
		private const int RadialIndexStride = 3;
	}
}
