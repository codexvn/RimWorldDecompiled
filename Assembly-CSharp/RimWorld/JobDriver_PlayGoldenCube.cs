using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_PlayGoldenCube : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass13_0
	{
		public JobDriver_PlayGoldenCube _003C_003E4__this;

		public Toil toil;

		internal bool _003CMakeNewToils_003Eb__0()
		{
			return !_003C_003E4__this.pawn.health.hediffSet.HasHediff(HediffDefOf.CubeInterest);
		}

		internal bool _003CMakeNewToils_003Eb__1()
		{
			return !_003C_003E4__this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
		}

		internal void _003CMakeNewToils_003Eb__2()
		{
			_003C_003E4__this.position = _003C_003E4__this.TargetThingA.PositionHeld;
		}

		internal void _003CMakeNewToils_003Eb__4()
		{
			_003C_003E4__this.job.locomotionUrgency = LocomotionUrgency.Walk;
		}

		internal void _003CMakeNewToils_003Eb__5(int delta)
		{
			Pawn actor = toil.actor;
			if (GenTicks.TicksGame >= _003C_003E4__this.nextMoveTick && !actor.pather.Moving && _003C_003E4__this.TryGetNextCell(out _003C_003E4__this.nextGoal))
			{
				actor.pather.StartPath(_003C_003E4__this.nextGoal, PathEndMode.OnCell);
				_003C_003E4__this.nextMoveTick = GenTicks.TicksGame + WaitTimeTicksRange.RandomInRange;
			}
		}

		internal void _003CMakeNewToils_003Eb__3()
		{
			if (_003C_003E4__this.pawn.needs?.joy != null)
			{
				_003C_003E4__this.pawn.needs.joy.CurLevelPercentage = 1f;
				JoyUtility.TryGainRecRoomThought(_003C_003E4__this.pawn);
			}
			_003C_003E4__this.pawn.health.hediffSet.GetFirstHediff<Hediff_CubeInterest>().Notify_PlayedWith();
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__13 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_PlayGoldenCube _003C_003E4__this;

		private _003C_003Ec__DisplayClass13_0 _003C_003E8__1;

		Toil IEnumerator<Toil>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CMakeNewToils_003Ed__13(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_PlayGoldenCube jobDriver_PlayGoldenCube = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass13_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				jobDriver_PlayGoldenCube.FailOnDestroyedOrNull(TargetIndex.A);
				jobDriver_PlayGoldenCube.FailOn(() => !_003C_003E8__1._003C_003E4__this.pawn.health.hediffSet.HasHediff(HediffDefOf.CubeInterest));
				jobDriver_PlayGoldenCube.FailOn(() => !_003C_003E8__1._003C_003E4__this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
				_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil3 = Toils_Haul.StartCarryThing(TargetIndex.A);
				toil3.AddPreInitAction(delegate
				{
					_003C_003E8__1._003C_003E4__this.position = _003C_003E8__1._003C_003E4__this.TargetThingA.PositionHeld;
				});
				_003C_003E2__current = toil3;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
			{
				_003C_003E1__state = -1;
				_003C_003E8__1.toil = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.toil.defaultCompleteMode = ToilCompleteMode.Delay;
				_003C_003E8__1.toil.defaultDuration = jobDriver_PlayGoldenCube.job.def.joyDuration;
				Toil toil = _003C_003E8__1.toil;
				toil.initAction = (Action)Delegate.Combine(toil.initAction, (Action)delegate
				{
					_003C_003E8__1._003C_003E4__this.job.locomotionUrgency = LocomotionUrgency.Walk;
				});
				Toil toil2 = _003C_003E8__1.toil;
				toil2.tickIntervalAction = (Action<int>)Delegate.Combine(toil2.tickIntervalAction, (Action<int>)delegate
				{
					Pawn actor = _003C_003E8__1.toil.actor;
					if (GenTicks.TicksGame >= _003C_003E8__1._003C_003E4__this.nextMoveTick && !actor.pather.Moving && _003C_003E8__1._003C_003E4__this.TryGetNextCell(out _003C_003E8__1._003C_003E4__this.nextGoal))
					{
						actor.pather.StartPath(_003C_003E8__1._003C_003E4__this.nextGoal, PathEndMode.OnCell);
						_003C_003E8__1._003C_003E4__this.nextMoveTick = GenTicks.TicksGame + WaitTimeTicksRange.RandomInRange;
					}
				});
				_003C_003E8__1.toil.AddFinishAction(delegate
				{
					if (_003C_003E8__1._003C_003E4__this.pawn.needs?.joy != null)
					{
						_003C_003E8__1._003C_003E4__this.pawn.needs.joy.CurLevelPercentage = 1f;
						JoyUtility.TryGainRecRoomThought(_003C_003E8__1._003C_003E4__this.pawn);
					}
					_003C_003E8__1._003C_003E4__this.pawn.health.hediffSet.GetFirstHediff<Hediff_CubeInterest>().Notify_PlayedWith();
				});
				_003C_003E2__current = _003C_003E8__1.toil;
				_003C_003E1__state = 3;
				return true;
			}
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.DropCarriedThing();
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CMakeNewToils_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__13(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Toil>)this).GetEnumerator();
		}
	}

	private IntVec3 nextGoal;

	private IntVec3 position;

	private int lastDist;

	private int nextMoveTick;

	private int stepsTillRotSwitch = SwitchDirStepsRange.RandomInRange;

	private bool clockwise;

	private float angle;

	private const int MinCircleDist = 1;

	private const int MaxCircleDist = 3;

	private static readonly IntRange SwitchDirStepsRange = new IntRange(6, 12);

	private static readonly IntRange WaitTimeTicksRange = new IntRange(180, 360);

	private const TargetIndex CubeIndex = TargetIndex.A;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.targetA, job, 1, 1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__13))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__13(-2)
		{
			_003C_003E4__this = this
		};
	}

	private bool TryGetNextCell(out IntVec3 dest)
	{
		dest = IntVec3.Invalid;
		IntVec3 intVec = position;
		int num = 0;
		int num2 = 0;
		if (stepsTillRotSwitch <= 0)
		{
			stepsTillRotSwitch = SwitchDirStepsRange.RandomInRange;
			clockwise = !clockwise;
		}
		do
		{
			angle = (angle + Rand.Range(45f, 90f) * (float)(clockwise ? 1 : (-1))) % 360f;
			for (int i = 1; i <= 3 && (intVec + IntVec3.FromPolar(angle, i)).Walkable(pawn.Map); i++)
			{
				num = i;
			}
		}
		while (num == 0 && num2++ < 10);
		if (num2 >= 10 || num == 0)
		{
			return false;
		}
		int num3 = Rand.Range(1, num + 1);
		dest = intVec + IntVec3.FromPolar(angle, num3);
		lastDist = num3;
		stepsTillRotSwitch--;
		return true;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref position, "position");
		Scribe_Values.Look(ref nextGoal, "nextGoal");
		Scribe_Values.Look(ref lastDist, "lastDist", 0);
		Scribe_Values.Look(ref stepsTillRotSwitch, "stepsTillSwitch", 0);
		Scribe_Values.Look(ref angle, "angle", 0f);
		Scribe_Values.Look(ref clockwise, "clockwise", defaultValue: false);
	}
}
