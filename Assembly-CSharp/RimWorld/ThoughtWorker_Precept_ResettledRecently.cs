using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ThoughtWorker_Precept_ResettledRecently : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
{
	[CompilerGenerated]
	private sealed class _003CGetDescriptionArgs_003Ed__5 : IEnumerable<NamedArgument>, IEnumerable, IEnumerator<NamedArgument>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private NamedArgument _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThoughtWorker_Precept_ResettledRecently _003C_003E4__this;

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
		public _003CGetDescriptionArgs_003Ed__5(int _003C_003E1__state)
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
			ThoughtWorker_Precept_ResettledRecently thoughtWorker_Precept_ResettledRecently = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = thoughtWorker_Precept_ResettledRecently.TicksSinceLastResettle.ToStringTicksToPeriod().Named("DURATION");
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
			_003CGetDescriptionArgs_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetDescriptionArgs_003Ed__5(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<NamedArgument>)this).GetEnumerator();
		}
	}

	private static readonly List<(int day, int index)> Levels = new List<(int, int)>
	{
		(5, 0),
		(10, 1),
		(20, -1),
		(30, 2),
		(int.MaxValue, 3)
	};

	public int TicksSinceLastResettle => GenTicks.TicksGame - Find.IdeoManager.lastResettledTick;

	protected override ThoughtState ShouldHaveThought(Pawn p)
	{
		if (!ModsConfig.OdysseyActive)
		{
			return false;
		}
		if (!p.IsColonist)
		{
			return false;
		}
		return Find.TickManager.TicksGame > Find.IdeoManager.lastResettledTick;
	}

	protected override ThoughtState CurrentStateInternal(Pawn p)
	{
		if (!base.CurrentStateInternal(p).Active)
		{
			return ThoughtState.Inactive;
		}
		float num = TicksSinceLastResettle.TicksToDays();
		foreach (var (num2, num3) in Levels)
		{
			if (num < (float)num2)
			{
				return (num3 < 0) ? ThoughtState.Inactive : ThoughtState.ActiveAtStage(num3);
			}
		}
		return ThoughtState.Inactive;
	}

	[IteratorStateMachine(typeof(_003CGetDescriptionArgs_003Ed__5))]
	public IEnumerable<NamedArgument> GetDescriptionArgs()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetDescriptionArgs_003Ed__5(-2)
		{
			_003C_003E4__this = this
		};
	}
}
