using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000CE RID: 206
	public struct FloatRange : IEquatable<FloatRange>
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x0001B98A File Offset: 0x00019B8A
		public static FloatRange Zero
		{
			get
			{
				return new FloatRange(0f, 0f);
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x0001B99B File Offset: 0x00019B9B
		public static FloatRange One
		{
			get
			{
				return new FloatRange(1f, 1f);
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x0001B9AC File Offset: 0x00019BAC
		public static FloatRange ZeroToOne
		{
			get
			{
				return new FloatRange(0f, 1f);
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x0001B9BD File Offset: 0x00019BBD
		public float Average
		{
			get
			{
				return (this.min + this.max) / 2f;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x0001B9D2 File Offset: 0x00019BD2
		public float RandomInRange
		{
			get
			{
				return Rand.Range(this.min, this.max);
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x0001B9E5 File Offset: 0x00019BE5
		public float TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x0001B9F8 File Offset: 0x00019BF8
		public float TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x0001BA0B File Offset: 0x00019C0B
		public float Span
		{
			get
			{
				return this.TrueMax - this.TrueMin;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x0001BA1A File Offset: 0x00019C1A
		public bool IsZeros
		{
			get
			{
				return this.min == 0f && this.max == 0f;
			}
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0001BA38 File Offset: 0x00019C38
		public FloatRange(float value)
		{
			this.min = value;
			this.max = value;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0001BA48 File Offset: 0x00019C48
		public FloatRange(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0001BA58 File Offset: 0x00019C58
		public float ClampToRange(float value)
		{
			return Mathf.Clamp(value, this.min, this.max);
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0001BA6C File Offset: 0x00019C6C
		public float RandomInRangeSeeded(int seed)
		{
			return Rand.RangeSeeded(this.min, this.max, seed);
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0001BA80 File Offset: 0x00019C80
		public float LerpThroughRange(float lerpPct)
		{
			return Mathf.Lerp(this.min, this.max, lerpPct);
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0001BA94 File Offset: 0x00019C94
		public float InverseLerpThroughRange(float f)
		{
			return Mathf.InverseLerp(this.min, this.max, f);
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0001BAA8 File Offset: 0x00019CA8
		public bool Includes(float f)
		{
			return f >= this.min && f <= this.max;
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0001BAC1 File Offset: 0x00019CC1
		public bool IncludesEpsilon(float f)
		{
			return f >= this.min - 1E-05f && f <= this.max + 1E-05f;
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0001BAE6 File Offset: 0x00019CE6
		public FloatRange ExpandedBy(float f)
		{
			return new FloatRange(this.min - f, this.max + f);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0001BAFD File Offset: 0x00019CFD
		public static bool operator ==(FloatRange a, FloatRange b)
		{
			return a.min == b.min && a.max == b.max;
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001BB1D File Offset: 0x00019D1D
		public static bool operator !=(FloatRange a, FloatRange b)
		{
			return a.min != b.min || a.max != b.max;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001BB40 File Offset: 0x00019D40
		public static FloatRange operator *(FloatRange r, float val)
		{
			return new FloatRange(r.min * val, r.max * val);
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0001BB57 File Offset: 0x00019D57
		public static FloatRange operator *(float val, FloatRange r)
		{
			return new FloatRange(r.min * val, r.max * val);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0001BB70 File Offset: 0x00019D70
		public static FloatRange FromString(string s)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			string[] array = s.Split('~', StringSplitOptions.None);
			if (array.Length == 1)
			{
				float num = Convert.ToSingle(array[0], invariantCulture);
				return new FloatRange(num, num);
			}
			return new FloatRange(Convert.ToSingle(array[0], invariantCulture), Convert.ToSingle(array[1], invariantCulture));
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0001BBBA File Offset: 0x00019DBA
		public override string ToString()
		{
			return this.min.ToString("G9") + "~" + this.max.ToString("G9");
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0001BBE6 File Offset: 0x00019DE6
		public string ToString(string format)
		{
			return this.min.ToString(format) + "~" + this.max.ToString(format);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0001BC0A File Offset: 0x00019E0A
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<float>(this.min.GetHashCode(), this.max);
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0001BC24 File Offset: 0x00019E24
		public override bool Equals(object obj)
		{
			if (obj is FloatRange)
			{
				FloatRange other = (FloatRange)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0001BC4B File Offset: 0x00019E4B
		public bool Equals(FloatRange other)
		{
			return other.min == this.min && other.max == this.max;
		}

		// Token: 0x040002E5 RID: 741
		public float min;

		// Token: 0x040002E6 RID: 742
		public float max;
	}
}
