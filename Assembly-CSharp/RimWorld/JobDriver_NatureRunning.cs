using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_NatureRunning : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass3_0
	{
		public JobDriver_NatureRunning _003C_003E4__this;

		public Toil wait;

		internal void _003CMakeNewToils_003Eb__0(int delta)
		{
			LearningUtility.LearningTickCheckEnd(_003C_003E4__this.pawn, delta);
		}

		internal void _003CMakeNewToils_003Eb__1()
		{
			wait.actor.pather.StopDead();
		}

		internal void _003CMakeNewToils_003Eb__2(int delta)
		{
			if (!LearningUtility.LearningTickCheckEnd(_003C_003E4__this.pawn, delta))
			{
				_003C_003E4__this.pawn.rotationTracker.FaceTarget(_003C_003E4__this.TargetA);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__3 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_NatureRunning _003C_003E4__this;

		private _003C_003Ec__DisplayClass3_0 _003C_003E8__1;

		private Toil _003CfindInterestingThing_003E5__2;

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
		public _003CMakeNewToils_003Ed__3(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003CfindInterestingThing_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_NatureRunning jobDriver_NatureRunning = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass3_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				jobDriver_NatureRunning.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				jobDriver_NatureRunning.FailOnChildLearningConditions();
				_003CfindInterestingThing_003E5__2 = jobDriver_NatureRunning.FindInterestingThing();
				_003C_003E2__current = _003CfindInterestingThing_003E5__2;
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell);
				toil.tickIntervalAction = delegate(int delta)
				{
					LearningUtility.LearningTickCheckEnd(_003C_003E8__1._003C_003E4__this.pawn, delta);
				};
				_003C_003E2__current = toil;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
				_003C_003E1__state = -1;
				_003C_003E8__1.wait = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.wait.initAction = delegate
				{
					_003C_003E8__1.wait.actor.pather.StopDead();
				};
				_003C_003E8__1.wait.tickIntervalAction = delegate(int delta)
				{
					if (!LearningUtility.LearningTickCheckEnd(_003C_003E8__1._003C_003E4__this.pawn, delta))
					{
						_003C_003E8__1._003C_003E4__this.pawn.rotationTracker.FaceTarget(_003C_003E8__1._003C_003E4__this.TargetA);
					}
				};
				_003C_003E8__1.wait.defaultCompleteMode = ToilCompleteMode.Delay;
				_003C_003E8__1.wait.defaultDuration = WaitTicksRange.RandomInRange;
				_003C_003E8__1.wait.handlingFacing = true;
				_003C_003E2__current = _003C_003E8__1.wait;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.Jump(_003CfindInterestingThing_003E5__2);
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
			_003CMakeNewToils_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__3(0)
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

	private static readonly IntRange WaitTicksRange = new IntRange(300, 600);

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return true;
	}

	private Toil FindInterestingThing()
	{
		Toil toil = ToilMaker.MakeToil("FindInterestingThing");
		toil.initAction = delegate
		{
			if (NatureRunningUtility.TryFindNatureInterestTarget(pawn, out var interestTarget))
			{
				job.SetTarget(TargetIndex.A, interestTarget);
			}
			else
			{
				EndJobWith(JobCondition.Incompletable);
			}
		};
		toil.defaultCompleteMode = ToilCompleteMode.Instant;
		return toil;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__3))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__3(-2)
		{
			_003C_003E4__this = this
		};
	}
}
