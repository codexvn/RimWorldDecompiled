using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014D6 RID: 5334
	public class JobGiver_UnloadYourInventory : ThinkNode_JobGiver
	{
		// Token: 0x06007F7C RID: 32636 RVA: 0x00265FA8 File Offset: 0x002641A8
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.inventory.UnloadEverything)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.UnloadYourInventory);
		}
	}
}
