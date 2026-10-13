using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013E5 RID: 5093
	public abstract class JobGiver_Binge : ThinkNode_JobGiver
	{
		// Token: 0x06007C56 RID: 31830 RVA: 0x0025721A File Offset: 0x0025541A
		protected bool IgnoreForbid(Pawn pawn)
		{
			return pawn.InMentalState;
		}

		// Token: 0x06007C57 RID: 31831
		protected abstract int IngestInterval(Pawn pawn);

		// Token: 0x06007C58 RID: 31832 RVA: 0x00257224 File Offset: 0x00255424
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Find.TickManager.TicksGame - pawn.mindState.lastIngestTick > this.IngestInterval(pawn))
			{
				Job job = this.IngestJob(pawn);
				if (job != null)
				{
					return job;
				}
			}
			return null;
		}

		// Token: 0x06007C59 RID: 31833 RVA: 0x00257260 File Offset: 0x00255460
		private Job IngestJob(Pawn pawn)
		{
			Thing thing = this.BestIngestTarget(pawn);
			if (thing == null)
			{
				return null;
			}
			ThingDef finalIngestibleDef = FoodUtility.GetFinalIngestibleDef(thing, false);
			Job job = JobMaker.MakeJob(JobDefOf.Ingest, thing);
			job.count = finalIngestibleDef.ingestible.defaultNumToIngestAtOnce;
			job.ignoreForbidden = this.IgnoreForbid(pawn);
			job.overeat = true;
			return job;
		}

		// Token: 0x06007C5A RID: 31834
		protected abstract Thing BestIngestTarget(Pawn pawn);
	}
}
