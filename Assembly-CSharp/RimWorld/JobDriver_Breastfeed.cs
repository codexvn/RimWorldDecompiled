using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001150 RID: 4432
	public class JobDriver_Breastfeed : JobDriver_FeedBaby
	{
		// Token: 0x170010B3 RID: 4275
		// (get) Token: 0x06006C5E RID: 27742 RVA: 0x002211AF File Offset: 0x0021F3AF
		// (set) Token: 0x06006C5F RID: 27743 RVA: 0x002211B7 File Offset: 0x0021F3B7
		protected override Toil FeedingToil { get; set; }

		// Token: 0x06006C60 RID: 27744 RVA: 0x002211C0 File Offset: 0x0021F3C0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return base.TryMakePreToilReservations(errorOnFailed) && this.pawn.Reserve(this.pawn, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006C61 RID: 27745 RVA: 0x000026EA File Offset: 0x000008EA
		public override void SetInitialPosture()
		{
		}

		// Token: 0x06006C62 RID: 27746 RVA: 0x002211EE File Offset: 0x0021F3EE
		public override bool CanBeginNowWhileLyingDown()
		{
			return this.pawn.Downed;
		}

		// Token: 0x06006C63 RID: 27747 RVA: 0x002211FB File Offset: 0x0021F3FB
		protected override IEnumerable<Toil> FeedBaby()
		{
			base.AddFailCondition(delegate
			{
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
				return !ChildcareUtility.CanBreastfeed(this.pawn, out breastfeedFailReason);
			});
			yield return this.Breastfeed();
			yield return this.TuckMomInIfDowned();
			yield break;
		}

		// Token: 0x06006C64 RID: 27748 RVA: 0x0022120C File Offset: 0x0021F40C
		private Toil Breastfeed()
		{
			this.FeedingToil = ToilMaker.MakeToil("Breastfeed");
			this.FeedingToil.initAction = delegate()
			{
				base.Baby.jobs.StartJob(ChildcareUtility.MakeBabySuckleJob(this.pawn), JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
				this.initialFoodPercentage = base.Baby.needs.food.CurLevelPercentage;
			};
			this.FeedingToil.tickIntervalAction = delegate(int delta)
			{
				bool flag = ChildcareUtility.SuckleFromLactatingPawn(base.Baby, this.pawn, delta);
				this.pawn.GainComfortFromCellIfPossible(delta, false);
				if (!this.pawn.Downed && this.pawn.Rotation == Rot4.North)
				{
					this.pawn.Rotation = Rot4.East;
				}
				if (!flag)
				{
					base.ReadyForNextToil();
				}
			};
			this.FeedingToil.AddFinishAction(delegate
			{
				if (base.Baby.needs.food.CurLevelPercentage - this.initialFoodPercentage > 0.6f)
				{
					base.Baby.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BreastfedMe, this.pawn, null);
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BreastfedBaby, base.Baby, null);
				}
				if (base.Baby.CurJobDef == JobDefOf.BabySuckle)
				{
					base.Baby.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
				}
				if (this.pawn.Downed)
				{
					Thing thing;
					this.pawn.carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Near, out thing, null);
				}
			});
			this.FeedingToil.AddFailCondition(delegate
			{
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
				return !ChildcareUtility.CanBreastfeed(this.pawn, out breastfeedFailReason);
			});
			this.FeedingToil.handlingFacing = true;
			this.FeedingToil.WithProgressBar(TargetIndex.A, () => base.Baby.needs.food.CurLevelPercentage, false, -0.5f, false);
			this.FeedingToil.defaultCompleteMode = ToilCompleteMode.Never;
			this.FeedingToil.WithEffect(EffecterDefOf.Breastfeeding, TargetIndex.A, null);
			return this.FeedingToil;
		}

		// Token: 0x06006C65 RID: 27749 RVA: 0x002212DE File Offset: 0x0021F4DE
		private Toil TuckMomInIfDowned()
		{
			Toil toil = ToilMaker.MakeToil("TuckMomInIfDowned");
			toil.initAction = delegate()
			{
				if (HealthAIUtility.ShouldSeekMedicalRest(this.pawn))
				{
					Building_Bed building_Bed = this.pawn.CurrentBed();
					if (building_Bed != null && building_Bed == RestUtility.FindPatientBedFor(this.pawn))
					{
						this.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.LayDown, building_Bed), JobCondition.Succeeded, null, false, true, null, new JobTag?(JobTag.RestingForMedicalReasons), false, false, null, false, true, false);
					}
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}
	}
}
