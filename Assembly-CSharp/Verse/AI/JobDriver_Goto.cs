using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using Verse.AI.Group;

namespace Verse.AI
{
	// Token: 0x02000E82 RID: 3714
	public class JobDriver_Goto : JobDriver
	{
		// Token: 0x06005D9F RID: 23967 RVA: 0x001DDCB7 File Offset: 0x001DBEB7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.job.targetA.Cell);
			return true;
		}

		// Token: 0x06005DA0 RID: 23968 RVA: 0x001DDCEB File Offset: 0x001DBEEB
		protected override IEnumerable<Toil> MakeNewToils()
		{
			LocalTargetInfo lookAtTarget = this.job.GetTarget(TargetIndex.B);
			Toil toil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			toil.AddPreTickAction(delegate
			{
				if (this.job.exitMapOnArrival && this.pawn.Map.exitMapGrid.IsExitCell(this.pawn.Position))
				{
					this.TryExitMap();
				}
			});
			toil.FailOn(() => this.job.failIfCantJoinOrCreateCaravan && !CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this.pawn));
			toil.FailOn(delegate()
			{
				Pawn pawn = this.job.GetTarget(TargetIndex.A).Thing as Pawn;
				return pawn != null && pawn.ParentHolder is Corpse;
			});
			toil.FailOn(delegate()
			{
				Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
				return thing != null && thing.Destroyed;
			});
			if (lookAtTarget.IsValid)
			{
				Toil toil2 = toil;
				toil2.tickIntervalAction = (Action<int>)Delegate.Combine(toil2.tickIntervalAction, new Action<int>(delegate(int delta)
				{
					this.pawn.rotationTracker.FaceCell(lookAtTarget.Cell);
				}));
				toil.handlingFacing = true;
			}
			toil.AddFinishAction(delegate
			{
				if (this.job.controlGroupTag == null)
				{
					return;
				}
				if (this.job.controlGroupTag != null)
				{
					Pawn overseer = this.pawn.GetOverseer();
					if (overseer != null)
					{
						overseer.mechanitor.GetControlGroup(this.pawn).SetTag(this.pawn, this.job.controlGroupTag);
					}
				}
			});
			yield return toil;
			Toil toil3 = ToilMaker.MakeToil("MakeNewToils");
			toil3.initAction = delegate()
			{
				if (this.pawn.mindState != null && this.pawn.mindState.forcedGotoPosition == this.TargetA.Cell)
				{
					this.pawn.mindState.forcedGotoPosition = IntVec3.Invalid;
				}
				if (!this.job.ritualTag.NullOrEmpty())
				{
					Lord lord = this.pawn.GetLord();
					LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
					if (lordJob_Ritual != null)
					{
						lordJob_Ritual.AddTagForPawn(this.pawn, this.job.ritualTag);
					}
				}
				if (this.job.exitMapOnArrival && (this.pawn.Position.OnEdge(this.pawn.Map) || this.pawn.Map.exitMapGrid.IsExitCell(this.pawn.Position)))
				{
					this.TryExitMap();
				}
			};
			toil3.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil3;
			yield break;
		}

		// Token: 0x06005DA1 RID: 23969 RVA: 0x001DDCFC File Offset: 0x001DBEFC
		private void TryExitMap()
		{
			if (this.job.failIfCantJoinOrCreateCaravan && !CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this.pawn))
			{
				return;
			}
			if (ModsConfig.BiotechActive)
			{
				MechanitorUtility.Notify_PawnGotoLeftMap(this.pawn, this.pawn.Map);
			}
			if (ModsConfig.AnomalyActive && !MetalhorrorUtility.TryPawnExitMap(this.pawn))
			{
				return;
			}
			this.pawn.ExitMap(true, CellRect.WholeMap(base.Map).GetClosestEdge(this.pawn.Position));
		}
	}
}
