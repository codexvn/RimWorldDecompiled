using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_HateChant : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass1_0
	{
		public JobDriver_HateChant _003C_003E4__this;

		public Toil stand;

		internal void _003CMakeNewToils_003Eb__0(int delta)
		{
			_003C_003E4__this.pawn.rotationTracker.FaceCell(_003C_003E4__this.pawn.Map.Center);
			_003C_003E4__this.pawn.GainComfortFromCellIfPossible(delta);
			Pawn actor = stand.actor;
			if (actor.IsHashIntervalTick(100, delta))
			{
				actor.jobs.CheckForJobOverride();
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__1 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_HateChant _003C_003E4__this;

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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass1_0 CS_0024_003C_003E8__locals11 = new _003C_003Ec__DisplayClass1_0();
				CS_0024_003C_003E8__locals11._003C_003E4__this = _003C_003E4__this;
				CS_0024_003C_003E8__locals11.stand = Toils_General.Wait(int.MaxValue);
				CS_0024_003C_003E8__locals11.stand.tickIntervalAction = delegate(int delta)
				{
					CS_0024_003C_003E8__locals11._003C_003E4__this.pawn.rotationTracker.FaceCell(CS_0024_003C_003E8__locals11._003C_003E4__this.pawn.Map.Center);
					CS_0024_003C_003E8__locals11._003C_003E4__this.pawn.GainComfortFromCellIfPossible(delta);
					Pawn actor = CS_0024_003C_003E8__locals11.stand.actor;
					if (actor.IsHashIntervalTick(100, delta))
					{
						actor.jobs.CheckForJobOverride();
					}
				};
				CS_0024_003C_003E8__locals11.stand.socialMode = RandomSocialMode.Off;
				CS_0024_003C_003E8__locals11.stand.defaultCompleteMode = ToilCompleteMode.Never;
				CS_0024_003C_003E8__locals11.stand.handlingFacing = true;
				_003C_003E2__current = CS_0024_003C_003E8__locals11.stand;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
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
