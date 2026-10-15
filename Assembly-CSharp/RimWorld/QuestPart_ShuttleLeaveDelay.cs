using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class QuestPart_ShuttleLeaveDelay : QuestPart_Delay
{
	[CompilerGenerated]
	private sealed class _003Cget_QuestLookTargets_003Ed__11 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_ShuttleLeaveDelay _003C_003E4__this;

		private IEnumerator<GlobalTargetInfo> _003C_003E7__wrap1;

		GlobalTargetInfo IEnumerator<GlobalTargetInfo>.Current
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
		public _003Cget_QuestLookTargets_003Ed__11(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				QuestPart_ShuttleLeaveDelay questPart_ShuttleLeaveDelay = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart_Delay)questPart_ShuttleLeaveDelay).QuestLookTargets.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						GlobalTargetInfo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (questPart_ShuttleLeaveDelay.shuttle != null)
					{
						_003C_003E2__current = questPart_ShuttleLeaveDelay.shuttle;
						_003C_003E1__state = 2;
						return true;
					}
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<GlobalTargetInfo> IEnumerable<GlobalTargetInfo>.GetEnumerator()
		{
			_003Cget_QuestLookTargets_003Ed__11 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_QuestLookTargets_003Ed__11(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<GlobalTargetInfo>)this).GetEnumerator();
		}
	}

	public Thing shuttle;

	public List<string> inSignalsDisable = new List<string>();

	public override AlertReport AlertReport
	{
		get
		{
			if (shuttle == null || !shuttle.Spawned)
			{
				return false;
			}
			return AlertReport.CulpritIs(shuttle);
		}
	}

	public override bool AlertCritical => base.TicksLeft < 60000;

	public override string AlertLabel => "QuestPartShuttleLeaveDelay".Translate(base.TicksLeft.ToStringTicksToPeriodVerbose());

	public override string AlertExplanation
	{
		get
		{
			if (quest.hidden)
			{
				return "QuestPartShuttleLeaveDelayDescHidden".Translate(base.TicksLeft.ToStringTicksToPeriodVerbose().Colorize(ColoredText.DateTimeColor));
			}
			return "QuestPartShuttleLeaveDelayDesc".Translate(quest.name, base.TicksLeft.ToStringTicksToPeriodVerbose().Colorize(ColoredText.DateTimeColor), shuttle.TryGetComp<CompShuttle>().RequiredThingsLabel);
		}
	}

	public override IEnumerable<GlobalTargetInfo> QuestLookTargets
	{
		[IteratorStateMachine(typeof(_003Cget_QuestLookTargets_003Ed__11))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_QuestLookTargets_003Ed__11(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override void Notify_QuestSignalReceived(Signal signal)
	{
		base.Notify_QuestSignalReceived(signal);
		if (base.State == QuestPartState.Enabled && inSignalsDisable.Contains(signal.tag))
		{
			Disable();
		}
	}

	public override string ExtraInspectString(ISelectable target)
	{
		if (target == shuttle)
		{
			return "ShuttleLeaveDelayInspectString".Translate(base.TicksLeft.ToStringTicksToPeriod());
		}
		return null;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref shuttle, "shuttle");
		Scribe_Collections.Look(ref inSignalsDisable, "inSignalsDisable", LookMode.Value);
		if (Scribe.mode == LoadSaveMode.PostLoadInit && inSignalsDisable == null)
		{
			inSignalsDisable = new List<string>();
		}
	}

	public override void ReplacePawnReferences(Pawn replace, Pawn with)
	{
		if (shuttle != null)
		{
			shuttle.TryGetComp<CompShuttle>().requiredPawns.Replace(replace, with);
		}
	}
}
