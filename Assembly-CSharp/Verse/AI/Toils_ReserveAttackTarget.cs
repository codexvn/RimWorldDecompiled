using System;

namespace Verse.AI
{
	// Token: 0x02000F44 RID: 3908
	public static class Toils_ReserveAttackTarget
	{
		// Token: 0x060060DD RID: 24797 RVA: 0x001EB20C File Offset: 0x001E940C
		public static Toil TryReserve(TargetIndex ind)
		{
			Toil toil = ToilMaker.MakeToil("TryReserve");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				IAttackTarget attackTarget = actor.CurJob.GetTarget(ind).Thing as IAttackTarget;
				if (attackTarget != null)
				{
					actor.Map.attackTargetReservationManager.Reserve(actor, toil.actor.CurJob, attackTarget);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}
	}
}
