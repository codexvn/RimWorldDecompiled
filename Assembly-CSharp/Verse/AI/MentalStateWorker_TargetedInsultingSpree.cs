using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000F9F RID: 3999
	public class MentalStateWorker_TargetedInsultingSpree : MentalStateWorker
	{
		// Token: 0x06006283 RID: 25219 RVA: 0x001F134E File Offset: 0x001EF54E
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(pawn, MentalStateWorker_TargetedInsultingSpree.candidates, false);
			bool result = MentalStateWorker_TargetedInsultingSpree.candidates.Any<Pawn>();
			MentalStateWorker_TargetedInsultingSpree.candidates.Clear();
			return result;
		}

		// Token: 0x0400467F RID: 18047
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
