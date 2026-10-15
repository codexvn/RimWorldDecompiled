using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ThoughtWorker_Precept_RecentConquest : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
{
	[CompilerGenerated]
	private sealed class _003CGetDescriptionArgs_003Ed__6 : IEnumerable<NamedArgument>, IEnumerable, IEnumerator<NamedArgument>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private NamedArgument _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThoughtWorker_Precept_RecentConquest _003C_003E4__this;

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
			int num = _003C_003E1__state;
			ThoughtWorker_Precept_RecentConquest thoughtWorker_Precept_RecentConquest = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = thoughtWorker_Precept_RecentConquest.DaysSinceLastRaidThreshold.Named("DAYSSINCELASTRAIDTHRESHOLD");
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
			_003CGetDescriptionArgs_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetDescriptionArgs_003Ed__6(0)
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

	private static readonly SimpleCurve MoodMultiplierCurve = new SimpleCurve
	{
		new CurvePoint(0f, 1f),
		new CurvePoint(25f, 0f),
		new CurvePoint(50f, 1f),
		new CurvePoint(75f, 1.5f)
	};

	private int DaysSinceLastRaidThreshold = 25;

	private float DaysSinceLastRaid => (float)(Find.TickManager.TicksGame - Find.History.lastTickPlayerRaidedSomeone) / 60000f;

	protected override ThoughtState ShouldHaveThought(Pawn p)
	{
		if (!p.IsColonist)
		{
			return false;
		}
		if (p.IsSlave)
		{
			return false;
		}
		int num = ((DaysSinceLastRaid > (float)DaysSinceLastRaidThreshold) ? 1 : 0);
		if (num == 1)
		{
			int num2 = Find.TickManager.SettleTick;
			if (p.Ideo.Fluid && p.Ideo.development.lastTickRaidingApproved > num2)
			{
				num2 = p.Ideo.development.lastTickRaidingApproved;
			}
			if ((float)(Find.TickManager.TicksGame - num2) < 1800000f)
			{
				return false;
			}
		}
		return ThoughtState.ActiveAtStage(num);
	}

	public override float MoodMultiplier(Pawn p)
	{
		return MoodMultiplierCurve.Evaluate(DaysSinceLastRaid);
	}

	[IteratorStateMachine(typeof(_003CGetDescriptionArgs_003Ed__6))]
	public IEnumerable<NamedArgument> GetDescriptionArgs()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetDescriptionArgs_003Ed__6(-2)
		{
			_003C_003E4__this = this
		};
	}
}
