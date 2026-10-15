using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_ExecuteEntity : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__7 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_ExecuteEntity _003C_003E4__this;

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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_ExecuteEntity CS_0024_003C_003E8__locals10 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals10.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				CS_0024_003C_003E8__locals10.FailOn(delegate
				{
					Pawn innerPawn = CS_0024_003C_003E8__locals10.InnerPawn;
					if (innerPawn == null || innerPawn.Destroyed)
					{
						return true;
					}
					if (CS_0024_003C_003E8__locals10.job.ignoreDesignations)
					{
						return false;
					}
					CompHoldingPlatformTarget compHoldingPlatformTarget = innerPawn.TryGetComp<CompHoldingPlatformTarget>();
					return compHoldingPlatformTarget == null || compHoldingPlatformTarget.containmentMode != EntityContainmentMode.Execute;
				});
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_General.Do(delegate
				{
					Messages.Message("MessageEntityExecuted".Translate(CS_0024_003C_003E8__locals10.pawn.Named("EXECUTIONER"), CS_0024_003C_003E8__locals10.InnerPawn.Named("VICTIM")), CS_0024_003C_003E8__locals10.pawn, MessageTypeDefOf.NeutralEvent);
					ExecutionUtility.DoExecutionByCut(CS_0024_003C_003E8__locals10.pawn, CS_0024_003C_003E8__locals10.InnerPawn);
					CS_0024_003C_003E8__locals10.pawn.MentalState?.Notify_SlaughteredTarget();
				});
				toil.activeSkill = () => SkillDefOf.Melee;
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

	private const TargetIndex PlatformIndex = TargetIndex.A;

	private Thing Platform => base.TargetThingA;

	private Pawn InnerPawn => (Platform as Building_HoldingPlatform)?.HeldPawn;

	public override string GetReport()
	{
		return JobUtility.GetResolvedJobReport(job.def.reportString, InnerPawn, job.targetB, job.targetC);
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(Platform, job, 1, -1, null, errorOnFailed);
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
}
