using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;
using Verse.AI;

public class JobDriver_BreastfeedCarryToMom : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__9 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_BreastfeedCarryToMom _003C_003E4__this;

		private Toil _003CcarryingBabyStart_003E5__2;

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
		public _003CMakeNewToils_003Ed__9(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CcarryingBabyStart_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_BreastfeedCarryToMom CS_0024_003C_003E8__locals16 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals16.FailOnDestroyedNullOrForbidden(TargetIndex.A);
				CS_0024_003C_003E8__locals16.FailOnDestroyedNullOrForbidden(TargetIndex.B);
				CS_0024_003C_003E8__locals16.AddFailCondition(() => (CS_0024_003C_003E8__locals16.MomMustBeImmobile && !CS_0024_003C_003E8__locals16.Mom.Downed && !CS_0024_003C_003E8__locals16.Mom.IsPrisoner) || !ChildcareUtility.CanMomBreastfeedBaby(CS_0024_003C_003E8__locals16.Mom, CS_0024_003C_003E8__locals16.Baby, out var _));
				CS_0024_003C_003E8__locals16.SetFinalizerJob((JobCondition condition) => (!CS_0024_003C_003E8__locals16.pawn.IsCarryingPawn(CS_0024_003C_003E8__locals16.Baby)) ? null : ChildcareUtility.MakeBringBabyToSafetyJob(CS_0024_003C_003E8__locals16.pawn, CS_0024_003C_003E8__locals16.Baby));
				_003CcarryingBabyStart_003E5__2 = Toils_General.Label();
				_003C_003E2__current = Toils_Jump.JumpIf(_003CcarryingBabyStart_003E5__2, () => CS_0024_003C_003E8__locals16.pawn.IsCarryingPawn(CS_0024_003C_003E8__locals16.Baby));
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.A);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CcarryingBabyStart_003E5__2;
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell).FailOnDestroyedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Reserve.Release(TargetIndex.A);
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Reserve.Release(TargetIndex.B);
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				_003C_003E2__current = CS_0024_003C_003E8__locals16.StartBreastfeedJobOnMom();
				_003C_003E1__state = 8;
				return true;
			case 8:
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
			_003CMakeNewToils_003Ed__9 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__9(0)
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

	private const TargetIndex BabyInd = TargetIndex.A;

	private const TargetIndex MomInd = TargetIndex.B;

	private Pawn Baby => (Pawn)base.TargetThingA;

	private Pawn Mom => (Pawn)base.TargetThingB;

	protected virtual bool MomMustBeImmobile => false;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (pawn.Reserve(Baby, job, 1, -1, null, errorOnFailed))
		{
			return pawn.Reserve(Mom, job, 1, -1, null, errorOnFailed);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__9))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__9(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Toil StartBreastfeedJobOnMom()
	{
		Toil toil = ToilMaker.MakeToil("StartBreastfeedJobOnMom");
		toil.initAction = delegate
		{
			Mom.jobs.SuspendCurrentJob(JobCondition.InterruptForced, cancelBusyStances: true, false);
			if (!pawn.carryTracker.innerContainer.TryTransferToContainer(pawn.carryTracker.CarriedThing, Mom.carryTracker.innerContainer))
			{
				Mom.jobs.EndCurrentJob(JobCondition.Incompletable);
				EndJobWith(JobCondition.Incompletable);
			}
			else
			{
				Pawn_JobTracker jobs = Mom.jobs;
				Job newJob = ChildcareUtility.MakeBreastfeedJob(Baby, Mom.CurrentBed());
				bool? keepCarryingThingOverride = true;
				jobs.StartJob(newJob, JobCondition.InterruptForced, null, resumeCurJobAfterwards: false, cancelBusyStances: true, null, null, fromQueue: false, canReturnCurJobToPool: false, keepCarryingThingOverride);
			}
		};
		return toil;
	}
}
