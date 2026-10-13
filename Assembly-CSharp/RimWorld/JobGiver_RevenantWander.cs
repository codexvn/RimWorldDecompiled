using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014C5 RID: 5317
	public class JobGiver_RevenantWander : ThinkNode_JobGiver
	{
		// Token: 0x06007F40 RID: 32576 RVA: 0x0026524C File Offset: 0x0026344C
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			CellFinder.TryFindRandomReachableNearbyCell(pawn.Position, pawn.Map, JobGiver_RevenantWander.WanderDist, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false), (IntVec3 x) => x.Standable(pawn.Map), null, out c, 999999);
			Job job = JobMaker.MakeJob(JobDefOf.RevenantWander, c);
			job.locomotionUrgency = LocomotionUrgency.Walk;
			return job;
		}

		// Token: 0x04005533 RID: 21811
		public static float WanderDist = 30f;
	}
}
