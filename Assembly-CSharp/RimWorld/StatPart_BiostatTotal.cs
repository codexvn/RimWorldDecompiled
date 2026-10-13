using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036E5 RID: 14053
	public abstract class StatPart_BiostatTotal : StatPart_Curve
	{
		// Token: 0x1700321F RID: 12831
		// (get) Token: 0x060137F4 RID: 79860
		public abstract string ExplanationLabelBase { get; }

		// Token: 0x060137F5 RID: 79861 RVA: 0x005C9A74 File Offset: 0x005C7C74
		protected override bool AppliesTo(StatRequest req)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					return pawn.genes != null;
				}
			}
			return false;
		}

		// Token: 0x060137F6 RID: 79862 RVA: 0x005C9AAE File Offset: 0x005C7CAE
		protected override string ExplanationLabel(StatRequest req)
		{
			return this.ExplanationLabelBase + " " + this.CurveXGetter(req).ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Absolute);
		}
	}
}
