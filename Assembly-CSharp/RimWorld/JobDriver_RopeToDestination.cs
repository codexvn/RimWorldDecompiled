using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200112B RID: 4395
	public abstract class JobDriver_RopeToDestination : JobDriver
	{
		// Token: 0x06006B30 RID: 27440
		protected abstract bool HasRopeeArrived(Pawn ropee, bool roperWaitingAtDest);

		// Token: 0x06006B31 RID: 27441
		protected abstract void ProcessArrivedRopee(Pawn ropee);

		// Token: 0x06006B32 RID: 27442
		protected abstract bool ShouldOpportunisticallyRopeAnimal(Pawn animal);

		// Token: 0x06006B33 RID: 27443 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual Thing FindDistantAnimalToRope()
		{
			return null;
		}

		// Token: 0x06006B34 RID: 27444 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool UpdateDestination()
		{
			return false;
		}

		// Token: 0x06006B35 RID: 27445 RVA: 0x0021C650 File Offset: 0x0021A850
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.roping.Ropees.NullOrEmpty<Pawn>())
			{
				foreach (Pawn t in this.pawn.roping.Ropees)
				{
					this.pawn.Reserve(t, this.job, 1, -1, null, errorOnFailed, false);
				}
			}
			this.UpdateDestination();
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.job.GetTarget(TargetIndex.B).Cell);
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006B36 RID: 27446 RVA: 0x0021C738 File Offset: 0x0021A938
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_General.Do(delegate
			{
				this.UpdateDestination();
			});
			base.AddFinishAction(delegate(JobCondition jobCondition)
			{
				Pawn pawn = this.pawn;
				if (pawn == null)
				{
					return;
				}
				Pawn_RopeTracker roping = pawn.roping;
				if (roping == null)
				{
					return;
				}
				roping.DropRopes();
			});
			Toil findAnotherAnimal = Toils_General.Label();
			Toil topOfLoop = Toils_General.Label();
			yield return topOfLoop;
			yield return Toils_Jump.JumpIf(findAnotherAnimal, delegate
			{
				Pawn pawn = this.job.GetTarget(TargetIndex.A).Thing as Pawn;
				return ((pawn != null) ? pawn.roping.RopedByPawn : null) == this.pawn;
			});
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null, false);
			Toil toil = Toils_Rope.GotoRopeAttachmentInteractionCell(TargetIndex.A);
			yield return toil;
			yield return Toils_Rope.RopePawn(TargetIndex.A);
			yield return findAnotherAnimal;
			yield return Toils_Jump.JumpIf(topOfLoop, new Func<bool>(this.FindAnotherAnimalToRope));
			findAnotherAnimal = null;
			topOfLoop = null;
			topOfLoop = Toils_General.Label();
			yield return topOfLoop;
			Toil toil2 = Toils_Goto.Goto(TargetIndex.B, PathEndMode.OnCell);
			this.MatchLocomotionUrgency(toil2);
			toil2.AddPreTickIntervalAction(delegate(int delta)
			{
				this.ProcessRopeesThatHaveArrived(false);
			});
			toil2.FailOn(() => !this.pawn.roping.IsRopingOthers);
			yield return toil2;
			yield return Toils_Jump.JumpIf(topOfLoop, new Func<bool>(this.UpdateDestination));
			topOfLoop = null;
			topOfLoop = Toils_General.Wait(60, TargetIndex.A);
			topOfLoop.AddPreTickIntervalAction(delegate(int delta)
			{
				this.ProcessRopeesThatHaveArrived(true);
			});
			yield return topOfLoop;
			yield return Toils_Jump.JumpIf(topOfLoop, () => this.pawn.roping.IsRopingOthers);
			topOfLoop = null;
			yield break;
		}

		// Token: 0x06006B37 RID: 27447 RVA: 0x0021C748 File Offset: 0x0021A948
		private void ProcessRopeesThatHaveArrived(bool roperWaitingAtDest)
		{
			JobDriver_RopeToDestination.tmpRopees.Clear();
			JobDriver_RopeToDestination.tmpRopees.AddRange(this.pawn.roping.Ropees);
			foreach (Pawn pawn in JobDriver_RopeToDestination.tmpRopees)
			{
				if (this.HasRopeeArrived(pawn, roperWaitingAtDest))
				{
					this.pawn.roping.DropRope(pawn);
					if (pawn.jobs != null && pawn.CurJob != null && pawn.jobs.curDriver is JobDriver_FollowRoper)
					{
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
					this.ProcessArrivedRopee(pawn);
				}
			}
		}

		// Token: 0x06006B38 RID: 27448 RVA: 0x0021C80C File Offset: 0x0021AA0C
		private void MatchLocomotionUrgency(Toil toil)
		{
			toil.AddPreInitAction(delegate
			{
				this.locomotionUrgencySameAs = this.SlowestRopee();
			});
			toil.AddFinishAction(delegate
			{
				this.locomotionUrgencySameAs = null;
			});
		}

		// Token: 0x06006B39 RID: 27449 RVA: 0x0021C834 File Offset: 0x0021AA34
		private Pawn SlowestRopee()
		{
			Pawn result;
			if (!this.pawn.roping.Ropees.TryMaxBy((Pawn p) => p.TicksPerMoveCardinal, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06006B3A RID: 27450 RVA: 0x0021C87C File Offset: 0x0021AA7C
		private bool FindAnotherAnimalToRope()
		{
			Pawn_MindState mindState = this.pawn.mindState;
			int? num;
			if (mindState == null)
			{
				num = null;
			}
			else
			{
				PawnDuty duty = mindState.duty;
				num = ((duty != null) ? duty.ropeeLimit : null);
			}
			int num2 = num ?? 10;
			if (this.pawn.roping.Ropees.Count >= num2)
			{
				return false;
			}
			Thing thing = GenClosest.ClosestThingReachable(this.pawn.Position, this.pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.Touch, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 10f, new Predicate<Thing>(this.<FindAnotherAnimalToRope>g__AnimalValidator|15_0), null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (thing == null)
			{
				thing = this.FindDistantAnimalToRope();
			}
			if (thing != null)
			{
				this.job.SetTarget(TargetIndex.A, thing);
				return true;
			}
			return false;
		}

		// Token: 0x06006B46 RID: 27462 RVA: 0x0021CA20 File Offset: 0x0021AC20
		[CompilerGenerated]
		private bool <FindAnotherAnimalToRope>g__AnimalValidator|15_0(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			return pawn != null && this.ShouldOpportunisticallyRopeAnimal(pawn);
		}

		// Token: 0x04004B8D RID: 19341
		private const TargetIndex AnimalInd = TargetIndex.A;

		// Token: 0x04004B8E RID: 19342
		public const TargetIndex DestCellInd = TargetIndex.B;

		// Token: 0x04004B8F RID: 19343
		private const int MaxAnimalsToRope = 10;

		// Token: 0x04004B90 RID: 19344
		private const int RopeMoreAnimalsScanRadius = 10;

		// Token: 0x04004B91 RID: 19345
		private static readonly List<Pawn> tmpRopees = new List<Pawn>();
	}
}
