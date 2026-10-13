using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F46 RID: 3910
	public static class Toils_Rope
	{
		// Token: 0x060060E0 RID: 24800 RVA: 0x001EB2CC File Offset: 0x001E94CC
		public static Toil RopePawn(TargetIndex ropeeInd)
		{
			Toil toil = ToilMaker.MakeToil("RopePawn");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = actor.jobs.curJob.GetTarget(ropeeInd).Thing as Pawn;
				if (pawn != null)
				{
					toil.actor.roping.RopePawn(pawn);
					Pawn_CallTracker caller = pawn.caller;
					if (caller != null)
					{
						caller.DoCall(false);
					}
					PawnUtility.ForceWait(pawn, 30, actor, false, false);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 30;
			toil.FailOnDespawnedOrNull(ropeeInd);
			toil.PlaySustainerOrSound(() => SoundDefOf.Roping, 1f);
			return toil;
		}

		// Token: 0x060060E1 RID: 24801 RVA: 0x001EB370 File Offset: 0x001E9570
		public static Toil UnropeFromSpot(TargetIndex ropeeInd)
		{
			Toil toil = ToilMaker.MakeToil("UnropeFromSpot");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = actor.jobs.curJob.GetTarget(ropeeInd).Thing as Pawn;
				if (pawn != null && pawn.roping.IsRopedToSpot)
				{
					pawn.roping.UnropeFromSpot();
					Pawn_CallTracker caller = pawn.caller;
					if (caller != null)
					{
						caller.DoCall(false);
					}
					PawnUtility.ForceWait(pawn, 30, actor, false, false);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 30;
			toil.FailOnDespawnedOrNull(ropeeInd);
			return toil;
		}

		// Token: 0x060060E2 RID: 24802 RVA: 0x001EB3E4 File Offset: 0x001E95E4
		public static Toil GotoRopeAttachmentInteractionCell(TargetIndex ropeeIndex)
		{
			Toil toil = ToilMaker.MakeToil("GotoRopeAttachmentInteractionCell");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn ropee = actor.CurJob.GetTarget(ropeeIndex).Thing as Pawn;
				IntVec3 intVec = AnimalPenUtility.RopeAttachmentInteractionCell(actor, ropee);
				if (!intVec.IsValid)
				{
					actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
				}
				if (actor.Position == intVec)
				{
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				actor.pather.StartPath(intVec, PathEndMode.OnCell);
			};
			toil.tickIntervalAction = delegate(int delta)
			{
				Pawn actor = toil.actor;
				Pawn ropee = actor.CurJob.GetTarget(ropeeIndex).Thing as Pawn;
				if (actor.pather.Moving && !AnimalPenUtility.IsGoodRopeAttachmentInteractionCell(actor, ropee, actor.pather.Destination.Cell))
				{
					IntVec3 c = AnimalPenUtility.RopeAttachmentInteractionCell(actor, ropee);
					if (c.IsValid)
					{
						actor.pather.StartPath(c, PathEndMode.OnCell);
						return;
					}
					actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil.FailOnDespawnedOrNull(ropeeIndex);
			return toil;
		}

		// Token: 0x040045DC RID: 17884
		private const int RopeWorkDuration = 30;
	}
}
