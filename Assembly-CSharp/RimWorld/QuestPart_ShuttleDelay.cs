using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class QuestPart_ShuttleDelay : QuestPart_Delay
{
	[CompilerGenerated]
	private sealed class _003Cget_QuestLookTargets_003Ed__3 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_ShuttleDelay _003C_003E4__this;

		private IEnumerator<GlobalTargetInfo> _003C_003E7__wrap1;

		private int _003Ci_003E5__3;

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
		public _003Cget_QuestLookTargets_003Ed__3(int _003C_003E1__state)
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
				QuestPart_ShuttleDelay questPart_ShuttleDelay = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart_Delay)questPart_ShuttleDelay).QuestLookTargets.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						_003Ci_003E5__3++;
						break;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						GlobalTargetInfo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003Ci_003E5__3 = 0;
					break;
				}
				if (_003Ci_003E5__3 < questPart_ShuttleDelay.lodgers.Count)
				{
					_003C_003E2__current = questPart_ShuttleDelay.lodgers[_003Ci_003E5__3];
					_003C_003E1__state = 2;
					return true;
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
			_003Cget_QuestLookTargets_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_QuestLookTargets_003Ed__3(0)
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

	public List<Pawn> lodgers = new List<Pawn>();

	public bool alert;

	public override IEnumerable<GlobalTargetInfo> QuestLookTargets
	{
		[IteratorStateMachine(typeof(_003Cget_QuestLookTargets_003Ed__3))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_QuestLookTargets_003Ed__3(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override AlertReport AlertReport
	{
		get
		{
			if (!alert || base.State != QuestPartState.Enabled)
			{
				return false;
			}
			return AlertReport.CulpritsAre(lodgers);
		}
	}

	public override bool AlertCritical => base.TicksLeft < 60000;

	public override string AlertLabel => "QuestPartShuttleArriveDelay".Translate(base.TicksLeft.ToStringTicksToPeriodVerbose());

	public override string AlertExplanation
	{
		get
		{
			if (quest.hidden)
			{
				return "QuestPartShuttleArriveDelayDescHidden".Translate(base.TicksLeft.ToStringTicksToPeriodVerbose().Colorize(ColoredText.DateTimeColor));
			}
			return "QuestPartShuttleArriveDelayDesc".Translate(quest.name, base.TicksLeft.ToStringTicksToPeriodVerbose().Colorize(ColoredText.DateTimeColor), lodgers.Select((Pawn p) => p.LabelShort).ToLineList("- "));
		}
	}

	public override string ExtraInspectString(ISelectable target)
	{
		if (target is Pawn item && lodgers.Contains(item))
		{
			return "ShuttleDelayInspectString".Translate(base.TicksLeft.ToStringTicksToPeriod());
		}
		return null;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Collections.Look(ref lodgers, "lodgers", LookMode.Reference);
		Scribe_Values.Look(ref alert, "alert", defaultValue: false);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			lodgers.RemoveAll((Pawn x) => x == null);
		}
	}

	public override void AssignDebugData()
	{
		base.AssignDebugData();
		if (Find.AnyPlayerHomeMap != null)
		{
			lodgers.AddRange(Find.RandomPlayerHomeMap.mapPawns.FreeColonists);
		}
	}

	public override void ReplacePawnReferences(Pawn replace, Pawn with)
	{
		lodgers.Replace(replace, with);
	}
}
