using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200159A RID: 5530
	public class WorkGiver_BottleFeedBaby : WorkGiver_FeedBabyManually
	{
		// Token: 0x060082DD RID: 33501 RVA: 0x00274E54 File Offset: 0x00273054
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.CanCreateManualFeedingJob(pawn, t, forced))
			{
				return null;
			}
			Pawn baby = (Pawn)t;
			Thing thing = ChildcareUtility.FindBabyFoodForBaby(pawn, baby);
			if (thing != null)
			{
				return ChildcareUtility.MakeBottlefeedJob(baby, thing);
			}
			JobFailReason.Is("NoBabyFood".Translate(), null);
			return null;
		}
	}
}
