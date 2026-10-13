using System;

namespace Verse.AI
{
	// Token: 0x02000FA5 RID: 4005
	public class MentalStateWorker_CorpseObsession : MentalStateWorker
	{
		// Token: 0x06006294 RID: 25236 RVA: 0x001F1554 File Offset: 0x001EF754
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && CorpseObsessionMentalStateUtility.GetClosestCorpseToDigUp(pawn) != null;
		}
	}
}
