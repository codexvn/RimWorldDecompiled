using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_ExtinguishFiresNearby : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass7_0
	{
		public JobDriver_ExtinguishFiresNearby _003C_003E4__this;

		public Toil beat;

		public Toil approach;

		public Predicate<Thing> _003C_003E9__7;

		public Comparison<LocalTargetInfo> _003C_003E9__9;

		internal JobCondition _003CMakeNewToils_003Eb__0()
		{
			if (_003C_003E4__this.ticksSpentExtinguishing < _003C_003E4__this.maxDurationTicks)
			{
				return JobCondition.Ongoing;
			}
			return JobCondition.Succeeded;
		}

		internal void _003CMakeNewToils_003Eb__2(int delta)
		{
			Thing thing = GenClosest.ClosestThingReachable(_003C_003E4__this.pawn.Position, _003C_003E4__this.pawn.Map, ThingRequest.ForDef(ThingDefOf.Fire), PathEndMode.Touch, TraverseParms.For(_003C_003E4__this.pawn), 9999f, (Thing f) => !_003C_003E4__this.job.GetTargetQueue(TargetIndex.A).Contains(f) && _003C_003E4__this.pawn.CanReserve(f), null, 0, 13);
			if (thing != null)
			{
				_003C_003E4__this.job.AddQueuedTarget(TargetIndex.A, thing);
				_003C_003E4__this.job.GetTargetQueue(TargetIndex.A).RemoveWhere((LocalTargetInfo f) => f == null || f.ThingDestroyed);
				_003C_003E4__this.job.GetTargetQueue(TargetIndex.A).Sort(delegate(LocalTargetInfo f1, LocalTargetInfo f2)
				{
					Fire fire = (Fire)(Thing)f1;
					Fire fire2 = (Fire)(Thing)f2;
					int num = _003C_003E4__this.pawn.Position.DistanceToSquared(fire.Position);
					int num2 = _003C_003E4__this.pawn.Position.DistanceToSquared(fire2.Position);
					if (num == num2)
					{
						return 0;
					}
					if (num < num2)
					{
						return -1;
					}
					return (num > num2) ? 1 : 0;
				});
			}
			_003C_003E4__this.ReadyForNextToil();
		}

		internal bool _003CMakeNewToils_003Eb__7(Thing f)
		{
			if (!_003C_003E4__this.job.GetTargetQueue(TargetIndex.A).Contains(f))
			{
				return _003C_003E4__this.pawn.CanReserve(f);
			}
			return false;
		}

		internal int _003CMakeNewToils_003Eb__9(LocalTargetInfo f1, LocalTargetInfo f2)
		{
			Fire fire = (Fire)(Thing)f1;
			Fire fire2 = (Fire)(Thing)f2;
			int num = _003C_003E4__this.pawn.Position.DistanceToSquared(fire.Position);
			int num2 = _003C_003E4__this.pawn.Position.DistanceToSquared(fire2.Position);
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
		}

		internal void _003CMakeNewToils_003Eb__4()
		{
			if (_003C_003E4__this.Map.reservationManager.CanReserve(_003C_003E4__this.pawn, _003C_003E4__this.TargetFire))
			{
				_003C_003E4__this.pawn.Reserve(_003C_003E4__this.TargetFire, _003C_003E4__this.job);
			}
			_003C_003E4__this.pawn.pather.StartPath(_003C_003E4__this.TargetFire, PathEndMode.Touch);
		}

		internal void _003CMakeNewToils_003Eb__5(int delta)
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

		internal void _003CMakeNewToils_003Eb__6()
		{
			_003C_003E4__this.ticksSpentExtinguishing++;
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
					_003C_003E4__this.ReadyForNextToil();
				}
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__7 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_ExtinguishFiresNearby _003C_003E4__this;

		private _003C_003Ec__DisplayClass7_0 _003C_003E8__1;

		private Toil _003CinitExtractTargetFromQueue_003E5__2;

		private Toil _003CfindClosestFire_003E5__3;

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
		public _003CMakeNewToils_003Ed__7(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003CinitExtractTargetFromQueue_003E5__2 = null;
			_003CfindClosestFire_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_ExtinguishFiresNearby jobDriver_ExtinguishFiresNearby = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass7_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				jobDriver_ExtinguishFiresNearby.AddEndCondition(() => (_003C_003E8__1._003C_003E4__this.ticksSpentExtinguishing < _003C_003E8__1._003C_003E4__this.maxDurationTicks) ? JobCondition.Ongoing : JobCondition.Succeeded);
				_003CinitExtractTargetFromQueue_003E5__2 = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A, (Thing thing) => !thing.Destroyed);
				_003CfindClosestFire_003E5__3 = ToilMaker.MakeToil("MakeNewToils");
				_003CfindClosestFire_003E5__3.tickIntervalAction = delegate
				{
					Thing thing2 = GenClosest.ClosestThingReachable(_003C_003E8__1._003C_003E4__this.pawn.Position, _003C_003E8__1._003C_003E4__this.pawn.Map, ThingRequest.ForDef(ThingDefOf.Fire), PathEndMode.Touch, TraverseParms.For(_003C_003E8__1._003C_003E4__this.pawn), 9999f, (Thing f) => !_003C_003E8__1._003C_003E4__this.job.GetTargetQueue(TargetIndex.A).Contains(f) && _003C_003E8__1._003C_003E4__this.pawn.CanReserve(f), null, 0, 13);
					if (thing2 != null)
					{
						_003C_003E8__1._003C_003E4__this.job.AddQueuedTarget(TargetIndex.A, thing2);
						_003C_003E8__1._003C_003E4__this.job.GetTargetQueue(TargetIndex.A).RemoveWhere((LocalTargetInfo f) => f == null || f.ThingDestroyed);
						_003C_003E8__1._003C_003E4__this.job.GetTargetQueue(TargetIndex.A).Sort(delegate(LocalTargetInfo f1, LocalTargetInfo f2)
						{
							Fire fire = (Fire)(Thing)f1;
							Fire fire2 = (Fire)(Thing)f2;
							int num2 = _003C_003E8__1._003C_003E4__this.pawn.Position.DistanceToSquared(fire.Position);
							int num3 = _003C_003E8__1._003C_003E4__this.pawn.Position.DistanceToSquared(fire2.Position);
							if (num2 == num3)
							{
								return 0;
							}
							if (num2 < num3)
							{
								return -1;
							}
							return (num2 > num3) ? 1 : 0;
						});
					}
					_003C_003E8__1._003C_003E4__this.ReadyForNextToil();
				};
				_003CfindClosestFire_003E5__3.defaultCompleteMode = ToilCompleteMode.Never;
				_003C_003E2__current = _003CinitExtractTargetFromQueue_003E5__2;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A, (Thing thing) => !thing.Destroyed);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
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
				_003C_003E8__1.approach.JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, _003CfindClosestFire_003E5__3);
				_003C_003E8__1.approach.defaultCompleteMode = ToilCompleteMode.PatherArrival;
				_003C_003E8__1.approach.atomicWithPrevious = true;
				_003C_003E2__current = _003C_003E8__1.approach;
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E8__1.beat.tickAction = delegate
				{
					_003C_003E8__1._003C_003E4__this.ticksSpentExtinguishing = _003C_003E8__1._003C_003E4__this.ticksSpentExtinguishing + 1;
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
							_003C_003E8__1._003C_003E4__this.ReadyForNextToil();
						}
					}
				};
				_003C_003E8__1.beat.JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, _003CfindClosestFire_003E5__3);
				_003C_003E8__1.beat.defaultCompleteMode = ToilCompleteMode.Never;
				_003C_003E2__current = _003C_003E8__1.beat;
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CfindClosestFire_003E5__3;
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
				_003C_003E1__state = 8;
				return true;
			case 8:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.Jump(_003CinitExtractTargetFromQueue_003E5__2);
				_003C_003E1__state = 9;
				return true;
			case 9:
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
			_003CMakeNewToils_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__7(0)
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

	private int ticksSpentExtinguishing;

	private readonly int maxDurationTicks = 600f.SecondsToTicks();

	private const TargetIndex FireInd = TargetIndex.A;

	private const float MaxDurationSeconds = 600f;

	protected Fire TargetFire => (Fire)job.targetA.Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__7))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__7(-2)
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

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref ticksSpentExtinguishing, "ticksSpentExtinguishing", 0);
	}
}
