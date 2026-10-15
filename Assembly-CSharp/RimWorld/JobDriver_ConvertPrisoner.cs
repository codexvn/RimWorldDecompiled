using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_ConvertPrisoner : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__4 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_ConvertPrisoner _003C_003E4__this;

		private int _003Ci_003E5__2;

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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_ConvertPrisoner CS_0024_003C_003E8__locals15 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!ModLister.CheckIdeology("Ideoligion conversion"))
				{
					return false;
				}
				CS_0024_003C_003E8__locals15.FailOnDespawnedOrNull(TargetIndex.A);
				CS_0024_003C_003E8__locals15.FailOnMentalState(TargetIndex.A);
				CS_0024_003C_003E8__locals15.FailOnNotAwake(TargetIndex.A);
				CS_0024_003C_003E8__locals15.FailOn(() => !CS_0024_003C_003E8__locals15.Prisoner.IsPrisonerOfColony || !CS_0024_003C_003E8__locals15.Prisoner.guest.PrisonerIsSecure);
				_003Ci_003E5__2 = 0;
				goto IL_012f;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Interpersonal.WaitToBeAbleToInteract(CS_0024_003C_003E8__locals15.pawn);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Interpersonal.ConvinceRecruitee(CS_0024_003C_003E8__locals15.pawn, CS_0024_003C_003E8__locals15.Prisoner, InteractionDefOf.Chitchat);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				goto IL_012f;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Interpersonal.WaitToBeAbleToInteract(CS_0024_003C_003E8__locals15.pawn);
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A).FailOn(() => !CS_0024_003C_003E8__locals15.Prisoner.guest.ScheduledForInteraction);
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Interpersonal.SetLastInteractTime(TargetIndex.A);
				_003C_003E1__state = 8;
				return true;
			case 8:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Interpersonal.TryConvert(TargetIndex.A);
				_003C_003E1__state = 9;
				return true;
			case 9:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_012f:
				if (_003Ci_003E5__2 < 3)
				{
					_003C_003E2__current = Toils_Interpersonal.GotoPrisoner(CS_0024_003C_003E8__locals15.pawn, CS_0024_003C_003E8__locals15.Prisoner, PrisonerInteractionModeDefOf.Convert);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = Toils_Interpersonal.GotoPrisoner(CS_0024_003C_003E8__locals15.pawn, CS_0024_003C_003E8__locals15.Prisoner, PrisonerInteractionModeDefOf.Convert);
				_003C_003E1__state = 5;
				return true;
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

	private const int NumTalks = 3;

	protected Pawn Prisoner => (Pawn)job.targetA.Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
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
