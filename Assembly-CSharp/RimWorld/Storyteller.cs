using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Storyteller : IExposable
{
	[CompilerGenerated]
	private sealed class _003CMakeIncidentsForInterval_003Ed__48 : IEnumerable<FiringIncident>, IEnumerable, IEnumerator<FiringIncident>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FiringIncident _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Storyteller _003C_003E4__this;

		private List<IIncidentTarget> _003Ctargets_003E5__2;

		private List<Quest> _003Cquests_003E5__3;

		private int _003Ci_003E5__4;

		private IEnumerator<FiringIncident> _003C_003E7__wrap4;

		private List<QuestPart> _003Cparts_003E5__6;

		private int _003Cj_003E5__7;

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
		public _003CMakeIncidentsForInterval_003Ed__48(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003Ctargets_003E5__2 = null;
			_003Cquests_003E5__3 = null;
			_003C_003E7__wrap4 = null;
			_003Cparts_003E5__6 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Storyteller storyteller = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003Ctargets_003E5__2 = storyteller.AllIncidentTargets;
					_003Ci_003E5__4 = 0;
					goto IL_00cd;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00a1;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_01ff;
					}
					IL_00cd:
					if (_003Ci_003E5__4 < storyteller.storytellerComps.Count)
					{
						_003C_003E7__wrap4 = storyteller.MakeIncidentsForInterval(storyteller.storytellerComps[_003Ci_003E5__4], _003Ctargets_003E5__2).GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_00a1;
					}
					_003Cquests_003E5__3 = Find.QuestManager.QuestsListForReading;
					_003Ci_003E5__4 = 0;
					goto IL_025a;
					IL_0219:
					_003Cj_003E5__7++;
					goto IL_022b;
					IL_025a:
					if (_003Ci_003E5__4 < _003Cquests_003E5__3.Count)
					{
						if (_003Cquests_003E5__3[_003Ci_003E5__4].State == QuestState.Ongoing)
						{
							_003Cparts_003E5__6 = _003Cquests_003E5__3[_003Ci_003E5__4].PartsListForReading;
							_003Cj_003E5__7 = 0;
							goto IL_022b;
						}
						goto IL_0248;
					}
					return false;
					IL_0248:
					_003Ci_003E5__4++;
					goto IL_025a;
					IL_01ff:
					if (_003C_003E7__wrap4.MoveNext())
					{
						FiringIncident current = _003C_003E7__wrap4.Current;
						current.sourceQuestPart = _003Cparts_003E5__6[_003Cj_003E5__7];
						current.parms.quest = _003Cquests_003E5__3[_003Ci_003E5__4];
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap4 = null;
					goto IL_0219;
					IL_00a1:
					if (_003C_003E7__wrap4.MoveNext())
					{
						FiringIncident current2 = _003C_003E7__wrap4.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap4 = null;
					_003Ci_003E5__4++;
					goto IL_00cd;
					IL_022b:
					if (_003Cj_003E5__7 < _003Cparts_003E5__6.Count)
					{
						if (_003Cparts_003E5__6[_003Cj_003E5__7] is IIncidentMakerQuestPart incidentMakerQuestPart && ((QuestPartActivable)_003Cparts_003E5__6[_003Cj_003E5__7]).State == QuestPartState.Enabled)
						{
							_003C_003E7__wrap4 = incidentMakerQuestPart.MakeIntervalIncidents().GetEnumerator();
							_003C_003E1__state = -4;
							goto IL_01ff;
						}
						goto IL_0219;
					}
					_003Cparts_003E5__6 = null;
					goto IL_0248;
				}
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
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<FiringIncident> IEnumerable<FiringIncident>.GetEnumerator()
		{
			_003CMakeIncidentsForInterval_003Ed__48 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeIncidentsForInterval_003Ed__48(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FiringIncident>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeIncidentsForInterval_003Ed__49 : IEnumerable<FiringIncident>, IEnumerable, IEnumerator<FiringIncident>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FiringIncident _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private StorytellerComp comp;

		public StorytellerComp _003C_003E3__comp;

		private List<IIncidentTarget> targets;

		public List<IIncidentTarget> _003C_003E3__targets;

		private int _003Ci_003E5__2;

		private IEnumerator<FiringIncident> _003C_003E7__wrap2;

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
		public _003CMakeIncidentsForInterval_003Ed__49(int _003C_003E1__state)
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
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					goto IL_01aa;
				}
				_003C_003E1__state = -1;
				if (GenDate.DaysPassedSinceSettleFloat <= comp.props.minDaysPassed)
				{
					return false;
				}
				_003Ci_003E5__2 = 0;
				goto IL_01d9;
				IL_01aa:
				while (_003C_003E7__wrap2.MoveNext())
				{
					FiringIncident current = _003C_003E7__wrap2.Current;
					if ((Find.Storyteller.difficulty.allowBigThreats || current.def.category != IncidentCategoryDefOf.ThreatBig) && (!ModsConfig.AnomalyActive || Find.TickManager.TicksGame - Find.Anomaly.metalHellClosedTick >= 300000 || current.def.category != IncidentCategoryDefOf.ThreatBig))
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap2 = null;
				goto IL_01c7;
				IL_01c7:
				_003Ci_003E5__2++;
				goto IL_01d9;
				IL_01d9:
				if (_003Ci_003E5__2 < targets.Count)
				{
					IIncidentTarget incidentTarget = targets[_003Ci_003E5__2];
					bool flag = false;
					bool flag2 = comp.props.allowedTargetTags.NullOrEmpty();
					foreach (IncidentTargetTagDef item in incidentTarget.IncidentTargetTags())
					{
						if (!comp.props.disallowedTargetTags.NullOrEmpty() && comp.props.disallowedTargetTags.Contains(item))
						{
							flag = true;
							break;
						}
						if (!flag2 && comp.props.allowedTargetTags.Contains(item))
						{
							flag2 = true;
						}
					}
					if (!flag && flag2)
					{
						_003C_003E7__wrap2 = comp.MakeIntervalIncidents(incidentTarget).GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_01aa;
					}
					goto IL_01c7;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<FiringIncident> IEnumerable<FiringIncident>.GetEnumerator()
		{
			_003CMakeIncidentsForInterval_003Ed__49 _003CMakeIncidentsForInterval_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CMakeIncidentsForInterval_003Ed__ = this;
			}
			else
			{
				_003CMakeIncidentsForInterval_003Ed__ = new _003CMakeIncidentsForInterval_003Ed__49(0);
			}
			_003CMakeIncidentsForInterval_003Ed__.comp = _003C_003E3__comp;
			_003CMakeIncidentsForInterval_003Ed__.targets = _003C_003E3__targets;
			return _003CMakeIncidentsForInterval_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FiringIncident>)this).GetEnumerator();
		}
	}

	public StorytellerDef def;

	public DifficultyDef difficultyDef;

	public Difficulty difficulty = new Difficulty();

	public List<StorytellerComp> storytellerComps;

	private static List<IncidentDef> anomalyIncidents;

	private static List<IncidentDef> nonAnomalyIncidents;

	public IncidentQueue incidentQueue = new IncidentQueue();

	private Queue<bool> recentIncidentsAnomaly = new Queue<bool>();

	private float recentAnomalyIncidentFactor = 0.5f;

	private int lastQuestGiverTraderTick;

	private int lastIncidentTick;

	private float anomalyThreatsInactiveFraction = 0.08f;

	private float anomalyThreatsActiveFraction = 0.3f;

	private float? overrideAnomalyThreatsFraction;

	private float studyEfficiencyFactor = 1f;

	private AnomalyPlaystyleDef anomalyPlaystyleDef;

	public static readonly Vector2 PortraitSizeTiny = new Vector2(122f, 130f);

	public static readonly Vector2 PortraitSizeLarge = new Vector2(580f, 620f);

	public const int IntervalsPerDay = 60;

	public const int CheckInterval = 1000;

	private const int RecentAnomalyIncidentsToRecord = 4;

	private const float QuestGiverTraderChance = 0.15f;

	private const int QuestGiverTraderMinIntervalTicks = 600000;

	private const int QuestGiverTraderMaxIntervalTicks = 3600000;

	private static readonly List<IIncidentTarget> tmpAllIncidentTargets = new List<IIncidentTarget>();

	private string debugStringCached = "Generating data...";

	[Obsolete("Use \"difficulty\" instead.")]
	public Difficulty difficultyValues => difficulty;

	public int LastIncidentTick => lastIncidentTick;

	public List<IIncidentTarget> AllIncidentTargets
	{
		get
		{
			tmpAllIncidentTargets.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				tmpAllIncidentTargets.Add(maps[i]);
			}
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int j = 0; j < caravans.Count; j++)
			{
				if (caravans[j].IsPlayerControlled)
				{
					tmpAllIncidentTargets.Add(caravans[j]);
				}
			}
			tmpAllIncidentTargets.Add(Find.World);
			return tmpAllIncidentTargets;
		}
	}

	public static List<IncidentDef> AnomalyIncidents => anomalyIncidents ?? (anomalyIncidents = DefDatabase<IncidentDef>.AllDefsListForReading.Where((IncidentDef incident) => incident.IsAnomalyIncident).ToList());

	public static List<IncidentDef> NonAnomalyIncidents => nonAnomalyIncidents ?? (nonAnomalyIncidents = DefDatabase<IncidentDef>.AllDefsListForReading.Where((IncidentDef incident) => !incident.IsAnomalyIncident).ToList());

	public float AnomalyIncidentChanceNow => Mathf.Clamp01(Find.Anomaly.AnomalyThreatFractionNow);

	public float QuestGiverTraderChanceNow
	{
		get
		{
			if (Find.TickManager.TicksGame - lastQuestGiverTraderTick < 600000)
			{
				return 0f;
			}
			if (Find.TickManager.TicksGame - lastQuestGiverTraderTick > 3600000)
			{
				return 1f;
			}
			return 0.15f;
		}
	}

	public static void StorytellerStaticUpdate()
	{
		tmpAllIncidentTargets.Clear();
	}

	public Storyteller()
	{
	}

	public Storyteller(StorytellerDef def, DifficultyDef difficultyDef)
		: this(def, difficultyDef, new Difficulty(difficultyDef))
	{
	}

	public Storyteller(StorytellerDef def, DifficultyDef difficultyDef, Difficulty difficulty)
	{
		this.def = def;
		this.difficultyDef = difficultyDef;
		this.difficulty = difficulty;
		InitializeStorytellerComps();
	}

	private void InitializeStorytellerComps()
	{
		storytellerComps = new List<StorytellerComp>();
		for (int i = 0; i < def.comps.Count; i++)
		{
			if (def.comps[i].Enabled)
			{
				StorytellerComp storytellerComp = (StorytellerComp)Activator.CreateInstance(def.comps[i].compClass);
				storytellerComp.props = def.comps[i];
				storytellerComp.Initialize();
				storytellerComps.Add(storytellerComp);
			}
		}
	}

	public void ExposeData()
	{
		Scribe_Defs.Look(ref def, "def");
		Scribe_Defs.Look(ref difficultyDef, "difficulty");
		Scribe_Deep.Look(ref incidentQueue, "incidentQueue");
		Scribe_Collections.Look(ref recentIncidentsAnomaly, "recentIncidentsAnomaly", LookMode.Value);
		Scribe_Values.Look(ref recentAnomalyIncidentFactor, "recentAnomalyIncidentFactor", 0.5f);
		Scribe_Values.Look(ref lastQuestGiverTraderTick, "lastQuestGiverTraderTick", 0);
		Scribe_Values.Look(ref lastIncidentTick, "lastIncidentTick", 0);
		if (difficultyDef == null)
		{
			Log.Error("Loaded storyteller without difficulty");
			difficultyDef = DifficultyDefOf.Rough;
		}
		if (difficultyDef.isCustom)
		{
			Scribe_Deep.Look(ref difficulty, "customDifficulty", difficultyDef);
		}
		else if (Scribe.mode == LoadSaveMode.LoadingVars)
		{
			difficulty = new Difficulty(difficultyDef);
		}
		if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
		{
			InitializeStorytellerComps();
		}
		if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.Saving)
		{
			SetupNonCustomDifficultyAnomalySettings();
		}
	}

	private void SetupNonCustomDifficultyAnomalySettings()
	{
		if (ModsConfig.AnomalyActive && !difficultyDef.isCustom)
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Scribe_Values.Look(ref difficulty.anomalyThreatsInactiveFraction, "anomalyThreatsInactiveFraction", 0.08f);
				Scribe_Values.Look(ref difficulty.anomalyThreatsActiveFraction, "anomalyThreatsActiveFraction", 0.3f);
				Scribe_Values.Look(ref difficulty.overrideAnomalyThreatsFraction, "overrideAnomalyThreatsFraction", null);
				Scribe_Values.Look(ref difficulty.studyEfficiencyFactor, "studyEfficiencyFactor", 1f);
				Scribe_Defs.Look(ref anomalyPlaystyleDef, "anomalyPlaystyleDef");
				difficulty.AnomalyPlaystyleDef = anomalyPlaystyleDef;
			}
			else if (Scribe.mode == LoadSaveMode.Saving)
			{
				anomalyThreatsInactiveFraction = difficulty.anomalyThreatsInactiveFraction;
				anomalyThreatsActiveFraction = difficulty.anomalyThreatsActiveFraction;
				overrideAnomalyThreatsFraction = difficulty.overrideAnomalyThreatsFraction;
				studyEfficiencyFactor = difficulty.studyEfficiencyFactor;
				anomalyPlaystyleDef = difficulty.AnomalyPlaystyleDef;
				Scribe_Values.Look(ref anomalyThreatsInactiveFraction, "anomalyThreatsInactiveFraction", 0.08f);
				Scribe_Values.Look(ref anomalyThreatsActiveFraction, "anomalyThreatsActiveFraction", 0.3f);
				Scribe_Values.Look(ref overrideAnomalyThreatsFraction, "overrideAnomalyThreatsFraction", null);
				Scribe_Values.Look(ref studyEfficiencyFactor, "studyEfficiencyFactor", 1f);
				Scribe_Defs.Look(ref anomalyPlaystyleDef, "anomalyPlaystyleDef");
			}
		}
	}

	public void StorytellerTick()
	{
		incidentQueue.IncidentQueueTick();
		if (Find.TickManager.TicksGame % 1000 != 0 || !DebugSettings.enableStoryteller)
		{
			return;
		}
		foreach (FiringIncident item in MakeIncidentsForInterval())
		{
			TryFire(item);
		}
	}

	public bool TryFire(FiringIncident fi, bool queued = false)
	{
		if (fi.def.Worker.CanFireNow(fi.parms) && fi.def.Worker.TryExecute(fi.parms))
		{
			fi.parms.target.StoryState.Notify_IncidentFired(fi);
			lastIncidentTick = GenTicks.TicksGame;
			return true;
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CMakeIncidentsForInterval_003Ed__48))]
	public IEnumerable<FiringIncident> MakeIncidentsForInterval()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeIncidentsForInterval_003Ed__48(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CMakeIncidentsForInterval_003Ed__49))]
	public IEnumerable<FiringIncident> MakeIncidentsForInterval(StorytellerComp comp, List<IIncidentTarget> targets)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeIncidentsForInterval_003Ed__49(-2)
		{
			_003C_003E3__comp = comp,
			_003C_003E3__targets = targets
		};
	}

	public void Notify_PawnEvent(Pawn pawn, AdaptationEvent ev, DamageInfo? dinfo = null)
	{
		Find.StoryWatcher.watcherAdaptation.Notify_PawnEvent(pawn, ev, dinfo);
		for (int i = 0; i < storytellerComps.Count; i++)
		{
			storytellerComps[i].Notify_PawnEvent(pawn, ev, dinfo);
		}
	}

	public void Notify_DissolutionEvent(Thing thing)
	{
		for (int i = 0; i < storytellerComps.Count; i++)
		{
			storytellerComps[i].Notify_DissolutionEvent(thing);
		}
	}

	public void Notify_QuestGiverTraderSpawned()
	{
		lastQuestGiverTraderTick = Find.TickManager.TicksGame;
	}

	public void Notify_DefChanged()
	{
		InitializeStorytellerComps();
	}

	public void RecordIncidentFired(IncidentDef incident)
	{
		if (ModsConfig.AnomalyActive && incident.category.canUseAnomalyChance)
		{
			if (recentIncidentsAnomaly == null)
			{
				recentIncidentsAnomaly = new Queue<bool>();
			}
			if (recentIncidentsAnomaly.Count >= 4)
			{
				recentIncidentsAnomaly.Dequeue();
			}
			recentIncidentsAnomaly.Enqueue(incident.IsAnomalyIncident);
			recentAnomalyIncidentFactor = recentIncidentsAnomaly.Average((bool b) => (!b) ? 0f : 1f);
		}
	}

	public static void ResetStaticData()
	{
		anomalyIncidents = null;
		nonAnomalyIncidents = null;
	}

	public string DebugString()
	{
		if (Time.frameCount % 60 == 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("GLOBAL STORYTELLER STATS");
			stringBuilder.AppendLine("------------------------");
			stringBuilder.AppendLine("Storyteller: ".PadRight(40) + def.label);
			stringBuilder.AppendLine("Adaptation days: ".PadRight(40) + Find.StoryWatcher.watcherAdaptation.AdaptDays.ToString("F1"));
			stringBuilder.AppendLine("Adapt points factor: ".PadRight(40) + Find.StoryWatcher.watcherAdaptation.TotalThreatPointsFactor.ToString("F2"));
			stringBuilder.AppendLine("Time points factor: ".PadRight(40) + Find.Storyteller.def.pointsFactorFromDaysPassed.Evaluate(GenDate.DaysPassedSinceSettle).ToString("F2"));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Ally incident fraction (neutral or ally): ".PadRight(40) + StorytellerUtility.AllyIncidentFraction(fullAlliesOnly: false).ToString("F2"));
			stringBuilder.AppendLine("Ally incident fraction (ally only): ".PadRight(40) + StorytellerUtility.AllyIncidentFraction(fullAlliesOnly: true).ToString("F2"));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(StorytellerUtilityPopulation.DebugReadout().TrimEndNewlines());
			IIncidentTarget incidentTarget = (Find.WorldSelector.SingleSelectedObject as IIncidentTarget) ?? Find.CurrentMap;
			if (incidentTarget != null)
			{
				Map map = incidentTarget as Map;
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("STATS FOR INCIDENT TARGET: " + incidentTarget);
				stringBuilder.AppendLine("------------------------");
				stringBuilder.AppendLine("Progress score: ".PadRight(40) + StorytellerUtility.GetProgressScore(incidentTarget).ToString("F2"));
				stringBuilder.AppendLine("Base points: ".PadRight(40) + StorytellerUtility.DefaultThreatPointsNow(incidentTarget).ToString("F0"));
				stringBuilder.AppendLine("Points factor random range: ".PadRight(40) + incidentTarget.IncidentPointsRandomFactorRange.ToString());
				stringBuilder.AppendLine("Wealth: ".PadRight(40) + incidentTarget.PlayerWealthForStoryteller.ToString("F0"));
				if (map != null)
				{
					if (Find.Storyteller.difficulty.fixedWealthMode)
					{
						stringBuilder.AppendLine($"- Wealth calculated using fixed model curve, time factor: {Find.Storyteller.difficulty.fixedWealthTimeFactor:F1}");
						stringBuilder.AppendLine("- Map age: ".PadRight(40) + map.AgeInDays.ToString("F1"));
					}
					stringBuilder.AppendLine("- Items: ".PadRight(40) + map.wealthWatcher.WealthItems.ToString("F0"));
					stringBuilder.AppendLine("- Buildings: ".PadRight(40) + map.wealthWatcher.WealthBuildings.ToString("F0"));
					stringBuilder.AppendLine("- Floors: ".PadRight(40) + map.wealthWatcher.WealthFloorsOnly.ToString("F0"));
					stringBuilder.AppendLine("- Pawns: ".PadRight(40) + map.wealthWatcher.WealthPawns.ToString("F0"));
				}
				stringBuilder.AppendLine("Pawn count human: ".PadRight(40) + incidentTarget.PlayerPawnsForStoryteller.Count((Pawn p) => p.def.race.Humanlike));
				stringBuilder.AppendLine("Pawn count animal: ".PadRight(40) + incidentTarget.PlayerPawnsForStoryteller.Count((Pawn p) => p.IsAnimal));
				if (map != null)
				{
					stringBuilder.AppendLine("StoryDanger: ".PadRight(40) + map.dangerWatcher.DangerRating);
					stringBuilder.AppendLine("FireDanger: ".PadRight(40) + map.fireWatcher.FireDanger.ToString("F2"));
					stringBuilder.AppendLine("LastThreatBigTick days ago: ".PadRight(40) + (Find.TickManager.TicksGame - map.storyState.LastThreatBigTick).ToStringTicksToDays());
				}
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("LIST OF ALL INCIDENT TARGETS");
			stringBuilder.AppendLine("------------------------");
			for (int i = 0; i < AllIncidentTargets.Count; i++)
			{
				stringBuilder.AppendLine(i + ". " + AllIncidentTargets[i]);
			}
			debugStringCached = stringBuilder.ToString();
		}
		return debugStringCached;
	}
}
