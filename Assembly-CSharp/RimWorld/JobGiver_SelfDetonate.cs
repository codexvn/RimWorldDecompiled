using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013E3 RID: 5091
	public class JobGiver_SelfDetonate : ThinkNode_JobGiver
	{
		// Token: 0x06007C52 RID: 31826 RVA: 0x00257162 File Offset: 0x00255362
		protected override Job TryGiveJob(Pawn pawn)
		{
			return JobMaker.MakeJob(JobDefOf.SelfDetonate);
		}
	}
}
