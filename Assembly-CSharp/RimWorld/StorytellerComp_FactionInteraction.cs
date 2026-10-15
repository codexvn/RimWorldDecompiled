using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class StorytellerComp_FactionInteraction : StorytellerComp
{
	[CompilerGenerated]
	private sealed class _003CMakeIntervalIncidents_003Ed__2 : IEnumerable<FiringIncident>, IEnumerable, IEnumerator<FiringIncident>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FiringIncident _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IIncidentTarget target;

		public IIncidentTarget _003C_003E3__target;

		public StorytellerComp_FactionInteraction _003C_003E4__this;

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
			StorytellerComp_FactionInteraction storytellerComp_FactionInteraction = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0162;
			}
			_003C_003E1__state = -1;
			Map map = target as Map;
			if (storytellerComp_FactionInteraction.Props.minDanger != 0 && (map == null || (int)map.dangerWatcher.DangerRating < (int)storytellerComp_FactionInteraction.Props.minDanger))
			{
				return false;
			}
			if (storytellerComp_FactionInteraction.Props.minWealth > 0f && (map == null || map.wealthWatcher.WealthTotal < storytellerComp_FactionInteraction.Props.minWealth))
			{
				return false;
			}
			float num2 = StorytellerUtility.AllyIncidentFraction(storytellerComp_FactionInteraction.Props.fullAlliesOnly);
			if (num2 <= 0f)
			{
				return false;
			}
			_003CincCount_003E5__2 = IncidentCycleUtility.IncidentCountThisInterval(target, Find.Storyteller.storytellerComps.IndexOf(storytellerComp_FactionInteraction), storytellerComp_FactionInteraction.Props.minDaysPassed, 60f, 0f, storytellerComp_FactionInteraction.Props.minSpacingDays, storytellerComp_FactionInteraction.Props.baseIncidentsPerYear, storytellerComp_FactionInteraction.Props.baseIncidentsPerYear, num2);
			_003Ci_003E5__3 = 0;
			goto IL_0174;
			IL_0174:
			if (_003Ci_003E5__3 < _003CincCount_003E5__2)
			{
				IncidentParms parms = storytellerComp_FactionInteraction.GenerateParms(storytellerComp_FactionInteraction.Props.incident.category, target);
				if (storytellerComp_FactionInteraction.Props.incident.Worker.CanFireNow(parms))
				{
					_003C_003E2__current = new FiringIncident(storytellerComp_FactionInteraction.Props.incident, storytellerComp_FactionInteraction, parms);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0162;
			}
			return false;
			IL_0162:
			_003Ci_003E5__3++;
			goto IL_0174;
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

	private StorytellerCompProperties_FactionInteraction Props => (StorytellerCompProperties_FactionInteraction)props;

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

	public override string ToString()
	{
		return base.ToString() + " (" + Props.incident.defName + ")";
	}
}
