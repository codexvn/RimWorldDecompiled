using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001193 RID: 4499
	public class JobDriver_ExtinguishFiresNearby : JobDriver
	{
		// Token: 0x1700110B RID: 4363
		// (get) Token: 0x06006E00 RID: 28160 RVA: 0x0022596F File Offset: 0x00223B6F
		protected Fire TargetFire
		{
			get
			{
				return (Fire)this.job.targetA.Thing;
			}
		}

		// Token: 0x06006E01 RID: 28161 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06006E02 RID: 28162 RVA: 0x00225E47 File Offset: 0x00224047
		protected override IEnumerable<Toil> MakeNewToils()
		{
			base.AddEndCondition(delegate
			{
				if (this.ticksSpentExtinguishing < this.maxDurationTicks)
				{
					return JobCondition.Ongoing;
				}
				return JobCondition.Succeeded;
			});
			Toil initExtractTargetFromQueue = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A, (Thing thing) => !thing.Destroyed);
			Toil findClosestFire = ToilMaker.MakeToil("MakeNewToils");
			Predicate<Thing> <>9__7;
			Comparison<LocalTargetInfo> <>9__9;
			findClosestFire.tickIntervalAction = delegate(int delta)
			{
				IntVec3 position = this.pawn.Position;
				Map map = this.pawn.Map;
				ThingRequest thingReq = ThingRequest.ForDef(ThingDefOf.Fire);
				PathEndMode peMode = PathEndMode.Touch;
				TraverseParms traverseParams = TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
				float maxDistance = 9999f;
				Predicate<Thing> validator;
				if ((validator = <>9__7) == null)
				{
					validator = (<>9__7 = ((Thing f) => !this.job.GetTargetQueue(TargetIndex.A).Contains(f) && this.pawn.CanReserve(f, 1, -1, null, false)));
				}
				Thing thing = GenClosest.ClosestThingReachable(position, map, thingReq, peMode, traverseParams, maxDistance, validator, null, 0, 13, false, RegionType.Set_Passable, false, false);
				if (thing != null)
				{
					this.job.AddQueuedTarget(TargetIndex.A, thing);
					this.job.GetTargetQueue(TargetIndex.A).RemoveWhere((LocalTargetInfo f) => f == null || f.ThingDestroyed);
					List<LocalTargetInfo> targetQueue = this.job.GetTargetQueue(TargetIndex.A);
					Comparison<LocalTargetInfo> comparison;
					if ((comparison = <>9__9) == null)
					{
						comparison = (<>9__9 = delegate(LocalTargetInfo f1, LocalTargetInfo f2)
						{
							Fire fire = (Fire)((Thing)f1);
							Fire fire2 = (Fire)((Thing)f2);
							int num = this.pawn.Position.DistanceToSquared(fire.Position);
							int num2 = this.pawn.Position.DistanceToSquared(fire2.Position);
							if (num == num2)
							{
								return 0;
							}
							if (num < num2)
							{
								return -1;
							}
							if (num > num2)
							{
								return 1;
							}
							return 0;
						});
					}
					targetQueue.Sort(comparison);
				}
				this.ReadyForNextToil();
			};
			findClosestFire.defaultCompleteMode = ToilCompleteMode.Never;
			yield return initExtractTargetFromQueue;
			yield return Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A, (Thing thing) => !thing.Destroyed);
			yield return Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
			yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A, true);
			Toil beat = ToilMaker.MakeToil("MakeNewToils");
			Toil approach = ToilMaker.MakeToil("MakeNewToils");
			approach.initAction = delegate()
			{
				if (this.Map.reservationManager.CanReserve(this.pawn, this.TargetFire, 1, -1, null, false))
				{
					this.pawn.Reserve(this.TargetFire, this.job, 1, -1, null, true, false);
				}
				this.pawn.pather.StartPath(this.TargetFire, PathEndMode.Touch);
			};
			approach.tickIntervalAction = delegate(int delta)
			{
				if (this.pawn.pather.Moving && this.pawn.pather.nextCell != this.TargetFire.Position)
				{
					this.StartBeatingFireIfAnyAt(this.pawn.pather.nextCell, beat);
				}
				if (this.pawn.Position != this.TargetFire.Position)
				{
					this.StartBeatingFireIfAnyAt(this.pawn.Position, beat);
				}
			};
			approach.JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, findClosestFire);
			approach.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			approach.atomicWithPrevious = true;
			yield return approach;
			beat.tickAction = delegate()
			{
				this.ticksSpentExtinguishing++;
				if (!this.pawn.CanReachImmediate(this.TargetFire, PathEndMode.Touch))
				{
					this.JumpToToil(approach);
					return;
				}
				if (this.pawn.Position != this.TargetFire.Position && this.StartBeatingFireIfAnyAt(this.pawn.Position, beat))
				{
					return;
				}
				this.pawn.natives.TryBeatFire(this.TargetFire);
				if (this.TargetFire.Destroyed)
				{
					this.pawn.records.Increment(RecordDefOf.FiresExtinguished);
					this.ReadyForNextToil();
					return;
				}
			};
			beat.JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, findClosestFire);
			beat.defaultCompleteMode = ToilCompleteMode.Never;
			yield return beat;
			yield return findClosestFire;
			yield return Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
			yield return Toils_Jump.Jump(initExtractTargetFromQueue);
			yield break;
		}

		// Token: 0x06006E03 RID: 28163 RVA: 0x00225E58 File Offset: 0x00224058
		private bool StartBeatingFireIfAnyAt(IntVec3 cell, Toil nextToil)
		{
			List<Thing> thingList = cell.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Fire fire = thingList[i] as Fire;
				if (fire != null && fire.parent == null)
				{
					this.job.targetA = fire;
					this.pawn.pather.StopDead();
					base.JumpToToil(nextToil);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006E04 RID: 28164 RVA: 0x00225EC6 File Offset: 0x002240C6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksSpentExtinguishing, "ticksSpentExtinguishing", 0, false);
		}

		// Token: 0x04004CDC RID: 19676
		private int ticksSpentExtinguishing;

		// Token: 0x04004CDD RID: 19677
		private readonly int maxDurationTicks = 600f.SecondsToTicks();

		// Token: 0x04004CDE RID: 19678
		private const TargetIndex FireInd = TargetIndex.A;

		// Token: 0x04004CDF RID: 19679
		private const float MaxDurationSeconds = 600f;
	}
}
