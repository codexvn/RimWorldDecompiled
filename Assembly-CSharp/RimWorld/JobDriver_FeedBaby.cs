using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001155 RID: 4437
	public abstract class JobDriver_FeedBaby : JobDriver
	{
		// Token: 0x170010B9 RID: 4281
		// (get) Token: 0x06006C8D RID: 27789 RVA: 0x000024E7 File Offset: 0x000006E7
		protected Pawn Baby
		{
			get
			{
				return (Pawn)base.TargetThingA;
			}
		}

		// Token: 0x06006C8E RID: 27790 RVA: 0x00221CD7 File Offset: 0x0021FED7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Baby, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006C8F RID: 27791
		protected abstract IEnumerable<Toil> FeedBaby();

		// Token: 0x170010BA RID: 4282
		// (get) Token: 0x06006C90 RID: 27792
		// (set) Token: 0x06006C91 RID: 27793
		protected abstract Toil FeedingToil { get; set; }

		// Token: 0x170010BB RID: 4283
		// (get) Token: 0x06006C92 RID: 27794 RVA: 0x00221CFA File Offset: 0x0021FEFA
		public bool Feeding
		{
			get
			{
				return base.CurToil == this.FeedingToil;
			}
		}

		// Token: 0x06006C93 RID: 27795 RVA: 0x00221D0A File Offset: 0x0021FF0A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			base.AddFailCondition(delegate
			{
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
				return !ChildcareUtility.CanSuckle(this.Baby, out breastfeedFailReason);
			});
			base.SetFinalizerJob(delegate(JobCondition _)
			{
				if (!this.pawn.IsCarryingPawn(this.Baby))
				{
					return null;
				}
				return ChildcareUtility.MakeBringBabyToSafetyJob(this.pawn, this.Baby);
			});
			IEnumerator<Toil> feedBabyEnumerator = this.FeedBaby().GetEnumerator();
			if (!feedBabyEnumerator.MoveNext())
			{
				throw new InvalidOperationException("There must be at least one toil in FeedBaby.");
			}
			Toil firstFeedingToil = feedBabyEnumerator.Current;
			Toil jumpIfDownedOrDrafted = Toils_Jump.JumpIf(firstFeedingToil, () => this.pawn.Downed || this.pawn.Drafted).FailOn(() => !this.pawn.IsCarryingPawn(this.Baby));
			yield return Toils_Jump.JumpIf(jumpIfDownedOrDrafted, () => this.pawn.IsCarryingPawn(this.Baby)).FailOn(() => !this.pawn.IsCarryingPawn(this.Baby) && (this.pawn.Downed || this.pawn.Drafted));
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell, false).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false, true, false);
			yield return jumpIfDownedOrDrafted;
			yield return this.GoToChair();
			yield return this.SitInChair();
			yield return firstFeedingToil;
			while (feedBabyEnumerator.MoveNext())
			{
				Toil toil = feedBabyEnumerator.Current;
				yield return toil;
			}
			feedBabyEnumerator.Dispose();
			yield break;
		}

		// Token: 0x06006C94 RID: 27796 RVA: 0x00221D1C File Offset: 0x0021FF1C
		private Toil GoToChair()
		{
			Toil toil = ToilMaker.MakeToil("GoToChair");
			toil.initAction = delegate()
			{
				IntVec3 breastfeedSpot = RCellFinder.SpotToStandDuringJob(this.pawn, null, null);
				Thing thing = GenClosest.ClosestThingReachable(this.pawn.Position, this.pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 32f, (Thing t) => this.IsValidBreastfeedChair(t) && t.Position.GetDangerFor(this.pawn, t.Map) <= breastfeedSpot.GetDangerFor(this.pawn, this.pawn.Map), null, 0, -1, false, RegionType.Set_Passable, false, false);
				if (thing != null)
				{
					Toils_Ingest.TryFindFreeSittingSpotOnThing(thing, this.pawn, out breastfeedSpot);
				}
				if (!breastfeedSpot.IsValid)
				{
					this.pawn.pather.StartPath(this.pawn.Position, PathEndMode.OnCell);
					return;
				}
				this.pawn.ReserveSittableOrSpot(breastfeedSpot, toil.actor.CurJob, true);
				this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.pawn.CurJob, breastfeedSpot);
				this.pawn.pather.StartPath(breastfeedSpot, PathEndMode.OnCell);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x06006C95 RID: 27797 RVA: 0x00221D6F File Offset: 0x0021FF6F
		private Toil SitInChair()
		{
			Toil toil = ToilMaker.MakeToil("SitInChair");
			toil.initAction = delegate()
			{
				if (!this.pawn.Spawned)
				{
					return;
				}
				Thing thing = this.pawn.Position.GetThingList(this.pawn.Map).FirstOrDefault(new Predicate<Thing>(this.IsValidBreastfeedChair));
				if (thing != null)
				{
					this.pawn.Rotation = thing.Rotation;
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x06006C96 RID: 27798 RVA: 0x00221D94 File Offset: 0x0021FF94
		private bool IsValidBreastfeedChair(Thing t)
		{
			IntVec3 intVec;
			return t.def.building != null && t.def.building.isSittable && Toils_Ingest.TryFindFreeSittingSpotOnThing(t, this.pawn, out intVec) && !t.IsForbidden(this.pawn) && this.pawn.CanReserve(t, 1, -1, null, false) && t.IsSociallyProper(this.pawn) && !t.IsBurning() && !t.HostileTo(this.pawn);
		}

		// Token: 0x06006C97 RID: 27799 RVA: 0x00221E28 File Offset: 0x00220028
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.initialFoodPercentage, "initialFoodPercentage", 0f, false);
		}

		// Token: 0x04004C40 RID: 19520
		public const float BabyFedMemoryFoodNeedIncrease = 0.6f;

		// Token: 0x04004C41 RID: 19521
		protected const TargetIndex BabyInd = TargetIndex.A;

		// Token: 0x04004C42 RID: 19522
		private const int ChairSearchRadius = 32;

		// Token: 0x04004C43 RID: 19523
		protected float initialFoodPercentage;
	}
}
