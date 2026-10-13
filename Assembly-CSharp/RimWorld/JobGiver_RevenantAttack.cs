using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014C2 RID: 5314
	public class JobGiver_RevenantAttack : ThinkNode_JobGiver
	{
		// Token: 0x06007F3A RID: 32570 RVA: 0x0026510E File Offset: 0x0026330E
		protected override Job TryGiveJob(Pawn pawn)
		{
			return JobMaker.MakeJob(JobDefOf.RevenantAttack, pawn.mindState.enemyTarget);
		}
	}
}
