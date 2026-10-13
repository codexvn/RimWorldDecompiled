using System;

namespace Verse.AI
{
	// Token: 0x02000F2C RID: 3884
	public static class Toils_Jump
	{
		// Token: 0x0600609C RID: 24732 RVA: 0x001EA134 File Offset: 0x001E8334
		public static Toil Jump(Toil jumpTarget)
		{
			Toil toil = ToilMaker.MakeToil("Jump");
			toil.initAction = delegate()
			{
				toil.actor.jobs.curDriver.JumpToToil(jumpTarget);
			};
			return toil;
		}

		// Token: 0x0600609D RID: 24733 RVA: 0x001EA17C File Offset: 0x001E837C
		public static Toil JumpIf(Toil jumpTarget, Func<bool> condition)
		{
			Toil toil = ToilMaker.MakeToil("JumpIf");
			toil.initAction = delegate()
			{
				if (condition())
				{
					toil.actor.jobs.curDriver.JumpToToil(jumpTarget);
				}
			};
			return toil;
		}

		// Token: 0x0600609E RID: 24734 RVA: 0x001EA1CC File Offset: 0x001E83CC
		public static Toil JumpIfTargetDespawnedOrNull(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = ToilMaker.MakeToil("JumpIfTargetDespawnedOrNull");
			toil.initAction = delegate()
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				if (thing == null || !thing.Spawned)
				{
					toil.actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x0600609F RID: 24735 RVA: 0x001EA21C File Offset: 0x001E841C
		public static Toil JumpIfTargetInvalid(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = ToilMaker.MakeToil("JumpIfTargetInvalid");
			toil.initAction = delegate()
			{
				if (!toil.actor.jobs.curJob.GetTarget(ind).IsValid)
				{
					toil.actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x060060A0 RID: 24736 RVA: 0x001EA26C File Offset: 0x001E846C
		public static Toil JumpIfTargetNotHittable(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = ToilMaker.MakeToil("JumpIfTargetNotHittable");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				LocalTargetInfo target = curJob.GetTarget(ind);
				if (curJob.verbToUse == null || !curJob.verbToUse.IsStillUsableBy(actor) || !curJob.verbToUse.CanHitTarget(target))
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x060060A1 RID: 24737 RVA: 0x001EA2BC File Offset: 0x001E84BC
		public static Toil JumpIfTargetDowned(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = ToilMaker.MakeToil("JumpIfTargetDowned");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = actor.jobs.curJob.GetTarget(ind).Thing as Pawn;
				if (pawn != null && pawn.Downed)
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x060060A2 RID: 24738 RVA: 0x001EA30C File Offset: 0x001E850C
		public static Toil JumpIfHaveTargetInQueue(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = ToilMaker.MakeToil("JumpIfHaveTargetInQueue");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				if (!actor.jobs.curJob.GetTargetQueue(ind).NullOrEmpty<LocalTargetInfo>())
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x060060A3 RID: 24739 RVA: 0x001EA35C File Offset: 0x001E855C
		public static Toil JumpIfCannotTouch(TargetIndex ind, PathEndMode peMode, Toil jumpToil)
		{
			Toil toil = ToilMaker.MakeToil("JumpIfCannotTouch");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				LocalTargetInfo target = actor.jobs.curJob.GetTarget(ind);
				if (!actor.CanReachImmediate(target, peMode))
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}
	}
}
