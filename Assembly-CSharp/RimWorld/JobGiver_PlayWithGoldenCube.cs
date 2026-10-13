using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014B7 RID: 5303
	public class JobGiver_PlayWithGoldenCube : ThinkNode_JobGiver
	{
		// Token: 0x06007F19 RID: 32537 RVA: 0x002646C4 File Offset: 0x002628C4
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.Downed || pawn.Drafted)
			{
				return null;
			}
			Hediff hediff;
			if (!pawn.health.hediffSet.TryGetHediff(HediffDefOf.CubeWithdrawal, out hediff))
			{
				return null;
			}
			if (hediff.Severity < 0.9f && pawn.Drafted)
			{
				return null;
			}
			if (pawn.mindState.duty != null)
			{
				return null;
			}
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return null;
			}
			Thing t;
			if (!JobGiver_PlayWithGoldenCube.TryGetNearestCube(pawn, out t))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.GoldenCubePlay, t);
			job.count = 1;
			return job;
		}

		// Token: 0x06007F1A RID: 32538 RVA: 0x00264760 File Offset: 0x00262960
		public static bool TryGetNearestCube(Pawn pawn, out Thing cube)
		{
			if (!pawn.Spawned)
			{
				cube = null;
				return false;
			}
			cube = GenClosest.ClosestThingReachable(pawn.PositionHeld, pawn.MapHeld, ThingRequest.ForDef(ThingDefOf.GoldenCube), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing t) => JobGiver_PlayWithGoldenCube.IsValid(t, pawn), null, 0, -1, false, RegionType.Set_Passable, false, false);
			return cube != null;
		}

		// Token: 0x06007F1B RID: 32539 RVA: 0x002647E3 File Offset: 0x002629E3
		private static bool IsValid(Thing thing, Pawn pawn)
		{
			return thing.def == ThingDefOf.GoldenCube && !thing.IsForbidden(pawn) && pawn.CanReserveAndReach(thing, PathEndMode.Touch, Danger.None, 1, -1, null, false);
		}

		// Token: 0x04005528 RID: 21800
		private const float SeverityToOverrideDrafted = 0.9f;
	}
}
