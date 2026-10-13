using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002733 RID: 10035
	public abstract class RitualOutcomeComp_Quality : RitualOutcomeComp
	{
		// Token: 0x170021BC RID: 8636
		// (get) Token: 0x0600DC3C RID: 56380 RVA: 0x0040D9D0 File Offset: 0x0040BBD0
		protected float MaxValue
		{
			get
			{
				return this.curve.Points[this.curve.PointsCount - 1].x;
			}
		}

		// Token: 0x0600DC3D RID: 56381 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool Applies(LordJob_Ritual ritual)
		{
			return true;
		}

		// Token: 0x0600DC3E RID: 56382
		public abstract float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data);

		// Token: 0x0600DC3F RID: 56383 RVA: 0x0040DA04 File Offset: 0x0040BC04
		public override string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
		{
			if ((this.DataRequired || ritual == null) && data == null)
			{
				return this.label + " (" + "MaxValue".Translate(this.MaxValue) + "): " + "OutcomeBonusDesc_Quality".Translate("+" + this.curve.Points[this.curve.PointsCount - 1].y.ToStringPercent()) + ".";
			}
			return string.Concat(new string[]
			{
				this.Count(ritual, data).ToString(),
				" / ",
				this.MaxValue.ToString(),
				" ",
				this.label,
				": "
			}) + "OutcomeBonusDesc_QualitySingleOffset".Translate("+" + this.QualityOffset(ritual, data).ToStringPercent()) + ".";
		}

		// Token: 0x0600DC40 RID: 56384 RVA: 0x0040DB3C File Offset: 0x0040BD3C
		public override string GetBonusDescShort()
		{
			return "OutcomeBonusDesc_Quality".Translate("+" + this.curve.Points[this.curve.PointsCount - 1].y.ToStringPercent()) + ".";
		}

		// Token: 0x0600DC41 RID: 56385 RVA: 0x0040DB9B File Offset: 0x0040BD9B
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			if (this.curve == null)
			{
				return 0f;
			}
			return this.curve.Evaluate(this.Count(ritual, data));
		}

		// Token: 0x0600DC42 RID: 56386 RVA: 0x0040DBC0 File Offset: 0x0040BDC0
		protected override string ExpectedOffsetDesc(bool positive, float quality = 0f)
		{
			return "QualityOutOf".Translate(quality.ToStringWithSign("0.#%"), this.curve.Points[this.curve.PointsCount - 1].y.ToStringWithSign("0.#%"));
		}

		// Token: 0x0600DC43 RID: 56387 RVA: 0x0040DC20 File Offset: 0x0040BE20
		public override string GetDescAbstract(bool positive, float quality = -1f)
		{
			quality = ((quality == -1f) ? this.qualityOffset : quality);
			return "OutcomeBonusDesc_QualitySingleOffset".Translate(quality.ToStringWithSign("0.#%"));
		}

		// Token: 0x040094BE RID: 38078
		public SimpleCurve curve;
	}
}
