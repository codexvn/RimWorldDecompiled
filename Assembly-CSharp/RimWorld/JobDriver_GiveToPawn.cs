using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001288 RID: 4744
	public class JobDriver_GiveToPawn : JobDriver
	{
		// Token: 0x1700121C RID: 4636
		// (get) Token: 0x06007411 RID: 29713 RVA: 0x002396DC File Offset: 0x002378DC
		private Thing Item
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700121D RID: 4637
		// (get) Token: 0x06007412 RID: 29714 RVA: 0x00239700 File Offset: 0x00237900
		private Pawn Receiver
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Pawn;
			}
		}

		// Token: 0x1700121E RID: 4638
		// (get) Token: 0x06007413 RID: 29715 RVA: 0x00239721 File Offset: 0x00237921
		public int CountBeingHauled
		{
			get
			{
				if (this.pawn.carryTracker.CarriedThing == null)
				{
					return 0;
				}
				return this.pawn.carryTracker.CarriedThing.stackCount;
			}
		}

		// Token: 0x06007414 RID: 29716 RVA: 0x0023974C File Offset: 0x0023794C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Item, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007415 RID: 29717 RVA: 0x0023976F File Offset: 0x0023796F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedNullOrForbidden(TargetIndex.B);
			this.FailOn(() => !GiveItemsToPawnUtility.IsWaitingForItems(this.Receiver));
			Toil setItemTarget = this.SetItemTarget();
			yield return setItemTarget;
			Toil reserve = Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null, false).FailOnDespawnedOrNull(TargetIndex.A);
			yield return reserve;
			yield return this.DetermineNumToHaul();
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			Toil checkForDuplicates = Toils_Haul.CheckForGetOpportunityDuplicate(reserve, TargetIndex.A, TargetIndex.None, true, (Thing x) => x.def == this.Item.def);
			Toil haulToContainer = Toils_Haul.CarryHauledThingToContainer();
			yield return Toils_Jump.JumpIf(haulToContainer, () => GiveItemsToPawnUtility.ItemCountLeftToCollect(this.Receiver) <= 0);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false, true, false);
			yield return Toils_Jump.JumpIf(haulToContainer, () => GiveItemsToPawnUtility.ItemCountLeftToCollect(this.Receiver) <= 0);
			yield return checkForDuplicates;
			yield return haulToContainer;
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.B);
			yield return Toils_Haul.DepositHauledThingInContainer(TargetIndex.B, TargetIndex.A, null);
			yield return Toils_Jump.JumpIf(setItemTarget, () => GiveItemsToPawnUtility.ItemCountLeftToCollect(this.Receiver) > 0);
			yield return Toils_General.Do(delegate
			{
				QuestUtility.SendQuestTargetSignals(this.Receiver.questTags, "ReceivedItems", this.pawn.Named("GIVER"), this.Receiver.Named("RECEIVER"));
			});
			yield break;
		}

		// Token: 0x06007416 RID: 29718 RVA: 0x0023977F File Offset: 0x0023797F
		private Toil DetermineNumToHaul()
		{
			Toil toil = ToilMaker.MakeToil("DetermineNumToHaul");
			toil.initAction = delegate()
			{
				int num = GiveItemsToPawnUtility.ItemCountLeftToCollect(this.Receiver);
				if (num <= 0)
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
					return;
				}
				this.job.count = num;
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x06007417 RID: 29719 RVA: 0x002397AB File Offset: 0x002379AB
		private Toil SetItemTarget()
		{
			Toil toil = ToilMaker.MakeToil("SetItemTarget");
			toil.initAction = delegate()
			{
				Thing thing = GiveItemsToPawnUtility.FindItemToGive(this.pawn, this.Item.def);
				if (thing == null)
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				this.job.SetTarget(TargetIndex.A, thing);
			};
			return toil;
		}

		// Token: 0x04004FDD RID: 20445
		private const TargetIndex ItemInd = TargetIndex.A;

		// Token: 0x04004FDE RID: 20446
		private const TargetIndex PawnInd = TargetIndex.B;
	}
}
