using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_ExecuteSlave : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass3_0
	{
		public JobDriver_ExecuteSlave _003C_003E4__this;

		public Toil execute;

		internal bool _003CMakeNewToils_003Eb__0()
		{
			return !_003C_003E4__this.Victim.IsSlaveOfColony;
		}

		internal void _003CMakeNewToils_003Eb__1()
		{
			ExecutionUtility.DoExecutionByCut(execute.actor, _003C_003E4__this.Victim);
			ThoughtUtility.GiveThoughtsForPawnExecuted(_003C_003E4__this.Victim, execute.actor, PawnExecutionKind.GenericBrutal);
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__3 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_ExecuteSlave _003C_003E4__this;

		private _003C_003Ec__DisplayClass3_0 _003C_003E8__1;

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
			_003C_003E8__1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_ExecuteSlave jobDriver_ExecuteSlave = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass3_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				if (!ModLister.CheckIdeology("Execute slave"))
				{
					return false;
				}
				jobDriver_ExecuteSlave.FailOnAggroMentalState(TargetIndex.A);
				jobDriver_ExecuteSlave.FailOnForbidden(TargetIndex.A);
				_003C_003E2__current = Toils_Interpersonal.GotoSlave(jobDriver_ExecuteSlave.pawn, jobDriver_ExecuteSlave.Victim).FailOn(() => !_003C_003E8__1._003C_003E4__this.Victim.IsSlaveOfColony);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E8__1.execute = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.execute.initAction = delegate
				{
					ExecutionUtility.DoExecutionByCut(_003C_003E8__1.execute.actor, _003C_003E8__1._003C_003E4__this.Victim);
					ThoughtUtility.GiveThoughtsForPawnExecuted(_003C_003E8__1._003C_003E4__this.Victim, _003C_003E8__1.execute.actor, PawnExecutionKind.GenericBrutal);
				};
				_003C_003E8__1.execute.defaultCompleteMode = ToilCompleteMode.Instant;
				_003C_003E8__1.execute.activeSkill = () => SkillDefOf.Melee;
				_003C_003E2__current = _003C_003E8__1.execute;
				_003C_003E1__state = 2;
				return true;
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

	protected Pawn Victim => (Pawn)job.targetA.Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(Victim, job, 1, -1, null, errorOnFailed);
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
