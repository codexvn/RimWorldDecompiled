using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI;

public class JobDriver_ReleasePrisoner : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass5_0
	{
		public JobDriver_ReleasePrisoner _003C_003E4__this;

		public Toil setReleased;

		internal bool _003CMakeNewToils_003Eb__0()
		{
			return ((Pawn)(Thing)_003C_003E4__this.GetActor().CurJob.GetTarget(TargetIndex.A)).guest.IsInteractionDisabled(PrisonerInteractionModeDefOf.Release);
		}

		internal bool _003CMakeNewToils_003Eb__1()
		{
			if (_003C_003E4__this.Prisoner.IsPrisonerOfColony)
			{
				return !_003C_003E4__this.Prisoner.guest.PrisonerIsSecure;
			}
			return true;
		}

		internal void _003CMakeNewToils_003Eb__2()
		{
			Pawn pawn = setReleased.actor.jobs.curJob.targetA.Thing as Pawn;
			GenGuest.PrisonerRelease(pawn);
			if (!PawnBanishUtility.WouldBeLeftToDie(pawn, pawn.Map.Tile))
			{
				GenGuest.AddHealthyPrisonerReleasedThoughts(pawn);
			}
			QuestUtility.SendQuestTargetSignals(pawn.questTags, "Released", pawn.Named("SUBJECT"));
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__5 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_ReleasePrisoner _003C_003E4__this;

		private _003C_003Ec__DisplayClass5_0 _003C_003E8__1;

		private Toil _003CcarryToCell_003E5__2;

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
			_003C_003E8__1 = null;
			_003CcarryToCell_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_ReleasePrisoner f = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass5_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				f.FailOnDestroyedOrNull(TargetIndex.A);
				f.FailOnBurningImmobile(TargetIndex.B);
				f.FailOn(() => ((Pawn)(Thing)_003C_003E8__1._003C_003E4__this.GetActor().CurJob.GetTarget(TargetIndex.A)).guest.IsInteractionDisabled(PrisonerInteractionModeDefOf.Release));
				f.FailOnDowned(TargetIndex.A);
				f.FailOnAggroMentalState(TargetIndex.A);
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOn(() => !_003C_003E8__1._003C_003E4__this.Prisoner.IsPrisonerOfColony || !_003C_003E8__1._003C_003E4__this.Prisoner.guest.PrisonerIsSecure).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.A);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003CcarryToCell_003E5__2 = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
				_003C_003E2__current = _003CcarryToCell_003E5__2;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, _003CcarryToCell_003E5__2, storageMode: false);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E8__1.setReleased = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.setReleased.initAction = delegate
				{
					Pawn pawn = _003C_003E8__1.setReleased.actor.jobs.curJob.targetA.Thing as Pawn;
					GenGuest.PrisonerRelease(pawn);
					if (!PawnBanishUtility.WouldBeLeftToDie(pawn, pawn.Map.Tile))
					{
						GenGuest.AddHealthyPrisonerReleasedThoughts(pawn);
					}
					QuestUtility.SendQuestTargetSignals(pawn.questTags, "Released", pawn.Named("SUBJECT"));
				};
				_003C_003E2__current = _003C_003E8__1.setReleased;
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

	private const TargetIndex PrisonerInd = TargetIndex.A;

	private const TargetIndex ReleaseCellInd = TargetIndex.B;

	private Pawn Prisoner => (Pawn)job.GetTarget(TargetIndex.A).Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(Prisoner, job, 1, -1, null, errorOnFailed);
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
