using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F68 RID: 3944
	public class MentalBreakWorker_Catatonic : MentalBreakWorker
	{
		// Token: 0x06006182 RID: 24962 RVA: 0x001EDCA7 File Offset: 0x001EBEA7
		public override bool BreakCanOccur(Pawn pawn)
		{
			return pawn.IsColonist && pawn.Spawned && base.BreakCanOccur(pawn);
		}

		// Token: 0x06006183 RID: 24963 RVA: 0x001EDCC4 File Offset: 0x001EBEC4
		public override bool TryStart(Pawn pawn, string reason, bool causedByMood)
		{
			pawn.health.AddHediff(HediffDefOf.CatatonicBreakdown, null, null, null);
			base.TrySendLetter(pawn, "LetterCatatonicMentalBreak", reason);
			return true;
		}
	}
}
