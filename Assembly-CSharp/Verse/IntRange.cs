using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000CF RID: 207
	public struct IntRange : IEquatable<IntRange>
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x0001BC6B File Offset: 0x00019E6B
		public static IntRange Zero
		{
			get
			{
				return new IntRange(0, 0);
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x0001BC74 File Offset: 0x00019E74
		[Obsolete]
		public static IntRange zero
		{
			get
			{
				return IntRange.Zero;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x0001BC7B File Offset: 0x00019E7B
		public static IntRange One
		{
			get
			{
				return new IntRange(1, 1);
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x0001BC84 File Offset: 0x00019E84
		[Obsolete]
		public static IntRange one
		{
			get
			{
				return IntRange.One;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x0001BC8B File Offset: 0x00019E8B
		public static IntRange Invalid
		{
			get
			{
				return new IntRange(-1, -1);
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x0001BC94 File Offset: 0x00019E94
		[Obsolete]
		public static IntRange invalid
		{
			get
			{
				return IntRange.Invalid;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x0001BC9B File Offset: 0x00019E9B
		public int TrueMin
		{
			get
			{
				return Mathf.Min(this.min, this.max);
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x0001BCAE File Offset: 0x00019EAE
		public int TrueMax
		{
			get
			{
				return Mathf.Max(this.min, this.max);
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x0001BCC1 File Offset: 0x00019EC1
		public float Average
		{
			get
			{
				return ((float)this.min + (float)this.max) / 2f;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x0001BCD8 File Offset: 0x00019ED8
		public int RandomInRange
		{
			get
			{
				return Rand.RangeInclusive(this.min, this.max);
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x0001BCEB File Offset: 0x00019EEB
		public bool IsValid
		{
			get
			{
				return this != IntRange.Invalid;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x0001BCFD File Offset: 0x00019EFD
		public bool IsInvalid
		{
			get
			{
				return this == IntRange.Invalid;
			}
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0001BD0F File Offset: 0x00019F0F
		public static IntRange Between(int min, int max)
		{
			return new IntRange(min, max);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0001BD18 File Offset: 0x00019F18
		public IntRange(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0001BD28 File Offset: 0x00019F28
		public IntRange(int val)
		{
			this.min = val;
			this.max = val;
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001BD38 File Offset: 0x00019F38
		public int Lerped(float lerpFactor)
		{
			return this.min + Mathf.RoundToInt(lerpFactor * (float)(this.max - this.min));
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0001BD58 File Offset: 0x00019F58
		public static IntRange FromString(string s)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			string[] array = s.Split('~', StringSplitOptions.None);
			if (array.Length == 1)
			{
				int num = Convert.ToInt32(array[0], invariantCulture);
				return new IntRange(num, num);
			}
			int num2 = array[0].NullOrEmpty() ? int.MinValue : Convert.ToInt32(array[0], invariantCulture);
			int num3 = array[1].NullOrEmpty() ? int.MaxValue : Convert.ToInt32(array[1], invariantCulture);
			return new IntRange(num2, num3);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001BDC6 File Offset: 0x00019FC6
		public override string ToString()
		{
			if (this.min == this.max)
			{
				return this.min.ToString();
			}
			return this.min.ToString() + "~" + this.max.ToString();
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001BE02 File Offset: 0x0001A002
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.min, this.max);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001BE18 File Offset: 0x0001A018
		public override bool Equals(object obj)
		{
			if (obj is IntRange)
			{
				IntRange other = (IntRange)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0001BE3F File Offset: 0x0001A03F
		public bool Equals(IntRange other)
		{
			return this.min == other.min && this.max == other.max;
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0001BE5F File Offset: 0x0001A05F
		public static bool operator ==(IntRange lhs, IntRange rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001BE69 File Offset: 0x0001A069
		public static bool operator !=(IntRange lhs, IntRange rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0001BE75 File Offset: 0x0001A075
		internal bool Includes(int val)
		{
			return val >= this.min && val <= this.max;
		}

		// Token: 0x040002E7 RID: 743
		public int min;

		// Token: 0x040002E8 RID: 744
		public int max;
	}
}
