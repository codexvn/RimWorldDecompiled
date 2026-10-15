using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_GoForWalk : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass1_0
	{
		public JobDriver_GoForWalk _003C_003E4__this;

		public Toil goToil;

		internal bool _003CMakeNewToils_003Eb__0()
		{
			return !JoyUtility.EnjoyableOutsideNow(_003C_003E4__this.pawn);
		}

		internal void _003CMakeNewToils_003Eb__1(int delta)
		{
			if (Find.TickManager.TicksGame > _003C_003E4__this.startTick + _003C_003E4__this.job.def.joyDuration)
			{
				_003C_003E4__this.EndJobWith(JobCondition.Succeeded);
			}
			else
			{
				JoyUtility.JoyTickCheckEnd(_003C_003E4__this.pawn, delta);
			}
		}

		internal void _003CMakeNewToils_003Eb__2()
		{
			if (_003C_003E4__this.job.targetQueueA.Count > 0)
			{
				LocalTargetInfo targetA = _003C_003E4__this.job.targetQueueA[0];
				_003C_003E4__this.job.targetQueueA.RemoveAt(0);
				_003C_003E4__this.job.targetA = targetA;
				_003C_003E4__this.JumpToToil(goToil);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__1 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_GoForWalk _003C_003E4__this;

		private _003C_003Ec__DisplayClass1_0 _003C_003E8__1;

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
		public _003CMakeNewToils_003Ed__1(int _003C_003E1__state)
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
			JobDriver_GoForWalk f = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass1_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				f.FailOn(() => !JoyUtility.EnjoyableOutsideNow(_003C_003E8__1._003C_003E4__this.pawn));
				_003C_003E8__1.goToil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
				_003C_003E8__1.goToil.tickIntervalAction = delegate(int delta)
				{
					if (Find.TickManager.TicksGame > _003C_003E8__1._003C_003E4__this.startTick + _003C_003E8__1._003C_003E4__this.job.def.joyDuration)
					{
						_003C_003E8__1._003C_003E4__this.EndJobWith(JobCondition.Succeeded);
					}
					else
					{
						JoyUtility.JoyTickCheckEnd(_003C_003E8__1._003C_003E4__this.pawn, delta);
					}
				};
				_003C_003E2__current = _003C_003E8__1.goToil;
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					if (_003C_003E8__1._003C_003E4__this.job.targetQueueA.Count > 0)
					{
						LocalTargetInfo targetA = _003C_003E8__1._003C_003E4__this.job.targetQueueA[0];
						_003C_003E8__1._003C_003E4__this.job.targetQueueA.RemoveAt(0);
						_003C_003E8__1._003C_003E4__this.job.targetA = targetA;
						_003C_003E8__1._003C_003E4__this.JumpToToil(_003C_003E8__1.goToil);
					}
				};
				_003C_003E2__current = toil;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
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
			_003CMakeNewToils_003Ed__1 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__1(0)
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

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__1))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__1(-2)
		{
			_003C_003E4__this = this
		};
	}
}
