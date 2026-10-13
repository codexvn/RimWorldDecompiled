using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000EB1 RID: 3761
	public class JobDriver_DoBill : JobDriver
	{
		// Token: 0x06005EA7 RID: 24231 RVA: 0x001E0E2C File Offset: 0x001DF02C
		public override string GetReport()
		{
			if (ModsConfig.BiotechActive)
			{
				Bill_Mech bill_Mech = this.job.bill as Bill_Mech;
				if (bill_Mech != null)
				{
					return MechanitorUtility.GetMechGestationJobString(this, this.pawn, bill_Mech);
				}
			}
			if (this.job.RecipeDef != null)
			{
				return this.ReportStringProcessed(this.job.RecipeDef.jobString);
			}
			return base.GetReport();
		}

		// Token: 0x17000F2C RID: 3884
		// (get) Token: 0x06005EA8 RID: 24232 RVA: 0x001E0E8C File Offset: 0x001DF08C
		public IBillGiver BillGiver
		{
			get
			{
				IBillGiver billGiver = this.job.GetTarget(TargetIndex.A).Thing as IBillGiver;
				if (billGiver == null)
				{
					throw new InvalidOperationException("DoBill on non-Billgiver.");
				}
				return billGiver;
			}
		}

		// Token: 0x17000F2D RID: 3885
		// (get) Token: 0x06005EA9 RID: 24233 RVA: 0x001E0EC0 File Offset: 0x001DF0C0
		public bool AnyIngredientsQueued
		{
			get
			{
				return !this.job.GetTargetQueue(TargetIndex.B).NullOrEmpty<LocalTargetInfo>();
			}
		}

		// Token: 0x06005EAA RID: 24234 RVA: 0x001E0ED6 File Offset: 0x001DF0D6
		public override bool IsContinuation(Job j)
		{
			return j.bill == this.job.bill;
		}

		// Token: 0x06005EAB RID: 24235 RVA: 0x001E0EEC File Offset: 0x001DF0EC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Values.Look<int>(ref this.billStartTick, "billStartTick", 0, false);
			Scribe_Values.Look<int>(ref this.ticksSpentDoingRecipeWork, "ticksSpentDoingRecipeWork", 0, false);
		}

		// Token: 0x06005EAC RID: 24236 RVA: 0x001E0F3C File Offset: 0x001DF13C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
			if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			if (thing != null && thing.def.hasInteractionCell && !this.pawn.ReserveSittableOrSpot(thing.InteractionCell, this.job, errorOnFailed))
			{
				return false;
			}
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06005EAD RID: 24237 RVA: 0x001E0FD1 File Offset: 0x001DF1D1
		protected override IEnumerable<Toil> MakeNewToils()
		{
			base.AddEndCondition(delegate
			{
				Thing thing = base.GetActor().jobs.curJob.GetTarget(TargetIndex.A).Thing;
				if (thing is Building && !thing.Spawned)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			this.FailOnBurningImmobile(TargetIndex.A);
			this.FailOn(delegate()
			{
				IBillGiver billGiver = this.job.GetTarget(TargetIndex.A).Thing as IBillGiver;
				if (billGiver != null)
				{
					if (this.job.bill.DeletedOrDereferenced)
					{
						return true;
					}
					if (!billGiver.CurrentlyUsableForBills())
					{
						return true;
					}
				}
				return false;
			});
			Toil gotoBillGiver = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				if (this.job.targetQueueB != null && this.job.targetQueueB.Count == 1)
				{
					UnfinishedThing unfinishedThing = this.job.targetQueueB[0].Thing as UnfinishedThing;
					if (unfinishedThing != null && !unfinishedThing.Destroyed)
					{
						unfinishedThing.BoundBill = (Bill_ProductionWithUft)this.job.bill;
					}
				}
				this.job.bill.Notify_DoBillStarted(this.pawn);
			};
			yield return toil;
			yield return Toils_Jump.JumpIf(gotoBillGiver, () => this.job.GetTargetQueue(TargetIndex.B).NullOrEmpty<LocalTargetInfo>());
			foreach (Toil toil2 in JobDriver_DoBill.CollectIngredientsToils(TargetIndex.B, TargetIndex.A, TargetIndex.C, false, true, this.BillGiver is Building_WorkTableAutonomous))
			{
				yield return toil2;
			}
			IEnumerator<Toil> enumerator = null;
			yield return gotoBillGiver;
			yield return Toils_Recipe.MakeUnfinishedThingIfNeeded();
			yield return Toils_Recipe.DoRecipeWork().FailOnDespawnedNullOrForbiddenPlacedThings(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			yield return Toils_Recipe.CheckIfRecipeCanFinishNow();
			yield return Toils_Recipe.FinishRecipeAndStartStoringProduct(TargetIndex.None);
			yield break;
			yield break;
		}

		// Token: 0x06005EAE RID: 24238 RVA: 0x001E0FE1 File Offset: 0x001DF1E1
		public static IEnumerable<Toil> CollectIngredientsToils(TargetIndex ingredientInd, TargetIndex billGiverInd, TargetIndex ingredientPlaceCellInd, bool subtractNumTakenFromJobCount = false, bool failIfStackCountLessThanJobCount = true, bool placeInBillGiver = false)
		{
			Toil extract = Toils_JobTransforms.ExtractNextTargetFromQueue(ingredientInd, true);
			yield return extract;
			Toil jumpIfHaveTargetInQueue = Toils_Jump.JumpIfHaveTargetInQueue(ingredientInd, extract);
			yield return JobDriver_DoBill.JumpIfTargetInsideBillGiver(jumpIfHaveTargetInQueue, ingredientInd, billGiverInd);
			Toil getToHaulTarget = Toils_Goto.GotoThing(ingredientInd, PathEndMode.ClosestTouch, true).FailOnForbidden(ingredientInd).FailOnSomeonePhysicallyInteracting(ingredientInd);
			yield return getToHaulTarget;
			yield return Toils_Haul.StartCarryThing(ingredientInd, true, subtractNumTakenFromJobCount, failIfStackCountLessThanJobCount, false, true);
			yield return JobDriver_DoBill.JumpToCollectNextIntoHandsForBill(getToHaulTarget, TargetIndex.B);
			yield return Toils_Goto.GotoThing(billGiverInd, PathEndMode.InteractionCell, false).FailOnDestroyedOrNull(ingredientInd);
			if (!placeInBillGiver)
			{
				Toil findPlaceTarget = Toils_JobTransforms.SetTargetToIngredientPlaceCell(billGiverInd, ingredientInd, ingredientPlaceCellInd);
				yield return findPlaceTarget;
				yield return Toils_Haul.PlaceHauledThingInCell(ingredientPlaceCellInd, findPlaceTarget, false, false);
				Toil physReserveToil = ToilMaker.MakeToil("CollectIngredientsToils");
				physReserveToil.initAction = delegate()
				{
					physReserveToil.actor.Map.physicalInteractionReservationManager.Reserve(physReserveToil.actor, physReserveToil.actor.CurJob, physReserveToil.actor.CurJob.GetTarget(ingredientInd));
				};
				yield return physReserveToil;
				findPlaceTarget = null;
			}
			else
			{
				yield return Toils_Haul.DepositHauledThingInContainer(billGiverInd, ingredientInd, null);
			}
			yield return jumpIfHaveTargetInQueue;
			yield break;
		}

		// Token: 0x06005EAF RID: 24239 RVA: 0x001E1018 File Offset: 0x001DF218
		private static Toil JumpIfTargetInsideBillGiver(Toil jumpToil, TargetIndex ingredient, TargetIndex billGiver)
		{
			Toil toil = ToilMaker.MakeToil("JumpIfTargetInsideBillGiver");
			toil.initAction = delegate()
			{
				Thing thing = toil.actor.CurJob.GetTarget(billGiver).Thing;
				if (thing == null || !thing.Spawned)
				{
					return;
				}
				Thing thing2 = toil.actor.jobs.curJob.GetTarget(ingredient).Thing;
				if (thing2 == null)
				{
					return;
				}
				ThingOwner thingOwner = thing.TryGetInnerInteractableThingOwner();
				if (thingOwner != null && thingOwner.Contains(thing2))
				{
					HaulAIUtility.UpdateJobWithPlacedThings(toil.actor.jobs.curJob, thing2, thing2.stackCount);
					toil.actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x06005EB0 RID: 24240 RVA: 0x001E1070 File Offset: 0x001DF270
		public static Toil JumpToCollectNextIntoHandsForBill(Toil gotoGetTargetToil, TargetIndex ind)
		{
			Toil toil = ToilMaker.MakeToil("JumpToCollectNextIntoHandsForBill");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				if (actor.carryTracker.CarriedThing == null)
				{
					string str = "JumpToAlsoCollectTargetInQueue run on ";
					Pawn pawn = actor;
					Log.Error(str + ((pawn != null) ? pawn.ToString() : null) + " who is not carrying something.");
					return;
				}
				if (actor.carryTracker.Full)
				{
					return;
				}
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
				if (targetQueue.NullOrEmpty<LocalTargetInfo>())
				{
					return;
				}
				for (int i = 0; i < targetQueue.Count; i++)
				{
					if (GenAI.CanUseItemForWork(actor, targetQueue[i].Thing) && targetQueue[i].Thing.CanStackWith(actor.carryTracker.CarriedThing) && (float)(actor.Position - targetQueue[i].Thing.Position).LengthHorizontalSquared <= 64f)
					{
						int num = (actor.carryTracker.CarriedThing == null) ? 0 : actor.carryTracker.CarriedThing.stackCount;
						int num2 = curJob.countQueue[i];
						num2 = Mathf.Min(num2, targetQueue[i].Thing.def.stackLimit - num);
						num2 = Mathf.Min(num2, actor.carryTracker.AvailableStackSpace(targetQueue[i].Thing.def));
						if (num2 > 0)
						{
							curJob.count = num2;
							curJob.SetTarget(ind, targetQueue[i].Thing);
							List<int> countQueue = curJob.countQueue;
							int index = i;
							countQueue[index] -= num2;
							if (curJob.countQueue[i] <= 0)
							{
								curJob.countQueue.RemoveAt(i);
								targetQueue.RemoveAt(i);
							}
							actor.jobs.curDriver.JumpToToil(gotoGetTargetToil);
							return;
						}
					}
				}
			};
			return toil;
		}

		// Token: 0x04004436 RID: 17462
		public float workLeft;

		// Token: 0x04004437 RID: 17463
		public int billStartTick;

		// Token: 0x04004438 RID: 17464
		public int ticksSpentDoingRecipeWork;

		// Token: 0x04004439 RID: 17465
		public const PathEndMode GotoIngredientPathEndMode = PathEndMode.ClosestTouch;

		// Token: 0x0400443A RID: 17466
		public const TargetIndex BillGiverInd = TargetIndex.A;

		// Token: 0x0400443B RID: 17467
		public const TargetIndex IngredientInd = TargetIndex.B;

		// Token: 0x0400443C RID: 17468
		public const TargetIndex IngredientPlaceCellInd = TargetIndex.C;
	}
}
