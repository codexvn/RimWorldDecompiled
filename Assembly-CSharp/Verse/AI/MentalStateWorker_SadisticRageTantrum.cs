using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000FA3 RID: 4003
	public class MentalStateWorker_SadisticRageTantrum : MentalStateWorker
	{
		// Token: 0x0600628F RID: 25231 RVA: 0x001F14C8 File Offset: 0x001EF6C8
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			MentalStateWorker_SadisticRageTantrum.tmpThings.Clear();
			TantrumMentalStateUtility.GetSmashableThingsNear(pawn, pawn.Position, MentalStateWorker_SadisticRageTantrum.tmpThings, (Thing x) => TantrumMentalStateUtility.CanAttackPrisoner(pawn, x), 0, 40);
			bool result = MentalStateWorker_SadisticRageTantrum.tmpThings.Any<Thing>();
			MentalStateWorker_SadisticRageTantrum.tmpThings.Clear();
			return result;
		}

		// Token: 0x04004683 RID: 18051
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
