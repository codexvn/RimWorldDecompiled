using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_EnslaveOrReduceWillPrisoner : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__3 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_EnslaveOrReduceWillPrisoner _003C_003E4__this;

		private PrisonerInteractionModeDef _003Cinteraction_003E5__2;

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
			_003Cinteraction_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_EnslaveOrReduceWillPrisoner CS_0024_003C_003E8__locals19 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals19.FailOnDespawnedOrNull(TargetIndex.A);
				CS_0024_003C_003E8__locals19.FailOnMentalState(TargetIndex.A);
				CS_0024_003C_003E8__locals19.FailOnNotAwake(TargetIndex.A);
				CS_0024_003C_003E8__locals19.FailOn(() => !CS_0024_003C_003E8__locals19.Prisoner.IsPrisonerOfColony || !CS_0024_003C_003E8__locals19.Prisoner.guest.PrisonerIsSecure);
				bool flag = CS_0024_003C_003E8__locals19.Prisoner.guest.IsInteractionEnabled(PrisonerInteractionModeDefOf.Enslave);
				_003Cinteraction_003E5__2 = (flag ? PrisonerInteractionModeDefOf.Enslave : PrisonerInteractionModeDefOf.ReduceWill);
				_003C_003E2__current = Toils_Interpersonal.GotoPrisoner(CS_0024_003C_003E8__locals19.pawn, CS_0024_003C_003E8__locals19.Prisoner, _003Cinteraction_003E5__2);
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Interpersonal.WaitToBeAbleToInteract(CS_0024_003C_003E8__locals19.pawn);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Interpersonal.ReduceWill(CS_0024_003C_003E8__locals19.pawn, CS_0024_003C_003E8__locals19.Prisoner);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Interpersonal.GotoPrisoner(CS_0024_003C_003E8__locals19.pawn, CS_0024_003C_003E8__locals19.Prisoner, _003Cinteraction_003E5__2);
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Interpersonal.ReduceWill(CS_0024_003C_003E8__locals19.pawn, CS_0024_003C_003E8__locals19.Prisoner);
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Interpersonal.GotoPrisoner(CS_0024_003C_003E8__locals19.pawn, CS_0024_003C_003E8__locals19.Prisoner, _003Cinteraction_003E5__2);
				_003C_003E1__state = 8;
				return true;
			case 8:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A).FailOn(() => !CS_0024_003C_003E8__locals19.Prisoner.guest.ScheduledForInteraction);
				_003C_003E1__state = 9;
				return true;
			case 9:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Interpersonal.SetLastInteractTime(TargetIndex.A);
				_003C_003E1__state = 10;
				return true;
			case 10:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Interpersonal.TryEnslave(TargetIndex.A);
				_003C_003E1__state = 11;
				return true;
			case 11:
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

	protected Pawn Prisoner => (Pawn)job.targetA.Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
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
