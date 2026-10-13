using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200065A RID: 1626
	public class Gene_Clotting : Gene
	{
		// Token: 0x06002903 RID: 10499 RVA: 0x000D809C File Offset: 0x000D629C
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.pawn.IsHashIntervalTick(360, delta))
			{
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = hediffs.Count - 1; i >= 0; i--)
				{
					if (hediffs[i].Bleeding)
					{
						hediffs[i].Tended(Gene_Clotting.TendingQualityRange.RandomInRange, Gene_Clotting.TendingQualityRange.TrueMax, 1);
					}
				}
			}
		}

		// Token: 0x0400219E RID: 8606
		private const int ClotCheckInterval = 360;

		// Token: 0x0400219F RID: 8607
		private static readonly FloatRange TendingQualityRange = new FloatRange(0.2f, 0.7f);
	}
}
