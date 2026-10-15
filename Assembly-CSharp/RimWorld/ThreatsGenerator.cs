using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public static class ThreatsGenerator
{
	[CompilerGenerated]
	private sealed class _003CGetPossibleIncidents_003Ed__5 : IEnumerable<IncidentDef>, IEnumerable, IEnumerator<IncidentDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IncidentDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private AllowedThreatsGeneratorThreats allowedThreats;

		public AllowedThreatsGeneratorThreats _003C_003E3__allowedThreats;

		IncidentDef IEnumerator<IncidentDef>.Current
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
		public _003CGetPossibleIncidents_003Ed__5(int _003C_003E1__state)
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
				if ((allowedThreats & AllowedThreatsGeneratorThreats.Raids) != 0)
				{
					_003C_003E2__current = IncidentDefOf.RaidEnemy;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0047;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0047;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0047:
				if ((allowedThreats & AllowedThreatsGeneratorThreats.MechClusters) != 0)
				{
					_003C_003E2__current = IncidentDefOf.MechCluster;
					_003C_003E1__state = 2;
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
		IEnumerator<IncidentDef> IEnumerable<IncidentDef>.GetEnumerator()
		{
			_003CGetPossibleIncidents_003Ed__5 _003CGetPossibleIncidents_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetPossibleIncidents_003Ed__ = this;
			}
			else
			{
				_003CGetPossibleIncidents_003Ed__ = new _003CGetPossibleIncidents_003Ed__5(0);
			}
			_003CGetPossibleIncidents_003Ed__.allowedThreats = _003C_003E3__allowedThreats;
			return _003CGetPossibleIncidents_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IncidentDef>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeIntervalIncidents_003Ed__1 : IEnumerable<FiringIncident>, IEnumerable, IEnumerator<FiringIncident>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FiringIncident _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IIncidentTarget target;

		public IIncidentTarget _003C_003E3__target;

		private ThreatsGeneratorParams parms;

		public ThreatsGeneratorParams _003C_003E3__parms;

		private int startTick;

		public int _003C_003E3__startTick;

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
		public _003CMakeIntervalIncidents_003Ed__1(int _003C_003E1__state)
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
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00e1;
			}
			_003C_003E1__state = -1;
			float num2 = ThreatScaleToCountFactorCurve.Evaluate(Find.Storyteller.difficulty.threatScale);
			_003CincCount_003E5__2 = IncidentCycleUtility.IncidentCountThisInterval(target, parms.randSeed, (float)GenDate.TickGameToSettled(startTick) / 60000f, parms.onDays, parms.offDays, parms.minSpacingDays, parms.numIncidentsRange.min * num2, parms.numIncidentsRange.max * num2);
			_003Ci_003E5__3 = 0;
			goto IL_00f1;
			IL_00e1:
			_003Ci_003E5__3++;
			goto IL_00f1;
			IL_00f1:
			if (_003Ci_003E5__3 < _003CincCount_003E5__2)
			{
				FiringIncident firingIncident = MakeThreat(parms, target);
				if (firingIncident != null)
				{
					_003C_003E2__current = firingIncident;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00e1;
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
		IEnumerator<FiringIncident> IEnumerable<FiringIncident>.GetEnumerator()
		{
			_003CMakeIntervalIncidents_003Ed__1 _003CMakeIntervalIncidents_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CMakeIntervalIncidents_003Ed__ = this;
			}
			else
			{
				_003CMakeIntervalIncidents_003Ed__ = new _003CMakeIntervalIncidents_003Ed__1(0);
			}
			_003CMakeIntervalIncidents_003Ed__.parms = _003C_003E3__parms;
			_003CMakeIntervalIncidents_003Ed__.target = _003C_003E3__target;
			_003CMakeIntervalIncidents_003Ed__.startTick = _003C_003E3__startTick;
			return _003CMakeIntervalIncidents_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FiringIncident>)this).GetEnumerator();
		}
	}

	private static readonly SimpleCurve ThreatScaleToCountFactorCurve = new SimpleCurve
	{
		new CurvePoint(0f, 0.1f),
		new CurvePoint(0.3f, 0.5f),
		new CurvePoint(0.6f, 0.8f),
		new CurvePoint(1f, 1f),
		new CurvePoint(1.55f, 1.1f),
		new CurvePoint(2.2f, 1.2f),
		new CurvePoint(10f, 2f)
	};

	[IteratorStateMachine(typeof(_003CMakeIntervalIncidents_003Ed__1))]
	public static IEnumerable<FiringIncident> MakeIntervalIncidents(ThreatsGeneratorParams parms, IIncidentTarget target, int startTick)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeIntervalIncidents_003Ed__1(-2)
		{
			_003C_003E3__parms = parms,
			_003C_003E3__target = target,
			_003C_003E3__startTick = startTick
		};
	}

	private static FiringIncident MakeThreat(ThreatsGeneratorParams parms, IIncidentTarget target)
	{
		IncidentParms incParms = GetIncidentParms(parms, target);
		if (!(from x in GetPossibleIncidents(parms.allowedThreats)
			where x.Worker.CanFireNow(incParms)
			select x).TryRandomElementByWeight((IncidentDef x) => x.Worker.BaseChanceThisGame, out var result))
		{
			return null;
		}
		return new FiringIncident
		{
			def = result,
			parms = incParms
		};
	}

	public static bool AnyIncidentPossible(ThreatsGeneratorParams parms, IIncidentTarget target)
	{
		IncidentParms incParms = GetIncidentParms(parms, target);
		return GetPossibleIncidents(parms.allowedThreats).Any((IncidentDef x) => x.Worker.BaseChanceThisGame > 0f && x.Worker.CanFireNow(incParms));
	}

	private static IncidentParms GetIncidentParms(ThreatsGeneratorParams parms, IIncidentTarget target)
	{
		IncidentParms incidentParms = new IncidentParms
		{
			target = target,
			points = (parms.threatPoints ?? (StorytellerUtility.DefaultThreatPointsNow(target) * parms.currentThreatPointsFactor))
		};
		if (parms.minThreatPoints.HasValue)
		{
			incidentParms.points = Mathf.Max(incidentParms.points, parms.minThreatPoints.Value);
		}
		incidentParms.faction = parms.faction;
		incidentParms.forced = true;
		return incidentParms;
	}

	[IteratorStateMachine(typeof(_003CGetPossibleIncidents_003Ed__5))]
	private static IEnumerable<IncidentDef> GetPossibleIncidents(AllowedThreatsGeneratorThreats allowedThreats)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetPossibleIncidents_003Ed__5(-2)
		{
			_003C_003E3__allowedThreats = allowedThreats
		};
	}
}
