using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Verse.AI;

public class JobDriver_Follow : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__3 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Follow _003C_003E4__this;

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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_Follow CS_0024_003C_003E8__locals11 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals11.FailOnDespawnedOrNull(TargetIndex.A);
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.tickIntervalAction = delegate
				{
					Pawn pawn = (Pawn)CS_0024_003C_003E8__locals11.job.GetTarget(TargetIndex.A).Thing;
					if (!CS_0024_003C_003E8__locals11.pawn.Position.InHorDistOf(pawn.Position, 4f) || !CS_0024_003C_003E8__locals11.pawn.Position.WithinRegions(pawn.Position, CS_0024_003C_003E8__locals11.Map, 2, TraverseParms.For(CS_0024_003C_003E8__locals11.pawn)))
					{
						if (!CS_0024_003C_003E8__locals11.pawn.CanReach(pawn, PathEndMode.Touch, Danger.Deadly))
						{
							CS_0024_003C_003E8__locals11.EndJobWith(JobCondition.Incompletable);
						}
						else if (!CS_0024_003C_003E8__locals11.pawn.pather.Moving || CS_0024_003C_003E8__locals11.pawn.pather.Destination != pawn)
						{
							CS_0024_003C_003E8__locals11.pawn.pather.StartPath(pawn, PathEndMode.Touch);
						}
					}
				};
				toil.defaultCompleteMode = ToilCompleteMode.Never;
				_003C_003E2__current = toil;
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

	private const TargetIndex FolloweeInd = TargetIndex.A;

	private const int Distance = 4;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return true;
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

	public override bool IsContinuation(Job j)
	{
		return job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
	}
}
