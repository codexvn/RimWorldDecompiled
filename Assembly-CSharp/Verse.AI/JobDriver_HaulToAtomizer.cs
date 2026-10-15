using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI;

public class JobDriver_HaulToAtomizer : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__4 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_HaulToAtomizer _003C_003E4__this;

		private Toil _003CclearQueue_003E5__2;

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
		public _003CMakeNewToils_003Ed__4(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CclearQueue_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_HaulToAtomizer CS_0024_003C_003E8__locals3 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals3.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				CS_0024_003C_003E8__locals3.AddEndCondition(() => (CS_0024_003C_003E8__locals3.TargetThingA.TryGetComp<CompAtomizer>().SpaceLeft > 0) ? JobCondition.Ongoing : JobCondition.Succeeded);
				_003CclearQueue_003E5__2 = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.B);
				_003C_003E2__current = _003CclearQueue_003E5__2;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.B);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Reserve.Reserve(TargetIndex.B);
				_003C_003E1__state = 4;
				return true;
			case 4:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
				_003C_003E2__current = toil;
				_003C_003E1__state = 5;
				return true;
			}
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.B, putRemainderInQueue: false, subtractNumTakenFromJobCount: true);
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.CheckForGetOpportunityDuplicate(_003CclearQueue_003E5__2, TargetIndex.B, TargetIndex.None, takeFromValidStorage: true);
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
				_003C_003E1__state = 8;
				return true;
			case 8:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.DepositHauledThingInContainer(TargetIndex.A, TargetIndex.B);
				_003C_003E1__state = 9;
				return true;
			case 9:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.Jump(_003CclearQueue_003E5__2);
				_003C_003E1__state = 10;
				return true;
			case 10:
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
			_003CMakeNewToils_003Ed__4 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__4(0)
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

	private const TargetIndex AtomizerInd = TargetIndex.A;

	private const TargetIndex WastepackInd = TargetIndex.B;

	private const TargetIndex ChargerCellInd = TargetIndex.C;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		pawn.ReserveAsManyAsPossible(job.GetTargetQueue(TargetIndex.B), job);
		return pawn.Reserve(job.GetTarget(TargetIndex.A), job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__4))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__4(-2)
		{
			_003C_003E4__this = this
		};
	}
}
