using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02001002 RID: 4098
	public abstract class JobGiver_ExitMap : ThinkNode_JobGiver
	{
		// Token: 0x0600649C RID: 25756 RVA: 0x001FDCEC File Offset: 0x001FBEEC
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_ExitMap jobGiver_ExitMap = (JobGiver_ExitMap)base.DeepCopy(resolve);
			jobGiver_ExitMap.defaultLocomotion = this.defaultLocomotion;
			jobGiver_ExitMap.jobMaxDuration = this.jobMaxDuration;
			jobGiver_ExitMap.canBash = this.canBash;
			jobGiver_ExitMap.forceCanDig = this.forceCanDig;
			jobGiver_ExitMap.forceCanDigIfAnyHostileActiveThreat = this.forceCanDigIfAnyHostileActiveThreat;
			jobGiver_ExitMap.forceCanDigIfCantReachMapEdge = this.forceCanDigIfCantReachMapEdge;
			jobGiver_ExitMap.failIfCantJoinOrCreateCaravan = this.failIfCantJoinOrCreateCaravan;
			jobGiver_ExitMap.canFlyOutOfMap = this.canFlyOutOfMap;
			return jobGiver_ExitMap;
		}

		// Token: 0x0600649D RID: 25757 RVA: 0x001FDD68 File Offset: 0x001FBF68
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.Downed && !pawn.Crawling)
			{
				return null;
			}
			if (!pawn.MapHeld.CanEverExit)
			{
				if (pawn.MapHeld.IsPocketMap)
				{
					foreach (Thing t in pawn.MapHeld.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.MapPortal)))
					{
						if (pawn.CanReach(t, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
						{
							return JobMaker.MakeJob(JobDefOf.EnterPortal, t);
						}
					}
				}
				return null;
			}
			if (this.canFlyOutOfMap && pawn.RaceProps.canLeaveMapFlying && !pawn.Position.Roofed(pawn.Map) && pawn.Faction != Faction.OfPlayer && pawn.flight.CanEverFly && !pawn.IsQuestLodger())
			{
				return JobMaker.MakeJob(JobDefOf.ExitMapFlying);
			}
			bool flag = this.forceCanDig || (pawn.mindState.duty != null && pawn.mindState.duty.canDig && !pawn.CanReachMapEdge()) || (this.forceCanDigIfCantReachMapEdge && !pawn.CanReachMapEdge()) || (this.forceCanDigIfAnyHostileActiveThreat && pawn.Faction != null && GenHostility.AnyHostileActiveThreatTo(pawn.Map, pawn.Faction, true, false));
			IntVec3 c;
			if (!this.TryFindGoodExitDest(pawn, flag, this.canBash, out c))
			{
				return null;
			}
			if (flag)
			{
				using (PawnPath pawnPath = pawn.Map.pathFinder.FindPathNow(pawn.Position, c, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassAllDestroyableThings, false, false, false, true), null, PathEndMode.OnCell, null))
				{
					IntVec3 cellBeforeBlocker;
					Thing thing = pawnPath.FirstBlockingBuilding(out cellBeforeBlocker, pawn);
					if (thing != null)
					{
						Job job = DigUtility.PassBlockerJob(pawn, thing, cellBeforeBlocker, true, true);
						if (job != null)
						{
							return job;
						}
					}
				}
			}
			Job job2 = JobMaker.MakeJob(JobDefOf.Goto, c);
			job2.exitMapOnArrival = true;
			job2.failIfCantJoinOrCreateCaravan = this.failIfCantJoinOrCreateCaravan;
			job2.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.defaultLocomotion, LocomotionUrgency.Jog);
			job2.expiryInterval = this.jobMaxDuration;
			job2.canBashDoors = this.canBash;
			return job2;
		}

		// Token: 0x0600649E RID: 25758
		protected abstract bool TryFindGoodExitDest(Pawn pawn, bool canDig, bool canBash, out IntVec3 dest);

		// Token: 0x040047F2 RID: 18418
		protected LocomotionUrgency defaultLocomotion;

		// Token: 0x040047F3 RID: 18419
		protected int jobMaxDuration = 999999;

		// Token: 0x040047F4 RID: 18420
		protected bool canBash;

		// Token: 0x040047F5 RID: 18421
		protected bool forceCanDig;

		// Token: 0x040047F6 RID: 18422
		protected bool forceCanDigIfAnyHostileActiveThreat;

		// Token: 0x040047F7 RID: 18423
		protected bool forceCanDigIfCantReachMapEdge;

		// Token: 0x040047F8 RID: 18424
		protected bool failIfCantJoinOrCreateCaravan;

		// Token: 0x040047F9 RID: 18425
		protected bool canFlyOutOfMap = true;
	}
}
