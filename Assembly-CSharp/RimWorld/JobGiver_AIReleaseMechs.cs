using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200140F RID: 5135
	public class JobGiver_AIReleaseMechs : ThinkNode_JobGiver
	{
		// Token: 0x06007D13 RID: 32019 RVA: 0x0025A54C File Offset: 0x0025874C
		protected override Job TryGiveJob(Pawn pawn)
		{
			CompMechCarrier compMechCarrier = pawn.TryGetComp<CompMechCarrier>();
			if (compMechCarrier != null && compMechCarrier.CanSpawn)
			{
				return JobMaker.MakeJob(JobDefOf.ReleaseMechs);
			}
			return null;
		}
	}
}
