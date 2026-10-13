using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F35 RID: 3893
	public static class Toils_Goto
	{
		// Token: 0x060060B4 RID: 24756 RVA: 0x001EA62D File Offset: 0x001E882D
		public static Toil Goto(TargetIndex ind, PathEndMode peMode)
		{
			return Toils_Goto.GotoThing(ind, peMode, false);
		}

		// Token: 0x060060B5 RID: 24757 RVA: 0x001EA638 File Offset: 0x001E8838
		public static Toil GotoThing(TargetIndex ind, PathEndMode peMode, bool canGotoSpawnedParent = false)
		{
			Toil toil = ToilMaker.MakeToil("GotoThing");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				LocalTargetInfo dest = actor.jobs.curJob.GetTarget(ind);
				Thing thing = dest.Thing;
				if (thing != null & canGotoSpawnedParent)
				{
					dest = thing.SpawnedParentOrMe;
				}
				actor.pather.StartPath(dest, peMode);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			if (canGotoSpawnedParent)
			{
				toil.FailOnSelfAndParentsDespawnedOrNull(ind);
			}
			else
			{
				toil.FailOnDespawnedOrNull(ind);
			}
			return toil;
		}

		// Token: 0x060060B6 RID: 24758 RVA: 0x001EA6C8 File Offset: 0x001E88C8
		public static Toil GotoThing(TargetIndex ind, IntVec3 exactCell)
		{
			Toil toil = ToilMaker.MakeToil("GotoThing");
			toil.initAction = delegate()
			{
				toil.actor.pather.StartPath(exactCell, PathEndMode.OnCell);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil.FailOnDespawnedOrNull(ind);
			return toil;
		}

		// Token: 0x060060B7 RID: 24759 RVA: 0x001EA728 File Offset: 0x001E8928
		public static Toil GotoCell(TargetIndex ind, PathEndMode peMode)
		{
			Toil toil = ToilMaker.MakeToil("GotoCell");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				LocalTargetInfo target = actor.jobs.curJob.GetTarget(ind);
				if (actor.Position == target.Cell)
				{
					actor.pather.StopDead();
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				actor.pather.StartPath(target, peMode);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x060060B8 RID: 24760 RVA: 0x001EA784 File Offset: 0x001E8984
		public static Toil GotoCell(IntVec3 cell, PathEndMode peMode)
		{
			Toil toil = ToilMaker.MakeToil("GotoCell");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				if (actor.Position == cell)
				{
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				actor.pather.StartPath(cell, peMode);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x060060B9 RID: 24761 RVA: 0x001EA7E0 File Offset: 0x001E89E0
		public static Toil MoveOffTargetBlueprint(TargetIndex targetInd)
		{
			Toil toil = ToilMaker.MakeToil("MoveOffTargetBlueprint");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Blueprint blueprint = actor.jobs.curJob.GetTarget(targetInd).Thing as Blueprint;
				if (blueprint.DestroyedOrNull() || !actor.Position.IsInside(blueprint))
				{
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				IntVec3 c;
				if (RCellFinder.TryFindGoodAdjacentSpotToTouch(actor, blueprint, out c))
				{
					actor.pather.StartPath(c, PathEndMode.OnCell);
					return;
				}
				actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x060060BA RID: 24762 RVA: 0x001EA834 File Offset: 0x001E8A34
		public static Toil GotoBuild(TargetIndex ind)
		{
			Toil toil = ToilMaker.MakeToil("GotoBuild");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				LocalTargetInfo target = actor.jobs.curJob.GetTarget(ind);
				Thing thing = target.Thing;
				IntVec3 c;
				if (RCellFinder.TryFindGoodAdjacentSpotToTouch(actor, thing, out c))
				{
					actor.pather.StartPath(c, PathEndMode.OnCell);
					return;
				}
				actor.pather.StartPath(target, PathEndMode.OnCell);
			};
			toil.tickIntervalAction = delegate(int _)
			{
				Pawn actor = toil.actor;
				Thing thing = actor.jobs.curJob.GetTarget(ind).Thing;
				if (actor.CanReachImmediate(thing, PathEndMode.Touch) && actor.Map.reservationManager.CanReserve(actor, actor.Position, 1, -1, null, false))
				{
					actor.Reserve(actor.Position, actor.CurJob, 1, -1, null, true, false);
					actor.pather.StopDead();
					actor.jobs.curDriver.ReadyForNextToil();
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil.FailOnDespawnedOrNull(ind);
			return toil;
		}
	}
}
