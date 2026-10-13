using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001470 RID: 5232
	public class JobGiver_UnnaturalCorpseIdle : ThinkNode_JobGiver
	{
		// Token: 0x06007E32 RID: 32306 RVA: 0x0025FF9C File Offset: 0x0025E19C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.Downed)
			{
				return null;
			}
			UnnaturalCorpseTracker unnaturalCorpseTracker;
			if (!Find.Anomaly.TryGetUnnaturalCorpseTrackerForAwoken(pawn, out unnaturalCorpseTracker))
			{
				return null;
			}
			Pawn haunted = unnaturalCorpseTracker.Haunted;
			if (haunted.DestroyedOrNull())
			{
				return null;
			}
			if (haunted.Dead)
			{
				return JobMaker.MakeJob(JobDefOf.Wait, 300, false);
			}
			return null;
		}
	}
}
