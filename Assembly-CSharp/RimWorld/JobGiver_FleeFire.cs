using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001488 RID: 5256
	public class JobGiver_FleeFire : ThinkNode_JobGiver
	{
		// Token: 0x06007E77 RID: 32375 RVA: 0x00260FE4 File Offset: 0x0025F1E4
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = FleeUtility.FleeLargeFireJob(pawn, 1, 20, 24);
			if (job != null)
			{
				return job;
			}
			return null;
		}

		// Token: 0x040054CF RID: 21711
		private const int FleeDistance = 24;

		// Token: 0x040054D0 RID: 21712
		private const int DistToFireToFlee = 20;
	}
}
