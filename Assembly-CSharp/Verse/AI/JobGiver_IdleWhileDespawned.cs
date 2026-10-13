using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200100C RID: 4108
	public class JobGiver_IdleWhileDespawned : ThinkNode_JobGiver
	{
		// Token: 0x060064B2 RID: 25778 RVA: 0x001FE202 File Offset: 0x001FC402
		protected override Job TryGiveJob(Pawn pawn)
		{
			return JobMaker.MakeJob(JobDefOf.IdleWhileDespawned);
		}
	}
}
