using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001393 RID: 5011
	public class JobDriver_Ingest : JobDriver, IEatingDriver
	{
		// Token: 0x1700135B RID: 4955
		// (get) Token: 0x06007ABF RID: 31423 RVA: 0x0024F7DC File Offset: 0x0024D9DC
		private Thing IngestibleSource
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700135C RID: 4956
		// (get) Token: 0x06007AC0 RID: 31424 RVA: 0x0024F7FD File Offset: 0x0024D9FD
		public bool EatingFromInventory
		{
			get
			{
				return this.eatingFromInventory;
			}
		}

		// Token: 0x1700135D RID: 4957
		// (get) Token: 0x06007AC1 RID: 31425 RVA: 0x0024F808 File Offset: 0x0024DA08
		public bool GainingNutritionNow
		{
			get
			{
				Thing ingestibleSource = this.IngestibleSource;
				return !ingestibleSource.DestroyedOrNull() && ingestibleSource.def.IsNutritionGivingIngestible && base.CurToil == this.chewing;
			}
		}

		// Token: 0x1700135E RID: 4958
		// (get) Token: 0x06007AC2 RID: 31426 RVA: 0x0024F844 File Offset: 0x0024DA44
		private float ChewDurationMultiplier
		{
			get
			{
				Thing ingestibleSource = this.IngestibleSource;
				if (ingestibleSource.def.ingestible != null && !ingestibleSource.def.ingestible.useEatingSpeedStat)
				{
					return 1f;
				}
				return 1f / this.pawn.GetStatValue(StatDefOf.EatingSpeed, true, -1);
			}
		}

		// Token: 0x06007AC3 RID: 31427 RVA: 0x0024F895 File Offset: 0x0024DA95
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.usingNutrientPasteDispenser, "usingNutrientPasteDispenser", false, false);
			Scribe_Values.Look<bool>(ref this.eatingFromInventory, "eatingFromInventory", false, false);
		}

		// Token: 0x06007AC4 RID: 31428 RVA: 0x0024F8C4 File Offset: 0x0024DAC4
		public override string GetReport()
		{
			if (this.usingNutrientPasteDispenser)
			{
				return JobUtility.GetResolvedJobReportRaw(this.job.def.reportString, ThingDefOf.MealNutrientPaste.label, ThingDefOf.MealNutrientPaste, "", "", "", "");
			}
			Thing thing = this.job.targetA.Thing;
			if (((thing != null) ? thing.def.ingestible : null) != null)
			{
				if (!thing.def.ingestible.ingestReportStringEat.NullOrEmpty() && (thing.def.ingestible.ingestReportString.NullOrEmpty() || this.pawn.RaceProps.intelligence < Intelligence.ToolUser))
				{
					return thing.def.ingestible.ingestReportStringEat.Formatted(this.job.targetA.Thing.LabelShort, this.job.targetA.Thing);
				}
				if (!thing.def.ingestible.ingestReportString.NullOrEmpty())
				{
					return thing.def.ingestible.ingestReportString.Formatted(this.job.targetA.Thing.LabelShort, this.job.targetA.Thing);
				}
			}
			return base.GetReport();
		}

		// Token: 0x06007AC5 RID: 31429 RVA: 0x0024FA2C File Offset: 0x0024DC2C
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usingNutrientPasteDispenser = (this.IngestibleSource is Building_NutrientPasteDispenser);
			this.eatingFromInventory = (this.pawn.inventory != null && this.pawn.inventory.Contains(this.IngestibleSource));
		}

		// Token: 0x06007AC6 RID: 31430 RVA: 0x0024FA80 File Offset: 0x0024DC80
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.pawn.Faction != null && !(this.IngestibleSource is Building_NutrientPasteDispenser))
			{
				Thing ingestibleSource = this.IngestibleSource;
				int maxAmountToPickup = FoodUtility.GetMaxAmountToPickup(ingestibleSource, this.pawn, this.job.count);
				if (!this.pawn.Reserve(ingestibleSource, this.job, 10, maxAmountToPickup, null, errorOnFailed, false))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007AC7 RID: 31431 RVA: 0x0024FAE8 File Offset: 0x0024DCE8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!this.usingNutrientPasteDispenser)
			{
				this.FailOn(() => !this.IngestibleSource.Destroyed && !this.IngestibleSource.IngestibleNow);
			}
			this.chewing = Toils_Ingest.ChewIngestible(this.pawn, this.ChewDurationMultiplier, TargetIndex.A, TargetIndex.B).FailOn((Toil x) => !this.IngestibleSource.Spawned && (this.pawn.carryTracker == null || this.pawn.carryTracker.CarriedThing != this.IngestibleSource)).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			foreach (Toil toil in this.PrepareToIngestToils(this.chewing))
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield return this.chewing;
			yield return Toils_Ingest.FinalizeIngest(this.pawn, TargetIndex.A);
			yield return Toils_Jump.JumpIf(this.chewing, () => this.job.GetTarget(TargetIndex.A).Thing is Corpse && this.pawn.needs.food.CurLevelPercentage < 0.9f);
			yield break;
			yield break;
		}

		// Token: 0x06007AC8 RID: 31432 RVA: 0x0024FAF8 File Offset: 0x0024DCF8
		private IEnumerable<Toil> PrepareToIngestToils(Toil chewToil)
		{
			if (this.usingNutrientPasteDispenser)
			{
				return this.PrepareToIngestToils_Dispenser();
			}
			if (this.pawn.RaceProps.ToolUser && (!this.pawn.IsMutant || !this.pawn.mutant.Def.disableEatingAtTable))
			{
				return this.PrepareToIngestToils_ToolUser(chewToil);
			}
			return this.PrepareToIngestToils_NonToolUser();
		}

		// Token: 0x06007AC9 RID: 31433 RVA: 0x0024FB58 File Offset: 0x0024DD58
		private IEnumerable<Toil> PrepareToIngestToils_Dispenser()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, this.pawn);
			yield return Toils_Ingest.CarryIngestibleToChewSpot(this.pawn, TargetIndex.A).FailOnDestroyedNullOrForbidden(TargetIndex.A);
			yield return Toils_Ingest.FindAdjacentEatSurface(TargetIndex.B, TargetIndex.A);
			yield break;
		}

		// Token: 0x06007ACA RID: 31434 RVA: 0x0024FB68 File Offset: 0x0024DD68
		private IEnumerable<Toil> PrepareToIngestToils_ToolUser(Toil chewToil)
		{
			if (this.eatingFromInventory)
			{
				yield return Toils_Misc.TakeItemFromInventoryToCarrier(this.pawn, TargetIndex.A);
			}
			else
			{
				yield return this.ReserveFood();
				Toil gotoToPickup = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false).FailOnDespawnedNullOrForbidden(TargetIndex.A);
				yield return Toils_Jump.JumpIf(gotoToPickup, () => this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false).FailOnDespawnedNullOrForbidden(TargetIndex.A);
				yield return Toils_Jump.Jump(chewToil);
				yield return gotoToPickup;
				yield return Toils_Ingest.PickupIngestible(TargetIndex.A, this.pawn);
				gotoToPickup = null;
			}
			if (this.job.takeExtraIngestibles > 0)
			{
				foreach (Toil toil in this.TakeExtraIngestibles())
				{
					yield return toil;
				}
				IEnumerator<Toil> enumerator = null;
			}
			if (!this.pawn.Drafted)
			{
				yield return Toils_Ingest.CarryIngestibleToChewSpot(this.pawn, TargetIndex.A).FailOnDestroyedOrNull(TargetIndex.A);
			}
			yield return Toils_Ingest.FindAdjacentEatSurface(TargetIndex.B, TargetIndex.A);
			yield break;
			yield break;
		}

		// Token: 0x06007ACB RID: 31435 RVA: 0x0024FB7F File Offset: 0x0024DD7F
		private IEnumerable<Toil> PrepareToIngestToils_NonToolUser()
		{
			yield return this.ReserveFood();
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			yield break;
		}

		// Token: 0x06007ACC RID: 31436 RVA: 0x0024FB8F File Offset: 0x0024DD8F
		private IEnumerable<Toil> TakeExtraIngestibles()
		{
			if (!this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				yield break;
			}
			Toil reserveExtraFoodToCollect = Toils_Ingest.ReserveFoodFromStackForIngesting(TargetIndex.C, null);
			Toil findExtraFoodToCollect = ToilMaker.MakeToil("TakeExtraIngestibles");
			Predicate<Thing> <>9__2;
			findExtraFoodToCollect.initAction = delegate()
			{
				if (this.pawn.inventory.innerContainer.TotalStackCountOfDef(this.IngestibleSource.def) >= this.job.takeExtraIngestibles)
				{
					return;
				}
				IntVec3 position = this.pawn.Position;
				Map map = this.pawn.Map;
				ThingRequest thingReq = ThingRequest.ForDef(this.IngestibleSource.def);
				PathEndMode peMode = PathEndMode.Touch;
				TraverseParms traverseParams = TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
				float maxDistance = 30f;
				Predicate<Thing> validator;
				if ((validator = <>9__2) == null)
				{
					validator = (<>9__2 = ((Thing x) => this.pawn.CanReserve(x, 10, 1, null, false) && !x.IsForbidden(this.pawn) && x.IsSociallyProper(this.pawn)));
				}
				Thing thing = GenClosest.ClosestThingReachable(position, map, thingReq, peMode, traverseParams, maxDistance, validator, null, 0, -1, false, RegionType.Set_Passable, false, false);
				if (thing == null)
				{
					return;
				}
				this.job.SetTarget(TargetIndex.C, thing);
				this.JumpToToil(reserveExtraFoodToCollect);
			};
			findExtraFoodToCollect.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return Toils_Jump.Jump(findExtraFoodToCollect);
			yield return reserveExtraFoodToCollect;
			yield return Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.Touch, false);
			yield return Toils_Haul.TakeToInventory(TargetIndex.C, () => this.job.takeExtraIngestibles - this.pawn.inventory.innerContainer.TotalStackCountOfDef(this.IngestibleSource.def));
			yield return findExtraFoodToCollect;
			yield break;
		}

		// Token: 0x06007ACD RID: 31437 RVA: 0x0024FB9F File Offset: 0x0024DD9F
		private Toil ReserveFood()
		{
			Toil toil = ToilMaker.MakeToil("ReserveFood");
			toil.initAction = delegate()
			{
				if (this.pawn.Faction == null)
				{
					return;
				}
				Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
				if (this.pawn.carryTracker.CarriedThing == thing)
				{
					return;
				}
				int maxAmountToPickup = FoodUtility.GetMaxAmountToPickup(thing, this.pawn, this.job.count);
				if (maxAmountToPickup == 0)
				{
					return;
				}
				if (!this.pawn.Reserve(thing, this.job, 10, maxAmountToPickup, null, true, false))
				{
					string[] array = new string[8];
					array[0] = "Pawn food reservation for ";
					int num = 1;
					Pawn pawn = this.pawn;
					array[num] = ((pawn != null) ? pawn.ToString() : null);
					array[2] = " on job ";
					array[3] = ((this != null) ? this.ToString() : null);
					array[4] = " failed, because it could not register food from ";
					int num2 = 5;
					Thing thing2 = thing;
					array[num2] = ((thing2 != null) ? thing2.ToString() : null);
					array[6] = " - amount: ";
					array[7] = maxAmountToPickup.ToString();
					Log.Error(string.Concat(array));
					this.pawn.jobs.EndCurrentJob(JobCondition.Errored, true, true);
				}
				this.job.count = maxAmountToPickup;
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x06007ACE RID: 31438 RVA: 0x0024FBCC File Offset: 0x0024DDCC
		public override bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool flip)
		{
			IntVec3 cell = this.job.GetTarget(TargetIndex.B).Cell;
			return JobDriver_Ingest.ModifyCarriedThingDrawPosWorker(ref drawPos, ref flip, cell, this.pawn);
		}

		// Token: 0x06007ACF RID: 31439 RVA: 0x0024FBFC File Offset: 0x0024DDFC
		public static bool ModifyCarriedThingDrawPosWorker(ref Vector3 drawPos, ref bool flip, IntVec3 placeCell, Pawn pawn)
		{
			if (pawn.pather.Moving)
			{
				return false;
			}
			Thing carriedThing = pawn.carryTracker.CarriedThing;
			if (carriedThing == null || !carriedThing.IngestibleNow)
			{
				return false;
			}
			if (placeCell.IsValid && placeCell.AdjacentToCardinal(pawn.Position) && placeCell.HasEatSurface(pawn.Map) && carriedThing.def.ingestible.ingestHoldUsesTable)
			{
				drawPos = new Vector3((float)placeCell.x + 0.5f, drawPos.y, (float)placeCell.z + 0.5f);
				return true;
			}
			HoldOffsetSet ingestHoldOffsetStanding = carriedThing.def.ingestible.ingestHoldOffsetStanding;
			HoldOffset holdOffset = (ingestHoldOffsetStanding != null) ? ingestHoldOffsetStanding.Pick(pawn.Rotation) : null;
			if (holdOffset != null)
			{
				drawPos += holdOffset.offset;
				flip = holdOffset.flip;
				return true;
			}
			return false;
		}

		// Token: 0x0400530C RID: 21260
		private bool usingNutrientPasteDispenser;

		// Token: 0x0400530D RID: 21261
		private bool eatingFromInventory;

		// Token: 0x0400530E RID: 21262
		private Toil chewing;

		// Token: 0x0400530F RID: 21263
		public const float EatCorpseBodyPartsUntilFoodLevelPct = 0.9f;

		// Token: 0x04005310 RID: 21264
		public const TargetIndex IngestibleSourceInd = TargetIndex.A;

		// Token: 0x04005311 RID: 21265
		private const TargetIndex TableCellInd = TargetIndex.B;

		// Token: 0x04005312 RID: 21266
		private const TargetIndex ExtraIngestiblesToCollectInd = TargetIndex.C;
	}
}
