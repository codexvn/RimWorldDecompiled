using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class StorytellerComp_OnOffCycle : StorytellerComp
{
	[CompilerGenerated]
	private sealed class _003CMakeIntervalIncidents_003Ed__2 : IEnumerable<FiringIncident>, IEnumerable, IEnumerator<FiringIncident>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FiringIncident _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StorytellerComp_OnOffCycle _003C_003E4__this;

		private IIncidentTarget target;

		public IIncidentTarget _003C_003E3__target;

		private int _003CincCount_003E5__2;

		private int _003Ci_003E5__3;

		FiringIncident IEnumerator<FiringIncident>.Current
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
		public _003CMakeIntervalIncidents_003Ed__2(int _003C_003E1__state)
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
			StorytellerComp_OnOffCycle storytellerComp_OnOffCycle = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_020a;
			}
			_003C_003E1__state = -1;
			float num2 = 1f;
			if (storytellerComp_OnOffCycle.Props.acceptFractionByDaysPassedCurve != null)
			{
				num2 *= storytellerComp_OnOffCycle.Props.acceptFractionByDaysPassedCurve.Evaluate(GenDate.DaysPassedSinceSettleFloat);
			}
			if (storytellerComp_OnOffCycle.Props.acceptPercentFactorPerThreatPointsCurve != null)
			{
				num2 *= storytellerComp_OnOffCycle.Props.acceptPercentFactorPerThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(target));
			}
			if (storytellerComp_OnOffCycle.Props.acceptPercentFactorPerProgressScoreCurve != null)
			{
				num2 *= storytellerComp_OnOffCycle.Props.acceptPercentFactorPerProgressScoreCurve.Evaluate(StorytellerUtility.GetProgressScore(target));
			}
			float onDays = storytellerComp_OnOffCycle.Props.onDays;
			float offDays = storytellerComp_OnOffCycle.Props.offDays;
			if ((storytellerComp_OnOffCycle.Props.onDaysNoTreeConnectors != 0f || storytellerComp_OnOffCycle.Props.offDaysNoTreeConnectors != 0f) && Faction.OfPlayer.ideos != null)
			{
				bool flag = false;
				foreach (Ideo allIdeo in Faction.OfPlayer.ideos.AllIdeos)
				{
					if (allIdeo.HasMeme(MemeDefOf.TreeConnection))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					if (storytellerComp_OnOffCycle.Props.onDaysNoTreeConnectors != 0f)
					{
						onDays = storytellerComp_OnOffCycle.Props.onDaysNoTreeConnectors;
					}
					if (storytellerComp_OnOffCycle.Props.offDaysNoTreeConnectors != 0f)
					{
						offDays = storytellerComp_OnOffCycle.Props.offDaysNoTreeConnectors;
					}
				}
			}
			_003CincCount_003E5__2 = IncidentCycleUtility.IncidentCountThisInterval(target, Find.Storyteller.storytellerComps.IndexOf(storytellerComp_OnOffCycle), storytellerComp_OnOffCycle.Props.minDaysPassed, onDays, offDays, storytellerComp_OnOffCycle.Props.minSpacingDays, storytellerComp_OnOffCycle.Props.numIncidentsRange.min, storytellerComp_OnOffCycle.Props.numIncidentsRange.max, num2);
			_003Ci_003E5__3 = 0;
			goto IL_021c;
			IL_021c:
			if (_003Ci_003E5__3 < _003CincCount_003E5__2)
			{
				FiringIncident firingIncident = storytellerComp_OnOffCycle.GenerateIncident(target);
				if (firingIncident != null)
				{
					_003C_003E2__current = firingIncident;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_020a;
			}
			return false;
			IL_020a:
			_003Ci_003E5__3++;
			goto IL_021c;
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
		IEnumerator<FiringIncident> IEnumerable<FiringIncident>.GetEnumerator()
		{
			_003CMakeIntervalIncidents_003Ed__2 _003CMakeIntervalIncidents_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CMakeIntervalIncidents_003Ed__ = this;
			}
			else
			{
				_003CMakeIntervalIncidents_003Ed__ = new _003CMakeIntervalIncidents_003Ed__2(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CMakeIntervalIncidents_003Ed__.target = _003C_003E3__target;
			return _003CMakeIntervalIncidents_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FiringIncident>)this).GetEnumerator();
		}
	}

	protected StorytellerCompProperties_OnOffCycle Props => (StorytellerCompProperties_OnOffCycle)props;

	[IteratorStateMachine(typeof(_003CMakeIntervalIncidents_003Ed__2))]
	public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeIntervalIncidents_003Ed__2(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__target = target
		};
	}

	private FiringIncident GenerateIncident(IIncidentTarget target)
	{
		if (Props.IncidentCategory == null)
		{
			return null;
		}
		IncidentParms parms = GenerateParms(Props.IncidentCategory, target);
		IncidentDef result;
		if ((float)GenDate.DaysPassedSinceSettle < Props.forceRaidEnemyBeforeDaysPassed)
		{
			if (!IncidentDefOf.RaidEnemy.Worker.CanFireNow(parms))
			{
				return null;
			}
			result = IncidentDefOf.RaidEnemy;
		}
		else if (Props.incident != null)
		{
			if (!Props.incident.Worker.CanFireNow(parms))
			{
				return null;
			}
			result = Props.incident;
		}
		else if (!UsableIncidentsInCategory(Props.IncidentCategory, parms).TryRandomElementByWeight((IncidentDef def) => IncidentChanceFinal(def, target), out result))
		{
			return null;
		}
		return new FiringIncident(result, this, parms);
	}

	public override string ToString()
	{
		if (Props.incident == null && Props.IncidentCategory == null)
		{
			return "";
		}
		return base.ToString() + " (" + ((Props.incident != null) ? Props.incident.defName : Props.IncidentCategory.defName) + ")";
	}
}
