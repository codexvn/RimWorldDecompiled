using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003719 RID: 14105
	public class StatPart_Quality : StatPart
	{
		// Token: 0x060138DF RID: 80095 RVA: 0x005CC718 File Offset: 0x005CA918
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (val <= 0f && !this.applyToNegativeValues)
			{
				return;
			}
			float num = val * this.QualityMultiplier(req.QualityCategory) - val;
			num = Mathf.Min(num, this.MaxGain(req.QualityCategory));
			val += num;
		}

		// Token: 0x060138E0 RID: 80096 RVA: 0x005CC768 File Offset: 0x005CA968
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && !this.applyToNegativeValues && req.Thing.GetStatValue(this.parentStat, true, -1) <= 0f)
			{
				return null;
			}
			QualityCategory qc;
			if (req.HasThing && req.Thing.TryGetQuality(out qc))
			{
				string text = "StatsReport_QualityMultiplier".Translate() + ": x" + this.QualityMultiplier(qc).ToStringPercent();
				float num = this.MaxGain(qc);
				if (num < 999999f)
				{
					text += "\n    (" + "StatsReport_MaxGain".Translate() + ": " + num.ToStringByStyle(this.parentStat.ToStringStyleUnfinalized, this.parentStat.toStringNumberSense) + ")";
				}
				return text;
			}
			return null;
		}

		// Token: 0x060138E1 RID: 80097 RVA: 0x005CC858 File Offset: 0x005CAA58
		private float QualityMultiplier(QualityCategory qc)
		{
			switch (qc)
			{
			case QualityCategory.Awful:
				return this.factorAwful;
			case QualityCategory.Poor:
				return this.factorPoor;
			case QualityCategory.Normal:
				return this.factorNormal;
			case QualityCategory.Good:
				return this.factorGood;
			case QualityCategory.Excellent:
				return this.factorExcellent;
			case QualityCategory.Masterwork:
				return this.factorMasterwork;
			case QualityCategory.Legendary:
				return this.factorLegendary;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x060138E2 RID: 80098 RVA: 0x005CC8C0 File Offset: 0x005CAAC0
		private float MaxGain(QualityCategory qc)
		{
			switch (qc)
			{
			case QualityCategory.Awful:
				return this.maxGainAwful;
			case QualityCategory.Poor:
				return this.maxGainPoor;
			case QualityCategory.Normal:
				return this.maxGainNormal;
			case QualityCategory.Good:
				return this.maxGainGood;
			case QualityCategory.Excellent:
				return this.maxGainExcellent;
			case QualityCategory.Masterwork:
				return this.maxGainMasterwork;
			case QualityCategory.Legendary:
				return this.maxGainLegendary;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x0400D8E1 RID: 55521
		private bool applyToNegativeValues;

		// Token: 0x0400D8E2 RID: 55522
		private float factorAwful = 1f;

		// Token: 0x0400D8E3 RID: 55523
		private float factorPoor = 1f;

		// Token: 0x0400D8E4 RID: 55524
		private float factorNormal = 1f;

		// Token: 0x0400D8E5 RID: 55525
		private float factorGood = 1f;

		// Token: 0x0400D8E6 RID: 55526
		private float factorExcellent = 1f;

		// Token: 0x0400D8E7 RID: 55527
		private float factorMasterwork = 1f;

		// Token: 0x0400D8E8 RID: 55528
		private float factorLegendary = 1f;

		// Token: 0x0400D8E9 RID: 55529
		private float maxGainAwful = 9999999f;

		// Token: 0x0400D8EA RID: 55530
		private float maxGainPoor = 9999999f;

		// Token: 0x0400D8EB RID: 55531
		private float maxGainNormal = 9999999f;

		// Token: 0x0400D8EC RID: 55532
		private float maxGainGood = 9999999f;

		// Token: 0x0400D8ED RID: 55533
		private float maxGainExcellent = 9999999f;

		// Token: 0x0400D8EE RID: 55534
		private float maxGainMasterwork = 9999999f;

		// Token: 0x0400D8EF RID: 55535
		private float maxGainLegendary = 9999999f;
	}
}
