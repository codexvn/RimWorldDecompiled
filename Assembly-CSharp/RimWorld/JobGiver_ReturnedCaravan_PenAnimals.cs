using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001428 RID: 5160
	public class JobGiver_ReturnedCaravan_PenAnimals : JobGiver_PrepareCaravan_RopePawns
	{
		// Token: 0x1700139A RID: 5018
		// (get) Token: 0x06007D69 RID: 32105 RVA: 0x0025C38D File Offset: 0x0025A58D
		protected override JobDef RopeJobDef
		{
			get
			{
				return JobDefOf.ReturnedCaravan_PenAnimals;
			}
		}

		// Token: 0x06007D6A RID: 32106 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool AnimalNeedsGathering(Pawn pawn, Pawn animal)
		{
			return false;
		}

		// Token: 0x06007D6B RID: 32107 RVA: 0x0025C394 File Offset: 0x0025A594
		protected override void DecorateJob(Job job)
		{
			base.DecorateJob(job);
			job.ropeToUnenclosedPens = true;
		}
	}
}
