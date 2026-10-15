using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

public class JobDriver_Sacrifice : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass5_0
	{
		public JobDriver_Sacrifice _003C_003E4__this;

		public Pawn victim;

		internal void _003CMakeNewToils_003Eb__0()
		{
			Lord lord = _003C_003E4__this.pawn.GetLord();
			if (lord != null && lord.LordJob is LordJob_Ritual lordJob_Ritual)
			{
				lordJob_Ritual.pawnsDeathIgnored.Add(victim);
			}
			ExecutionUtility.DoExecutionByCut(_003C_003E4__this.pawn, victim, 0, spawnBlood: false);
			ThoughtUtility.GiveThoughtsForPawnExecuted(victim, _003C_003E4__this.pawn, PawnExecutionKind.GenericBrutal);
			TaleRecorder.RecordTale(TaleDefOf.ExecutedPrisoner, _003C_003E4__this.pawn, victim);
			victim.health.killedByRitual = true;
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__5 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Sacrifice _003C_003E4__this;

		private _003C_003Ec__DisplayClass5_0 _003C_003E8__1;

		private Toil _003Cexecute_003E5__2;

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
			_003Cexecute_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_Sacrifice jobDriver_Sacrifice = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass5_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				if (!ModLister.CheckIdeology("Sacrifice"))
				{
					return false;
				}
				jobDriver_Sacrifice.FailOnDestroyedOrNull(TargetIndex.A);
				_003C_003E8__1.victim = jobDriver_Sacrifice.Victim;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Wait(35);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003Cexecute_003E5__2 = ToilMaker.MakeToil("MakeNewToils");
				_003Cexecute_003E5__2.initAction = delegate
				{
					Lord lord = _003C_003E8__1._003C_003E4__this.pawn.GetLord();
					if (lord != null && lord.LordJob is LordJob_Ritual lordJob_Ritual)
					{
						lordJob_Ritual.pawnsDeathIgnored.Add(_003C_003E8__1.victim);
					}
					ExecutionUtility.DoExecutionByCut(_003C_003E8__1._003C_003E4__this.pawn, _003C_003E8__1.victim, 0, spawnBlood: false);
					ThoughtUtility.GiveThoughtsForPawnExecuted(_003C_003E8__1.victim, _003C_003E8__1._003C_003E4__this.pawn, PawnExecutionKind.GenericBrutal);
					TaleRecorder.RecordTale(TaleDefOf.ExecutedPrisoner, _003C_003E8__1._003C_003E4__this.pawn, _003C_003E8__1.victim);
					_003C_003E8__1.victim.health.killedByRitual = true;
				};
				_003Cexecute_003E5__2.defaultCompleteMode = ToilCompleteMode.Instant;
				_003C_003E2__current = Toils_Reserve.Release(TargetIndex.A);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003Cexecute_003E5__2;
				_003C_003E1__state = 4;
				return true;
			case 4:
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

	private const TargetIndex VictimIndex = TargetIndex.A;

	private const TargetIndex StandingIndex = TargetIndex.B;

	protected Pawn Victim => (Pawn)job.GetTarget(TargetIndex.A).Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(Victim, job, 1, -1, null, errorOnFailed);
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
