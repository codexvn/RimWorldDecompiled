using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using Verse.AI.Group;

namespace Verse.AI;

public class JobDriver_Goto : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass1_0
	{
		public JobDriver_Goto _003C_003E4__this;

		public LocalTargetInfo lookAtTarget;

		internal void _003CMakeNewToils_003Eb__0()
		{
			if (_003C_003E4__this.job.exitMapOnArrival && _003C_003E4__this.pawn.Map.exitMapGrid.IsExitCell(_003C_003E4__this.pawn.Position))
			{
				_003C_003E4__this.TryExitMap();
			}
		}

		internal bool _003CMakeNewToils_003Eb__1()
		{
			if (_003C_003E4__this.job.failIfCantJoinOrCreateCaravan)
			{
				return !CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(_003C_003E4__this.pawn);
			}
			return false;
		}

		internal bool _003CMakeNewToils_003Eb__2()
		{
			if (!(_003C_003E4__this.job.GetTarget(TargetIndex.A).Thing is Pawn pawn))
			{
				return false;
			}
			return pawn.ParentHolder is Corpse;
		}

		internal bool _003CMakeNewToils_003Eb__3()
		{
			return _003C_003E4__this.job.GetTarget(TargetIndex.A).Thing?.Destroyed ?? false;
		}

		internal void _003CMakeNewToils_003Eb__5(int delta)
		{
			_003C_003E4__this.pawn.rotationTracker.FaceCell(lookAtTarget.Cell);
		}

		internal void _003CMakeNewToils_003Eb__4()
		{
			if (_003C_003E4__this.job.controlGroupTag != null && _003C_003E4__this.job.controlGroupTag != null)
			{
				_003C_003E4__this.pawn.GetOverseer()?.mechanitor.GetControlGroup(_003C_003E4__this.pawn).SetTag(_003C_003E4__this.pawn, _003C_003E4__this.job.controlGroupTag);
			}
		}

		internal void _003CMakeNewToils_003Eb__6()
		{
			if (_003C_003E4__this.pawn.mindState != null && _003C_003E4__this.pawn.mindState.forcedGotoPosition == _003C_003E4__this.TargetA.Cell)
			{
				_003C_003E4__this.pawn.mindState.forcedGotoPosition = IntVec3.Invalid;
			}
			if (!_003C_003E4__this.job.ritualTag.NullOrEmpty() && _003C_003E4__this.pawn.GetLord()?.LordJob is LordJob_Ritual lordJob_Ritual)
			{
				lordJob_Ritual.AddTagForPawn(_003C_003E4__this.pawn, _003C_003E4__this.job.ritualTag);
			}
			if (_003C_003E4__this.job.exitMapOnArrival && (_003C_003E4__this.pawn.Position.OnEdge(_003C_003E4__this.pawn.Map) || _003C_003E4__this.pawn.Map.exitMapGrid.IsExitCell(_003C_003E4__this.pawn.Position)))
			{
				_003C_003E4__this.TryExitMap();
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__1 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Goto _003C_003E4__this;

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
			JobDriver_Goto jobDriver_Goto = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass1_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				_003C_003E8__1.lookAtTarget = jobDriver_Goto.job.GetTarget(TargetIndex.B);
				Toil toil2 = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
				toil2.AddPreTickAction(delegate
				{
					if (_003C_003E8__1._003C_003E4__this.job.exitMapOnArrival && _003C_003E8__1._003C_003E4__this.pawn.Map.exitMapGrid.IsExitCell(_003C_003E8__1._003C_003E4__this.pawn.Position))
					{
						_003C_003E8__1._003C_003E4__this.TryExitMap();
					}
				});
				toil2.FailOn(() => _003C_003E8__1._003C_003E4__this.job.failIfCantJoinOrCreateCaravan && !CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(_003C_003E8__1._003C_003E4__this.pawn));
				toil2.FailOn(() => _003C_003E8__1._003C_003E4__this.job.GetTarget(TargetIndex.A).Thing is Pawn pawn && pawn.ParentHolder is Corpse);
				toil2.FailOn(() => _003C_003E8__1._003C_003E4__this.job.GetTarget(TargetIndex.A).Thing?.Destroyed ?? false);
				if (_003C_003E8__1.lookAtTarget.IsValid)
				{
					toil2.tickIntervalAction = (Action<int>)Delegate.Combine(toil2.tickIntervalAction, (Action<int>)delegate
					{
						_003C_003E8__1._003C_003E4__this.pawn.rotationTracker.FaceCell(_003C_003E8__1.lookAtTarget.Cell);
					});
					toil2.handlingFacing = true;
				}
				toil2.AddFinishAction(delegate
				{
					if (_003C_003E8__1._003C_003E4__this.job.controlGroupTag != null && _003C_003E8__1._003C_003E4__this.job.controlGroupTag != null)
					{
						_003C_003E8__1._003C_003E4__this.pawn.GetOverseer()?.mechanitor.GetControlGroup(_003C_003E8__1._003C_003E4__this.pawn).SetTag(_003C_003E8__1._003C_003E4__this.pawn, _003C_003E8__1._003C_003E4__this.job.controlGroupTag);
					}
				});
				_003C_003E2__current = toil2;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					if (_003C_003E8__1._003C_003E4__this.pawn.mindState != null && _003C_003E8__1._003C_003E4__this.pawn.mindState.forcedGotoPosition == _003C_003E8__1._003C_003E4__this.TargetA.Cell)
					{
						_003C_003E8__1._003C_003E4__this.pawn.mindState.forcedGotoPosition = IntVec3.Invalid;
					}
					if (!_003C_003E8__1._003C_003E4__this.job.ritualTag.NullOrEmpty() && _003C_003E8__1._003C_003E4__this.pawn.GetLord()?.LordJob is LordJob_Ritual lordJob_Ritual)
					{
						lordJob_Ritual.AddTagForPawn(_003C_003E8__1._003C_003E4__this.pawn, _003C_003E8__1._003C_003E4__this.job.ritualTag);
					}
					if (_003C_003E8__1._003C_003E4__this.job.exitMapOnArrival && (_003C_003E8__1._003C_003E4__this.pawn.Position.OnEdge(_003C_003E8__1._003C_003E4__this.pawn.Map) || _003C_003E8__1._003C_003E4__this.pawn.Map.exitMapGrid.IsExitCell(_003C_003E8__1._003C_003E4__this.pawn.Position)))
					{
						_003C_003E8__1._003C_003E4__this.TryExitMap();
					}
				};
				toil.defaultCompleteMode = ToilCompleteMode.Instant;
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
		pawn.Map.pawnDestinationReservationManager.Reserve(pawn, job, job.targetA.Cell);
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

	private void TryExitMap()
	{
		if (!job.failIfCantJoinOrCreateCaravan || CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(pawn))
		{
			if (ModsConfig.BiotechActive)
			{
				MechanitorUtility.Notify_PawnGotoLeftMap(pawn, pawn.Map);
			}
			if (!ModsConfig.AnomalyActive || MetalhorrorUtility.TryPawnExitMap(pawn))
			{
				pawn.ExitMap(allowedToJoinOrCreateCaravan: true, CellRect.WholeMap(base.Map).GetClosestEdge(pawn.Position));
			}
		}
	}
}
