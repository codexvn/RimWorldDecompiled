using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001468 RID: 5224
	public class JobGiver_Steal : ThinkNode_JobGiver
	{
		// Token: 0x06007E17 RID: 32279 RVA: 0x0025F658 File Offset: 0x0025D858
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (!RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn, true))
			{
				return null;
			}
			Thing thing;
			if (StealAIUtility.TryFindBestItemToSteal(pawn.Position, pawn.Map, 12f, out thing, pawn, null) && !GenAI.InDangerousCombat(pawn))
			{
				Job job = JobMaker.MakeJob(JobDefOf.Steal);
				job.targetA = thing;
				job.targetB = c;
				job.count = Mathf.Min(thing.stackCount, (int)(pawn.GetStatValue(StatDefOf.CarryingCapacity, true, -1) / thing.def.VolumePerUnit));
				return job;
			}
			return null;
		}

		// Token: 0x040054A2 RID: 21666
		public const float ItemsSearchRadiusInitial = 7f;

		// Token: 0x040054A3 RID: 21667
		private const float ItemsSearchRadiusOngoing = 12f;
	}
}
