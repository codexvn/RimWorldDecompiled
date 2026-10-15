using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI.Group;

namespace RimWorld;

public class RitualCancellationTrigger_CannotScarify : RitualCancellationTrigger
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass1_0
	{
		public RitualRoleAssignments assignments;

		public RitualCancellationTrigger_CannotScarify _003C_003E4__this;

		internal bool _003CCancellationTriggers_003Eb__0(TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && GenTicks.TicksGame % 60 == 0)
			{
				Pawn pawn = assignments.FirstAssignedPawn(_003C_003E4__this.roleId);
				if (pawn == null)
				{
					return true;
				}
				if (!JobDriver_Scarify.AvailableOnNow(pawn))
				{
					return true;
				}
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003CCancellationTriggers_003Ed__1 : IEnumerable<Trigger>, IEnumerable, IEnumerator<Trigger>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Trigger _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private RitualRoleAssignments assignments;

		public RitualRoleAssignments _003C_003E3__assignments;

		public RitualCancellationTrigger_CannotScarify _003C_003E4__this;

		Trigger IEnumerator<Trigger>.Current
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
		public _003CCancellationTriggers_003Ed__1(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Trigger_Custom(new _003C_003Ec__DisplayClass1_0
				{
					assignments = assignments,
					_003C_003E4__this = _003C_003E4__this
				}._003CCancellationTriggers_003Eb__0);
				_003C_003E1__state = 1;
				return true;
			case 1:
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
		IEnumerator<Trigger> IEnumerable<Trigger>.GetEnumerator()
		{
			_003CCancellationTriggers_003Ed__1 _003CCancellationTriggers_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCancellationTriggers_003Ed__ = this;
			}
			else
			{
				_003CCancellationTriggers_003Ed__ = new _003CCancellationTriggers_003Ed__1(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CCancellationTriggers_003Ed__.assignments = _003C_003E3__assignments;
			return _003CCancellationTriggers_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Trigger>)this).GetEnumerator();
		}
	}

	[NoTranslate]
	public string roleId;

	[IteratorStateMachine(typeof(_003CCancellationTriggers_003Ed__1))]
	public override IEnumerable<Trigger> CancellationTriggers(RitualRoleAssignments assignments)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCancellationTriggers_003Ed__1(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__assignments = assignments
		};
	}
}
