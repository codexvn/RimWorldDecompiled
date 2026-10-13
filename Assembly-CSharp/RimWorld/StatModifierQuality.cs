using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019C7 RID: 6599
	public class StatModifierQuality
	{
		// Token: 0x170016B1 RID: 5809
		// (get) Token: 0x06009528 RID: 38184 RVA: 0x002BB994 File Offset: 0x002B9B94
		public string ToStringAsOffsetRange
		{
			get
			{
				string str = this.stat.Worker.ValueToString(this.GetValue(QualityCategory.Awful), false, ToStringNumberSense.Offset);
				string str2 = this.stat.Worker.ValueToString(this.GetValue(QualityCategory.Legendary), false, ToStringNumberSense.Offset);
				return str + " ~ " + str2;
			}
		}

		// Token: 0x170016B2 RID: 5810
		// (get) Token: 0x06009529 RID: 38185 RVA: 0x002BB9E0 File Offset: 0x002B9BE0
		public string ToStringAsFactorRange
		{
			get
			{
				string str = this.GetValue(QualityCategory.Awful).ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Absolute);
				string str2 = this.GetValue(QualityCategory.Legendary).ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Absolute);
				return str + " ~ " + str2;
			}
		}

		// Token: 0x0600952A RID: 38186 RVA: 0x002BBA18 File Offset: 0x002B9C18
		public float GetValue(QualityCategory qc)
		{
			switch (qc)
			{
			case QualityCategory.Awful:
				return this.awful;
			case QualityCategory.Poor:
				return this.poor;
			case QualityCategory.Normal:
				return this.normal;
			case QualityCategory.Good:
				return this.good;
			case QualityCategory.Excellent:
				return this.excellent;
			case QualityCategory.Masterwork:
				return this.masterwork;
			case QualityCategory.Legendary:
				return this.legendary;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x04006122 RID: 24866
		public StatDef stat;

		// Token: 0x04006123 RID: 24867
		private float awful;

		// Token: 0x04006124 RID: 24868
		private float poor;

		// Token: 0x04006125 RID: 24869
		private float normal;

		// Token: 0x04006126 RID: 24870
		private float good;

		// Token: 0x04006127 RID: 24871
		private float excellent;

		// Token: 0x04006128 RID: 24872
		private float masterwork;

		// Token: 0x04006129 RID: 24873
		private float legendary;
	}
}
