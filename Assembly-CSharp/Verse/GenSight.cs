using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D09 RID: 3337
	public static class GenSight
	{
		// Token: 0x06005563 RID: 21859 RVA: 0x001B8668 File Offset: 0x001B6868
		public static bool LineOfSight(IntVec3 start, IntVec3 end, Map map, bool skipFirstCell = false, Func<IntVec3, bool> validator = null, int halfXOffset = 0, int halfZOffset = 0)
		{
			if (!start.InBounds(map) || !end.InBounds(map))
			{
				return false;
			}
			bool flag;
			if (start.x == end.x)
			{
				flag = (start.z < end.z);
			}
			else
			{
				flag = (start.x < end.x);
			}
			int num = Mathf.Abs(end.x - start.x);
			int num2 = Mathf.Abs(end.z - start.z);
			int num3 = start.x;
			int num4 = start.z;
			int i = 1 + num + num2;
			int num5 = (end.x > start.x) ? 1 : -1;
			int num6 = (end.z > start.z) ? 1 : -1;
			num *= 4;
			num2 *= 4;
			num += halfXOffset * 2;
			num2 += halfZOffset * 2;
			int num7 = num / 2 - num2 / 2;
			IntVec3 intVec = default(IntVec3);
			while (i > 1)
			{
				intVec.x = num3;
				intVec.z = num4;
				if (!skipFirstCell || intVec != start)
				{
					if (!intVec.CanBeSeenOverFast(map))
					{
						return false;
					}
					if (validator != null && !validator(intVec))
					{
						return false;
					}
				}
				if (num7 > 0 || (num7 == 0 && flag))
				{
					num3 += num5;
					num7 -= num2;
				}
				else
				{
					num4 += num6;
					num7 += num;
				}
				i--;
			}
			return true;
		}

		// Token: 0x06005564 RID: 21860 RVA: 0x001B87B4 File Offset: 0x001B69B4
		public static bool LineOfSight(IntVec3 start, IntVec3 end, Map map, CellRect startRect, CellRect endRect, Func<IntVec3, bool> validator = null, bool forLeaning = false)
		{
			if (!start.InBounds(map) || !end.InBounds(map))
			{
				return false;
			}
			bool flag;
			if (start.x == end.x)
			{
				flag = (start.z < end.z);
			}
			else
			{
				flag = (start.x < end.x);
			}
			int num = Mathf.Abs(end.x - start.x);
			int num2 = Mathf.Abs(end.z - start.z);
			int num3 = start.x;
			int num4 = start.z;
			int i = 1 + num + num2;
			int num5 = (end.x > start.x) ? 1 : -1;
			int num6 = (end.z > start.z) ? 1 : -1;
			int num7 = num - num2;
			num *= 2;
			num2 *= 2;
			IntVec3 intVec = default(IntVec3);
			while (i > 1)
			{
				intVec.x = num3;
				intVec.z = num4;
				if (endRect.Contains(intVec))
				{
					return true;
				}
				if (!startRect.Contains(intVec))
				{
					if (!intVec.CanBeSeenOverFast(map))
					{
						return false;
					}
					if (validator != null && !validator(intVec))
					{
						return false;
					}
				}
				if (num7 > 0 || (num7 == 0 && flag))
				{
					num3 += num5;
					num7 -= num2;
				}
				else
				{
					num4 += num6;
					num7 += num;
				}
				i--;
			}
			return true;
		}

		// Token: 0x06005565 RID: 21861 RVA: 0x001B88F8 File Offset: 0x001B6AF8
		public static bool LineOfSight(IntVec3 start, IntVec3 end, Map map)
		{
			return GenSight.LineOfSight(start, end, map, CellRect.SingleCell(start), CellRect.SingleCell(end), null, false);
		}

		// Token: 0x06005566 RID: 21862 RVA: 0x001B8910 File Offset: 0x001B6B10
		public static IEnumerable<IntVec3> PointsOnLineOfSight(IntVec3 start, IntVec3 end)
		{
			bool sideOnEqual;
			if (start.x == end.x)
			{
				sideOnEqual = (start.z < end.z);
			}
			else
			{
				sideOnEqual = (start.x < end.x);
			}
			int dx = Mathf.Abs(end.x - start.x);
			int dz = Mathf.Abs(end.z - start.z);
			int x = start.x;
			int z = start.z;
			int i = 1 + dx + dz;
			int x_inc = (end.x > start.x) ? 1 : -1;
			int z_inc = (end.z > start.z) ? 1 : -1;
			int error = dx - dz;
			dx *= 2;
			dz *= 2;
			IntVec3 c = default(IntVec3);
			while (i > 0)
			{
				c.x = x;
				c.z = z;
				yield return c;
				if (error > 0 || (error == 0 && sideOnEqual))
				{
					x += x_inc;
					error -= dz;
				}
				else
				{
					z += z_inc;
					error += dx;
				}
				int num = i - 1;
				i = num;
			}
			yield break;
		}

		// Token: 0x06005567 RID: 21863 RVA: 0x001B8928 File Offset: 0x001B6B28
		public static void PointsOnLineOfSight(IntVec3 start, IntVec3 end, Action<IntVec3> visitor)
		{
			bool flag;
			if (start.x == end.x)
			{
				flag = (start.z < end.z);
			}
			else
			{
				flag = (start.x < end.x);
			}
			int num = Mathf.Abs(end.x - start.x);
			int num2 = Mathf.Abs(end.z - start.z);
			int num3 = start.x;
			int num4 = start.z;
			int i = 1 + num + num2;
			int num5 = (end.x > start.x) ? 1 : -1;
			int num6 = (end.z > start.z) ? 1 : -1;
			int num7 = num - num2;
			num *= 2;
			num2 *= 2;
			IntVec3 obj = default(IntVec3);
			while (i > 1)
			{
				obj.x = num3;
				obj.z = num4;
				visitor(obj);
				if (num7 > 0 || (num7 == 0 && flag))
				{
					num3 += num5;
					num7 -= num2;
				}
				else
				{
					num4 += num6;
					num7 += num;
				}
				i--;
			}
		}

		// Token: 0x06005568 RID: 21864 RVA: 0x001B8A2C File Offset: 0x001B6C2C
		public static IntVec3 LastPointOnLineOfSight(IntVec3 start, IntVec3 end, Func<IntVec3, bool> validator, bool skipFirstCell = false)
		{
			foreach (IntVec3 intVec in GenSight.PointsOnLineOfSight(start, end))
			{
				if (!skipFirstCell || !(intVec == start))
				{
					if (intVec == end)
					{
						return end;
					}
					if (!validator(intVec))
					{
						return intVec;
					}
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06005569 RID: 21865 RVA: 0x001B8AA0 File Offset: 0x001B6CA0
		public static bool LineOfSightToEdges(IntVec3 start, IntVec3 end, Map map, bool skipFirstCell = false, Func<IntVec3, bool> validator = null)
		{
			if (GenSight.LineOfSight(start, end, map, skipFirstCell, validator, 0, 0))
			{
				return true;
			}
			int num = (start * 2).DistanceToSquared(end * 2);
			for (int i = 0; i < 4; i++)
			{
				if ((start * 2).DistanceToSquared(end * 2 + GenAdj.CardinalDirections[i]) <= num && GenSight.LineOfSight(start, end, map, skipFirstCell, validator, GenAdj.CardinalDirections[i].x, GenAdj.CardinalDirections[i].z))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600556A RID: 21866 RVA: 0x001B8B34 File Offset: 0x001B6D34
		public static bool LineOfSightToThing(IntVec3 start, Thing t, Map map, bool skipFirstCell = false, Func<IntVec3, bool> validator = null)
		{
			if (t.def.size == IntVec2.One)
			{
				return GenSight.LineOfSight(start, t.Position, map);
			}
			foreach (IntVec3 end in t.OccupiedRect())
			{
				if (GenSight.LineOfSight(start, end, map, skipFirstCell, validator, 0, 0))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600556B RID: 21867 RVA: 0x001B8BC0 File Offset: 0x001B6DC0
		public static List<IntVec3> BresenhamCellsBetween(IntVec3 a, IntVec3 b)
		{
			return GenSight.BresenhamCellsBetween(a.x, a.z, b.x, b.z);
		}

		// Token: 0x0600556C RID: 21868 RVA: 0x001B8BE0 File Offset: 0x001B6DE0
		public static List<IntVec3> BresenhamCellsBetween(int x0, int y0, int x1, int y1)
		{
			GenSight.tmpCells.Clear();
			int num = Mathf.Abs(x1 - x0);
			int num2 = (x0 < x1) ? 1 : -1;
			int num3 = -Mathf.Abs(y1 - y0);
			int num4 = (y0 < y1) ? 1 : -1;
			int num5 = num + num3;
			int num6 = 1000;
			do
			{
				GenSight.tmpCells.Add(new IntVec3(x0, 0, y0));
				if (x0 == x1 && y0 == y1)
				{
					goto IL_8B;
				}
				int num7 = 2 * num5;
				if (num7 >= num3)
				{
					num5 += num3;
					x0 += num2;
				}
				if (num7 <= num)
				{
					num5 += num;
					y0 += num4;
				}
				num6--;
			}
			while (num6 > 0);
			Log.Error("BresenhamCellsBetween exceeded iterations limit of 1000.");
			IL_8B:
			return GenSight.tmpCells;
		}

		// Token: 0x0600556D RID: 21869 RVA: 0x001B8C80 File Offset: 0x001B6E80
		public static void DebugDrawFOVSymmetry_Update()
		{
			Pawn pawn = Find.Selector.SelectedPawns.FirstOrDefault<Pawn>();
			if (pawn == null || !pawn.Spawned)
			{
				return;
			}
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(pawn.Position, 25f, true))
			{
				if (intVec.InBounds(Find.CurrentMap))
				{
					if (GenSight.LineOfSight(pawn.Position, intVec, Find.CurrentMap, true, null, 0, 0))
					{
						GenSight.tmpViewCells.Add(intVec);
					}
					if (ShootLeanUtility.CellCanSeeCell(pawn.Position, intVec, pawn.Map) != ShootLeanUtility.CellCanSeeCell(intVec, pawn.Position, pawn.Map))
					{
						GenSight.tmpBadCells.Add(intVec);
					}
					else
					{
						GenSight.tmpGoodCells.Add(intVec);
					}
				}
			}
			GenDraw.DrawFieldEdges(GenSight.tmpGoodCells, Color.cyan, null, null, 2900);
			GenDraw.DrawFieldEdges(GenSight.tmpBadCells, Color.magenta, null, null, 2900);
			GenDraw.DrawFieldEdges(GenSight.tmpViewCells, GenSight.InViewCol, null, null, 2900);
			GenSight.tmpGoodCells.Clear();
			GenSight.tmpBadCells.Clear();
			GenSight.tmpViewCells.Clear();
		}

		// Token: 0x04003DB5 RID: 15797
		private static readonly List<IntVec3> tmpCells = new List<IntVec3>();

		// Token: 0x04003DB6 RID: 15798
		private static List<IntVec3> tmpGoodCells = new List<IntVec3>();

		// Token: 0x04003DB7 RID: 15799
		private static List<IntVec3> tmpBadCells = new List<IntVec3>();

		// Token: 0x04003DB8 RID: 15800
		private static List<IntVec3> tmpViewCells = new List<IntVec3>();

		// Token: 0x04003DB9 RID: 15801
		private static readonly Color InViewCol = new Color(1f, 1f, 1f, 0.7f);
	}
}
