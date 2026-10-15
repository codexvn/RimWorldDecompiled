using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ThoughtWorker_Precept_HighLife : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
{
	[CompilerGenerated]
	private sealed class _003CGetDescriptionArgs_003Ed__6 : IEnumerable<NamedArgument>, IEnumerable, IEnumerator<NamedArgument>, IEnumerator, IDisposable
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
		public _003CGetDescriptionArgs_003Ed__6(int _003C_003E1__state)
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
				_003C_003E2__current = 0.75f.ToString("F2").Named("DAYSSATISIFED");
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
			return new _003CGetDescriptionArgs_003Ed__6(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<NamedArgument>)this).GetEnumerator();
		}
	}

	private const float DaysSatisfied = 0.75f;

	private const float DaysNoBonus = 1f;

	private const float DaysMissing = 2f;

	private const float DaysMissing_Major = 11f;

	public static readonly SimpleCurve MoodOffsetFromDaysSinceLastDrugCurve = new SimpleCurve
	{
		new CurvePoint(0.75f, 3f),
		new CurvePoint(1f, 0f),
		new CurvePoint(2f, -1f),
		new CurvePoint(11f, -10f)
	};

	protected override ThoughtState ShouldHaveThought(Pawn p)
	{
		if (!ThoughtUtility.ThoughtNullified(p, def))
		{
			float num = (float)(Find.TickManager.TicksGame - p.mindState.lastTakeRecreationalDrugTick) / 60000f;
			if (num > 1f && def.minExpectationForNegativeThought != null && p.MapHeld != null && ExpectationsUtility.CurrentExpectationFor(p.MapHeld).order < def.minExpectationForNegativeThought.order)
			{
				return false;
			}
			if (num < 1f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (num < 2f)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (num < 11f)
			{
				return ThoughtState.ActiveAtStage(2);
			}
			return ThoughtState.ActiveAtStage(3);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetDescriptionArgs_003Ed__6))]
	public IEnumerable<NamedArgument> GetDescriptionArgs()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetDescriptionArgs_003Ed__6(-2);
	}
}
