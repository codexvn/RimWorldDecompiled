using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019AF RID: 6575
	public class IngestionOutcomeDoer_Psilocap : IngestionOutcomeDoer
	{
		// Token: 0x060094C4 RID: 38084 RVA: 0x002BA608 File Offset: 0x002B8808
		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
		{
			if (Rand.Value < this.chanceBreakdown)
			{
				pawn.mindState.mentalBreaker.TryDoMentalBreak("MentalBreakReason_Psilocap".Translate(), MentalBreakDefOf.Catatonic);
				return;
			}
			if (Rand.Value < this.chanceInspiration)
			{
				pawn.mindState.inspirationHandler.TryStartInspiration(InspirationDefOf.Inspired_Creativity, "LetterInspirationBeginPsilocap".Translate(), true);
			}
		}

		// Token: 0x04006099 RID: 24729
		public float chanceBreakdown = 0.01f;

		// Token: 0x0400609A RID: 24730
		public float chanceInspiration = 0.02f;
	}
}
