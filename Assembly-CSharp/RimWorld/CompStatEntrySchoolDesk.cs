using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld;

public class CompStatEntrySchoolDesk : CompStatEntry
{
	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__2 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompStatEntrySchoolDesk _003C_003E4__this;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CSpecialDisplayStats_003Ed__2(int _003C_003E1__state)
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
			CompStatEntrySchoolDesk compStatEntrySchoolDesk = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				float f = LearningUtility.SchoolDeskLearningRate(compStatEntrySchoolDesk.parent);
				int num2 = LearningUtility.ConnectedBlackboards(compStatEntrySchoolDesk.parent);
				StringBuilder stringBuilder = new StringBuilder(compStatEntrySchoolDesk.Props.reportText);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("StatsReport_BaseValue".Translate() + ": " + 1f.ToStringPercent());
				stringBuilder.AppendLine();
				if (num2 > 0)
				{
					TaggedString taggedString = "StatsReport_Connected".Translate(Find.ActiveLanguageWorker.Pluralize(ThingDefOf.Blackboard.label));
					taggedString += " " + num2 + " / " + 3 + ": x" + (1f + (float)num2 * 0.2f).ToStringPercent();
					stringBuilder.AppendLine(taggedString);
				}
				stringBuilder.AppendLine("StatsReport_FinalValue".Translate() + ": " + f.ToStringPercent());
				_003C_003E2__current = new StatDrawEntry(compStatEntrySchoolDesk.Props.statCategoryDef, compStatEntrySchoolDesk.Props.statLabel, f.ToStringPercent(), stringBuilder.ToString(), compStatEntrySchoolDesk.Props.displayPriorityInCategory);
				_003C_003E1__state = 1;
				return true;
			}
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__2 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__2(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	private CompProperties_StatEntry Props => (CompProperties_StatEntry)props;

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__2))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__2(-2)
		{
			_003C_003E4__this = this
		};
	}
}
