using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_BeatFire : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass3_0
	{
		public JobDriver_BeatFire _003C_003E4__this;

		public Toil beat;

		public Toil approach;

		internal void _003CMakeNewToils_003Eb__0()
		{
			if (_003C_003E4__this.Map.reservationManager.CanReserve(_003C_003E4__this.pawn, _003C_003E4__this.TargetFire))
			{
				_003C_003E4__this.pawn.Reserve(_003C_003E4__this.TargetFire, _003C_003E4__this.job);
			}
			_003C_003E4__this.pawn.pather.StartPath(_003C_003E4__this.TargetFire, PathEndMode.Touch);
		}

		internal void _003CMakeNewToils_003Eb__1(int delta)
		{
			if (_003C_003E4__this.pawn.pather.Moving && _003C_003E4__this.pawn.pather.nextCell != _003C_003E4__this.TargetFire.Position)
			{
				_003C_003E4__this.StartBeatingFireIfAnyAt(_003C_003E4__this.pawn.pather.nextCell, beat);
			}
			if (_003C_003E4__this.pawn.Position != _003C_003E4__this.TargetFire.Position)
			{
				_003C_003E4__this.StartBeatingFireIfAnyAt(_003C_003E4__this.pawn.Position, beat);
			}
		}

		internal void _003CMakeNewToils_003Eb__2()
		{
			if (!_003C_003E4__this.pawn.CanReachImmediate(_003C_003E4__this.TargetFire, PathEndMode.Touch))
			{
				_003C_003E4__this.JumpToToil(approach);
			}
			else if (!(_003C_003E4__this.pawn.Position != _003C_003E4__this.TargetFire.Position) || !_003C_003E4__this.StartBeatingFireIfAnyAt(_003C_003E4__this.pawn.Position, beat))
			{
				_003C_003E4__this.pawn.natives.TryBeatFire(_003C_003E4__this.TargetFire);
				if (_003C_003E4__this.TargetFire.Destroyed)
				{
					_003C_003E4__this.pawn.records.Increment(RecordDefOf.FiresExtinguished);
					_003C_003E4__this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded);
				}
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__3 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_BeatFire _003C_003E4__this;

		private _003C_003Ec__DisplayClass3_0 _003C_003E8__1;

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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_BeatFire f = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass3_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				f.FailOnDespawnedOrNull(TargetIndex.A);
				_003C_003E8__1.beat = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.approach = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.approach.initAction = delegate
				{
					if (_003C_003E8__1._003C_003E4__this.Map.reservationManager.CanReserve(_003C_003E8__1._003C_003E4__this.pawn, _003C_003E8__1._003C_003E4__this.TargetFire))
					{
						_003C_003E8__1._003C_003E4__this.pawn.Reserve(_003C_003E8__1._003C_003E4__this.TargetFire, _003C_003E8__1._003C_003E4__this.job);
					}
					_003C_003E8__1._003C_003E4__this.pawn.pather.StartPath(_003C_003E8__1._003C_003E4__this.TargetFire, PathEndMode.Touch);
				};
				_003C_003E8__1.approach.tickIntervalAction = delegate
				{
					if (_003C_003E8__1._003C_003E4__this.pawn.pather.Moving && _003C_003E8__1._003C_003E4__this.pawn.pather.nextCell != _003C_003E8__1._003C_003E4__this.TargetFire.Position)
					{
						_003C_003E8__1._003C_003E4__this.StartBeatingFireIfAnyAt(_003C_003E8__1._003C_003E4__this.pawn.pather.nextCell, _003C_003E8__1.beat);
					}
					if (_003C_003E8__1._003C_003E4__this.pawn.Position != _003C_003E8__1._003C_003E4__this.TargetFire.Position)
					{
						_003C_003E8__1._003C_003E4__this.StartBeatingFireIfAnyAt(_003C_003E8__1._003C_003E4__this.pawn.Position, _003C_003E8__1.beat);
					}
				};
				_003C_003E8__1.approach.FailOnDespawnedOrNull(TargetIndex.A);
				_003C_003E8__1.approach.defaultCompleteMode = ToilCompleteMode.PatherArrival;
				_003C_003E8__1.approach.atomicWithPrevious = true;
				_003C_003E2__current = _003C_003E8__1.approach;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E8__1.beat.tickAction = delegate
				{
					if (!_003C_003E8__1._003C_003E4__this.pawn.CanReachImmediate(_003C_003E8__1._003C_003E4__this.TargetFire, PathEndMode.Touch))
					{
						_003C_003E8__1._003C_003E4__this.JumpToToil(_003C_003E8__1.approach);
					}
					else if (!(_003C_003E8__1._003C_003E4__this.pawn.Position != _003C_003E8__1._003C_003E4__this.TargetFire.Position) || !_003C_003E8__1._003C_003E4__this.StartBeatingFireIfAnyAt(_003C_003E8__1._003C_003E4__this.pawn.Position, _003C_003E8__1.beat))
					{
						_003C_003E8__1._003C_003E4__this.pawn.natives.TryBeatFire(_003C_003E8__1._003C_003E4__this.TargetFire);
						if (_003C_003E8__1._003C_003E4__this.TargetFire.Destroyed)
						{
							_003C_003E8__1._003C_003E4__this.pawn.records.Increment(RecordDefOf.FiresExtinguished);
							_003C_003E8__1._003C_003E4__this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded);
						}
					}
				};
				_003C_003E8__1.beat.FailOnDespawnedOrNull(TargetIndex.A);
				_003C_003E8__1.beat.defaultCompleteMode = ToilCompleteMode.Never;
				_003C_003E2__current = _003C_003E8__1.beat;
				_003C_003E1__state = 2;
				return true;
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

	protected Fire TargetFire => (Fire)job.targetA.Thing;

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

	private bool StartBeatingFireIfAnyAt(IntVec3 cell, Toil nextToil)
	{
		List<Thing> thingList = cell.GetThingList(base.Map);
		for (int i = 0; i < thingList.Count; i++)
		{
			if (thingList[i] is Fire { parent: null } fire)
			{
				job.targetA = fire;
				pawn.pather.StopDead();
				JumpToToil(nextToil);
				return true;
			}
		}
		return false;
	}
}
