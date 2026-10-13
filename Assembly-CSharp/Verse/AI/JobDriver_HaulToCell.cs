using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000EBE RID: 3774
	public class JobDriver_HaulToCell : JobDriver
	{
		// Token: 0x17000F32 RID: 3890
		// (get) Token: 0x06005EEC RID: 24300 RVA: 0x001E2E04 File Offset: 0x001E1004
		public Thing ToHaul
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000F33 RID: 3891
		// (get) Token: 0x06005EED RID: 24301 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool DropCarriedThingIfNotTarget
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005EEE RID: 24302 RVA: 0x001E2E25 File Offset: 0x001E1025
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.forbiddenInitially, "forbiddenInitially", false, false);
		}

		// Token: 0x06005EEF RID: 24303 RVA: 0x001E2E40 File Offset: 0x001E1040
		public override string GetReport()
		{
			IntVec3 cell = this.job.targetB.Cell;
			Thing thing = null;
			if (this.pawn.CurJob == this.job && this.pawn.carryTracker.CarriedThing != null)
			{
				thing = this.pawn.carryTracker.CarriedThing;
			}
			else if (base.TargetThingA != null && base.TargetThingA.Spawned)
			{
				thing = base.TargetThingA;
			}
			if (thing == null)
			{
				return "ReportHaulingUnknown".Translate();
			}
			string text = null;
			SlotGroup slotGroup = cell.GetSlotGroup(base.Map);
			if (slotGroup != null)
			{
				text = slotGroup.parent.SlotYielderLabel();
			}
			if (text != null)
			{
				return "ReportHaulingTo".Translate(thing.Label, text.Named("DESTINATION"), thing.Named("THING"));
			}
			return "ReportHauling".Translate(thing.Label, thing);
		}

		// Token: 0x06005EF0 RID: 24304 RVA: 0x001E2F3C File Offset: 0x001E113C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005EF1 RID: 24305 RVA: 0x001E2F91 File Offset: 0x001E1191
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.forbiddenInitially = (base.TargetThingA != null && base.TargetThingA.IsForbidden(this.pawn));
		}

		// Token: 0x06005EF2 RID: 24306 RVA: 0x001E2FBB File Offset: 0x001E11BB
		protected virtual Toil BeforeDrop()
		{
			return Toils_General.Label();
		}

		// Token: 0x06005EF3 RID: 24307 RVA: 0x001E2FC2 File Offset: 0x001E11C2
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.B);
			this.FailOnForbidden(TargetIndex.B);
			if (!this.forbiddenInitially)
			{
				this.FailOnForbidden(TargetIndex.A);
			}
			yield return Toils_General.DoAtomic(delegate
			{
				this.startTick = Find.TickManager.TicksGame;
			});
			Toil reserveTargetA = Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null, false);
			yield return reserveTargetA;
			Toil postCarry = Toils_General.Label();
			Toil checkJumpPostCarry = Toils_Jump.JumpIf(postCarry, delegate
			{
				Thing carriedThing = this.pawn.carryTracker.CarriedThing;
				return carriedThing != null && (this.pawn.carryTracker.AvailableStackSpace(this.ToHaul.def) <= 0 || carriedThing == this.ToHaul);
			});
			yield return checkJumpPostCarry;
			yield return Toils_General.DoAtomic(delegate
			{
				if (this.DropCarriedThingIfNotTarget && this.pawn.IsCarrying())
				{
					if (DebugViewSettings.logCarriedBetweenJobs)
					{
						Log.Message(string.Format("Dropping {0} because it is not the designated Thing to haul.", this.pawn.carryTracker.CarriedThing));
					}
					Thing thing;
					this.pawn.carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Near, out thing, null);
				}
			});
			Toil toilGoto = null;
			toilGoto = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, true).FailOnSomeonePhysicallyInteracting(TargetIndex.A).FailOn(delegate()
			{
				Pawn actor = toilGoto.actor;
				Job curJob = actor.jobs.curJob;
				if (curJob.haulMode == HaulMode.ToCellStorage)
				{
					Thing thing = curJob.GetTarget(TargetIndex.A).Thing;
					if (!actor.jobs.curJob.GetTarget(TargetIndex.B).Cell.IsValidStorageFor(this.Map, thing))
					{
						return true;
					}
				}
				return false;
			});
			yield return toilGoto;
			yield return checkJumpPostCarry;
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, true, false, true, HaulAIUtility.IsInHaulableInventory(this.ToHaul));
			yield return postCarry;
			if (this.job.haulOpportunisticDuplicates)
			{
				yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserveTargetA, TargetIndex.A, TargetIndex.B, false, null);
			}
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B, PathEndMode.ClosestTouch);
			yield return carryToCell;
			yield return this.PossiblyDelay();
			yield return this.BeforeDrop();
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true, false);
			yield break;
		}

		// Token: 0x06005EF4 RID: 24308 RVA: 0x001E2FD2 File Offset: 0x001E11D2
		private Toil PossiblyDelay()
		{
			Toil toil = ToilMaker.MakeToil("PossiblyDelay");
			toil.atomicWithPrevious = true;
			toil.tickIntervalAction = delegate(int delta)
			{
				if (Find.TickManager.TicksGame >= this.startTick + 30)
				{
					base.ReadyForNextToil();
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			return toil;
		}

		// Token: 0x0400446B RID: 17515
		private bool forbiddenInitially;

		// Token: 0x0400446C RID: 17516
		private const TargetIndex HaulableInd = TargetIndex.A;

		// Token: 0x0400446D RID: 17517
		private const TargetIndex StoreCellInd = TargetIndex.B;

		// Token: 0x0400446E RID: 17518
		private const int MinimumHaulingJobTicks = 30;
	}
}
