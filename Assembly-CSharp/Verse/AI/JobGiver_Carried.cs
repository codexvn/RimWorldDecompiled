using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02001001 RID: 4097
	public class JobGiver_Carried : ThinkNode_JobGiver
	{
		// Token: 0x0600649A RID: 25754 RVA: 0x001FDCC8 File Offset: 0x001FBEC8
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.Awake() && pawn.CurJob != null)
			{
				return pawn.CurJob;
			}
			return JobMaker.MakeJob(JobDefOf.Carried);
		}
	}
}
