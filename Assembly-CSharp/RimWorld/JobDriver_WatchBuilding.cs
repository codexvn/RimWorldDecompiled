using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_WatchBuilding : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass2_0
	{
		public Toil watch;

		public JobDriver_WatchBuilding _003C_003E4__this;

		internal bool _003CMakeNewToils_003Eb__0()
		{
			return !watch.actor.Awake();
		}

		internal void _003CMakeNewToils_003Eb__1()
		{
			JoyUtility.TryGainRecRoomThought(_003C_003E4__this.pawn);
		}

		internal EffecterDef _003CMakeNewToils_003Eb__3()
		{
			return _003C_003E4__this.TargetA.Thing.def.building.effectWatching;
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__2 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_WatchBuilding _003C_003E4__this;

		private _003C_003Ec__DisplayClass2_0 _003C_003E8__1;

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
		public _003CMakeNewToils_003Ed__2(int _003C_003E1__state)
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
			JobDriver_WatchBuilding jobDriver_WatchBuilding = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass2_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				jobDriver_WatchBuilding.EndOnDespawnedOrNull(TargetIndex.A);
				if (jobDriver_WatchBuilding.TargetC.HasThing && jobDriver_WatchBuilding.TargetC.Thing is Building_Bed)
				{
					jobDriver_WatchBuilding.KeepLyingDown(TargetIndex.C);
					_003C_003E2__current = Toils_Bed.ClaimBedIfNonMedical(TargetIndex.C);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
				_003C_003E1__state = 3;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Bed.GotoBed(TargetIndex.C);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E8__1.watch = Toils_LayDown.LayDown(TargetIndex.C, hasBed: true, lookForOtherJobs: false);
				_003C_003E8__1.watch.AddFailCondition(() => !_003C_003E8__1.watch.actor.Awake());
				goto IL_012d;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E8__1.watch = ToilMaker.MakeToil("MakeNewToils");
				goto IL_012d;
			case 4:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_012d:
				_003C_003E8__1.watch.AddPreTickIntervalAction(jobDriver_WatchBuilding.WatchTickAction);
				_003C_003E8__1.watch.AddFinishAction(delegate
				{
					JoyUtility.TryGainRecRoomThought(_003C_003E8__1._003C_003E4__this.pawn);
				});
				_003C_003E8__1.watch.defaultCompleteMode = ToilCompleteMode.Delay;
				_003C_003E8__1.watch.defaultDuration = jobDriver_WatchBuilding.job.def.joyDuration;
				_003C_003E8__1.watch.handlingFacing = true;
				if (jobDriver_WatchBuilding.TargetA.Thing.def.building != null && jobDriver_WatchBuilding.TargetA.Thing.def.building.effectWatching != null)
				{
					_003C_003E8__1.watch.WithEffect(() => _003C_003E8__1._003C_003E4__this.TargetA.Thing.def.building.effectWatching, EffectTargetGetter, null);
				}
				_003C_003E2__current = _003C_003E8__1.watch;
				_003C_003E1__state = 4;
				return true;
			}
			LocalTargetInfo EffectTargetGetter()
			{
				return ((_003C_003Ec__DisplayClass2_0)this)._003C_003E4__this.TargetA.Thing.OccupiedRect().RandomCell + IntVec3.North.RotatedBy(((_003C_003Ec__DisplayClass2_0)this)._003C_003E4__this.TargetA.Thing.Rotation);
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
			_003CMakeNewToils_003Ed__2 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__2(0)
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
		if (!pawn.Reserve(job.targetA, job, job.def.joyMaxParticipants, 0, null, errorOnFailed))
		{
			return false;
		}
		if (!pawn.ReserveSittableOrSpot(job.targetB.Cell, job, errorOnFailed))
		{
			return false;
		}
		if (base.TargetC.HasThing && base.TargetC.Thing is Building_Bed && !pawn.Reserve(job.targetC, job, ((Building_Bed)base.TargetC.Thing).SleepingSlotsCount, 0, null, errorOnFailed))
		{
			return false;
		}
		return true;
	}

	public override bool CanBeginNowWhileLyingDown()
	{
		if (base.TargetC.HasThing && base.TargetC.Thing is Building_Bed)
		{
			return JobInBedUtility.InBedOrRestSpotNow(pawn, base.TargetC);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__2))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__2(-2)
		{
			_003C_003E4__this = this
		};
	}

	protected virtual void WatchTickAction(int delta)
	{
		pawn.rotationTracker.FaceCell(base.TargetA.Cell);
		pawn.GainComfortFromCellIfPossible(delta);
		JoyUtility.JoyTickCheckEnd(pawn, delta, JoyTickFullJoyAction.EndJob, 1f, (Building)base.TargetThingA);
	}

	public override object[] TaleParameters()
	{
		return new object[2]
		{
			pawn,
			base.TargetA.Thing.def
		};
	}
}
