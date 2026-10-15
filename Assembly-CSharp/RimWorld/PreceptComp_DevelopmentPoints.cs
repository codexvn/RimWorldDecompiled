using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class PreceptComp_DevelopmentPoints : PreceptComp
{
	[CompilerGenerated]
	private sealed class _003CGetDescriptions_003Ed__8 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PreceptComp_DevelopmentPoints _003C_003E4__this;

		string IEnumerator<string>.Current
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
		public _003CGetDescriptions_003Ed__8(int _003C_003E1__state)
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
			PreceptComp_DevelopmentPoints preceptComp_DevelopmentPoints = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = preceptComp_DevelopmentPoints.LabelCap + ": " + preceptComp_DevelopmentPoints.points.ToStringWithSign();
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CGetDescriptions_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetDescriptions_003Ed__8(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public HistoryEventDef eventDef;

	public int points;

	[MustTranslate]
	public string eventLabel;

	private string Label
	{
		get
		{
			if (eventLabel.NullOrEmpty())
			{
				return eventDef.label;
			}
			return eventLabel.Formatted().Resolve();
		}
	}

	private string LabelCap => (!eventLabel.NullOrEmpty()) ? eventLabel.Formatted().CapitalizeFirst() : eventDef.LabelCap;

	public override void Notify_HistoryEvent(HistoryEvent ev, Precept precept)
	{
		if (ev.def != eventDef || precept.ideo != Faction.OfPlayer.ideos.FluidIdeo || !precept.ideo.development.CanBeDevelopedNow)
		{
			return;
		}
		ev.args.TryGetArg(HistoryEventArgsNames.Ideo, out Ideo arg);
		if (arg == null || arg == precept.ideo)
		{
			int num = precept.ideo.development.Points;
			if (precept.ideo.development.TryAddDevelopmentPoints(points))
			{
				ev.args.TryGetArg(HistoryEventArgsNames.Doer, out Pawn arg2);
				ev.args.TryGetArg(HistoryEventArgsNames.Quest, out Quest arg3);
				Messages.Message("MessageDevelopmentPointsEarned".Translate(num, precept.ideo.development.Points, Label), arg2, MessageTypeDefOf.PositiveEvent, arg3);
			}
		}
	}

	[IteratorStateMachine(typeof(_003CGetDescriptions_003Ed__8))]
	public override IEnumerable<string> GetDescriptions()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetDescriptions_003Ed__8(-2)
		{
			_003C_003E4__this = this
		};
	}
}
