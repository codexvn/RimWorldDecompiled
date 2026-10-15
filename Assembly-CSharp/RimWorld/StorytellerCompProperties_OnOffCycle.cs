using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class StorytellerCompProperties_OnOffCycle : StorytellerCompProperties
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__15 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StorytellerCompProperties_OnOffCycle _003C_003E4__this;

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
		public _003CConfigErrors_003Ed__15(int _003C_003E1__state)
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
			StorytellerCompProperties_OnOffCycle storytellerCompProperties_OnOffCycle = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (storytellerCompProperties_OnOffCycle.incident != null && storytellerCompProperties_OnOffCycle.category != null)
				{
					_003C_003E2__current = "incident and category should not both be defined";
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_005c;
			case 1:
				_003C_003E1__state = -1;
				goto IL_005c;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0084;
			case 3:
				_003C_003E1__state = -1;
				goto IL_00b1;
			case 4:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0084:
				if (storytellerCompProperties_OnOffCycle.numIncidentsRange.TrueMax <= 0f)
				{
					_003C_003E2__current = "numIncidentRange not configured";
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_00b1;
				IL_005c:
				if (storytellerCompProperties_OnOffCycle.onDays <= 0f)
				{
					_003C_003E2__current = "onDays must be above zero";
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_0084;
				IL_00b1:
				if (storytellerCompProperties_OnOffCycle.minSpacingDays * storytellerCompProperties_OnOffCycle.numIncidentsRange.TrueMax > storytellerCompProperties_OnOffCycle.onDays * 0.9f)
				{
					_003C_003E2__current = "minSpacingDays too high compared to max number of incidents.";
					_003C_003E1__state = 4;
					return true;
				}
				break;
			}
			return false;
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
			_003CConfigErrors_003Ed__15 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__15(0)
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

	public float onDays;

	public float offDays;

	public float onDaysNoTreeConnectors;

	public float offDaysNoTreeConnectors;

	public float minSpacingDays;

	public FloatRange numIncidentsRange = FloatRange.Zero;

	public SimpleCurve acceptFractionByDaysPassedCurve;

	public SimpleCurve acceptPercentFactorPerThreatPointsCurve;

	public SimpleCurve acceptPercentFactorPerProgressScoreCurve;

	public IncidentDef incident;

	private IncidentCategoryDef category;

	public float forceRaidEnemyBeforeDaysPassed;

	public IncidentCategoryDef IncidentCategory
	{
		get
		{
			if (incident != null)
			{
				return incident.category;
			}
			return category;
		}
	}

	public StorytellerCompProperties_OnOffCycle()
	{
		compClass = typeof(StorytellerComp_OnOffCycle);
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__15))]
	public override IEnumerable<string> ConfigErrors(StorytellerDef parentDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__15(-2)
		{
			_003C_003E4__this = this
		};
	}
}
