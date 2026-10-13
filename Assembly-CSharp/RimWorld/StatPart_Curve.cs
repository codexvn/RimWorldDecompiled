using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036F0 RID: 14064
	public abstract class StatPart_Curve : StatPart
	{
		// Token: 0x17003223 RID: 12835
		// (get) Token: 0x06013819 RID: 79897 RVA: 0x005CA090 File Offset: 0x005C8290
		public SimpleCurve Curve
		{
			get
			{
				return this.curve;
			}
		}

		// Token: 0x0601381A RID: 79898
		protected abstract bool AppliesTo(StatRequest req);

		// Token: 0x0601381B RID: 79899
		protected abstract float CurveXGetter(StatRequest req);

		// Token: 0x0601381C RID: 79900
		protected abstract string ExplanationLabel(StatRequest req);

		// Token: 0x0601381D RID: 79901 RVA: 0x005CA098 File Offset: 0x005C8298
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && this.AppliesTo(req))
			{
				val *= this.curve.Evaluate(this.CurveXGetter(req));
			}
		}

		// Token: 0x0601381E RID: 79902 RVA: 0x005CA0C3 File Offset: 0x005C82C3
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.AppliesTo(req))
			{
				return this.ExplanationLabel(req) + ": x" + this.curve.Evaluate(this.CurveXGetter(req)).ToStringPercent();
			}
			return null;
		}

		// Token: 0x0400D898 RID: 55448
		protected SimpleCurve curve;
	}
}
