using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000F3C RID: 3900
	public static class Toils_Reserve
	{
		// Token: 0x060060C8 RID: 24776 RVA: 0x001EAB9C File Offset: 0x001E8D9C
		public static Toil ReserveDestination(TargetIndex ind)
		{
			Toil toil = ToilMaker.MakeToil("ReserveDestination");
			toil.initAction = delegate()
			{
				toil.actor.Map.pawnDestinationReservationManager.Reserve(toil.actor, toil.actor.CurJob, toil.actor.jobs.curJob.GetTarget(ind).Cell);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x060060C9 RID: 24777 RVA: 0x001EABFC File Offset: 0x001E8DFC
		public static Toil Reserve(TargetIndex ind, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			Toil toil = ToilMaker.MakeToil("Reserve");
			toil.initAction = delegate()
			{
				if (!toil.actor.Reserve(toil.actor.jobs.curJob.GetTarget(ind), toil.actor.CurJob, maxPawns, stackCount, layer, ignoreOtherReservations, false))
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x060060CA RID: 24778 RVA: 0x001EAC78 File Offset: 0x001E8E78
		public static Toil ReserveDestinationOrThing(TargetIndex ind)
		{
			Toil toil = ToilMaker.MakeToil("ReserveDestinationOrThing");
			toil.initAction = delegate()
			{
				LocalTargetInfo target = toil.actor.jobs.curJob.GetTarget(ind);
				if (target.HasThing)
				{
					if (!toil.actor.Reserve(target, toil.actor.CurJob, 1, -1, null, true, false))
					{
						toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
						return;
					}
				}
				else
				{
					toil.actor.Map.pawnDestinationReservationManager.Reserve(toil.actor, toil.actor.CurJob, target.Cell);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x060060CB RID: 24779 RVA: 0x001EACD8 File Offset: 0x001E8ED8
		public static Toil ReserveQueue(TargetIndex ind, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null)
		{
			Toil toil = ToilMaker.MakeToil("ReserveQueue");
			toil.initAction = delegate()
			{
				List<LocalTargetInfo> targetQueue = toil.actor.jobs.curJob.GetTargetQueue(ind);
				if (targetQueue != null)
				{
					for (int i = 0; i < targetQueue.Count; i++)
					{
						if (!toil.actor.Reserve(targetQueue[i], toil.actor.CurJob, maxPawns, stackCount, layer, true, false))
						{
							toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
						}
					}
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x060060CC RID: 24780 RVA: 0x001EAD4C File Offset: 0x001E8F4C
		public static Toil Release(TargetIndex ind)
		{
			Toil toil = ToilMaker.MakeToil("Release");
			toil.initAction = delegate()
			{
				LocalTargetInfo target = toil.actor.jobs.curJob.GetTarget(ind);
				toil.actor.Map.reservationManager.Release(target, toil.actor, toil.actor.CurJob);
				toil.actor.jobs.ReleaseReservations(target);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x060060CD RID: 24781 RVA: 0x001EADAC File Offset: 0x001E8FAC
		public static Toil ReleaseDestination()
		{
			Toil toil = ToilMaker.MakeToil("ReleaseDestination");
			toil.initAction = delegate()
			{
				toil.actor.Map.pawnDestinationReservationManager.ReleaseClaimedBy(toil.actor, toil.actor.CurJob);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x060060CE RID: 24782 RVA: 0x001EAE04 File Offset: 0x001E9004
		public static Toil ReleaseDestinationOrThing(TargetIndex ind)
		{
			Toil toil = ToilMaker.MakeToil("ReleaseDestinationOrThing");
			toil.initAction = delegate()
			{
				LocalTargetInfo target = toil.actor.jobs.curJob.GetTarget(ind);
				if (target.HasThing)
				{
					toil.actor.Map.reservationManager.Release(target, toil.actor, toil.actor.CurJob);
					toil.actor.jobs.ReleaseReservations(target);
					return;
				}
				toil.actor.Map.pawnDestinationReservationManager.ReleaseClaimedBy(toil.actor, toil.actor.CurJob);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}
	}
}
