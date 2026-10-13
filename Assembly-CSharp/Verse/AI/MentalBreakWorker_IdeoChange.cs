using System;

namespace Verse.AI
{
	// Token: 0x02000F66 RID: 3942
	public class MentalBreakWorker_IdeoChange : MentalBreakWorker
	{
		// Token: 0x0600617D RID: 24957 RVA: 0x001EDC2A File Offset: 0x001EBE2A
		public override bool BreakCanOccur(Pawn pawn)
		{
			return ModsConfig.IdeologyActive && !Find.IdeoManager.classicMode && base.BreakCanOccur(pawn);
		}
	}
}
