using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Insult : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__4 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Insult _003C_003E4__this;

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
			JobDriver_Insult jobDriver_Insult = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				jobDriver_Insult.FailOnDespawnedOrNull(TargetIndex.A);
				_003C_003E2__current = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = jobDriver_Insult.InsultingSpreeDelayToil();
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Interpersonal.WaitToBeAbleToInteract(jobDriver_Insult.pawn);
				_003C_003E1__state = 3;
				return true;
			case 3:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
				toil.socialMode = RandomSocialMode.Off;
				_003C_003E2__current = toil;
				_003C_003E1__state = 4;
				return true;
			}
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = jobDriver_Insult.InteractToil();
				_003C_003E1__state = 5;
				return true;
			case 5:
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

	private const TargetIndex TargetInd = TargetIndex.A;

	private Pawn Target => (Pawn)(Thing)pawn.CurJob.GetTarget(TargetIndex.A);

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return true;
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

	private Toil InteractToil()
	{
		return Toils_General.Do(delegate
		{
			if (pawn.interactions.TryInteractWith(Target, InteractionDefOf.Insult) && pawn.MentalState is MentalState_InsultingSpree mentalState_InsultingSpree)
			{
				mentalState_InsultingSpree.lastInsultTicks = Find.TickManager.TicksGame;
				if (mentalState_InsultingSpree.target == Target)
				{
					mentalState_InsultingSpree.insultedTargetAtLeastOnce = true;
				}
			}
		});
	}

	private Toil InsultingSpreeDelayToil()
	{
		Toil toil = ToilMaker.MakeToil("InsultingSpreeDelayToil");
		toil.initAction = WaitAction;
		toil.tickIntervalAction = delegate
		{
			WaitAction();
		};
		toil.socialMode = RandomSocialMode.Off;
		toil.defaultCompleteMode = ToilCompleteMode.Never;
		return toil;
		void WaitAction()
		{
			if (!(pawn.MentalState is MentalState_InsultingSpree mentalState_InsultingSpree) || Find.TickManager.TicksGame - mentalState_InsultingSpree.lastInsultTicks >= 1200)
			{
				pawn.jobs.curDriver.ReadyForNextToil();
			}
		}
	}
}
