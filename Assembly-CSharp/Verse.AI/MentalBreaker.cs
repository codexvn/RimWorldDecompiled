using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI;

public class MentalBreaker : IExposable
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass43_0
	{
		public MentalBreaker _003C_003E4__this;

		public bool anomalyBreak;
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass43_1
	{
		public MentalBreakIntensity intensityInner;

		public _003C_003Ec__DisplayClass43_0 CS_0024_003C_003E8__locals1;

		internal bool _003CGetBreaksForIntensity_003Eb__0(MentalBreakDef d)
		{
			if (d.intensity == intensityInner && d.Worker.BreakCanOccur(CS_0024_003C_003E8__locals1._003C_003E4__this.pawn))
			{
				return d.anomalousBreak == CS_0024_003C_003E8__locals1.anomalyBreak;
			}
			return false;
		}

		internal bool _003CGetBreaksForIntensity_003Eb__1(MentalBreakDef d)
		{
			if (d.intensity == intensityInner && d.Worker.BreakCanOccur(CS_0024_003C_003E8__locals1._003C_003E4__this.pawn))
			{
				return !d.anomalousBreak;
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetBreaksForIntensity_003Ed__43 : IEnumerable<MentalBreakDef>, IEnumerable, IEnumerator<MentalBreakDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private MentalBreakDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MentalBreaker _003C_003E4__this;

		private bool anomalyBreak;

		public bool _003C_003E3__anomalyBreak;

		private _003C_003Ec__DisplayClass43_0 _003C_003E8__1;

		private MentalBreakIntensity intensity;

		public MentalBreakIntensity _003C_003E3__intensity;

		private bool tryNonAnomalyBreakIfNoneAvailable;

		public bool _003C_003E3__tryNonAnomalyBreakIfNoneAvailable;

		private HashSet<MentalBreakDef>.Enumerator _003C_003E7__wrap1;

		private IEnumerator<MentalBreakDef> _003C_003E7__wrap2;

		MentalBreakDef IEnumerator<MentalBreakDef>.Current
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
		public _003CGetBreaksForIntensity_003Ed__43(int _003C_003E1__state)
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
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = default(HashSet<MentalBreakDef>.Enumerator);
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				MentalBreaker mentalBreaker = _003C_003E4__this;
				bool flag2;
				bool flag;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass43_0();
					_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
					_003C_003E8__1.anomalyBreak = anomalyBreak;
					if (ModsConfig.IdeologyActive && mentalBreaker.pawn.Ideo != null)
					{
						flag2 = false;
						HashSet<MentalBreakDef> cachedPossibleMentalBreaks = mentalBreaker.pawn.Ideo.cachedPossibleMentalBreaks;
						_003C_003E7__wrap1 = cachedPossibleMentalBreaks.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_00e1;
					}
					goto IL_01fb;
				case 1:
					_003C_003E1__state = -3;
					flag2 = true;
					goto IL_00e1;
				case 2:
					{
						_003C_003E1__state = -4;
						flag = true;
						goto IL_01c6;
					}
					IL_01c6:
					if (_003C_003E7__wrap2.MoveNext())
					{
						MentalBreakDef current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					if (flag)
					{
						return false;
					}
					intensity--;
					goto IL_01fb;
					IL_01fb:
					if (intensity != 0)
					{
						_003C_003Ec__DisplayClass43_1 CS_0024_003C_003E8__locals6 = new _003C_003Ec__DisplayClass43_1
						{
							CS_0024_003C_003E8__locals1 = _003C_003E8__1,
							intensityInner = intensity
						};
						IEnumerable<MentalBreakDef> enumerable = DefDatabase<MentalBreakDef>.AllDefsListForReading.Where((MentalBreakDef d) => d.intensity == CS_0024_003C_003E8__locals6.intensityInner && d.Worker.BreakCanOccur(CS_0024_003C_003E8__locals6.CS_0024_003C_003E8__locals1._003C_003E4__this.pawn) && d.anomalousBreak == CS_0024_003C_003E8__locals6.CS_0024_003C_003E8__locals1.anomalyBreak);
						if ((CS_0024_003C_003E8__locals6.CS_0024_003C_003E8__locals1.anomalyBreak && enumerable.EnumerableNullOrEmpty()) & tryNonAnomalyBreakIfNoneAvailable)
						{
							enumerable = DefDatabase<MentalBreakDef>.AllDefsListForReading.Where((MentalBreakDef d) => d.intensity == CS_0024_003C_003E8__locals6.intensityInner && d.Worker.BreakCanOccur(CS_0024_003C_003E8__locals6.CS_0024_003C_003E8__locals1._003C_003E4__this.pawn) && !d.anomalousBreak);
						}
						flag = false;
						_003C_003E7__wrap2 = enumerable.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_01c6;
					}
					return false;
					IL_00e1:
					while (_003C_003E7__wrap1.MoveNext())
					{
						MentalBreakDef current2 = _003C_003E7__wrap1.Current;
						if (current2.Worker.BreakCanOccur(mentalBreaker.pawn))
						{
							_003C_003E2__current = current2;
							_003C_003E1__state = 1;
							return true;
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(HashSet<MentalBreakDef>.Enumerator);
					if (flag2)
					{
						return false;
					}
					goto IL_01fb;
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
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
		IEnumerator<MentalBreakDef> IEnumerable<MentalBreakDef>.GetEnumerator()
		{
			_003CGetBreaksForIntensity_003Ed__43 _003CGetBreaksForIntensity_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetBreaksForIntensity_003Ed__ = this;
			}
			else
			{
				_003CGetBreaksForIntensity_003Ed__ = new _003CGetBreaksForIntensity_003Ed__43(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetBreaksForIntensity_003Ed__.intensity = _003C_003E3__intensity;
			_003CGetBreaksForIntensity_003Ed__.anomalyBreak = _003C_003E3__anomalyBreak;
			_003CGetBreaksForIntensity_003Ed__.tryNonAnomalyBreakIfNoneAvailable = _003C_003E3__tryNonAnomalyBreakIfNoneAvailable;
			return _003CGetBreaksForIntensity_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<MentalBreakDef>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_CurrentPossibleMoodBreaks_003Ed__34 : IEnumerable<MentalBreakDef>, IEnumerable, IEnumerator<MentalBreakDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private MentalBreakDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MentalBreaker _003C_003E4__this;

		private IEnumerator<MentalBreakDef> _003C_003E7__wrap1;

		MentalBreakDef IEnumerator<MentalBreakDef>.Current
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
		public _003Cget_CurrentPossibleMoodBreaks_003Ed__34(int _003C_003E1__state)
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
				MentalBreaker mentalBreaker = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = mentalBreaker.GetBreaksForIntensity(mentalBreaker.CurrentDesiredMoodBreakIntensity, mentalBreaker.ChooseAnomalyBreak).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					MentalBreakDef current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
		IEnumerator<MentalBreakDef> IEnumerable<MentalBreakDef>.GetEnumerator()
		{
			_003Cget_CurrentPossibleMoodBreaks_003Ed__34 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_CurrentPossibleMoodBreaks_003Ed__34(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<MentalBreakDef>)this).GetEnumerator();
		}
	}

	private Pawn pawn;

	private int ticksUntilCanDoMentalBreak;

	private int ticksBelowExtreme;

	private int ticksBelowMajor;

	private int ticksBelowMinor;

	private const int CheckInterval = 150;

	private const float ExtremeBreakMTBDays = 0.5f;

	private const float MajorBreakMTBDays = 0.8f;

	private const float MinorBreakMTBDays = 4f;

	private const int MinTicksBelowToBreak = 2000;

	private const int MinTicksSinceRecoveryToBreak = 15000;

	private const float MajorBreakMoodFraction = 0.5714286f;

	private const float ExtremeBreakMoodFraction = 1f / 7f;

	private static List<Thought> tmpThoughts = new List<Thought>();

	public float BreakThresholdExtreme => pawn.GetStatValue(StatDefOf.MentalBreakThreshold, applyPostProcess: true, 5) * (1f / 7f);

	public float BreakThresholdMajor => pawn.GetStatValue(StatDefOf.MentalBreakThreshold, applyPostProcess: true, 5) * 0.5714286f;

	public float BreakThresholdMinor => pawn.GetStatValue(StatDefOf.MentalBreakThreshold, applyPostProcess: true, 5);

	public bool CanDoRandomMentalBreaks
	{
		get
		{
			LifeStageDef curLifeStage = pawn.ageTracker.CurLifeStage;
			if (curLifeStage == null)
			{
				if (pawn.RaceProps.Humanlike)
				{
					if (!pawn.Spawned)
					{
						return pawn.IsCaravanMember();
					}
					return true;
				}
				return false;
			}
			return curLifeStage.canDoRandomMentalBreaks;
		}
	}

	public bool BreakExtremeIsImminent
	{
		get
		{
			if (pawn.MentalStateDef == null && CurMood < BreakThresholdExtreme)
			{
				return !Blocked;
			}
			return false;
		}
	}

	public bool BreakMajorIsImminent
	{
		get
		{
			if (pawn.MentalStateDef == null && !BreakExtremeIsImminent && CurMood < BreakThresholdMajor)
			{
				return !Blocked;
			}
			return false;
		}
	}

	public bool BreakMinorIsImminent
	{
		get
		{
			if (pawn.MentalStateDef == null && !BreakExtremeIsImminent && !BreakMajorIsImminent && CurMood < BreakThresholdMinor)
			{
				return !Blocked;
			}
			return false;
		}
	}

	public bool BreakExtremeIsApproaching
	{
		get
		{
			if (pawn.MentalStateDef == null && BreakExtremeIsImminent && CurMood < BreakThresholdExtreme + 0.1f)
			{
				return !Blocked;
			}
			return false;
		}
	}

	public bool Blocked => pawn.mindState.mentalStateHandler.MentalBreaksBlocked();

	public float CurMood
	{
		get
		{
			if (pawn.needs.mood == null)
			{
				return 0.5f;
			}
			return pawn.needs.mood.CurLevel;
		}
	}

	private IEnumerable<MentalBreakDef> CurrentPossibleMoodBreaks
	{
		[IteratorStateMachine(typeof(_003Cget_CurrentPossibleMoodBreaks_003Ed__34))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_CurrentPossibleMoodBreaks_003Ed__34(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	private bool ChooseAnomalyBreak
	{
		get
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (pawn.IsCreepJoiner)
			{
				return true;
			}
			MonolithLevelDef levelDef = Find.Anomaly.LevelDef;
			if (levelDef != null)
			{
				return Rand.Chance(levelDef.anomalyMentalBreakChance);
			}
			return false;
		}
	}

	private MentalBreakIntensity CurrentDesiredMoodBreakIntensity
	{
		get
		{
			if (ticksBelowExtreme >= 2000)
			{
				return MentalBreakIntensity.Extreme;
			}
			if (ticksBelowMajor >= 2000)
			{
				return MentalBreakIntensity.Major;
			}
			if (ticksBelowMinor >= 2000)
			{
				return MentalBreakIntensity.Minor;
			}
			return MentalBreakIntensity.None;
		}
	}

	public MentalBreaker()
	{
	}

	public MentalBreaker(Pawn pawn)
	{
		this.pawn = pawn;
	}

	internal void Reset()
	{
		ticksBelowExtreme = 0;
		ticksBelowMajor = 0;
		ticksBelowMinor = 0;
	}

	public void ExposeData()
	{
		Scribe_Values.Look(ref ticksUntilCanDoMentalBreak, "ticksUntilCanDoMentalBreak", 0);
		Scribe_Values.Look(ref ticksBelowExtreme, "ticksBelowExtreme", 0);
		Scribe_Values.Look(ref ticksBelowMajor, "ticksBelowMajor", 0);
		Scribe_Values.Look(ref ticksBelowMinor, "ticksBelowMinor", 0);
	}

	[IteratorStateMachine(typeof(_003CGetBreaksForIntensity_003Ed__43))]
	private IEnumerable<MentalBreakDef> GetBreaksForIntensity(MentalBreakIntensity intensity, bool anomalyBreak, bool tryNonAnomalyBreakIfNoneAvailable = true)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetBreaksForIntensity_003Ed__43(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__intensity = intensity,
			_003C_003E3__anomalyBreak = anomalyBreak,
			_003C_003E3__tryNonAnomalyBreakIfNoneAvailable = tryNonAnomalyBreakIfNoneAvailable
		};
	}

	public void MentalBreakerTickInterval(int delta)
	{
		if (ticksUntilCanDoMentalBreak > 0 && pawn.Awake())
		{
			ticksUntilCanDoMentalBreak -= delta;
		}
		if (!CanDoRandomMentalBreaks || pawn.MentalStateDef != null || !pawn.IsHashIntervalTick(150, delta) || !DebugSettings.enableRandomMentalStates)
		{
			return;
		}
		if (CurMood < BreakThresholdExtreme)
		{
			ticksBelowExtreme += 150;
		}
		else
		{
			ticksBelowExtreme = 0;
		}
		if (CurMood < BreakThresholdMajor)
		{
			ticksBelowMajor += 150;
		}
		else
		{
			ticksBelowMajor = 0;
		}
		if (CurMood < BreakThresholdMinor)
		{
			ticksBelowMinor += 150;
		}
		else
		{
			ticksBelowMinor = 0;
		}
		if ((!TestMoodMentalBreak() || (!AlwaysBerserkUtility.TryTriggerBerserkBloodRage(pawn) && !AlwaysBerserkUtility.TryTriggerBerserkFrenzyInducer(pawn) && !TryDoRandomMoodCausedMentalBreak())) && pawn.story != null)
		{
			List<Trait> allTraits = pawn.story.traits.allTraits;
			for (int i = 0; i < allTraits.Count && (allTraits[i].Suppressed || !allTraits[i].CurrentData.MentalStateGiver.CheckGive(pawn, 150)); i++)
			{
			}
		}
	}

	private bool TestMoodMentalBreak()
	{
		if (ticksUntilCanDoMentalBreak > 0)
		{
			return false;
		}
		if (ticksBelowExtreme > 2000)
		{
			return Rand.MTBEventOccurs(0.5f, 60000f, 150f);
		}
		if (ticksBelowMajor > 2000)
		{
			return Rand.MTBEventOccurs(0.8f, 60000f, 150f);
		}
		if (ticksBelowMinor > 2000)
		{
			return Rand.MTBEventOccurs(4f, 60000f, 150f);
		}
		return false;
	}

	public bool TryDoRandomMoodCausedMentalBreak()
	{
		if (!CanHaveMentalBreak())
		{
			return false;
		}
		if (!CurrentPossibleMoodBreaks.TryRandomElementByWeight((MentalBreakDef d) => d.Worker.CommonalityFor(pawn, moodCaused: true), out var result))
		{
			return false;
		}
		Thought thought = RandomFinalStraw();
		TaggedString taggedString = "MentalStateReason_Mood".Translate();
		if (thought != null)
		{
			taggedString += "\n\n" + "FinalStraw".Translate(thought.LabelCap);
		}
		return TryDoMentalBreak(taggedString, result);
	}

	public bool TryDoMentalBreak(string reason, MentalBreakDef breakDef)
	{
		if (!CanHaveMentalBreak())
		{
			return false;
		}
		return breakDef.Worker.TryStart(pawn, reason, causedByMood: true);
	}

	public bool TryGetRandomMentalBreak(MentalBreakIntensity intensity, out MentalBreakDef breakDef)
	{
		return GetBreaksForIntensity(intensity, ChooseAnomalyBreak).TryRandomElementByWeight((MentalBreakDef d) => d.Worker.CommonalityFor(pawn, moodCaused: true), out breakDef);
	}

	private bool CanHaveMentalBreak()
	{
		if (!CanDoRandomMentalBreaks || pawn.Downed || !pawn.Awake() || pawn.InMentalState)
		{
			return false;
		}
		if (pawn.IsMutant && pawn.mutant.Def.preventsMentalBreaks)
		{
			return false;
		}
		if (pawn.Faction != Faction.OfPlayer && CurrentDesiredMoodBreakIntensity != MentalBreakIntensity.Extreme)
		{
			return false;
		}
		if (QuestUtility.AnyQuestDisablesRandomMoodCausedMentalBreaksFor(pawn))
		{
			return false;
		}
		if (Blocked)
		{
			return false;
		}
		return true;
	}

	private Thought RandomFinalStraw()
	{
		pawn.needs.mood.thoughts.GetAllMoodThoughts(tmpThoughts);
		float num = 0f;
		for (int i = 0; i < tmpThoughts.Count; i++)
		{
			float num2 = tmpThoughts[i].MoodOffset();
			if (num2 < num)
			{
				num = num2;
			}
		}
		float maxMoodOffset = num * 0.5f;
		tmpThoughts.Where((Thought x) => x.MoodOffset() <= maxMoodOffset).TryRandomElementByWeight((Thought x) => 0f - x.MoodOffset(), out var result);
		tmpThoughts.Clear();
		return result;
	}

	public void Notify_RecoveredFromMentalState()
	{
		ticksUntilCanDoMentalBreak = 15000;
	}

	public float MentalBreakThresholdFor(MentalBreakIntensity intensity)
	{
		return intensity switch
		{
			MentalBreakIntensity.Extreme => BreakThresholdExtreme, 
			MentalBreakIntensity.Major => BreakThresholdMajor, 
			MentalBreakIntensity.Minor => BreakThresholdMinor, 
			_ => throw new NotImplementedException(), 
		};
	}

	internal string DebugString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(pawn.ToString());
		stringBuilder.AppendLine("   ticksUntilCanDoMentalBreak=" + ticksUntilCanDoMentalBreak);
		stringBuilder.AppendLine("   ticksBelowExtreme=" + ticksBelowExtreme + "/" + 2000);
		stringBuilder.AppendLine("   ticksBelowSerious=" + ticksBelowMajor + "/" + 2000);
		stringBuilder.AppendLine("   ticksBelowMinor=" + ticksBelowMinor + "/" + 2000);
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("Current desired mood break intensity: " + CurrentDesiredMoodBreakIntensity);
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("Current possible mood breaks:");
		float num = CurrentPossibleMoodBreaks.Select((MentalBreakDef d) => d.Worker.CommonalityFor(pawn, moodCaused: true)).Sum();
		foreach (MentalBreakDef currentPossibleMoodBreak in CurrentPossibleMoodBreaks)
		{
			float num2 = currentPossibleMoodBreak.Worker.CommonalityFor(pawn, moodCaused: true);
			stringBuilder.AppendLine("   " + currentPossibleMoodBreak?.ToString() + "     " + (num2 / num).ToStringPercent());
		}
		return stringBuilder.ToString();
	}

	internal void LogPossibleMentalBreaks()
	{
		StringBuilder stringBuilder = new StringBuilder();
		MentalBreakIntensity currentDesiredMoodBreakIntensity = CurrentDesiredMoodBreakIntensity;
		stringBuilder.AppendLine(pawn?.ToString() + " current possible mood mental breaks:").AppendLine("CurrentDesiredMoodBreakIntensity: " + currentDesiredMoodBreakIntensity);
		foreach (MentalBreakDef item in GetBreaksForIntensity(currentDesiredMoodBreakIntensity, anomalyBreak: false))
		{
			stringBuilder.AppendLine("  " + item);
		}
		if (ModsConfig.AnomalyActive)
		{
			MonolithLevelDef levelDef = Find.Anomaly.LevelDef;
			if (levelDef != null && levelDef.anomalyMentalBreakChance > 0f)
			{
				foreach (MentalBreakDef item2 in GetBreaksForIntensity(currentDesiredMoodBreakIntensity, anomalyBreak: true, tryNonAnomalyBreakIfNoneAvailable: false))
				{
					stringBuilder.AppendLine("  " + item2);
				}
			}
		}
		Log.Message(stringBuilder.ToString());
	}
}
