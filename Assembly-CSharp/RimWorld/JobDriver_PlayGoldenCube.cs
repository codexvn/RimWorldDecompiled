using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011E1 RID: 4577
	public class JobDriver_PlayGoldenCube : JobDriver
	{
		// Token: 0x06006FD7 RID: 28631 RVA: 0x0022C53B File Offset: 0x0022A73B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, 1, null, errorOnFailed, false);
		}

		// Token: 0x06006FD8 RID: 28632 RVA: 0x0022C55E File Offset: 0x0022A75E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOn(() => !this.pawn.health.hediffSet.HasHediff(HediffDefOf.CubeInterest, false));
			this.FailOn(() => !this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			Toil toil4 = Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false, true, false);
			toil4.AddPreInitAction(delegate
			{
				this.position = this.TargetThingA.PositionHeld;
			});
			yield return toil4;
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = this.job.def.joyDuration;
			Toil toil2 = toil;
			toil2.initAction = (Action)Delegate.Combine(toil2.initAction, new Action(delegate()
			{
				this.job.locomotionUrgency = LocomotionUrgency.Walk;
			}));
			Toil toil3 = toil;
			toil3.tickIntervalAction = (Action<int>)Delegate.Combine(toil3.tickIntervalAction, new Action<int>(delegate(int delta)
			{
				Pawn actor = toil.actor;
				if (GenTicks.TicksGame >= this.nextMoveTick && !actor.pather.Moving && this.TryGetNextCell(out this.nextGoal))
				{
					actor.pather.StartPath(this.nextGoal, PathEndMode.OnCell);
					this.nextMoveTick = GenTicks.TicksGame + JobDriver_PlayGoldenCube.WaitTimeTicksRange.RandomInRange;
				}
			}));
			toil.AddFinishAction(delegate
			{
				Pawn_NeedsTracker needs = this.pawn.needs;
				if (((needs != null) ? needs.joy : null) != null)
				{
					this.pawn.needs.joy.CurLevelPercentage = 1f;
					JoyUtility.TryGainRecRoomThought(this.pawn);
				}
				this.pawn.health.hediffSet.GetFirstHediff<Hediff_CubeInterest>().Notify_PlayedWith();
			});
			yield return toil;
			yield return Toils_Haul.DropCarriedThing();
			yield break;
		}

		// Token: 0x06006FD9 RID: 28633 RVA: 0x0022C570 File Offset: 0x0022A770
		private bool TryGetNextCell(out IntVec3 dest)
		{
			dest = IntVec3.Invalid;
			IntVec3 a = this.position;
			int num = 0;
			int num2 = 0;
			if (this.stepsTillRotSwitch <= 0)
			{
				this.stepsTillRotSwitch = JobDriver_PlayGoldenCube.SwitchDirStepsRange.RandomInRange;
				this.clockwise = !this.clockwise;
			}
			do
			{
				this.angle = (this.angle + Rand.Range(45f, 90f) * (float)(this.clockwise ? 1 : -1)) % 360f;
				int num3 = 1;
				while (num3 <= 3 && (a + IntVec3.FromPolar(this.angle, (float)num3)).Walkable(this.pawn.Map))
				{
					num = num3;
					num3++;
				}
			}
			while (num == 0 && num2++ < 10);
			if (num2 >= 10 || num == 0)
			{
				return false;
			}
			int num4 = Rand.Range(1, num + 1);
			dest = a + IntVec3.FromPolar(this.angle, (float)num4);
			this.lastDist = num4;
			this.stepsTillRotSwitch--;
			return true;
		}

		// Token: 0x06006FDA RID: 28634 RVA: 0x0022C678 File Offset: 0x0022A878
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.position, "position", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.nextGoal, "nextGoal", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.lastDist, "lastDist", 0, false);
			Scribe_Values.Look<int>(ref this.stepsTillRotSwitch, "stepsTillSwitch", 0, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
			Scribe_Values.Look<bool>(ref this.clockwise, "clockwise", false, false);
		}

		// Token: 0x04004DE5 RID: 19941
		private IntVec3 nextGoal;

		// Token: 0x04004DE6 RID: 19942
		private IntVec3 position;

		// Token: 0x04004DE7 RID: 19943
		private int lastDist;

		// Token: 0x04004DE8 RID: 19944
		private int nextMoveTick;

		// Token: 0x04004DE9 RID: 19945
		private int stepsTillRotSwitch = JobDriver_PlayGoldenCube.SwitchDirStepsRange.RandomInRange;

		// Token: 0x04004DEA RID: 19946
		private bool clockwise;

		// Token: 0x04004DEB RID: 19947
		private float angle;

		// Token: 0x04004DEC RID: 19948
		private const int MinCircleDist = 1;

		// Token: 0x04004DED RID: 19949
		private const int MaxCircleDist = 3;

		// Token: 0x04004DEE RID: 19950
		private static readonly IntRange SwitchDirStepsRange = new IntRange(6, 12);

		// Token: 0x04004DEF RID: 19951
		private static readonly IntRange WaitTimeTicksRange = new IntRange(180, 360);

		// Token: 0x04004DF0 RID: 19952
		private const TargetIndex CubeIndex = TargetIndex.A;
	}
}
