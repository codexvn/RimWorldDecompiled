using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Romance : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__4 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Romance _003C_003E4__this;

		private Toil _003CfinalGoto_003E5__2;

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
			_003CfinalGoto_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_Romance CS_0024_003C_003E8__locals10 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals10.FailOnDespawnedOrNull(TargetIndex.A);
				Toil toil2 = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
				toil2.socialMode = RandomSocialMode.Off;
				_003C_003E2__current = toil2;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				_003CfinalGoto_003E5__2 = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
				_003C_003E2__current = Toils_Jump.JumpIf(_003CfinalGoto_003E5__2, () => !CS_0024_003C_003E8__locals10.OtherPawn.Awake());
				_003C_003E1__state = 2;
				return true;
			case 2:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_Interpersonal.WaitToBeAbleToInteract(CS_0024_003C_003E8__locals10.pawn);
				toil.socialMode = RandomSocialMode.Off;
				_003C_003E2__current = toil;
				_003C_003E1__state = 3;
				return true;
			}
			case 3:
				_003C_003E1__state = -1;
				_003CfinalGoto_003E5__2.socialMode = RandomSocialMode.Off;
				_003C_003E2__current = _003CfinalGoto_003E5__2;
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					if (!CS_0024_003C_003E8__locals10.OtherPawn.Awake())
					{
						CS_0024_003C_003E8__locals10.OtherPawn.jobs.SuspendCurrentJob(JobCondition.InterruptForced, cancelBusyStances: true, null);
						if (!CS_0024_003C_003E8__locals10.pawn.interactions.CanInteractNowWith(CS_0024_003C_003E8__locals10.OtherPawn, InteractionDefOf.RomanceAttempt))
						{
							Messages.Message("RomanceFailedUnexpected".Translate(CS_0024_003C_003E8__locals10.pawn, CS_0024_003C_003E8__locals10.OtherPawn), MessageTypeDefOf.NegativeEvent, historical: false);
						}
					}
				});
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Interpersonal.Interact(TargetIndex.A, CS_0024_003C_003E8__locals10.job.interaction);
				_003C_003E1__state = 6;
				return true;
			case 6:
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

	private const TargetIndex OtherPawnInd = TargetIndex.A;

	private Pawn OtherPawn => (Pawn)job.GetTarget(TargetIndex.A).Thing;

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
}
