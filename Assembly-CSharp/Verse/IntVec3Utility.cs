using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000D3 RID: 211
	public static class IntVec3Utility
	{
		// Token: 0x060005A0 RID: 1440 RVA: 0x0001CB17 File Offset: 0x0001AD17
		public static IntVec3 ToIntVec3(this Vector3 vect)
		{
			return new IntVec3(vect);
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001CB20 File Offset: 0x0001AD20
		public static float DistanceTo(this IntVec3 a, IntVec3 b)
		{
			return (a - b).LengthHorizontal;
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001CB3C File Offset: 0x0001AD3C
		public static int DistanceToSquared(this IntVec3 a, IntVec3 b)
		{
			return (a - b).LengthHorizontalSquared;
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0001CB58 File Offset: 0x0001AD58
		public static IntVec3 RotatedBy(this IntVec3 orig, RotationDirection rot)
		{
			switch (rot)
			{
			case RotationDirection.None:
				return orig;
			case RotationDirection.Clockwise:
				return new IntVec3(orig.z, orig.y, -orig.x);
			case RotationDirection.Opposite:
				return new IntVec3(-orig.x, orig.y, -orig.z);
			case RotationDirection.Counterclockwise:
				return new IntVec3(-orig.z, orig.y, orig.x);
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0001CBD0 File Offset: 0x0001ADD0
		public static IntVec3 RotatedBy(this IntVec3 orig, Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
				return orig;
			case 1:
				return new IntVec3(orig.z, orig.y, -orig.x);
			case 2:
				return new IntVec3(-orig.x, orig.y, -orig.z);
			case 3:
				return new IntVec3(-orig.z, orig.y, orig.x);
			default:
				return orig;
			}
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001C3F8 File Offset: 0x0001A5F8
		public static IntVec3 Inverse(this IntVec3 orig)
		{
			return new IntVec3(-orig.x, -orig.y, -orig.z);
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001CC4C File Offset: 0x0001AE4C
		public static int ManhattanDistanceFlat(IntVec3 a, IntVec3 b)
		{
			return Math.Abs(a.x - b.x) + Math.Abs(a.z - b.z);
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001CC73 File Offset: 0x0001AE73
		public static IntVec3 RandomHorizontalOffset(float maxDist)
		{
			return Vector3Utility.RandomHorizontalOffset(maxDist).ToIntVec3();
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001CC80 File Offset: 0x0001AE80
		public static int DistanceToEdge(this IntVec3 v, Map map)
		{
			return Mathf.Max(Mathf.Min(Mathf.Min(Mathf.Min(v.x, v.z), map.Size.x - v.x - 1), map.Size.z - v.z - 1), 0);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001CCD8 File Offset: 0x0001AED8
		public static int Determinant(this IntVec3 p, IntVec3 p1, IntVec3 p2)
		{
			int num = (p2.x - p1.x) * (p.z - p1.z) - (p.x - p1.x) * (p2.z - p1.z);
			if (num > 0)
			{
				return -1;
			}
			if (num < 0)
			{
				return 1;
			}
			return 0;
		}
	}
}
