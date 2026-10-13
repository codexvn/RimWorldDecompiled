using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019AD RID: 6573
	public class IngestionOutcomeDoer_UseThing : IngestionOutcomeDoer
	{
		// Token: 0x060094B9 RID: 38073 RVA: 0x002BA56B File Offset: 0x002B876B
		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
		{
			ingested.TryGetComp<CompUsable>().UsedBy(pawn);
		}

		// Token: 0x060094BA RID: 38074 RVA: 0x002BA579 File Offset: 0x002B8779
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			yield break;
		}
	}
}
