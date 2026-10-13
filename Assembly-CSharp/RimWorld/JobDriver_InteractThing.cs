using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200129F RID: 4767
	public class JobDriver_InteractThing : JobDriver
	{
		// Token: 0x1700123F RID: 4671
		// (get) Token: 0x060074C4 RID: 29892 RVA: 0x0023BCD0 File Offset: 0x00239ED0
		private Thing InteractableThing
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17001240 RID: 4672
		// (get) Token: 0x060074C5 RID: 29893 RVA: 0x0023BCF4 File Offset: 0x00239EF4
		private Thing OptionalThing
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x17001241 RID: 4673
		// (get) Token: 0x060074C6 RID: 29894 RVA: 0x0023BD15 File Offset: 0x00239F15
		private bool HasOptionalThing
		{
			get
			{
				return this.OptionalThing != null;
			}
		}

		// Token: 0x17001242 RID: 4674
		// (get) Token: 0x060074C7 RID: 29895 RVA: 0x0023BD20 File Offset: 0x00239F20
		private CompInteractable Interactable
		{
			get
			{
				if (this.job.interactableIndex == -1)
				{
					return this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompInteractable>();
				}
				ThingWithComps thingWithComps = this.job.GetTarget(TargetIndex.A).Thing as ThingWithComps;
				if (thingWithComps == null)
				{
					return null;
				}
				return thingWithComps.GetComps<CompInteractable>().ToList<CompInteractable>()[this.job.interactableIndex];
			}
		}

		// Token: 0x060074C8 RID: 29896 RVA: 0x0023BD90 File Offset: 0x00239F90
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (base.TargetThingB != null)
			{
				this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			}
			return (base.TargetThingB == null || this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null, errorOnFailed, false)) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060074C9 RID: 29897 RVA: 0x0023BE15 File Offset: 0x0023A015
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (this.job.GetTarget(TargetIndex.B).HasThing)
			{
				Toil opt = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch, true).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B).FailOn(() => !this.Interactable.CanInteract(this.pawn, true));
				yield return opt;
				yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false, true, true);
				yield return Toils_Haul.JumpIfAlsoCollectingNextTargetInQueue(opt, TargetIndex.B);
				opt = null;
			}
			if (this.job.GetTarget(TargetIndex.A).Thing.SpawnedParentOrMe != null)
			{
				this.job.SetTarget(TargetIndex.C, this.job.GetTarget(TargetIndex.A).Thing.SpawnedParentOrMe);
			}
			Toil toil = Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.Touch, false).FailOnDespawnedNullOrForbidden(TargetIndex.C).FailOnSomeonePhysicallyInteracting(TargetIndex.C).FailOn(() => !this.Interactable.CanInteract(this.pawn, true));
			yield return toil;
			if (this.Interactable.TicksToActivate != 0)
			{
				int num = this.Interactable.TicksToActivate;
				if (this.Interactable.Props.activateStat != null && this.pawn.GetStatValue(this.Interactable.Props.activateStat, true, -1) > 0f)
				{
					num = Mathf.RoundToInt((float)num / this.pawn.GetStatValue(this.Interactable.Props.activateStat, true, -1));
				}
				int remainingTicks = Mathf.RoundToInt((float)num * (1f - this.Interactable.progress));
				yield return this.WaitForActivate(remainingTicks, num);
			}
			yield return Toils_General.Do(delegate
			{
				if (this.HasOptionalThing)
				{
					if (!this.pawn.IsCarryingThing(this.OptionalThing))
					{
						Log.ErrorOnce("Interact driver required item (" + this.OptionalThing.Label + ") but was not carrying it.", 77979685);
						return;
					}
					this.pawn.carryTracker.DestroyCarriedThing();
				}
				this.Interactable.Interact(this.pawn, false);
			});
			yield break;
		}

		// Token: 0x060074CA RID: 29898 RVA: 0x0023BE28 File Offset: 0x0023A028
		public override string GetReport()
		{
			if (base.CurToilIndex != 1 || this.ticksLeftThisToil <= 0)
			{
				if (string.IsNullOrEmpty(this.Interactable.Props.activatingStringPending))
				{
					return base.GetReport();
				}
				return this.Interactable.Props.activatingStringPending.Formatted(this.Interactable.parent.LabelShort);
			}
			else
			{
				int ticksToActivate = this.Interactable.TicksToActivate;
				int num = (this.ticksLeftThisToil < 0) ? ticksToActivate : this.ticksLeftThisToil;
				float value = (float)(ticksToActivate - (ticksToActivate - num)) / 60f;
				if (string.IsNullOrEmpty(this.Interactable.Props.activatingString))
				{
					return base.GetReport();
				}
				return this.Interactable.Props.activatingString.Formatted(this.Interactable.parent.LabelShort, value);
			}
		}

		// Token: 0x060074CB RID: 29899 RVA: 0x0023BF14 File Offset: 0x0023A114
		private Toil WaitForActivate(int remainingTicks, int totalTicks)
		{
			Toil toil = ToilMaker.MakeToil("WaitForActivate").FailOn(() => !this.Interactable.CanInteract(this.pawn, true));
			if (this.Interactable.Props.maintainProgress)
			{
				toil.WithProgressBar(TargetIndex.A, () => this.Interactable.progress, false, -0.5f, false);
			}
			else
			{
				toil.WithProgressBarToilDelay(TargetIndex.A, remainingTicks, false, -0.5f);
			}
			Toil toil5 = toil;
			toil5.initAction = (Action)Delegate.Combine(toil5.initAction, new Action(delegate()
			{
				this.Interactable.Notify_InteractionStarted();
				toil.actor.pather.StopDead();
			}));
			Toil toil2 = toil;
			toil2.tickIntervalAction = (Action<int>)Delegate.Combine(toil2.tickIntervalAction, new Action<int>(delegate(int delta)
			{
				this.pawn.rotationTracker.FaceTarget(this.TargetA);
			}));
			if (this.Interactable.Props.maintainProgress)
			{
				Toil toil3 = toil;
				toil3.tickIntervalAction = (Action<int>)Delegate.Combine(toil3.tickIntervalAction, new Action<int>(delegate(int delta)
				{
					this.Interactable.progress = 1f - (float)this.ticksLeftThisToil / (float)totalTicks * (float)delta;
				}));
			}
			if (this.Interactable.Props.forceNormalSpeedOnInteracting)
			{
				Toil toil4 = toil;
				toil4.tickAction = (Action)Delegate.Combine(toil4.tickAction, new Action(delegate()
				{
					Find.TickManager.slower.SignalForceNormalSpeed();
				}));
			}
			toil.AddFinishAction(delegate
			{
				this.Interactable.Notify_InteractionEnded();
			});
			toil.handlingFacing = true;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultDuration = remainingTicks;
			return toil;
		}

		// Token: 0x04005028 RID: 20520
		private const TargetIndex InteractableIndex = TargetIndex.A;

		// Token: 0x04005029 RID: 20521
		private const TargetIndex OptionalItemIndex = TargetIndex.B;

		// Token: 0x0400502A RID: 20522
		private const TargetIndex SpawnedOrParentIndex = TargetIndex.C;
	}
}
