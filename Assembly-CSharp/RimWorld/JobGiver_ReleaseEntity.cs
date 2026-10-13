using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014BF RID: 5311
	public class JobGiver_ReleaseEntity : ThinkNode_JobGiver
	{
		// Token: 0x06007F33 RID: 32563 RVA: 0x00264F94 File Offset: 0x00263194
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = AnomalyUtility.FindEntityOnPlatform(pawn.Map, EntityQueryType.ForRelease);
			Building_HoldingPlatform building_HoldingPlatform = (Building_HoldingPlatform)((pawn2 != null) ? pawn2.ParentHolder : null);
			if (building_HoldingPlatform == null || !pawn.CanReserveAndReach(building_HoldingPlatform, PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, false))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.ReleaseEntity, building_HoldingPlatform, pawn2).WithCount(1);
			job.ignoreDesignations = true;
			return job;
		}
	}
}
