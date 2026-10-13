using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200122B RID: 4651
	public class JobDriver_CarryToBiosculpterPod : JobDriver
	{
		// Token: 0x170011AC RID: 4524
		// (get) Token: 0x060071A4 RID: 29092 RVA: 0x00231D68 File Offset: 0x0022FF68
		private Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x170011AD RID: 4525
		// (get) Token: 0x060071A5 RID: 29093 RVA: 0x00231D90 File Offset: 0x0022FF90
		private CompBiosculpterPod Pod
		{
			get
			{
				return this.job.GetTarget(TargetIndex.C).Thing.TryGetComp<CompBiosculpterPod>();
			}
		}

		// Token: 0x060071A6 RID: 29094 RVA: 0x00231DB8 File Offset: 0x0022FFB8
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed, false) || !this.pawn.Reserve(this.Pod.parent, this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			List<LocalTargetInfo> targetQueue = this.job.GetTargetQueue(TargetIndex.B);
			for (int i = 0; i < targetQueue.Count; i++)
			{
				if (!this.pawn.Reserve(targetQueue[i], this.job, 1, -1, null, errorOnFailed, false))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060071A7 RID: 29095 RVA: 0x00231E52 File Offset: 0x00230052
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Biosculpting"))
			{
				yield break;
			}
			base.AddFinishAction(delegate(JobCondition jobCondition)
			{
				if (this.Pod != null)
				{
					if (this.Pod.queuedEnterJob == this.job)
					{
						this.Pod.ClearQueuedInformation();
					}
					if (this.Pod.Occupant != this.Takee)
					{
						foreach (Thing thing in this.pickedUpIngredients)
						{
							if (this.pawn.inventory.Contains(thing))
							{
								Thing thing2;
								this.pawn.inventory.innerContainer.TryDrop(thing, ThingPlaceMode.Near, out thing2, null, null);
							}
							else if (this.Takee.inventory.Contains(thing))
							{
								Thing thing2;
								this.Takee.inventory.innerContainer.TryDrop(thing, ThingPlaceMode.Near, out thing2, null, null);
							}
						}
					}
				}
			});
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedOrNull(TargetIndex.C);
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOn(() => this.job.biosculpterCycleKey == null || !this.Pod.CanAcceptOnceCycleChosen(this.Takee));
			Toil goToTakee = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell, false).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.C).FailOn(() => this.Takee.IsFreeColonist && !this.Takee.Downed).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			Toil startCarryingTakee = Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false, true, false);
			Toil goToThing = Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.InteractionCell, false);
			Toil toil = Toils_Jump.JumpIf(goToThing, () => this.pawn.IsCarryingPawn(this.Takee) && this.job.GetTargetQueue(TargetIndex.B).NullOrEmpty<LocalTargetInfo>());
			Toil jumpIfGoToTakee = Toils_Jump.JumpIf(goToTakee, () => this.job.GetTargetQueue(TargetIndex.B).NullOrEmpty<LocalTargetInfo>());
			yield return toil;
			yield return jumpIfGoToTakee;
			yield return this.DropCarryToGrabIngredients();
			foreach (Toil toil2 in JobDriver_EnterBiosculpterPod.CollectIngredientsToilsHelper(TargetIndex.B, this.pawn, this.pickedUpIngredients))
			{
				yield return toil2;
			}
			IEnumerator<Toil> enumerator = null;
			yield return goToTakee;
			yield return this.TransferIngredientsAndPrepareCarryDownedPawn();
			yield return startCarryingTakee;
			yield return goToThing.FailOn(() => !this.Pod.PawnCarryingExtraCycleIngredients(this.Takee, this.job.biosculpterCycleKey, false));
			yield return JobDriver_EnterBiosculpterPod.PrepareToEnterToil(TargetIndex.C);
			Toil toil3 = ToilMaker.MakeToil("MakeNewToils");
			toil3.initAction = delegate()
			{
				this.Pod.TryAcceptPawn(this.Takee, this.job.biosculpterCycleKey);
			};
			toil3.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil3;
			yield break;
			yield break;
		}

		// Token: 0x060071A8 RID: 29096 RVA: 0x00231E62 File Offset: 0x00230062
		private Toil TransferIngredientsAndPrepareCarryDownedPawn()
		{
			Toil toil = ToilMaker.MakeToil("TransferIngredientsAndPrepareCarryDownedPawn");
			toil.initAction = delegate()
			{
				List<ThingDefCountClass> extraRequiredIngredients = this.Pod.GetCycle(this.job.biosculpterCycleKey).Props.extraRequiredIngredients;
				if (extraRequiredIngredients != null && !this.Pod.devFillPodLatch)
				{
					ThingOwner<Thing> innerContainer = this.pawn.inventory.innerContainer;
					foreach (ThingDefCountClass thingDefCountClass in extraRequiredIngredients)
					{
						if (this.Takee.inventory.Count(thingDefCountClass.thingDef) < thingDefCountClass.count)
						{
							if (this.pawn.inventory.Count(thingDefCountClass.thingDef) < thingDefCountClass.count)
							{
								base.EndJobWith(JobCondition.Incompletable);
								return;
							}
							this.thingsToTransfer.Clear();
							int num = 0;
							foreach (Thing thing in innerContainer)
							{
								if (num >= thingDefCountClass.count)
								{
									break;
								}
								if (thing.def == thingDefCountClass.thingDef)
								{
									int num2 = Mathf.Min(thingDefCountClass.count - num, thing.stackCount);
									this.thingsToTransfer.Add(new ThingCount(thing, Mathf.Min(thingDefCountClass.count - num, thing.stackCount), false));
									num += num2;
								}
							}
							foreach (ThingCount thingCount in this.thingsToTransfer)
							{
								int num3 = this.Takee.inventory.innerContainer.TryAddOrTransfer(thingCount.Thing, thingCount.Count, true);
								if (num3 != thingCount.Count)
								{
									Log.Warning(string.Format("Only able to transfer x{0} of the expected x{1} of {2} while CarryToBiosculpter", num3, thingCount.Count, thingCount.Thing.Label));
									base.EndJobWith(JobCondition.Incompletable);
									return;
								}
							}
						}
					}
				}
				this.job.count = 1;
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x060071A9 RID: 29097 RVA: 0x00231E87 File Offset: 0x00230087
		private Toil DropCarryToGrabIngredients()
		{
			Toil toil = ToilMaker.MakeToil("DropCarryToGrabIngredients");
			toil.initAction = delegate()
			{
				if (this.pawn.carryTracker.CarriedThing != null)
				{
					Thing thing;
					this.pawn.carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Near, out thing, null);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x060071AA RID: 29098 RVA: 0x00231EAC File Offset: 0x002300AC
		public override string GetReport()
		{
			if (!this.Pod.PawnCarryingExtraCycleIngredients(this.Takee, this.job.biosculpterCycleKey, false) && !this.Pod.PawnCarryingExtraCycleIngredients(this.pawn, this.job.biosculpterCycleKey, false))
			{
				return "BiosculpterJobReportCollectIngredients".Translate();
			}
			return base.GetReport();
		}

		// Token: 0x060071AB RID: 29099 RVA: 0x00231F0D File Offset: 0x0023010D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.pickedUpIngredients, "pickedUpIngredients", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x04004EB2 RID: 20146
		private const TargetIndex TakeeInd = TargetIndex.A;

		// Token: 0x04004EB3 RID: 20147
		private const TargetIndex IngredientInd = TargetIndex.B;

		// Token: 0x04004EB4 RID: 20148
		private const TargetIndex PodInd = TargetIndex.C;

		// Token: 0x04004EB5 RID: 20149
		private List<Thing> pickedUpIngredients = new List<Thing>();

		// Token: 0x04004EB6 RID: 20150
		private List<ThingCount> thingsToTransfer = new List<ThingCount>();
	}
}
