using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000D2 RID: 210
	public struct IntVec3 : IEquatable<IntVec3>
	{
		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x0001C1FE File Offset: 0x0001A3FE
		public IntVec2 ToIntVec2
		{
			get
			{
				return new IntVec2(this.x, this.z);
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x0001C211 File Offset: 0x0001A411
		public bool IsValid
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.y >= 0;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x0001C21F File Offset: 0x0001A41F
		public int LengthHorizontalSquared
		{
			get
			{
				return this.x * this.x + this.z * this.z;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x0001C23C File Offset: 0x0001A43C
		public float LengthHorizontal
		{
			get
			{
				return GenMath.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x0001C25F File Offset: 0x0001A45F
		public int LengthManhattan
		{
			get
			{
				return ((this.x >= 0) ? this.x : (-this.x)) + ((this.z >= 0) ? this.z : (-this.z));
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x0001C292 File Offset: 0x0001A492
		public float Magnitude
		{
			get
			{
				return Mathf.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x0001C2B5 File Offset: 0x0001A4B5
		public float SqrMagnitude
		{
			get
			{
				return (float)(this.x * this.x + this.z * this.z);
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x0001C2D3 File Offset: 0x0001A4D3
		public bool IsCardinal
		{
			get
			{
				return this.x == 0 || this.z == 0;
			}
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0001C2E8 File Offset: 0x0001A4E8
		public static implicit operator int3(IntVec3 value)
		{
			return new int3(value.x, value.y, value.z);
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0001C301 File Offset: 0x0001A501
		public static implicit operator float3(IntVec3 value)
		{
			return new float3((float)value.x, (float)value.y, (float)value.z);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0001C31D File Offset: 0x0001A51D
		public static implicit operator IntVec3(int3 value)
		{
			return new IntVec3(value.x, value.y, value.z);
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0001C336 File Offset: 0x0001A536
		public static implicit operator IntVec3(float3 value)
		{
			return new IntVec3(Mathf.FloorToInt(value.x), Mathf.FloorToInt(value.y), Mathf.FloorToInt(value.z));
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x0001C360 File Offset: 0x0001A560
		public float AngleFlat
		{
			get
			{
				if (this.x == 0 && this.z == 0)
				{
					return 0f;
				}
				return Quaternion.LookRotation(this.ToVector3()).eulerAngles.y;
			}
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0001C39B File Offset: 0x0001A59B
		public IntVec3(int newX, int newY, int newZ)
		{
			this.x = newX;
			this.y = newY;
			this.z = newZ;
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0001C3B2 File Offset: 0x0001A5B2
		public IntVec3(Vector3 v)
		{
			this.x = (int)v.x;
			this.y = 0;
			this.z = (int)v.z;
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0001C3D5 File Offset: 0x0001A5D5
		public IntVec3(Vector2 v)
		{
			this.x = (int)v.x;
			this.y = 0;
			this.z = (int)v.y;
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0001C3F8 File Offset: 0x0001A5F8
		public static IntVec3 operator -(IntVec3 a)
		{
			return new IntVec3(-a.x, -a.y, -a.z);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0001C414 File Offset: 0x0001A614
		public static IntVec3 FromString(string str)
		{
			str = str.TrimStart('(');
			str = str.TrimEnd(')');
			string[] array = str.Split(',', StringSplitOptions.None);
			IntVec3 result;
			try
			{
				CultureInfo invariantCulture = CultureInfo.InvariantCulture;
				int newX = Convert.ToInt32(array[0], invariantCulture);
				int newY = Convert.ToInt32(array[1], invariantCulture);
				int newZ = Convert.ToInt32(array[2], invariantCulture);
				result = new IntVec3(newX, newY, newZ);
			}
			catch (Exception ex)
			{
				string str2 = str;
				string str3 = " is not a valid IntVec3 format. Exception: ";
				Exception ex2 = ex;
				Log.Warning(str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				result = IntVec3.Invalid;
			}
			return result;
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0001C4AC File Offset: 0x0001A6AC
		public Vector2 ToVector2()
		{
			return new Vector2((float)this.x, (float)this.z);
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0001C4C1 File Offset: 0x0001A6C1
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, (float)this.y, (float)this.z);
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0001C4DD File Offset: 0x0001A6DD
		public Vector3 ToVector3Shifted()
		{
			return new Vector3((float)this.x + 0.5f, (float)this.y, (float)this.z + 0.5f);
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0001C505 File Offset: 0x0001A705
		public Vector3 ToVector3ShiftedWithAltitude(AltitudeLayer AltLayer)
		{
			return this.ToVector3ShiftedWithAltitude(AltLayer.AltitudeFor());
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0001C513 File Offset: 0x0001A713
		public Vector3 ToVector3ShiftedWithAltitude(float AddedAltitude)
		{
			return new Vector3((float)this.x + 0.5f, (float)this.y + AddedAltitude, (float)this.z + 0.5f);
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0001C540 File Offset: 0x0001A740
		public bool InHorDistOf(IntVec3 otherLoc, float maxDist)
		{
			float num = (float)(this.x - otherLoc.x);
			float num2 = (float)(this.z - otherLoc.z);
			return num * num + num2 * num2 <= maxDist * maxDist;
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0001C578 File Offset: 0x0001A778
		public static IntVec3 FromVector3(Vector3 v)
		{
			return IntVec3.FromVector3(v, 0);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0001C581 File Offset: 0x0001A781
		public static IntVec3 FromVector3(Vector3 v, int newY)
		{
			return new IntVec3((int)v.x, newY, (int)v.z);
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0001C598 File Offset: 0x0001A798
		public static IntVec3 FromPolar(float angle, float distance)
		{
			int num = (int)(Mathf.Cos(angle * 0.017453292f) * distance);
			float num2 = Mathf.Sin(angle * 0.017453292f) * distance;
			return new IntVec3(num, 0, (int)num2);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0001C5CB File Offset: 0x0001A7CB
		public Vector2 ToUIPosition()
		{
			return this.ToVector3Shifted().MapToUIPosition();
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0001C5D8 File Offset: 0x0001A7D8
		public Rect ToUIRect()
		{
			Vector2 vector = this.ToVector3().MapToUIPosition();
			Vector2 vector2 = (this + IntVec3.NorthEast).ToVector3().MapToUIPosition();
			return new Rect(vector.x, vector2.y, vector2.x - vector.x, vector.y - vector2.y);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0001C63C File Offset: 0x0001A83C
		public bool AdjacentToCardinal(IntVec3 other)
		{
			return this.IsValid && ((other.z == this.z && (other.x == this.x + 1 || other.x == this.x - 1)) || (other.x == this.x && (other.z == this.z + 1 || other.z == this.z - 1)));
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0001C6B4 File Offset: 0x0001A8B4
		public bool CardinalTo(IntVec3 other)
		{
			return this.IsValid && other.IsValid && (other.z == this.z || other.x == this.x);
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001C6E7 File Offset: 0x0001A8E7
		public bool AdjacentToDiagonal(IntVec3 other)
		{
			return this.IsValid && Mathf.Abs(this.x - other.x) == 1 && Mathf.Abs(this.z - other.z) == 1;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001C71F File Offset: 0x0001A91F
		public bool AdjacentTo(IntVec3 other)
		{
			return this.AdjacentToDiagonal(other) || this.AdjacentToCardinal(other);
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0001C734 File Offset: 0x0001A934
		public bool AdjacentToCardinal(District district)
		{
			if (!this.IsValid)
			{
				return false;
			}
			Map map = district.Map;
			if (this.InBounds(map) && this.GetDistrict(map, RegionType.Set_All) == district)
			{
				return true;
			}
			IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
			for (int i = 0; i < cardinalDirections.Length; i++)
			{
				IntVec3 intVec = this + cardinalDirections[i];
				if (intVec.InBounds(map) && intVec.GetDistrict(map, RegionType.Set_All) == district)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0001C7B2 File Offset: 0x0001A9B2
		public IntVec3 ClampInsideMap(Map map)
		{
			return this.ClampInsideRect(CellRect.WholeMap(map));
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0001C7C0 File Offset: 0x0001A9C0
		public IntVec3 ClampMagnitude(float magnitude)
		{
			float lengthHorizontal = this.LengthHorizontal;
			if (lengthHorizontal <= magnitude)
			{
				return this;
			}
			float num = (float)this.x / lengthHorizontal;
			float num2 = (float)this.z / lengthHorizontal;
			this.x = Mathf.RoundToInt(num * magnitude);
			this.z = Mathf.RoundToInt(num2 * magnitude);
			return this;
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001C818 File Offset: 0x0001AA18
		public IntVec3 ClampInsideRect(CellRect rect)
		{
			this.x = Mathf.Clamp(this.x, rect.minX, rect.maxX);
			this.y = 0;
			this.z = Mathf.Clamp(this.z, rect.minZ, rect.maxZ);
			return this;
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001C86C File Offset: 0x0001AA6C
		public static IntVec3 operator +(IntVec3 a, IntVec3 b)
		{
			return new IntVec3(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0001C89A File Offset: 0x0001AA9A
		public static IntVec3 operator -(IntVec3 a, IntVec3 b)
		{
			return new IntVec3(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0001C8C8 File Offset: 0x0001AAC8
		public static IntVec3 operator *(IntVec3 a, int i)
		{
			return new IntVec3(a.x * i, a.y * i, a.z * i);
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0001C8E7 File Offset: 0x0001AAE7
		public static IntVec3 operator *(int i, IntVec3 a)
		{
			return new IntVec3(a.x * i, a.y * i, a.z * i);
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0001C906 File Offset: 0x0001AB06
		public static IntVec3 operator /(IntVec3 a, int i)
		{
			return new IntVec3(a.x / i, a.y / i, a.z / i);
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0001C925 File Offset: 0x0001AB25
		public static bool operator ==(IntVec3 a, IntVec3 b)
		{
			return a.x == b.x && a.z == b.z && a.y == b.y;
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0001C954 File Offset: 0x0001AB54
		public static bool operator !=(IntVec3 a, IntVec3 b)
		{
			return a.x != b.x || a.z != b.z || a.y != b.y;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001C983 File Offset: 0x0001AB83
		public override bool Equals(object obj)
		{
			return obj is IntVec3 && this.Equals((IntVec3)obj);
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001C99B File Offset: 0x0001AB9B
		public bool Equals(IntVec3 other)
		{
			return this.x == other.x && this.z == other.z && this.y == other.y;
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0001C9C9 File Offset: 0x0001ABC9
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(Gen.HashCombineInt(Gen.HashCombineInt(0, this.x), this.y), this.z);
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0001C9ED File Offset: 0x0001ABED
		public ulong UniqueHashCode()
		{
			return (ulong)(0L + (long)this.x + 4096L * (long)this.z + 16777216L * (long)this.y);
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0001CA18 File Offset: 0x0001AC18
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.x.ToString(),
				", ",
				this.y.ToString(),
				", ",
				this.z.ToString(),
				")"
			});
		}

		// Token: 0x040002EB RID: 747
		public int x;

		// Token: 0x040002EC RID: 748
		public int y;

		// Token: 0x040002ED RID: 749
		public int z;

		// Token: 0x040002EE RID: 750
		public static readonly IntVec3 Zero = new IntVec3(0, 0, 0);

		// Token: 0x040002EF RID: 751
		public static readonly IntVec3 North = new IntVec3(0, 0, 1);

		// Token: 0x040002F0 RID: 752
		public static readonly IntVec3 East = new IntVec3(1, 0, 0);

		// Token: 0x040002F1 RID: 753
		public static readonly IntVec3 South = new IntVec3(0, 0, -1);

		// Token: 0x040002F2 RID: 754
		public static readonly IntVec3 West = new IntVec3(-1, 0, 0);

		// Token: 0x040002F3 RID: 755
		public static readonly IntVec3 NorthWest = new IntVec3(-1, 0, 1);

		// Token: 0x040002F4 RID: 756
		public static readonly IntVec3 NorthEast = new IntVec3(1, 0, 1);

		// Token: 0x040002F5 RID: 757
		public static readonly IntVec3 SouthWest = new IntVec3(-1, 0, -1);

		// Token: 0x040002F6 RID: 758
		public static readonly IntVec3 SouthEast = new IntVec3(1, 0, -1);

		// Token: 0x040002F7 RID: 759
		public static readonly IntVec3 Invalid = new IntVec3(-1000, -1000, -1000);
	}
}
