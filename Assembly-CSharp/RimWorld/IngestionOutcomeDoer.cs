using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020019A4 RID: 6564
	public abstract class IngestionOutcomeDoer
	{
		// Token: 0x06009488 RID: 38024 RVA: 0x002B9ED3 File Offset: 0x002B80D3
		public void DoIngestionOutcome(Pawn pawn, Thing ingested, int ingestedCount)
		{
			if (Rand.Value < this.chance)
			{
				this.DoIngestionOutcomeSpecial(pawn, ingested, ingestedCount);
			}
		}

		// Token: 0x06009489 RID: 38025
		protected abstract void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount);

		// Token: 0x0600948A RID: 38026 RVA: 0x000444B9 File Offset: 0x000426B9
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			return Enumerable.Empty<StatDrawEntry>();
		}

		// Token: 0x04006076 RID: 24694
		public float chance = 1f;

		// Token: 0x04006077 RID: 24695
		public bool doToGeneratedPawnIfAddicted;
	}
}
