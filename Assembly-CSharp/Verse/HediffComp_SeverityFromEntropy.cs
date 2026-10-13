using System;

namespace Verse
{
	// Token: 0x02000701 RID: 1793
	public class HediffComp_SeverityFromEntropy : HediffComp
	{
		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x06002BED RID: 11245 RVA: 0x000E4AE2 File Offset: 0x000E2CE2
		private float EntropyAmount
		{
			get
			{
				if (base.Pawn.psychicEntropy != null)
				{
					return base.Pawn.psychicEntropy.EntropyRelativeValue;
				}
				return 0f;
			}
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06002BEE RID: 11246 RVA: 0x000E4B07 File Offset: 0x000E2D07
		public override bool CompShouldRemove
		{
			get
			{
				return this.EntropyAmount < float.Epsilon;
			}
		}

		// Token: 0x06002BEF RID: 11247 RVA: 0x000E4B16 File Offset: 0x000E2D16
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.parent.Severity = this.EntropyAmount;
		}
	}
}
