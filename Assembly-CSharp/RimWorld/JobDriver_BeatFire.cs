using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001190 RID: 4496
	public class JobDriver_BeatFire : JobDriver
	{
		// Token: 0x17001108 RID: 4360
		// (get) Token: 0x06006DEF RID: 28143 RVA: 0x0022596F File Offset: 0x00223B6F
		protected Fire TargetFire
		{
			get
			{
				return (Fire)this.job.targetA.Thing;
			}
		}

		// Token: 0x06006DF0 RID: 28144 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06006DF1 RID: 28145 RVA: 0x00225986 File Offset: 0x00223B86
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
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
			approach.FailOnDespawnedOrNull(TargetIndex.A);
			approach.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			approach.atomicWithPrevious = true;
			yield return approach;
			beat.tickAction = delegate()
			{
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
					this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
				}
			};
			beat.FailOnDespawnedOrNull(TargetIndex.A);
			beat.defaultCompleteMode = ToilCompleteMode.Never;
			yield return beat;
			yield break;
		}

		// Token: 0x06006DF2 RID: 28146 RVA: 0x00225998 File Offset: 0x00223B98
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
	}
}
