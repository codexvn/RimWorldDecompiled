using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001494 RID: 5268
	public class JobGiver_GetEnergy_SelfShutdown : JobGiver_GetEnergy
	{
		// Token: 0x06007E9A RID: 32410 RVA: 0x00261870 File Offset: 0x0025FA70
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!this.ShouldAutoRecharge(pawn))
			{
				return null;
			}
			IntVec3 c;
			if (RCellFinder.TryFindRandomMechSelfShutdownSpot(pawn.Position, pawn, pawn.Map, out c, false))
			{
				Job job = JobMaker.MakeJob(JobDefOf.SelfShutdown, c);
				job.checkOverrideOnExpire = true;
				job.expiryInterval = 500;
				return job;
			}
			return null;
		}
	}
}
