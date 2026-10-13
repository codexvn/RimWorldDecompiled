using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200100E RID: 4110
	public class JobGiver_Orders : ThinkNode_JobGiver
	{
		// Token: 0x060064B7 RID: 25783 RVA: 0x001FE2F9 File Offset: 0x001FC4F9
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.Drafted)
			{
				return JobMaker.MakeJob(JobDefOf.Wait_Combat, pawn.Position);
			}
			return null;
		}
	}
}
