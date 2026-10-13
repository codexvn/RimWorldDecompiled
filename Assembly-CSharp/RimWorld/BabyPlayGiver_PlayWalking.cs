using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200153B RID: 5435
	public class BabyPlayGiver_PlayWalking : BabyPlayGiver
	{
		// Token: 0x06008112 RID: 33042 RVA: 0x0026D964 File Offset: 0x0026BB64
		public override bool CanDo(Pawn pawn, Pawn other)
		{
			return pawn.IsCarryingPawn(other) || pawn.CanReserveAndReach(other, PathEndMode.Touch, Danger.Some, 1, -1, null, false);
		}

		// Token: 0x06008113 RID: 33043 RVA: 0x0026DB90 File Offset: 0x0026BD90
		public override Job TryGiveJob(Pawn pawn, Pawn other)
		{
			IntVec3 c = JobDriver_PlayWalking.TryFindWanderCell(pawn, other.PositionHeld);
			if (!c.IsValid)
			{
				return null;
			}
			if (!pawn.CanReserveAndReach(c, PathEndMode.Touch, Danger.Some, 1, -1, null, false))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(this.def.jobDef, other, c);
			job.count = 1;
			return job;
		}
	}
}
