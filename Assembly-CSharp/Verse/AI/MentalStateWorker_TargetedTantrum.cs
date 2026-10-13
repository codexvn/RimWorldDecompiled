using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000FA2 RID: 4002
	public class MentalStateWorker_TargetedTantrum : MentalStateWorker
	{
		// Token: 0x0600628C RID: 25228 RVA: 0x001F146C File Offset: 0x001EF66C
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			MentalStateWorker_TargetedTantrum.tmpThings.Clear();
			TantrumMentalStateUtility.GetSmashableThingsNear(pawn, pawn.Position, MentalStateWorker_TargetedTantrum.tmpThings, null, 300, 40);
			bool result = MentalStateWorker_TargetedTantrum.tmpThings.Any<Thing>();
			MentalStateWorker_TargetedTantrum.tmpThings.Clear();
			return result;
		}

		// Token: 0x04004682 RID: 18050
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
