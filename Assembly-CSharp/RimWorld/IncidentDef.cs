using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class IncidentDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__66 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public IncidentDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

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
		public _003CConfigErrors_003Ed__66(int _003C_003E1__state)
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
				IncidentDef incidentDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)incidentDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0086;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0086;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00c8;
				case 3:
					_003C_003E1__state = -1;
					goto IL_00fd;
				case 4:
					_003C_003E1__state = -1;
					goto IL_0156;
				case 5:
					_003C_003E1__state = -1;
					goto IL_0193;
				case 6:
					_003C_003E1__state = -1;
					goto IL_01ca;
				case 7:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0086:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (incidentDef.category == null)
					{
						_003C_003E2__current = "category is undefined.";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00c8;
					IL_0193:
					if (!incidentDef.allowedBiomes.NullOrEmpty() && !incidentDef.disallowedBiomes.NullOrEmpty())
					{
						_003C_003E2__current = "allowedBiomes (white list) and disallowedBiomes (black list) are both defined.";
						_003C_003E1__state = 6;
						return true;
					}
					goto IL_01ca;
					IL_00c8:
					if (incidentDef.targetTags == null || incidentDef.targetTags.Count == 0)
					{
						_003C_003E2__current = "no target type";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_00fd;
					IL_0156:
					if (incidentDef.TargetTagAllowed(IncidentTargetTagDefOf.World) && (incidentDef.allowedBiomes != null || incidentDef.disallowedBiomes != null))
					{
						_003C_003E2__current = "world-targeting incident has a biome restriction list";
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_0193;
					IL_00fd:
					if (incidentDef.TargetTagAllowed(IncidentTargetTagDefOf.World) && incidentDef.targetTags.Any((IncidentTargetTagDef tt) => tt != IncidentTargetTagDefOf.World))
					{
						_003C_003E2__current = "allows world target type along with other targets. World targeting incidents should only target the world.";
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_0156;
					IL_01ca:
					if (incidentDef.questScriptDef != null && incidentDef.questScriptDef.rootSelectionWeight != 0f)
					{
						_003C_003E2__current = "quest is run from both incident and random quest.";
						_003C_003E1__state = 7;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__66 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__66(0)
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

	public Type workerClass;

	public IncidentCategoryDef category;

	public List<IncidentTargetTagDef> targetTags;

	public float baseChance;

	public float baseChanceWithRoyalty = -1f;

	public IncidentPopulationEffect populationEffect;

	public int earliestDay;

	public int minPopulation;

	public bool requireColonistsPresent;

	public float minRefireDays;

	[Obsolete]
	public int minDifficulty;

	public DifficultyConditionConfig disabledWhen = new DifficultyConditionConfig();

	public bool pointsScaleable;

	public float minThreatPoints = float.MinValue;

	public float maxThreatPoints = float.MaxValue;

	public List<BiomeDef> allowedBiomes;

	public List<BiomeDef> disallowedBiomes;

	[NoTranslate]
	public List<string> tags;

	[NoTranslate]
	public List<string> refireCheckTags;

	public SimpleCurve chanceFactorByPopulationCurve;

	public TaleDef tale;

	public int minAnomalyThreatLevel = -1;

	public int minGreatestPopulation = -1;

	private bool ignoreRecentSelectionWeighting;

	public bool hidden;

	public List<PlanetLayerDef> layerWhitelist;

	public List<PlanetLayerDef> layerBlacklist;

	public bool canOccurOnAllPlanetLayers;

	public EntityCodexEntryDef codexEntry;

	[MustTranslate]
	public string letterText;

	[MustTranslate]
	public string letterLabel;

	public LetterDef letterDef;

	public List<HediffDef> letterHyperlinkHediffDefs;

	public bool letterSingularForm;

	public PawnKindDef pawnKind;

	public bool pawnMustBeCapableOfViolence;

	public Gender pawnFixedGender;

	public HediffDef pawnHediff;

	public GameConditionDef gameCondition;

	public FloatRange durationDays;

	public HediffDef diseaseIncident;

	public FloatRange diseaseVictimFractionRange = new FloatRange(0f, 0.49f);

	public int diseaseMaxVictims = 99999;

	public List<BiomeDiseaseRecord> diseaseBiomeRecords;

	public List<BodyPartDef> diseasePartsToAffect;

	[MustTranslate]
	public string diseaseLethalLetterText;

	public DevelopmentalStage diseaseDevelopmentStage = DevelopmentalStage.Baby | DevelopmentalStage.Child | DevelopmentalStage.Adult;

	public ThingDef mechClusterBuilding;

	public List<MTBByBiome> mtbDaysByBiome;

	public QuestScriptDef questScriptDef;

	public ThingDef treeDef;

	public GenStepDef treeGenStepDef;

	public string letterTextPlural;

	public float treeGrowth;

	[Unsaved(false)]
	private IncidentWorker workerInt;

	[Unsaved(false)]
	private List<IncidentDef> cachedRefireCheckIncidents;

	public bool ShouldIgnoreRecentWeighting
	{
		get
		{
			if (!ignoreRecentSelectionWeighting && category != IncidentCategoryDefOf.Special)
			{
				return category == IncidentCategoryDefOf.GiveQuest;
			}
			return true;
		}
	}

	public IncidentWorker Worker
	{
		get
		{
			if (workerInt == null)
			{
				workerInt = (IncidentWorker)Activator.CreateInstance(workerClass);
				workerInt.def = this;
			}
			return workerInt;
		}
	}

	public List<IncidentDef> RefireCheckIncidents
	{
		get
		{
			if (refireCheckTags == null)
			{
				return null;
			}
			if (cachedRefireCheckIncidents == null)
			{
				cachedRefireCheckIncidents = new List<IncidentDef>();
				List<IncidentDef> allDefsListForReading = DefDatabase<IncidentDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (ShouldDoRefireCheckWith(allDefsListForReading[i]))
					{
						cachedRefireCheckIncidents.Add(allDefsListForReading[i]);
					}
				}
			}
			return cachedRefireCheckIncidents;
		}
	}

	public bool IsAnomalyIncident => minAnomalyThreatLevel >= 0;

	public static IncidentDef Named(string defName)
	{
		return DefDatabase<IncidentDef>.GetNamed(defName);
	}

	private bool ShouldDoRefireCheckWith(IncidentDef other)
	{
		if (other.tags == null)
		{
			return false;
		}
		if (other == this)
		{
			return false;
		}
		for (int i = 0; i < other.tags.Count; i++)
		{
			for (int j = 0; j < refireCheckTags.Count; j++)
			{
				if (other.tags[i] == refireCheckTags[j])
				{
					return true;
				}
			}
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__66))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__66(-2)
		{
			_003C_003E4__this = this
		};
	}

	public bool TargetTagAllowed(IncidentTargetTagDef target)
	{
		return targetTags.Contains(target);
	}

	public bool TargetAllowed(IIncidentTarget target)
	{
		return targetTags.Intersect(target.IncidentTargetTags()).Any();
	}
}
