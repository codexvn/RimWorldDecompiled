using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001622 RID: 5666
	public class WorkGiver_TendOtherUrgent : WorkGiver_TendOther
	{
		// Token: 0x0600859E RID: 34206 RVA: 0x0027E9E6 File Offset: 0x0027CBE6
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return base.HasJobOnThing(pawn, t, forced) && HealthAIUtility.ShouldBeTendedNowByPlayerUrgent((Pawn)t);
		}
	}
}
