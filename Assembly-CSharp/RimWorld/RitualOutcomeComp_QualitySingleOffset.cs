using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002734 RID: 10036
	public abstract class RitualOutcomeComp_QualitySingleOffset : RitualOutcomeComp_Quality
	{
		// Token: 0x170021BD RID: 8637
		// (get) Token: 0x0600DC45 RID: 56389 RVA: 0x0040DC61 File Offset: 0x0040BE61
		protected virtual string LabelForDesc
		{
			get
			{
				return this.label.CapitalizeFirst();
			}
		}

		// Token: 0x0600DC46 RID: 56390 RVA: 0x0040DC70 File Offset: 0x0040BE70
		public override string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
		{
			string str = (this.qualityOffset < 0f) ? "" : "+";
			return this.LabelForDesc + ": " + "OutcomeBonusDesc_QualitySingleOffset".Translate(str + this.qualityOffset.ToStringPercent()) + ".";
		}

		// Token: 0x0600DC47 RID: 56391 RVA: 0x0040DCDB File Offset: 0x0040BEDB
		public override string GetBonusDescShort()
		{
			return "OutcomeBonusDesc_QualitySingleOffset".Translate("+" + this.qualityOffset.ToStringPercent()) + ".";
		}

		// Token: 0x0600DC48 RID: 56392 RVA: 0x0004FC72 File Offset: 0x0004DE72
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return 1f;
		}

		// Token: 0x0600DC49 RID: 56393 RVA: 0x0040DD10 File Offset: 0x0040BF10
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return this.qualityOffset;
		}

		// Token: 0x0600DC4A RID: 56394 RVA: 0x0040DD18 File Offset: 0x0040BF18
		protected override string ExpectedOffsetDesc(bool positive, float quality = -1f)
		{
			quality = ((quality == -1f) ? this.qualityOffset : quality);
			return positive ? quality.ToStringWithSign("0.#%") : "QualityOutOf".Translate("+0", quality.ToStringWithSign("0.#%"));
		}
	}
}
