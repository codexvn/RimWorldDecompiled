using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001538 RID: 5432
	public class BabyPlayGiver_PlayStatic : BabyPlayGiver
	{
		// Token: 0x06008108 RID: 33032 RVA: 0x0026D964 File Offset: 0x0026BB64
		public override bool CanDo(Pawn pawn, Pawn other)
		{
			return pawn.IsCarryingPawn(other) || pawn.CanReserveAndReach(other, PathEndMode.Touch, Danger.Some, 1, -1, null, false);
		}

		// Token: 0x06008109 RID: 33033 RVA: 0x0026D945 File Offset: 0x0026BB45
		public override Job TryGiveJob(Pawn pawn, Pawn other)
		{
			Job job = JobMaker.MakeJob(this.def.jobDef, other);
			job.count = 1;
			return job;
		}
	}
}
