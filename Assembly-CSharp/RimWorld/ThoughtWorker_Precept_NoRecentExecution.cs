using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class ThoughtWorker_Precept_NoRecentExecution : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
{
	[CompilerGenerated]
	private sealed class _003CGetDescriptionArgs_003Ed__2 : IEnumerable<NamedArgument>, IEnumerable, IEnumerator<NamedArgument>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private NamedArgument _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		NamedArgument IEnumerator<NamedArgument>.Current
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
		public _003CGetDescriptionArgs_003Ed__2(int _003C_003E1__state)
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
				_003C_003E2__current = 30.Named("MINDAYSLASTEXECUTION");
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
		IEnumerator<NamedArgument> IEnumerable<NamedArgument>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CGetDescriptionArgs_003Ed__2(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<NamedArgument>)this).GetEnumerator();
		}
	}

	private const int MinDaysSinceLastExecutionForThought = 30;

	protected override ThoughtState ShouldHaveThought(Pawn p)
	{
		if (!p.IsColonist || p.IsSlave)
		{
			return false;
		}
		int num = Mathf.Max(0, p.Faction.lastExecutionTick);
		return Find.TickManager.TicksGame - num > 1800000;
	}

	[IteratorStateMachine(typeof(_003CGetDescriptionArgs_003Ed__2))]
	public IEnumerable<NamedArgument> GetDescriptionArgs()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetDescriptionArgs_003Ed__2(-2);
	}
}
