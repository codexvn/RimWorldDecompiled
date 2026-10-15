using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class ScenPart_PursuingMechanoids : ScenPart
{
	[CompilerGenerated]
	private sealed class _003CGetAlerts_003Ed__34 : IEnumerable<Alert>, IEnumerable, IEnumerator<Alert>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Alert _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ScenPart_PursuingMechanoids _003C_003E4__this;

		Alert IEnumerator<Alert>.Current
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
		public _003CGetAlerts_003Ed__34(int _003C_003E1__state)
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
			ScenPart_PursuingMechanoids scenPart_PursuingMechanoids = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				Map currentMap = Find.CurrentMap;
				if (currentMap == null || !currentMap.IsPlayerHome)
				{
					return false;
				}
				if (scenPart_PursuingMechanoids.AlertCached != null)
				{
					_003C_003E2__current = scenPart_PursuingMechanoids.AlertCached;
					_003C_003E1__state = 1;
					return true;
				}
				break;
			}
			case 1:
				_003C_003E1__state = -1;
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
		IEnumerator<Alert> IEnumerable<Alert>.GetEnumerator()
		{
			_003CGetAlerts_003Ed__34 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetAlerts_003Ed__34(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Alert>)this).GetEnumerator();
		}
	}

	private bool onStartMap = true;

	private Dictionary<Map, int> mapWarningTimers = new Dictionary<Map, int>();

	private Dictionary<Map, int> mapRaidTimers = new Dictionary<Map, int>();

	private bool questCompleted;

	private const int InitialWarningDelay = 2700;

	private const int InitialRaidDelay = 30000;

	private static IntRange WarningDelayRange = new IntRange(840000, 960000);

	private static IntRange RaidDelayRange = new IntRange(1080000, 2100000);

	private const int TickInterval = 2500;

	private const int SecondRaidDelay = 30000;

	private Map cachedAlertMap;

	private Alert_MechThreat alertCached;

	private List<Map> tmpWarningKeys;

	private List<int> tmpWarningValues;

	private List<Map> tmpRaidKeys;

	private List<int> tmpRaidValues;

	private List<Map> tmpMaps = new List<Map>();

	private Alert_MechThreat AlertCached
	{
		get
		{
			if (Disabled)
			{
				return null;
			}
			if (cachedAlertMap != Find.CurrentMap)
			{
				alertCached = null;
			}
			if (alertCached != null && Find.TickManager.TicksGame > TimerIntervalTick(alertCached.raidTick + 30000))
			{
				alertCached = null;
			}
			if (alertCached != null)
			{
				return alertCached;
			}
			if (mapWarningTimers.TryGetValue(Find.CurrentMap, out var value) && Find.TickManager.TicksGame > TimerIntervalTick(value) && mapRaidTimers.TryGetValue(Find.CurrentMap, out var value2) && Find.TickManager.TicksGame < TimerIntervalTick(value2 + 30000))
			{
				alertCached = new Alert_MechThreat
				{
					raidTick = mapRaidTimers[Find.CurrentMap]
				};
				cachedAlertMap = Find.CurrentMap;
			}
			return alertCached;
		}
	}

	private bool Disabled => questCompleted;

	public override bool OverrideDangerMusic => onStartMap;

	public override void ExposeData()
	{
		base.ExposeData();
		if (Scribe.mode == LoadSaveMode.Saving)
		{
			foreach (Map item in mapWarningTimers.Keys.ToList())
			{
				if (item?.Parent == null || item.Parent.Destroyed)
				{
					mapWarningTimers.Remove(item);
					mapRaidTimers.Remove(item);
				}
			}
		}
		Scribe_Values.Look(ref onStartMap, "initialMap", defaultValue: false);
		Scribe_Collections.Look(ref mapWarningTimers, "mapWarningTimers", LookMode.Reference, LookMode.Value, ref tmpWarningKeys, ref tmpWarningValues);
		Scribe_Collections.Look(ref mapRaidTimers, "mapRaidTimers", LookMode.Reference, LookMode.Value, ref tmpRaidKeys, ref tmpRaidValues);
		Scribe_Values.Look(ref questCompleted, "questCompleted", defaultValue: false);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			if (mapWarningTimers == null)
			{
				mapWarningTimers = new Dictionary<Map, int>();
			}
			if (mapRaidTimers == null)
			{
				mapRaidTimers = new Dictionary<Map, int>();
			}
		}
	}

	public override void PostWorldGenerate()
	{
		onStartMap = true;
		mapWarningTimers.Clear();
		mapRaidTimers.Clear();
	}

	public override void PostMapGenerate(Map map)
	{
		if (onStartMap)
		{
			StartTimers(map);
			onStartMap = false;
		}
	}

	public override void PostGravshipLanded(Map map)
	{
		onStartMap = false;
		StartTimers(map);
	}

	public override void MapRemoved(Map map)
	{
		if (mapWarningTimers.Remove(map))
		{
			mapRaidTimers.Remove(map);
			onStartMap = false;
		}
	}

	private int TimerIntervalTick(int timer)
	{
		return (timer + 2500 - 1) / 2500 * 2500;
	}

	public override void Tick()
	{
		if (Find.TickManager.TicksGame % 2500 != 0)
		{
			return;
		}
		tmpMaps.Clear();
		tmpMaps.AddRange(mapWarningTimers.Keys);
		foreach (Map tmpMap in tmpMaps)
		{
			if (Disabled || GravshipUtility.GetPlayerGravEngine_NewTemp(tmpMap) == null)
			{
				mapWarningTimers.Remove(tmpMap);
				mapRaidTimers.Remove(tmpMap);
				continue;
			}
			if (Find.TickManager.TicksGame == TimerIntervalTick(mapWarningTimers[tmpMap]))
			{
				Thing thing = tmpMap.listerThings.ThingsOfDef(ThingDefOf.PilotConsole).FirstOrDefault();
				Find.LetterStack.ReceiveLetter("LetterLabelMechanoidThreat".Translate(), "LetterTextMechanoidThreat".Translate(), LetterDefOf.ThreatSmall, thing);
			}
			if (Find.TickManager.TicksGame == TimerIntervalTick(mapRaidTimers[tmpMap]))
			{
				FireRaid_NewTemp(tmpMap, 1.5f, 2000f);
			}
			if (Find.TickManager.TicksGame == TimerIntervalTick(mapRaidTimers[tmpMap] + 30000))
			{
				FireRaid_NewTemp(tmpMap, 2f, 8000f);
			}
		}
	}

	private void StartTimers(Map map)
	{
		if (map.generatorDef != MapGeneratorDefOf.Mechhive)
		{
			if (onStartMap)
			{
				mapWarningTimers[map] = Find.TickManager.TicksGame + 2700;
				mapRaidTimers[map] = Find.TickManager.TicksGame + 30000;
			}
			else
			{
				mapWarningTimers[map] = Find.TickManager.TicksGame + WarningDelayRange.RandomInRange;
				mapRaidTimers[map] = Find.TickManager.TicksGame + RaidDelayRange.RandomInRange;
			}
		}
	}

	public void Notify_QuestCompleted()
	{
		questCompleted = true;
	}

	private void FireRaid(Map map)
	{
		FireRaid_NewTemp(map, 1.5f, 5000f);
	}

	private void FireRaid_NewTemp(Map map, float pointsMultiplier, float minPoints)
	{
		IncidentParms incidentParms = new IncidentParms();
		incidentParms.forced = true;
		incidentParms.target = map;
		incidentParms.points = Mathf.Max(minPoints, StorytellerUtility.DefaultThreatPointsNow(map) * pointsMultiplier);
		incidentParms.faction = Faction.OfMechanoids;
		incidentParms.raidArrivalMode = PawnsArrivalModeDefOf.RandomDrop;
		incidentParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
		IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
	}

	[IteratorStateMachine(typeof(_003CGetAlerts_003Ed__34))]
	public override IEnumerable<Alert> GetAlerts()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetAlerts_003Ed__34(-2)
		{
			_003C_003E4__this = this
		};
	}
}
