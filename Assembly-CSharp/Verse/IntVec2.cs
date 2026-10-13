using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000D0 RID: 208
	public struct IntVec2 : IEquatable<IntVec2>
	{
		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x0600054B RID: 1355 RVA: 0x0001BE8E File Offset: 0x0001A08E
		public bool IsInvalid
		{
			get
			{
				return this.x < -500;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x0001BE9D File Offset: 0x0001A09D
		public bool IsValid
		{
			get
			{
				return this.x >= -500;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x0001BEAF File Offset: 0x0001A0AF
		public static IntVec2 Zero
		{
			get
			{
				return new IntVec2(0, 0);
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x0001BEB8 File Offset: 0x0001A0B8
		public static IntVec2 One
		{
			get
			{
				return new IntVec2(1, 1);
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x0001BEC1 File Offset: 0x0001A0C1
		public static IntVec2 Two
		{
			get
			{
				return new IntVec2(2, 2);
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x0001BECA File Offset: 0x0001A0CA
		public static IntVec2 North
		{
			get
			{
				return new IntVec2(0, 1);
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x0001BED3 File Offset: 0x0001A0D3
		public static IntVec2 East
		{
			get
			{
				return new IntVec2(1, 0);
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x0001BEDC File Offset: 0x0001A0DC
		public static IntVec2 South
		{
			get
			{
				return new IntVec2(0, -1);
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x0001BEE5 File Offset: 0x0001A0E5
		public static IntVec2 West
		{
			get
			{
				return new IntVec2(-1, 0);
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x0001BEEE File Offset: 0x0001A0EE
		public float Magnitude
		{
			get
			{
				return Mathf.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x0001BF11 File Offset: 0x0001A111
		public int MagnitudeManhattan
		{
			get
			{
				return Mathf.Abs(this.x) + Mathf.Abs(this.z);
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x0001BF2A File Offset: 0x0001A12A
		public int Area
		{
			get
			{
				return Mathf.Abs(this.x) * Mathf.Abs(this.z);
			}
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0001BF43 File Offset: 0x0001A143
		public IntVec2(int newX, int newZ)
		{
			this.x = newX;
			this.z = newZ;
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0001BF53 File Offset: 0x0001A153
		public IntVec2(Vector2 v2)
		{
			this.x = (int)v2.x;
			this.z = (int)v2.y;
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0001BF6F File Offset: 0x0001A16F
		public Vector2 ToVector2()
		{
			return new Vector2((float)this.x, (float)this.z);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001BF84 File Offset: 0x0001A184
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, 0f, (float)this.z);
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001BF9E File Offset: 0x0001A19E
		public IntVec2 Rotated()
		{
			return new IntVec2(this.z, this.x);
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001BFB4 File Offset: 0x0001A1B4
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.x.ToString(),
				", ",
				this.z.ToString(),
				")"
			});
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0001C000 File Offset: 0x0001A200
		public string ToStringCross()
		{
			return this.x.ToString() + " x " + this.z.ToString();
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0001C024 File Offset: 0x0001A224
		public static IntVec2 FromString(string str)
		{
			str = str.TrimStart('(');
			str = str.TrimEnd(')');
			string[] array = str.Split(',', StringSplitOptions.None);
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			int newX = Convert.ToInt32(array[0], invariantCulture);
			int newZ = Convert.ToInt32(array[1], invariantCulture);
			return new IntVec2(newX, newZ);
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600055F RID: 1375 RVA: 0x0001C06E File Offset: 0x0001A26E
		public static IntVec2 Invalid
		{
			get
			{
				return new IntVec2(-1000, -1000);
			}
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0001C07F File Offset: 0x0001A27F
		public Vector2 ToVector2Shifted()
		{
			return new Vector2((float)this.x + 0.5f, (float)this.z + 0.5f);
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0001C0A0 File Offset: 0x0001A2A0
		public static IntVec2 operator +(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x + b.x, a.z + b.z);
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0001C0C1 File Offset: 0x0001A2C1
		public static IntVec2 operator -(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x - b.x, a.z - b.z);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0001C0E2 File Offset: 0x0001A2E2
		public static IntVec2 operator *(IntVec2 a, int b)
		{
			return new IntVec2(a.x * b, a.z * b);
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0001C0F9 File Offset: 0x0001A2F9
		public static IntVec2 operator *(IntVec2 a, float b)
		{
			return new IntVec2(Mathf.RoundToInt((float)a.x * b), Mathf.RoundToInt((float)a.z * b));
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0001C11C File Offset: 0x0001A31C
		public static IntVec2 operator /(IntVec2 a, int b)
		{
			return new IntVec2(a.x / b, a.z / b);
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x0001C133 File Offset: 0x0001A333
		public IntVec3 ToIntVec3
		{
			get
			{
				return new IntVec3(this.x, 0, this.z);
			}
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001C147 File Offset: 0x0001A347
		public static bool operator ==(IntVec2 a, IntVec2 b)
		{
			return a.x == b.x && a.z == b.z;
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0001C168 File Offset: 0x0001A368
		public static bool operator !=(IntVec2 a, IntVec2 b)
		{
			return a.x != b.x || a.z != b.z;
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001C189 File Offset: 0x0001A389
		public override bool Equals(object obj)
		{
			return obj is IntVec2 && this.Equals((IntVec2)obj);
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001C1A1 File Offset: 0x0001A3A1
		public bool Equals(IntVec2 other)
		{
			return this.x == other.x && this.z == other.z;
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001C1C1 File Offset: 0x0001A3C1
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.x, this.z);
		}

		// Token: 0x040002E9 RID: 745
		public int x;

		// Token: 0x040002EA RID: 746
		public int z;
	}
}
