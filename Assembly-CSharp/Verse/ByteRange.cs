using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000B8 RID: 184
	public struct ByteRange : IEquatable<ByteRange>
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x0001705F File Offset: 0x0001525F
		public static ByteRange Zero
		{
			get
			{
				return new ByteRange(0, 0);
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x00017068 File Offset: 0x00015268
		public static ByteRange One
		{
			get
			{
				return new ByteRange(1, 1);
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x00017071 File Offset: 0x00015271
		public byte TrueMin
		{
			get
			{
				return Math.Min(this.min, this.max);
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x00017084 File Offset: 0x00015284
		public byte TrueMax
		{
			get
			{
				return Math.Max(this.min, this.max);
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x00017097 File Offset: 0x00015297
		public float Average
		{
			get
			{
				return ((float)this.min + (float)this.max) / 2f;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x000170AE File Offset: 0x000152AE
		public int RandomInRange
		{
			get
			{
				return Rand.RangeInclusive((int)this.min, (int)this.max);
			}
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x000170C1 File Offset: 0x000152C1
		public ByteRange(byte min, byte max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x000170D1 File Offset: 0x000152D1
		public int Lerped(float lerpFactor)
		{
			return (int)this.min + Mathf.RoundToInt(lerpFactor * (float)(this.max - this.min));
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x000170F0 File Offset: 0x000152F0
		public static ByteRange FromString(string s)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			string[] array = s.Split('~', StringSplitOptions.None);
			if (array.Length == 1)
			{
				byte b = Convert.ToByte(array[0], invariantCulture);
				return new ByteRange(b, b);
			}
			byte b2 = array[0].NullOrEmpty() ? 0 : Convert.ToByte(array[0], invariantCulture);
			byte b3 = array[1].NullOrEmpty() ? byte.MaxValue : Convert.ToByte(array[1], invariantCulture);
			return new ByteRange(b2, b3);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0001715A File Offset: 0x0001535A
		public override string ToString()
		{
			if (this.min == this.max)
			{
				return this.min.ToString();
			}
			return this.min.ToString() + "~" + this.max.ToString();
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00017196 File Offset: 0x00015396
		public override int GetHashCode()
		{
			return Gen.HashCombineInt((int)this.min, (int)this.max);
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x000171AC File Offset: 0x000153AC
		public override bool Equals(object obj)
		{
			if (obj is ByteRange)
			{
				ByteRange other = (ByteRange)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x000171D3 File Offset: 0x000153D3
		public bool Equals(ByteRange other)
		{
			return this.min == other.min && this.max == other.max;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x000171F3 File Offset: 0x000153F3
		public static bool operator ==(ByteRange lhs, ByteRange rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x000171FD File Offset: 0x000153FD
		public static bool operator !=(ByteRange lhs, ByteRange rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00017209 File Offset: 0x00015409
		internal bool Includes(int val)
		{
			return val >= (int)this.min && val <= (int)this.max;
		}

		// Token: 0x04000271 RID: 625
		public byte min;

		// Token: 0x04000272 RID: 626
		public byte max;
	}
}
