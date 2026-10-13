using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000F9E RID: 3998
	public class MentalStateWorker_InsultingSpreeAll : MentalStateWorker
	{
		// Token: 0x06006280 RID: 25216 RVA: 0x001F130F File Offset: 0x001EF50F
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(pawn, MentalStateWorker_InsultingSpreeAll.candidates, true);
			bool result = MentalStateWorker_InsultingSpreeAll.candidates.Count >= 2;
			MentalStateWorker_InsultingSpreeAll.candidates.Clear();
			return result;
		}

		// Token: 0x0400467E RID: 18046
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
