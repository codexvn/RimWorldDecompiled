using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001425 RID: 5157
	public abstract class JobGiver_PrepareCaravan_RopePawns : ThinkNode_JobGiver
	{
		// Token: 0x17001397 RID: 5015
		// (get) Token: 0x06007D5B RID: 32091
		protected abstract JobDef RopeJobDef { get; }

		// Token: 0x06007D5C RID: 32092 RVA: 0x0025C1A0 File Offset: 0x0025A3A0
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			Pawn pawn2;
			if (pawn.roping.IsRopingOthers)
			{
				pawn2 = pawn.roping.Ropees[0];
			}
			else
			{
				pawn2 = this.FindAnimalNeedingGathering(pawn);
			}
			if (pawn2 == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(this.RopeJobDef, pawn2, cell);
			job.lord = pawn.GetLord();
			this.DecorateJob(job);
			return job;
		}

		// Token: 0x06007D5D RID: 32093 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void DecorateJob(Job job)
		{
		}

		// Token: 0x06007D5E RID: 32094 RVA: 0x0025C220 File Offset: 0x0025A420
		private Pawn FindAnimalNeedingGathering(Pawn pawn)
		{
			foreach (Pawn pawn2 in pawn.GetLord().ownedPawns)
			{
				if (this.AnimalNeedsGathering(pawn, pawn2))
				{
					return pawn2;
				}
			}
			return null;
		}

		// Token: 0x06007D5F RID: 32095
		protected abstract bool AnimalNeedsGathering(Pawn pawn, Pawn animal);
	}
}
