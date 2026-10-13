using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019A9 RID: 6569
	public class IngestionOutcomeDoer_OffsetPsyfocus : IngestionOutcomeDoer
	{
		// Token: 0x060094A3 RID: 38051 RVA: 0x002BA2CB File Offset: 0x002B84CB
		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
		{
			Pawn_PsychicEntropyTracker psychicEntropy = pawn.psychicEntropy;
			if (psychicEntropy == null)
			{
				return;
			}
			psychicEntropy.OffsetPsyfocusDirectly(this.offset * (float)ingestedCount);
		}

		// Token: 0x060094A4 RID: 38052 RVA: 0x002BA2E6 File Offset: 0x002B84E6
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			if (ModsConfig.RoyaltyActive)
			{
				string str = (this.offset >= 0f) ? "+" : string.Empty;
				yield return new StatDrawEntry(StatCategoryDefOf.Drug, "Psyfocus".Translate(), str + this.offset.ToStringPercent(), "PsyfocusDesc".Translate() + ".", 1000, null, null, false, false);
			}
			yield break;
		}

		// Token: 0x0400608C RID: 24716
		public float offset;
	}
}
