using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000E8 RID: 232
	public static class GenAdj
	{
		// Token: 0x06000683 RID: 1667 RVA: 0x0001F6F4 File Offset: 0x0001D8F4
		static GenAdj()
		{
			GenAdj.SetupAdjacencyTables();
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0001F784 File Offset: 0x0001D984
		private static void SetupAdjacencyTables()
		{
			GenAdj.CardinalDirections[0] = new IntVec3(0, 0, 1);
			GenAdj.CardinalDirections[1] = new IntVec3(1, 0, 0);
			GenAdj.CardinalDirections[2] = new IntVec3(0, 0, -1);
			GenAdj.CardinalDirections[3] = new IntVec3(-1, 0, 0);
			GenAdj.CardinalDirectionsAndInside[0] = new IntVec3(0, 0, 1);
			GenAdj.CardinalDirectionsAndInside[1] = new IntVec3(1, 0, 0);
			GenAdj.CardinalDirectionsAndInside[2] = new IntVec3(0, 0, -1);
			GenAdj.CardinalDirectionsAndInside[3] = new IntVec3(-1, 0, 0);
			GenAdj.CardinalDirectionsAndInside[4] = new IntVec3(0, 0, 0);
			GenAdj.CardinalDirectionsAround[0] = new IntVec3(0, 0, -1);
			GenAdj.CardinalDirectionsAround[1] = new IntVec3(-1, 0, 0);
			GenAdj.CardinalDirectionsAround[2] = new IntVec3(0, 0, 1);
			GenAdj.CardinalDirectionsAround[3] = new IntVec3(1, 0, 0);
			GenAdj.DiagonalDirections[0] = new IntVec3(-1, 0, -1);
			GenAdj.DiagonalDirections[1] = new IntVec3(-1, 0, 1);
			GenAdj.DiagonalDirections[2] = new IntVec3(1, 0, 1);
			GenAdj.DiagonalDirections[3] = new IntVec3(1, 0, -1);
			GenAdj.DiagonalDirectionsAround[0] = new IntVec3(-1, 0, -1);
			GenAdj.DiagonalDirectionsAround[1] = new IntVec3(-1, 0, 1);
			GenAdj.DiagonalDirectionsAround[2] = new IntVec3(1, 0, 1);
			GenAdj.DiagonalDirectionsAround[3] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCells[0] = new IntVec3(0, 0, 1);
			GenAdj.AdjacentCells[1] = new IntVec3(1, 0, 0);
			GenAdj.AdjacentCells[2] = new IntVec3(0, 0, -1);
			GenAdj.AdjacentCells[3] = new IntVec3(-1, 0, 0);
			GenAdj.AdjacentCells[4] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCells[5] = new IntVec3(1, 0, 1);
			GenAdj.AdjacentCells[6] = new IntVec3(-1, 0, 1);
			GenAdj.AdjacentCells[7] = new IntVec3(-1, 0, -1);
			GenAdj.AdjacentCellsAndInside[0] = new IntVec3(0, 0, 1);
			GenAdj.AdjacentCellsAndInside[1] = new IntVec3(1, 0, 0);
			GenAdj.AdjacentCellsAndInside[2] = new IntVec3(0, 0, -1);
			GenAdj.AdjacentCellsAndInside[3] = new IntVec3(-1, 0, 0);
			GenAdj.AdjacentCellsAndInside[4] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCellsAndInside[5] = new IntVec3(1, 0, 1);
			GenAdj.AdjacentCellsAndInside[6] = new IntVec3(-1, 0, 1);
			GenAdj.AdjacentCellsAndInside[7] = new IntVec3(-1, 0, -1);
			GenAdj.AdjacentCellsAndInside[8] = new IntVec3(0, 0, 0);
			GenAdj.AdjacentCellsAround[0] = new IntVec3(0, 0, 1);
			GenAdj.AdjacentCellsAround[1] = new IntVec3(1, 0, 1);
			GenAdj.AdjacentCellsAround[2] = new IntVec3(1, 0, 0);
			GenAdj.AdjacentCellsAround[3] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCellsAround[4] = new IntVec3(0, 0, -1);
			GenAdj.AdjacentCellsAround[5] = new IntVec3(-1, 0, -1);
			GenAdj.AdjacentCellsAround[6] = new IntVec3(-1, 0, 0);
			GenAdj.AdjacentCellsAround[7] = new IntVec3(-1, 0, 1);
			GenAdj.AdjacentCellsAroundBottom[0] = new IntVec3(0, 0, -1);
			GenAdj.AdjacentCellsAroundBottom[1] = new IntVec3(-1, 0, -1);
			GenAdj.AdjacentCellsAroundBottom[2] = new IntVec3(-1, 0, 0);
			GenAdj.AdjacentCellsAroundBottom[3] = new IntVec3(-1, 0, 1);
			GenAdj.AdjacentCellsAroundBottom[4] = new IntVec3(0, 0, 1);
			GenAdj.AdjacentCellsAroundBottom[5] = new IntVec3(1, 0, 1);
			GenAdj.AdjacentCellsAroundBottom[6] = new IntVec3(1, 0, 0);
			GenAdj.AdjacentCellsAroundBottom[7] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCellsAroundBottom[8] = new IntVec3(0, 0, 0);
			GenAdj.AdjacentCellsAndInsideForUV[0] = new IntVec3(0, 0, -1);
			GenAdj.AdjacentCellsAndInsideForUV[1] = new IntVec3(-1, 0, -1);
			GenAdj.AdjacentCellsAndInsideForUV[2] = new IntVec3(-1, 0, 0);
			GenAdj.AdjacentCellsAndInsideForUV[3] = new IntVec3(-1, 0, 1);
			GenAdj.AdjacentCellsAndInsideForUV[4] = new IntVec3(0, 0, 1);
			GenAdj.AdjacentCellsAndInsideForUV[5] = new IntVec3(1, 0, 1);
			GenAdj.AdjacentCellsAndInsideForUV[6] = new IntVec3(1, 0, 0);
			GenAdj.AdjacentCellsAndInsideForUV[7] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCellsAndInsideForUV[8] = new IntVec3(0, 0, 0);
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0001FC54 File Offset: 0x0001DE54
		public static List<IntVec3> AdjacentCells8WayRandomized()
		{
			if (GenAdj.adjRandomOrderList == null)
			{
				GenAdj.adjRandomOrderList = new List<IntVec3>();
				for (int i = 0; i < 8; i++)
				{
					GenAdj.adjRandomOrderList.Add(GenAdj.AdjacentCells[i]);
				}
			}
			GenAdj.adjRandomOrderList.Shuffle<IntVec3>();
			return GenAdj.adjRandomOrderList;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0001FCA2 File Offset: 0x0001DEA2
		public static IEnumerable<IntVec3> CellsOccupiedBy(Thing t)
		{
			if (t.def.size.x == 1 && t.def.size.z == 1)
			{
				yield return t.Position;
			}
			else
			{
				foreach (IntVec3 intVec in GenAdj.CellsOccupiedBy(t.Position, t.Rotation, t.def.size))
				{
					yield return intVec;
				}
				IEnumerator<IntVec3> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0001FCB2 File Offset: 0x0001DEB2
		public static IEnumerable<IntVec3> CellsOccupiedBy(IntVec3 center, Rot4 rotation, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rotation);
			int num = center.x - (size.x - 1) / 2;
			int minZ = center.z - (size.z - 1) / 2;
			int maxX = num + size.x - 1;
			int maxZ = minZ + size.z - 1;
			int num2;
			for (int i = num; i <= maxX; i = num2 + 1)
			{
				for (int j = minZ; j <= maxZ; j = num2 + 1)
				{
					yield return new IntVec3(i, 0, j);
					num2 = j;
				}
				num2 = i;
			}
			yield break;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0001FCD0 File Offset: 0x0001DED0
		public static IEnumerable<IntVec3> CellsAdjacent8Way(TargetInfo pack)
		{
			if (pack.HasThing)
			{
				foreach (IntVec3 intVec in GenAdj.CellsAdjacent8Way(pack.Thing))
				{
					yield return intVec;
				}
				IEnumerator<IntVec3> enumerator = null;
			}
			else
			{
				int num;
				for (int i = 0; i < 8; i = num + 1)
				{
					yield return pack.Cell + GenAdj.AdjacentCells[i];
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0001FCE0 File Offset: 0x0001DEE0
		public static IEnumerable<IntVec3> CellsAdjacent8Way(Thing t)
		{
			return GenAdj.CellsAdjacent8Way(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0001FCFE File Offset: 0x0001DEFE
		public static IEnumerable<IntVec3> CellsAdjacent8Way(IntVec3 thingCenter, Rot4 thingRot, IntVec2 thingSize)
		{
			GenAdj.AdjustForRotation(ref thingCenter, ref thingSize, thingRot);
			int minX = thingCenter.x - (thingSize.x - 1) / 2 - 1;
			int maxX = minX + thingSize.x + 1;
			int minZ = thingCenter.z - (thingSize.z - 1) / 2 - 1;
			int maxZ = minZ + thingSize.z + 1;
			IntVec3 cur = new IntVec3(minX - 1, 0, minZ);
			do
			{
				cur.x++;
				yield return cur;
			}
			while (cur.x < maxX);
			do
			{
				cur.z++;
				yield return cur;
			}
			while (cur.z < maxZ);
			do
			{
				cur.x--;
				yield return cur;
			}
			while (cur.x > minX);
			do
			{
				cur.z--;
				yield return cur;
			}
			while (cur.z > minZ + 1);
			yield break;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0001FD1C File Offset: 0x0001DF1C
		public static IEnumerable<IntVec3> CellsAdjacentCardinal(Thing t)
		{
			return GenAdj.CellsAdjacentCardinal(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0001FD3A File Offset: 0x0001DF3A
		public static IEnumerable<IntVec3> CellsAdjacentCardinal(IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			int minX = center.x - (size.x - 1) / 2 - 1;
			int maxX = minX + size.x + 1;
			int minZ = center.z - (size.z - 1) / 2 - 1;
			int maxZ = minZ + size.z + 1;
			IntVec3 cur = new IntVec3(minX, 0, minZ);
			do
			{
				cur.x++;
				yield return cur;
			}
			while (cur.x < maxX - 1);
			cur.x++;
			do
			{
				cur.z++;
				yield return cur;
			}
			while (cur.z < maxZ - 1);
			cur.z++;
			do
			{
				cur.x--;
				yield return cur;
			}
			while (cur.x > minX + 1);
			cur.x--;
			do
			{
				cur.z--;
				yield return cur;
			}
			while (cur.z > minZ + 1);
			yield break;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0001FD58 File Offset: 0x0001DF58
		public static IEnumerable<IntVec3> CellsAdjacentAlongEdge(IntVec3 center, Rot4 rot, IntVec2 size, int dir)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			int minX = center.x - (size.x - 1) / 2;
			int maxX = minX + size.x;
			int minZ = center.z - (size.z - 1) / 2;
			int maxZ = minZ + size.z;
			if (dir == 0)
			{
				int num;
				for (int x = minX; x < maxX; x = num + 1)
				{
					yield return new IntVec3(x, center.y, maxZ + 1);
					num = x;
				}
			}
			if (dir == 1)
			{
				int num;
				for (int x = minZ; x < maxZ; x = num + 1)
				{
					yield return new IntVec3(maxX + 1, center.y, x);
					num = x;
				}
			}
			if (dir == 2)
			{
				int num;
				for (int x = minX; x < maxX; x = num + 1)
				{
					yield return new IntVec3(x, center.y, minZ - 1);
					num = x;
				}
			}
			if (dir == 3)
			{
				int num;
				for (int x = minZ; x < maxZ; x = num + 1)
				{
					yield return new IntVec3(minX - 1, center.y, x);
					num = x;
				}
			}
			yield break;
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0001FD7D File Offset: 0x0001DF7D
		public static IEnumerable<IntVec3> CellsAdjacent8WayAndInside(this Thing thing)
		{
			IntVec3 position = thing.Position;
			IntVec2 size = thing.def.size;
			Rot4 rotation = thing.Rotation;
			GenAdj.AdjustForRotation(ref position, ref size, rotation);
			int num = position.x - (size.x - 1) / 2 - 1;
			int minZ = position.z - (size.z - 1) / 2 - 1;
			int maxX = num + size.x + 1;
			int maxZ = minZ + size.z + 1;
			int num2;
			for (int i = num; i <= maxX; i = num2 + 1)
			{
				for (int j = minZ; j <= maxZ; j = num2 + 1)
				{
					yield return new IntVec3(i, 0, j);
					num2 = j;
				}
				num2 = i;
			}
			yield break;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0001FD8D File Offset: 0x0001DF8D
		public static void GetAdjacentCorners(LocalTargetInfo target, out IntVec3 BL, out IntVec3 TL, out IntVec3 TR, out IntVec3 BR)
		{
			if (target.HasThing)
			{
				GenAdj.GetAdjacentCorners(target.Thing.OccupiedRect(), out BL, out TL, out TR, out BR);
				return;
			}
			GenAdj.GetAdjacentCorners(CellRect.SingleCell(target.Cell), out BL, out TL, out TR, out BR);
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0001FDC8 File Offset: 0x0001DFC8
		private static void GetAdjacentCorners(CellRect rect, out IntVec3 BL, out IntVec3 TL, out IntVec3 TR, out IntVec3 BR)
		{
			BL = new IntVec3(rect.minX - 1, 0, rect.minZ - 1);
			TL = new IntVec3(rect.minX - 1, 0, rect.maxZ + 1);
			TR = new IntVec3(rect.maxX + 1, 0, rect.maxZ + 1);
			BR = new IntVec3(rect.maxX + 1, 0, rect.minZ - 1);
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0001FE46 File Offset: 0x0001E046
		public static IntVec3 RandomAdjacentCell8Way(this IntVec3 root)
		{
			return root + GenAdj.AdjacentCells[Rand.RangeInclusive(0, 7)];
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0001FE5F File Offset: 0x0001E05F
		public static IntVec3 RandomAdjacentCellCardinal(this IntVec3 root)
		{
			return root + GenAdj.CardinalDirections[Rand.RangeInclusive(0, 3)];
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0001FE78 File Offset: 0x0001E078
		public static IntVec3 RandomAdjacentCell8Way(this Thing t)
		{
			CellRect cellRect = t.OccupiedRect();
			CellRect cellRect2 = cellRect.ExpandedBy(1);
			IntVec3 randomCell;
			do
			{
				randomCell = cellRect2.RandomCell;
			}
			while (cellRect.Contains(randomCell));
			return randomCell;
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0001FEA8 File Offset: 0x0001E0A8
		public static IntVec3 RandomAdjacentCellCardinal(this Thing t)
		{
			CellRect cellRect = t.OccupiedRect();
			IntVec3 randomCell = cellRect.RandomCell;
			if (Rand.Value < 0.5f)
			{
				if (Rand.Value < 0.5f)
				{
					randomCell.x = cellRect.minX - 1;
				}
				else
				{
					randomCell.x = cellRect.maxX + 1;
				}
			}
			else if (Rand.Value < 0.5f)
			{
				randomCell.z = cellRect.minZ - 1;
			}
			else
			{
				randomCell.z = cellRect.maxZ + 1;
			}
			return randomCell;
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0001FF2B File Offset: 0x0001E12B
		public static bool TryFindRandomAdjacentCell8WayWithRoom(Thing t, out IntVec3 result)
		{
			return GenAdj.TryFindRandomAdjacentCell8WayWithRoom(t.Position, t.Rotation, t.def.size, t.MapHeld, out result);
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0001FF50 File Offset: 0x0001E150
		public static bool TryFindRandomAdjacentCell8WayWithRoom(IntVec3 center, Rot4 rot, IntVec2 size, Map map, out IntVec3 result)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			GenAdj.validCells.Clear();
			foreach (IntVec3 intVec in GenAdj.CellsAdjacent8Way(center, rot, size))
			{
				if (intVec.InBounds(map) && intVec.GetRoom(map) != null)
				{
					GenAdj.validCells.Add(intVec);
				}
			}
			return GenAdj.validCells.TryRandomElement(out result);
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0001FFD8 File Offset: 0x0001E1D8
		public static bool AdjacentTo8WayOrInside(this IntVec3 me, LocalTargetInfo other)
		{
			if (other.HasThing)
			{
				return me.AdjacentTo8WayOrInside(other.Thing);
			}
			return me.AdjacentTo8WayOrInside(other.Cell);
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00020000 File Offset: 0x0001E200
		public static bool AdjacentTo8Way(this IntVec3 me, IntVec3 other)
		{
			int num = me.x - other.x;
			int num2 = me.z - other.z;
			if (num == 0 && num2 == 0)
			{
				return false;
			}
			if (num < 0)
			{
				num *= -1;
			}
			if (num2 < 0)
			{
				num2 *= -1;
			}
			return num <= 1 && num2 <= 1;
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00020050 File Offset: 0x0001E250
		public static bool AdjacentTo8WayOrInside(this IntVec3 me, IntVec3 other)
		{
			int num = me.x - other.x;
			int num2 = me.z - other.z;
			if (num < 0)
			{
				num *= -1;
			}
			if (num2 < 0)
			{
				num2 *= -1;
			}
			return num <= 1 && num2 <= 1;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00020098 File Offset: 0x0001E298
		public static bool IsAdjacentToCardinalOrInside(this IntVec3 me, CellRect other)
		{
			if (other.IsEmpty)
			{
				return false;
			}
			CellRect cellRect = other.ExpandedBy(1);
			return cellRect.Contains(me) && !cellRect.IsCorner(me);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x000200D0 File Offset: 0x0001E2D0
		public static bool IsAdjacentToCardinalOrInside(this Thing t1, Thing t2)
		{
			return GenAdj.IsAdjacentToCardinalOrInside(t1.OccupiedRect(), t2.OccupiedRect());
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x000200E4 File Offset: 0x0001E2E4
		public static bool IsAdjacentToCardinalOrInside(CellRect rect1, CellRect rect2)
		{
			if (rect1.IsEmpty || rect2.IsEmpty)
			{
				return false;
			}
			CellRect cellRect = rect1.ExpandedBy(1);
			int minX = cellRect.minX;
			int maxX = cellRect.maxX;
			int minZ = cellRect.minZ;
			int maxZ = cellRect.maxZ;
			int i = minX;
			int j = minZ;
			while (i <= maxX)
			{
				if (rect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
				{
					return true;
				}
				i++;
			}
			i--;
			for (j++; j <= maxZ; j++)
			{
				if (rect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
				{
					return true;
				}
			}
			j--;
			for (i--; i >= minX; i--)
			{
				if (rect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
				{
					return true;
				}
			}
			i++;
			for (j--; j > minZ; j--)
			{
				if (rect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0002027B File Offset: 0x0001E47B
		public static bool AdjacentTo8WayOrInside(this IntVec3 root, Thing t)
		{
			return root.AdjacentTo8WayOrInside(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0002029C File Offset: 0x0001E49C
		public static bool AdjacentTo8WayOrInside(this IntVec3 root, IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			int num = center.x - (size.x - 1) / 2 - 1;
			int num2 = center.z - (size.z - 1) / 2 - 1;
			int num3 = num + size.x + 1;
			int num4 = num2 + size.z + 1;
			return root.x >= num && root.x <= num3 && root.z >= num2 && root.z <= num4;
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00020318 File Offset: 0x0001E518
		public static bool AdjacentTo8WayOrInside(this Thing a, Thing b)
		{
			return GenAdj.AdjacentTo8WayOrInside(a.OccupiedRect(), b.OccupiedRect());
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0002032C File Offset: 0x0001E52C
		public static bool AdjacentTo8WayOrInside(CellRect rect1, CellRect rect2)
		{
			return !rect1.IsEmpty && !rect2.IsEmpty && rect1.ExpandedBy(1).Overlaps(rect2);
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0002035E File Offset: 0x0001E55E
		public static bool IsInside(this IntVec3 root, Thing t)
		{
			return GenAdj.IsInside(root, t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00020380 File Offset: 0x0001E580
		public static bool IsInside(IntVec3 root, IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			int num = center.x - (size.x - 1) / 2;
			int num2 = center.z - (size.z - 1) / 2;
			int num3 = num + size.x - 1;
			int num4 = num2 + size.z - 1;
			return root.x >= num && root.x <= num3 && root.z >= num2 && root.z <= num4;
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x000203FA File Offset: 0x0001E5FA
		public static CellRect OccupiedRect(this Thing t)
		{
			return GenAdj.OccupiedRect(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00020418 File Offset: 0x0001E618
		public static CellRect OccupiedDrawRect(this Thing t)
		{
			Vector2 drawSize = t.DrawSize;
			return GenAdj.OccupiedRect(t.Position, t.Rotation, new IntVec2(Mathf.CeilToInt(drawSize.x), Mathf.CeilToInt(drawSize.y)));
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00020458 File Offset: 0x0001E658
		public static Bounds DrawBounds(this Thing t)
		{
			Vector3 center = t.TrueCenter();
			Vector3 size = t.Rotation.AsQuat * t.def.size.ToVector3();
			return new Bounds(center, size);
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00020495 File Offset: 0x0001E695
		public static CellRect OccupiedRect(IntVec3 center, Rot4 rot, int width, int height)
		{
			return GenAdj.OccupiedRect(center, rot, new IntVec2(width, height));
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x000204A5 File Offset: 0x0001E6A5
		public static CellRect OccupiedRect(IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			return new CellRect(center.x - (size.x - 1) / 2, center.z - (size.z - 1) / 2, size.x, size.z);
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x000204E4 File Offset: 0x0001E6E4
		public static CellRect RectAbout(this IntVec3 center, int width, int height)
		{
			return GenAdj.OccupiedRect(center, Rot4.North, new IntVec2(width, height));
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x000204F8 File Offset: 0x0001E6F8
		public static CellRect RectAbout(this IntVec3 center, int width, int height, Rot4 rot)
		{
			return GenAdj.OccupiedRect(center, rot, new IntVec2(width, height));
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x00020508 File Offset: 0x0001E708
		public static CellRect RectAbout(this IntVec3 center, IntVec2 size)
		{
			return GenAdj.OccupiedRect(center, Rot4.North, size);
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x00020516 File Offset: 0x0001E716
		public static CellRect RectAbout(this IntVec3 center, IntVec2 size, Rot4 rot)
		{
			return GenAdj.OccupiedRect(center, rot, size);
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00020520 File Offset: 0x0001E720
		public static void AdjustForRotation(ref IntVec3 center, ref IntVec2 size, Rot4 rot)
		{
			if (size.x == 1 && size.z == 1)
			{
				return;
			}
			if (rot.IsHorizontal)
			{
				ref int ptr = ref size.x;
				int z = size.z;
				int x = size.x;
				ptr = z;
				size.z = x;
			}
			switch (rot.AsInt)
			{
			case 0:
				break;
			case 1:
				if (size.z % 2 == 0)
				{
					center.z--;
					return;
				}
				break;
			case 2:
				if (size.x % 2 == 0)
				{
					center.x--;
				}
				if (size.z % 2 == 0)
				{
					center.z--;
					return;
				}
				break;
			case 3:
				if (size.x % 2 == 0)
				{
					center.x--;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x04000349 RID: 841
		public static IntVec3[] CardinalDirections = new IntVec3[4];

		// Token: 0x0400034A RID: 842
		public static IntVec3[] CardinalDirectionsAndInside = new IntVec3[5];

		// Token: 0x0400034B RID: 843
		public static IntVec3[] CardinalDirectionsAround = new IntVec3[4];

		// Token: 0x0400034C RID: 844
		public static IntVec3[] DiagonalDirections = new IntVec3[4];

		// Token: 0x0400034D RID: 845
		public static IntVec3[] DiagonalDirectionsAround = new IntVec3[4];

		// Token: 0x0400034E RID: 846
		public static IntVec3[] AdjacentCells = new IntVec3[8];

		// Token: 0x0400034F RID: 847
		public static IntVec3[] AdjacentCellsAndInside = new IntVec3[9];

		// Token: 0x04000350 RID: 848
		public static IntVec3[] AdjacentCellsAround = new IntVec3[8];

		// Token: 0x04000351 RID: 849
		public static IntVec3[] AdjacentCellsAroundBottom = new IntVec3[9];

		// Token: 0x04000352 RID: 850
		public static IntVec3[] AdjacentCellsAndInsideForUV = new IntVec3[9];

		// Token: 0x04000353 RID: 851
		private static List<IntVec3> adjRandomOrderList;

		// Token: 0x04000354 RID: 852
		private static List<IntVec3> validCells = new List<IntVec3>();
	}
}
