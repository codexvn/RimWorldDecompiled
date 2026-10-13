using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012E2 RID: 4834
	public class JobDriver_Reading : JobDriver
	{
		// Token: 0x17001298 RID: 4760
		// (get) Token: 0x06007674 RID: 30324 RVA: 0x00241350 File Offset: 0x0023F550
		public Book Book
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing as Book;
			}
		}

		// Token: 0x17001299 RID: 4761
		// (get) Token: 0x06007675 RID: 30325 RVA: 0x00241376 File Offset: 0x0023F576
		public bool IsReading
		{
			get
			{
				return this.isReading;
			}
		}

		// Token: 0x06007676 RID: 30326 RVA: 0x0024137E File Offset: 0x0023F57E
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Book, this.job, 1, 1, null, errorOnFailed, false);
		}

		// Token: 0x06007677 RID: 30327 RVA: 0x002413A1 File Offset: 0x0023F5A1
		protected override IEnumerable<Toil> MakeNewToils()
		{
			base.SetFinalizerJob(delegate(JobCondition condition)
			{
				if (!this.pawn.IsCarryingThing(this.Book))
				{
					return null;
				}
				if (condition != JobCondition.Succeeded)
				{
					Thing thing;
					this.pawn.carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Direct, out thing, null);
					return null;
				}
				return HaulAIUtility.HaulToStorageJob(this.pawn, this.Book, false);
			});
			foreach (Toil toil in this.PrepareToReadBook())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			int duration = this.job.playerForced ? 5000 : this.job.def.joyDuration;
			yield return this.ReadBook(duration);
			yield break;
			yield break;
		}

		// Token: 0x06007678 RID: 30328 RVA: 0x002413B4 File Offset: 0x0023F5B4
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.job.count = 1;
			this.hasInInventory = (this.pawn.inventory != null && this.pawn.inventory.Contains(this.Book));
			Pawn pawn = this.pawn;
			this.carrying = (((pawn != null) ? pawn.carryTracker.CarriedThing : null) == this.Book);
			Pawn pawn2 = this.pawn;
			this.isLearningDesire = (((pawn2 != null) ? pawn2.learning : null) != null && this.pawn.learning.ActiveLearningDesires.Contains(LearningDesireDefOf.Reading));
		}

		// Token: 0x06007679 RID: 30329 RVA: 0x0024145B File Offset: 0x0023F65B
		private IEnumerable<Toil> PrepareToReadBook()
		{
			if (this.carrying)
			{
				yield break;
			}
			if (this.hasInInventory)
			{
				yield return Toils_Misc.TakeItemFromInventoryToCarrier(this.pawn, TargetIndex.A);
			}
			else
			{
				yield return Toils_Goto.GotoCell(this.Book.PositionHeld, PathEndMode.ClosestTouch).FailOnDestroyedOrNull(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
				yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false, true, true);
			}
			yield return this.CarryToReadingSpot().FailOnDestroyedOrNull(TargetIndex.A);
			yield return this.FindAdjacentReadingSurface();
			yield break;
		}

		// Token: 0x0600767A RID: 30330 RVA: 0x0024146C File Offset: 0x0023F66C
		private Toil ReadBook(int duration)
		{
			Toil toil = Toils_General.Wait(duration, TargetIndex.None);
			toil.debugName = "Reading";
			toil.FailOnDestroyedNullOrForbidden(TargetIndex.A);
			toil.handlingFacing = true;
			toil.initAction = delegate()
			{
				this.Book.IsOpen = true;
				this.pawn.pather.StopDead();
				this.job.showCarryingInspectLine = false;
			};
			toil.tickIntervalAction = delegate(int delta)
			{
				if (this.job.GetTarget(TargetIndex.B).IsValid)
				{
					this.pawn.rotationTracker.FaceCell(this.job.GetTarget(TargetIndex.B).Cell);
				}
				else if (this.Book.Spawned)
				{
					this.pawn.rotationTracker.FaceCell(this.Book.Position);
				}
				else if (this.pawn.Rotation == Rot4.North)
				{
					this.pawn.Rotation = new Rot4(Rand.Range(1, 4));
				}
				float readingBonus = BookUtility.GetReadingBonus(this.pawn);
				this.isReading = true;
				this.Book.OnBookReadTick(this.pawn, delta, readingBonus);
				Pawn_SkillTracker skills = this.pawn.skills;
				if (skills != null)
				{
					skills.Learn(SkillDefOf.Intellectual, 0.1f * (float)delta, false, false);
				}
				this.pawn.GainComfortFromCellIfPossible(delta, false);
				if (this.pawn.CurJob != null)
				{
					Pawn_NeedsTracker needs = this.pawn.needs;
					if (((needs != null) ? needs.joy : null) != null)
					{
						JoyTickFullJoyAction fullJoyAction = JoyTickFullJoyAction.GoToNextToil;
						if (this.pawn.CurJob.playerForced || this.pawn.learning != null)
						{
							fullJoyAction = JoyTickFullJoyAction.None;
						}
						JoyUtility.JoyTickCheckEnd(this.pawn, delta, fullJoyAction, this.Book.JoyFactor * readingBonus, null);
					}
				}
				if (this.isLearningDesire && this.job != null)
				{
					Pawn_NeedsTracker needs2 = this.pawn.needs;
					if (((needs2 != null) ? needs2.learning : null) != null)
					{
						LearningUtility.LearningTickCheckEnd(this.pawn, delta, this.job.playerForced);
					}
					else
					{
						this.pawn.jobs.curDriver.EndJobWith(JobCondition.Succeeded);
					}
				}
				if (this.pawn.IsHashIntervalTick(600, delta))
				{
					this.pawn.jobs.CheckForJobOverride(9.1f, true);
				}
			};
			toil.AddEndCondition(delegate
			{
				string text;
				if (!BookUtility.CanReadBook(this.Book, this.pawn, out text))
				{
					return JobCondition.InterruptForced;
				}
				return JobCondition.Ongoing;
			});
			toil.AddFinishAction(delegate
			{
				this.Book.IsOpen = false;
				TaleRecorder.RecordTale(TaleDefOf.ReadBook, new object[]
				{
					this.pawn,
					this.Book
				});
				JoyUtility.TryGainRecRoomThought(this.pawn);
			});
			if (this.isLearningDesire && !this.job.playerForced)
			{
				toil.defaultCompleteMode = ToilCompleteMode.Never;
			}
			return toil;
		}

		// Token: 0x0600767B RID: 30331 RVA: 0x00241500 File Offset: 0x0023F700
		private Toil CarryToReadingSpot()
		{
			Toil toil = ToilMaker.MakeToil("CarryToReadingSpot");
			toil.initAction = delegate()
			{
				IntVec3 intVec;
				if (!this.TryGetClosestChairFreeSittingSpot(true, out intVec) && !this.TryGetClosestChairFreeSittingSpot(false, out intVec))
				{
					intVec = RCellFinder.SpotToChewStandingNear(this.pawn, this.Book, (IntVec3 c) => !c.Fogged(this.pawn.Map) && this.pawn.CanReserveSittableOrSpot(c, false));
				}
				if (!intVec.IsValid)
				{
					this.pawn.pather.StartPath(this.pawn.Position, PathEndMode.OnCell);
					return;
				}
				this.pawn.ReserveSittableOrSpot(intVec, this.pawn.CurJob, true);
				this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.pawn.CurJob, intVec);
				this.pawn.pather.StartPath(intVec, PathEndMode.OnCell);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x0600767C RID: 30332 RVA: 0x00241528 File Offset: 0x0023F728
		private bool TryGetClosestChairFreeSittingSpot(bool skipInteractionCells, out IntVec3 cell)
		{
			Thing thing = GenClosest.ClosestThingReachable(this.pawn.Position, this.pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 32f, (Thing t) => JobDriver_Reading.ValidateChair(t, this.pawn, skipInteractionCells) && t.Position.GetDangerFor(this.pawn, t.Map) == Danger.None, null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (thing != null)
			{
				return JobDriver_Reading.TryFindFreeSittingSpotOnThing(thing, this.pawn, skipInteractionCells, out cell);
			}
			cell = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0600767D RID: 30333 RVA: 0x002415BA File Offset: 0x0023F7BA
		private Toil FindAdjacentReadingSurface()
		{
			Toil toil = ToilMaker.MakeToil("FindAdjacentReadingSurface");
			toil.initAction = delegate()
			{
				Map map = this.pawn.Map;
				IntVec3 position = this.pawn.Position;
				Building firstThing = this.pawn.Position.GetFirstThing(this.pawn.Map);
				if (firstThing == null || firstThing.def.building == null || !firstThing.def.building.isSittable)
				{
					this.TryFaceClosestSurface(position, map);
					return;
				}
				if (this.TryFaceClosestSurface(position, map))
				{
					return;
				}
				this.job.SetTarget(TargetIndex.B, position + firstThing.Rotation.FacingCell);
				this.pawn.jobs.curDriver.rotateToFace = TargetIndex.B;
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x0600767E RID: 30334 RVA: 0x002415E0 File Offset: 0x0023F7E0
		private bool TryFaceClosestSurface(IntVec3 pos, Map map)
		{
			for (int i = 0; i < 4; i++)
			{
				Rot4 rot = new Rot4(i);
				IntVec3 c = pos + rot.FacingCell;
				if (c.GetSurfaceType(map) == SurfaceType.Eat)
				{
					this.job.SetTarget(TargetIndex.B, c);
					this.pawn.jobs.curDriver.rotateToFace = TargetIndex.B;
					return true;
				}
			}
			for (int j = 0; j < 4; j++)
			{
				Rot4 rot2 = new Rot4(j);
				IntVec3 c2 = pos + rot2.FacingCell;
				if (c2.GetSurfaceType(map) == SurfaceType.Item)
				{
					this.job.SetTarget(TargetIndex.B, c2);
					this.pawn.jobs.curDriver.rotateToFace = TargetIndex.B;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600767F RID: 30335 RVA: 0x002416A0 File Offset: 0x0023F8A0
		private static bool ValidateChair(Thing t, Pawn pawn, bool skipInteractionCells)
		{
			IntVec3 intVec;
			return t.def.building != null && t.def.building.isSittable && JobDriver_Reading.TryFindFreeSittingSpotOnThing(t, pawn, skipInteractionCells, out intVec) && !t.Fogged() && !t.IsForbidden(pawn) && pawn.CanReserve(t, 1, -1, null, false) && t.IsSociallyProper(pawn) && !t.IsBurning() && !t.HostileTo(pawn);
		}

		// Token: 0x06007680 RID: 30336 RVA: 0x00241728 File Offset: 0x0023F928
		private static bool TryFindFreeSittingSpotOnThing(Thing t, Pawn pawn, bool skipInteractionCells, out IntVec3 cell)
		{
			foreach (IntVec3 intVec in t.OccupiedRect())
			{
				if ((!skipInteractionCells || !intVec.IsBuildingInteractionCell(pawn.Map)) && !intVec.Fogged(pawn.Map) && pawn.CanReserveSittableOrSpot(intVec, false))
				{
					cell = intVec;
					return true;
				}
			}
			cell = default(IntVec3);
			return false;
		}

		// Token: 0x06007681 RID: 30337 RVA: 0x002417B8 File Offset: 0x0023F9B8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.carrying, "carrying", false, false);
			Scribe_Values.Look<bool>(ref this.hasInInventory, "hasInInventory", false, false);
			Scribe_Values.Look<bool>(ref this.isLearningDesire, "wasLearningDesire", false, false);
		}

		// Token: 0x040050EF RID: 20719
		private bool hasInInventory;

		// Token: 0x040050F0 RID: 20720
		private bool carrying;

		// Token: 0x040050F1 RID: 20721
		private bool isLearningDesire;

		// Token: 0x040050F2 RID: 20722
		private bool isReading;

		// Token: 0x040050F3 RID: 20723
		public const TargetIndex BookIndex = TargetIndex.A;

		// Token: 0x040050F4 RID: 20724
		public const TargetIndex SurfaceIndex = TargetIndex.B;

		// Token: 0x040050F5 RID: 20725
		private const int ManualReadTicks = 5000;

		// Token: 0x040050F6 RID: 20726
		private const int ChairSearchRadius = 32;

		// Token: 0x040050F7 RID: 20727
		private const int UrgentJobCheckIntervalTicks = 600;
	}
}
