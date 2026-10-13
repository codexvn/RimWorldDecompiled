using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000D4 RID: 212
	public static class Vector3Utility
	{
		// Token: 0x060005AA RID: 1450 RVA: 0x0001CD2A File Offset: 0x0001AF2A
		public static Vector3 HorizontalVectorFromAngle(float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001CD44 File Offset: 0x0001AF44
		public static float AngleFlat(this Vector3 v)
		{
			if (v.x == 0f && v.z == 0f)
			{
				return 0f;
			}
			return Quaternion.LookRotation(v).eulerAngles.y;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0001CD84 File Offset: 0x0001AF84
		public static Vector3 RandomHorizontalOffset(float maxDist)
		{
			float d = Rand.Range(0f, maxDist);
			float y = (float)Rand.Range(0, 360);
			return Quaternion.Euler(new Vector3(0f, y, 0f)) * Vector3.forward * d;
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0001CDCF File Offset: 0x0001AFCF
		public static Vector3 Yto0(this Vector3 v3)
		{
			return new Vector3(v3.x, 0f, v3.z);
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0001CDE7 File Offset: 0x0001AFE7
		public static Vector3 WithYOffset(this Vector3 v3, float offset)
		{
			return new Vector3(v3.x, v3.y + offset, v3.z);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0001CE02 File Offset: 0x0001B002
		public static Vector3 WithY(this Vector3 v3, float y)
		{
			return new Vector3(v3.x, y, v3.z);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0001CE16 File Offset: 0x0001B016
		public static Vector3 SetToAltitude(this Vector3 v3, AltitudeLayer altitude)
		{
			v3.y = altitude.AltitudeFor();
			return v3;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001CE26 File Offset: 0x0001B026
		public static Vector3 RotatedBy(this Vector3 v3, float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.up) * v3;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0001CE3C File Offset: 0x0001B03C
		public static Vector3 RotatedBy(this Vector3 orig, Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
				return orig;
			case 1:
				return new Vector3(orig.z, orig.y, -orig.x);
			case 2:
				return new Vector3(-orig.x, orig.y, -orig.z);
			case 3:
				return new Vector3(-orig.z, orig.y, orig.x);
			default:
				return orig;
			}
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0001CEB8 File Offset: 0x0001B0B8
		public static float AngleToFlat(this Vector3 a, Vector3 b)
		{
			return new Vector2(a.x, a.z).AngleTo(new Vector2(b.x, b.z));
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001CEE4 File Offset: 0x0001B0E4
		public static Vector3 FromAngleFlat(float angle)
		{
			Vector2 vector = Vector2Utility.FromAngle(angle);
			return new Vector3(vector.x, 0f, vector.y);
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0001CF0E File Offset: 0x0001B10E
		public static float ToAngleFlat(this Vector3 v)
		{
			return new Vector2(v.x, v.z).ToAngle();
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0001CF26 File Offset: 0x0001B126
		public static Vector3 Abs(this Vector3 v)
		{
			return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0001CF50 File Offset: 0x0001B150
		public static Vector3 ClosestTo(this IEnumerable<Vector3> pts, Vector3 target)
		{
			float num = float.MaxValue;
			Vector3 result = pts.FirstOrDefault<Vector3>();
			foreach (Vector3 vector in pts)
			{
				float num2 = Vector3.Distance(vector, target);
				if (num2 < num)
				{
					num = num2;
					result = vector;
				}
			}
			return result;
		}
	}
}
