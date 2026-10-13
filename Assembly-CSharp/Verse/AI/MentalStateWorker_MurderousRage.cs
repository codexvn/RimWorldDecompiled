using System;

namespace Verse.AI
{
	// Token: 0x02000FAA RID: 4010
	public class MentalStateWorker_MurderousRage : MentalStateWorker
	{
		// Token: 0x0600629E RID: 25246 RVA: 0x001F15E7 File Offset: 0x001EF7E7
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && MurderousRageMentalStateUtility.FindPawnToKill(pawn) != null;
		}
	}
}
