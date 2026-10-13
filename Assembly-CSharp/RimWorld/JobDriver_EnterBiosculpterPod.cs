using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001259 RID: 4697
	public class JobDriver_EnterBiosculpterPod : JobDriver
	{
		// Token: 0x170011EB RID: 4587
		// (get) Token: 0x060072EA RID: 29418 RVA: 0x00236133 File Offset: 0x00234333
		private CompBiosculpterPod BiosculpterPod
		{
			get
			{
				return this.job.targetA.Thing.TryGetComp<CompBiosculpterPod>();
			}
		}

		// Token: 0x060072EB RID: 29419 RVA: 0x0023614C File Offset: 0x0023434C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false))
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

		// Token: 0x060072EC RID: 29420 RVA: 0x002361BE File Offset: 0x002343BE
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Biosculpting"))
			{
				yield break;
			}
			base.AddFinishAction(delegate(JobCondition jobCondition)
			{
				if (this.BiosculpterPod != null)
				{
					if (this.BiosculpterPod.queuedEnterJob == this.job)
					{
						this.BiosculpterPod.ClearQueuedInformation();
					}
					if (this.BiosculpterPod.Occupant != base.GetActor())
					{
						foreach (Thing thing in this.pickedUpIngredients)
						{
							if (this.pawn.inventory.Contains(thing))
							{
								Thing thing2;
								this.pawn.inventory.innerContainer.TryDrop(thing, ThingPlaceMode.Near, out thing2, null, null);
							}
						}
					}
				}
			});
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn(() => this.job.biosculpterCycleKey == null || !this.BiosculpterPod.CanAcceptOnceCycleChosen(base.GetActor()));
			Toil goToPod = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false);
			yield return Toils_Jump.JumpIf(goToPod, () => this.job.GetTargetQueue(TargetIndex.B).NullOrEmpty<LocalTargetInfo>());
			foreach (Toil toil in JobDriver_EnterBiosculpterPod.CollectIngredientsToilsHelper(TargetIndex.B, this.pawn, this.pickedUpIngredients))
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield return goToPod.FailOn(() => !this.BiosculpterPod.PawnCarryingExtraCycleIngredients(this.pawn, this.job.biosculpterCycleKey, false));
			yield return JobDriver_EnterBiosculpterPod.PrepareToEnterToil(TargetIndex.A);
			Toil enter = ToilMaker.MakeToil("MakeNewToils");
			enter.initAction = delegate()
			{
				this.BiosculpterPod.TryAcceptPawn(enter.actor, this.job.biosculpterCycleKey);
			};
			enter.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return enter;
			yield break;
			yield break;
		}

		// Token: 0x060072ED RID: 29421 RVA: 0x002361CE File Offset: 0x002343CE
		public override string GetReport()
		{
			if (!this.BiosculpterPod.PawnCarryingExtraCycleIngredients(this.pawn, this.job.biosculpterCycleKey, false))
			{
				return "BiosculpterJobReportCollectIngredients".Translate();
			}
			return base.GetReport();
		}

		// Token: 0x060072EE RID: 29422 RVA: 0x00236205 File Offset: 0x00234405
		public static IEnumerable<Toil> CollectIngredientsToilsHelper(TargetIndex ingredientIndex, Pawn carrier, List<Thing> pickedUpIngredients)
		{
			Toil extract = Toils_JobTransforms.ExtractNextTargetFromQueue(ingredientIndex, true);
			yield return extract;
			Toil toil = Toils_Goto.GotoThing(ingredientIndex, PathEndMode.ClosestTouch, false).FailOnDespawnedNullOrForbidden(ingredientIndex).FailOnSomeonePhysicallyInteracting(ingredientIndex);
			yield return toil;
			yield return Toils_Haul.StartCarryThing(ingredientIndex, true, false, true, true, false);
			yield return JobDriver_EnterBiosculpterPod.HaulBiosculpterIngredientInInventory(carrier, pickedUpIngredients);
			yield return Toils_Jump.JumpIfHaveTargetInQueue(ingredientIndex, extract);
			yield break;
		}

		// Token: 0x060072EF RID: 29423 RVA: 0x00236224 File Offset: 0x00234424
		private static Toil HaulBiosculpterIngredientInInventory(Pawn carrier, List<Thing> pickedUpIngredients)
		{
			Toil toil = ToilMaker.MakeToil("HaulBiosculpterIngredientInInventory");
			toil.initAction = delegate()
			{
				Thing carriedThing = carrier.carryTracker.CarriedThing;
				Thing thing;
				carrier.carryTracker.innerContainer.TryTransferToContainer(carriedThing, carrier.inventory.innerContainer, carriedThing.stackCount, out thing, true);
				if (thing != null)
				{
					pickedUpIngredients.Add(thing);
				}
			};
			return toil;
		}

		// Token: 0x060072F0 RID: 29424 RVA: 0x00236264 File Offset: 0x00234464
		public static Toil PrepareToEnterToil(TargetIndex podIndex)
		{
			Toil prepare = Toils_General.Wait(70, TargetIndex.None);
			prepare.FailOnCannotTouch(podIndex, PathEndMode.InteractionCell);
			prepare.WithProgressBarToilDelay(podIndex, false, -0.5f);
			prepare.PlaySustainerOrSound(delegate()
			{
				Thing thing = prepare.actor.CurJob.GetTarget(podIndex).Thing;
				if (thing == null)
				{
					return null;
				}
				CompBiosculpterPod compBiosculpterPod = thing.TryGetComp<CompBiosculpterPod>();
				if (compBiosculpterPod == null)
				{
					return null;
				}
				return compBiosculpterPod.Props.enterSound;
			}, 1f);
			return prepare;
		}

		// Token: 0x060072F1 RID: 29425 RVA: 0x002362DA File Offset: 0x002344DA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.pickedUpIngredients, "pickedUpIngredients", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x04004F51 RID: 20305
		public const int EnterPodDelay = 70;

		// Token: 0x04004F52 RID: 20306
		private List<Thing> pickedUpIngredients = new List<Thing>();

		// Token: 0x04004F53 RID: 20307
		private const TargetIndex BiosculpterIndex = TargetIndex.A;

		// Token: 0x04004F54 RID: 20308
		private const TargetIndex IngredientInd = TargetIndex.B;
	}
}
