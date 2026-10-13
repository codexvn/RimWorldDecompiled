using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000FA0 RID: 4000
	public class MentalStateWorker_TantrumAll : MentalStateWorker
	{
		// Token: 0x06006286 RID: 25222 RVA: 0x001F1388 File Offset: 0x001EF588
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			MentalStateWorker_TantrumAll.tmpThings.Clear();
			TantrumMentalStateUtility.GetSmashableThingsNear(pawn, pawn.Position, MentalStateWorker_TantrumAll.tmpThings, null, 0, 40);
			bool result = MentalStateWorker_TantrumAll.tmpThings.Count >= 2;
			MentalStateWorker_TantrumAll.tmpThings.Clear();
			return result;
		}

		// Token: 0x04004680 RID: 18048
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
