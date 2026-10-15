using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI;

public class JobDriver_EmancipateSlave : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__5 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_EmancipateSlave _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__5(int _003C_003E1__state)
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
			JobDriver_EmancipateSlave CS_0024_003C_003E8__locals10 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!ModLister.CheckIdeology("Emancipate slave"))
				{
					return false;
				}
				CS_0024_003C_003E8__locals10.FailOnDestroyedOrNull(TargetIndex.A);
				CS_0024_003C_003E8__locals10.FailOn(() => CS_0024_003C_003E8__locals10.Slave.guest.slaveInteractionMode != SlaveInteractionModeDefOf.Emancipate);
				CS_0024_003C_003E8__locals10.FailOnDowned(TargetIndex.A);
				CS_0024_003C_003E8__locals10.FailOnAggroMentalState(TargetIndex.A);
				CS_0024_003C_003E8__locals10.FailOnForbidden(TargetIndex.A);
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOn(() => !CS_0024_003C_003E8__locals10.Slave.IsSlaveOfColony || !CS_0024_003C_003E8__locals10.Slave.guest.SlaveIsSecure).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					GenGuest.EmancipateSlave(CS_0024_003C_003E8__locals10.pawn, CS_0024_003C_003E8__locals10.Slave);
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
			_003CMakeNewToils_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__5(0)
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

	private const TargetIndex SlaveInd = TargetIndex.A;

	private const TargetIndex BedInd = TargetIndex.B;

	private Pawn Slave => (Pawn)job.GetTarget(TargetIndex.A).Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(Slave, job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__5))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__5(-2)
		{
			_003C_003E4__this = this
		};
	}
}
