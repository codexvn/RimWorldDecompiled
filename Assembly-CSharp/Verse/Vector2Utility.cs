using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000D5 RID: 213
	public static class Vector2Utility
	{
		// Token: 0x060005B8 RID: 1464 RVA: 0x0001CFB4 File Offset: 0x0001B1B4
		public static Vector2 Rotated(this Vector2 v)
		{
			return new Vector2(v.y, v.x);
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0001CFC7 File Offset: 0x0001B1C7
		public static Vector2 RotatedBy(this Vector2 v, Rot4 rot)
		{
			return v.RotatedBy(rot.AsAngle);
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0001CFD8 File Offset: 0x0001B1D8
		public static Vector2 RotatedBy(this Vector2 v, float degrees)
		{
			float num = Mathf.Sin(degrees * 0.017453292f);
			float num2 = Mathf.Cos(degrees * 0.017453292f);
			return new Vector2(num2 * v.x - num * v.y, num * v.x + num2 * v.y);
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0001D026 File Offset: 0x0001B226
		public static float AngleTo(this Vector2 a, Vector2 b)
		{
			return Mathf.Atan2(-(b.y - a.y), b.x - a.x) * 57.29578f;
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0001D04E File Offset: 0x0001B24E
		public static Vector3 ScaledBy(this Vector3 a, Vector3 b)
		{
			return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0001D07C File Offset: 0x0001B27C
		public static Vector2 Moved(this Vector2 v, float angle, float distance)
		{
			return new Vector2(v.x + Mathf.Cos(angle * 0.017453292f) * distance, v.y - Mathf.Sin(angle * 0.017453292f) * distance);
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0001D0AD File Offset: 0x0001B2AD
		public static Vector2 FromAngle(float angle)
		{
			return new Vector2(Mathf.Cos(angle * 0.017453292f), -Mathf.Sin(angle * 0.017453292f));
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001D0CD File Offset: 0x0001B2CD
		public static float ToAngle(this Vector2 v)
		{
			return Mathf.Atan2(-v.y, v.x) * 57.29578f;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0001D0E7 File Offset: 0x0001B2E7
		public static Vector3 ToVector3(this Vector2 v)
		{
			return new Vector3(v.x, 0f, v.y);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001D0FF File Offset: 0x0001B2FF
		public static Vector2 ToVector2(this Vector3 v)
		{
			return new Vector2(v.x, v.z);
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0001D112 File Offset: 0x0001B312
		public static float Cross(this Vector2 u, Vector2 v)
		{
			return u.x * v.y - u.y * v.x;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0001D130 File Offset: 0x0001B330
		public static float DistanceToRect(this Vector2 u, Rect rect)
		{
			if (rect.Contains(u))
			{
				return 0f;
			}
			if (u.x < rect.xMin && u.y < rect.yMin)
			{
				return Vector2.Distance(u, new Vector2(rect.xMin, rect.yMin));
			}
			if (u.x > rect.xMax && u.y < rect.yMin)
			{
				return Vector2.Distance(u, new Vector2(rect.xMax, rect.yMin));
			}
			if (u.x < rect.xMin && u.y > rect.yMax)
			{
				return Vector2.Distance(u, new Vector2(rect.xMin, rect.yMax));
			}
			if (u.x > rect.xMax && u.y > rect.yMax)
			{
				return Vector2.Distance(u, new Vector2(rect.xMax, rect.yMax));
			}
			if (u.x < rect.xMin)
			{
				return rect.xMin - u.x;
			}
			if (u.x > rect.xMax)
			{
				return u.x - rect.xMax;
			}
			if (u.y < rect.yMin)
			{
				return rect.yMin - u.y;
			}
			return u.y - rect.yMax;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0001D04E File Offset: 0x0001B24E
		public static Vector3 MultipliedBy(this Vector3 a, Vector3 b)
		{
			return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0001D295 File Offset: 0x0001B495
		public static Vector2 Cardinalize(this Vector2 vec)
		{
			return vec.Cardinalize(Vector2.zero);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0001D2A4 File Offset: 0x0001B4A4
		public static Vector2 Cardinalize(this Vector2 vec, Vector2 bias)
		{
			if (vec.sqrMagnitude == 0f)
			{
				return vec;
			}
			Vector2 normalized = (vec + bias).normalized;
			float num = Mathf.Abs(normalized.x);
			float num2 = Mathf.Abs(normalized.y);
			if (num > num2)
			{
				if (normalized.x > 0f)
				{
					return Vector2.right;
				}
				return Vector2.left;
			}
			else
			{
				if (normalized.y > 0f)
				{
					return Vector2.up;
				}
				return Vector2.down;
			}
		}
	}
}
