using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002EFD RID: 12029
	public struct QualityRange : IEquatable<QualityRange>
	{
		// Token: 0x17002BDC RID: 11228
		// (get) Token: 0x06010FB7 RID: 69559 RVA: 0x004E2EA5 File Offset: 0x004E10A5
		public static QualityRange All
		{
			get
			{
				return new QualityRange(QualityCategory.Awful, QualityCategory.Legendary);
			}
		}

		// Token: 0x06010FB8 RID: 69560 RVA: 0x004E2EAE File Offset: 0x004E10AE
		public QualityRange(QualityCategory min, QualityCategory max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x06010FB9 RID: 69561 RVA: 0x004E2EBE File Offset: 0x004E10BE
		public bool Includes(QualityCategory p)
		{
			return p >= this.min && p <= this.max;
		}

		// Token: 0x06010FBA RID: 69562 RVA: 0x004E2ED7 File Offset: 0x004E10D7
		public static bool operator ==(QualityRange a, QualityRange b)
		{
			return a.min == b.min && a.max == b.max;
		}

		// Token: 0x06010FBB RID: 69563 RVA: 0x004E2EF7 File Offset: 0x004E10F7
		public static bool operator !=(QualityRange a, QualityRange b)
		{
			return !(a == b);
		}

		// Token: 0x06010FBC RID: 69564 RVA: 0x004E2F04 File Offset: 0x004E1104
		public static QualityRange FromString(string s)
		{
			string[] array = s.Split('~', StringSplitOptions.None);
			return new QualityRange(ParseHelper.FromString<QualityCategory>(array[0]), ParseHelper.FromString<QualityCategory>(array[1]));
		}

		// Token: 0x06010FBD RID: 69565 RVA: 0x004E2F30 File Offset: 0x004E1130
		public override string ToString()
		{
			return this.min.ToString() + "~" + this.max.ToString();
		}

		// Token: 0x06010FBE RID: 69566 RVA: 0x004E2F5E File Offset: 0x004E115E
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<QualityCategory>(this.min.GetHashCode(), this.max);
		}

		// Token: 0x06010FBF RID: 69567 RVA: 0x004E2F7C File Offset: 0x004E117C
		public override bool Equals(object obj)
		{
			if (!(obj is QualityRange))
			{
				return false;
			}
			QualityRange qualityRange = (QualityRange)obj;
			return qualityRange.min == this.min && qualityRange.max == this.max;
		}

		// Token: 0x06010FC0 RID: 69568 RVA: 0x004E2FB8 File Offset: 0x004E11B8
		public bool Equals(QualityRange other)
		{
			return other.min == this.min && other.max == this.max;
		}

		// Token: 0x0400B097 RID: 45207
		public QualityCategory min;

		// Token: 0x0400B098 RID: 45208
		public QualityCategory max;
	}
}
