using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse;

public class PawnKindDef : Def
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass156_0
	{
		public PawnKindDef _003C_003E4__this;

		public int i;

		public Func<ThingDef, bool> _003C_003E9__0;

		internal bool _003CConfigErrors_003Eb__0(ThingDef d)
		{
			if (d.weaponTags != null)
			{
				return d.weaponTags.Contains(_003C_003E4__this.weaponTags[i]);
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__156 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PawnKindDef _003C_003E4__this;

		private _003C_003Ec__DisplayClass156_0 _003C_003E8__1;

		private IEnumerator<string> _003C_003E7__wrap1;

		private int _003Ci_003E5__3;

		private int _003Cj_003E5__4;

		private List<AlternateGraphic>.Enumerator _003C_003E7__wrap4;

		private List<StartingHediff>.Enumerator _003C_003E7__wrap5;

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
		public _003CConfigErrors_003Ed__156(int _003C_003E1__state)
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
			case 9:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 12:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap4 = default(List<AlternateGraphic>.Enumerator);
			_003C_003E7__wrap5 = default(List<StartingHediff>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				PawnKindDef pawnKindDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass156_0();
					_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
					_003C_003E7__wrap1 = ((Def)pawnKindDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00b6;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00b6;
				case 2:
					_003C_003E1__state = -1;
					goto IL_0100;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0128;
				case 4:
					_003C_003E1__state = -1;
					goto IL_0160;
				case 5:
					_003C_003E1__state = -1;
					goto IL_02c2;
				case 6:
					_003C_003E1__state = -1;
					goto IL_02c2;
				case 7:
					_003C_003E1__state = -1;
					goto IL_0343;
				case 8:
					_003C_003E1__state = -1;
					goto IL_0424;
				case 9:
					_003C_003E1__state = -4;
					goto IL_04d0;
				case 10:
					_003C_003E1__state = -1;
					goto IL_052a;
				case 11:
					_003C_003E1__state = -1;
					goto IL_0565;
				case 12:
					{
						_003C_003E1__state = -5;
						goto IL_05e9;
					}
					IL_00b6:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (pawnKindDef.backstoryFilters != null && pawnKindDef.backstoryFiltersOverride != null)
					{
						_003C_003E2__current = "both backstoryCategories and backstoryCategoriesOverride are defined";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0100;
					IL_02c2:
					if (!pawnKindDef.RaceProps.Humanlike && pawnKindDef.lifeStages.Count != pawnKindDef.RaceProps.lifeStageAges.Count)
					{
						_003C_003E2__current = "PawnKindDef defines " + pawnKindDef.lifeStages.Count + " lifeStages while race def defines " + pawnKindDef.RaceProps.lifeStageAges.Count;
						_003C_003E1__state = 7;
						return true;
					}
					goto IL_0343;
					IL_04d0:
					while (_003C_003E7__wrap4.MoveNext())
					{
						if (_003C_003E7__wrap4.Current.Weight < 0f)
						{
							_003C_003E2__current = "alternate graphic has negative weight.";
							_003C_003E1__state = 9;
							return true;
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap4 = default(List<AlternateGraphic>.Enumerator);
					goto IL_04ef;
					IL_0343:
					if (pawnKindDef.apparelRequired != null)
					{
						_003Ci_003E5__3 = 0;
						goto IL_045e;
					}
					goto IL_0474;
					IL_0100:
					if (pawnKindDef.race == null)
					{
						_003C_003E2__current = "no race";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0128;
					IL_052a:
					if (pawnKindDef.RaceProps.Humanlike && !pawnKindDef.initialWillRange.HasValue)
					{
						_003C_003E2__current = "initial will range is undefined for humanlike pawn kind.";
						_003C_003E1__state = 11;
						return true;
					}
					goto IL_0565;
					IL_0128:
					if (pawnKindDef.combatPower < 0f)
					{
						_003C_003E2__current = pawnKindDef.defName + " has no combatPower.";
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_0160;
					IL_045e:
					if (_003Ci_003E5__3 < pawnKindDef.apparelRequired.Count)
					{
						_003Cj_003E5__4 = _003Ci_003E5__3 + 1;
						goto IL_0436;
					}
					goto IL_0474;
					IL_0160:
					if (pawnKindDef.weaponMoney != FloatRange.Zero)
					{
						if (pawnKindDef.weaponTags == null)
						{
							_003C_003E2__current = "weaponMoney is set but weaponTags is not.";
							_003C_003E1__state = 5;
							return true;
						}
						float num2 = 999999f;
						_003C_003E8__1.i = 0;
						while (_003C_003E8__1.i < pawnKindDef.weaponTags.Count)
						{
							IEnumerable<ThingDef> source = DefDatabase<ThingDef>.AllDefs.Where((ThingDef d) => d.weaponTags != null && d.weaponTags.Contains(_003C_003E8__1._003C_003E4__this.weaponTags[_003C_003E8__1.i]));
							if (source.Any())
							{
								num2 = Mathf.Min(num2, source.Min((Func<ThingDef, float>)PawnWeaponGenerator.CheapestNonDerpPriceFor));
							}
							int num3 = _003C_003E8__1.i;
							_003C_003E8__1.i = num3 + 1;
						}
						if (num2 < 999999f && num2 > pawnKindDef.weaponMoney.min)
						{
							_003C_003E2__current = "Cheapest weapon with one of my weaponTags costs " + num2 + " but weaponMoney min is " + pawnKindDef.weaponMoney.min + ", so could end up weaponless.";
							_003C_003E1__state = 6;
							return true;
						}
					}
					goto IL_02c2;
					IL_0565:
					if (pawnKindDef.startingHediffs == null)
					{
						break;
					}
					_003C_003E7__wrap5 = pawnKindDef.startingHediffs.GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_05e9;
					IL_0436:
					if (_003Cj_003E5__4 < pawnKindDef.apparelRequired.Count)
					{
						if (!ApparelUtility.CanWearTogether(pawnKindDef.apparelRequired[_003Ci_003E5__3], pawnKindDef.apparelRequired[_003Cj_003E5__4], pawnKindDef.race.race.body))
						{
							_003C_003E2__current = "required apparel can't be worn together (" + pawnKindDef.apparelRequired[_003Ci_003E5__3]?.ToString() + ", " + pawnKindDef.apparelRequired[_003Cj_003E5__4]?.ToString() + ")";
							_003C_003E1__state = 8;
							return true;
						}
						goto IL_0424;
					}
					_003Ci_003E5__3++;
					goto IL_045e;
					IL_0424:
					_003Cj_003E5__4++;
					goto IL_0436;
					IL_0474:
					if (pawnKindDef.alternateGraphics != null)
					{
						_003C_003E7__wrap4 = pawnKindDef.alternateGraphics.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_04d0;
					}
					goto IL_04ef;
					IL_05e9:
					while (_003C_003E7__wrap5.MoveNext())
					{
						StartingHediff current2 = _003C_003E7__wrap5.Current;
						if (current2.durationTicksRange.HasValue && current2.def.CompProps<HediffCompProperties_Disappears>() == null)
						{
							_003C_003E2__current = "starting hediff " + current2.def.defName + " has duration ticks set but doesn't have Disappears comp.";
							_003C_003E1__state = 12;
							return true;
						}
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap5 = default(List<StartingHediff>.Enumerator);
					break;
					IL_04ef:
					if (pawnKindDef.RaceProps.Humanlike && !pawnKindDef.initialResistanceRange.HasValue)
					{
						_003C_003E2__current = "initial resistance range is undefined for humanlike pawn kind.";
						_003C_003E1__state = 10;
						return true;
					}
					goto IL_052a;
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap4/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap5/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__156 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__156(0)
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

	public ThingDef race;

	[LoadAlias("defaultFactionType")]
	public FactionDef defaultFactionDef;

	[NoTranslate]
	public List<BackstoryCategoryFilter> backstoryFilters;

	[NoTranslate]
	public List<BackstoryCategoryFilter> backstoryFiltersOverride;

	[NoTranslate]
	public List<string> backstoryCategories;

	[MustTranslate]
	public string labelPlural;

	public List<PawnKindLifeStage> lifeStages = new List<PawnKindLifeStage>();

	public List<AlternateGraphic> alternateGraphics;

	[XmlInheritanceAllowDuplicateNodes]
	public List<TraitRequirement> forcedTraits;

	[XmlInheritanceAllowDuplicateNodes]
	public List<TraitRequirement> disallowedTraitsWithDegree;

	public List<TraitDef> disallowedTraits;

	public float alternateGraphicChance;

	public MutantDef mutant;

	public XenotypeSet xenotypeSet;

	public bool useFactionXenotypes = true;

	[LoadAlias("hairTags")]
	public List<StyleItemTagWeighted> styleItemTags;

	public HairDef forcedHair;

	public Color? forcedHairColor;

	public List<MissingPart> missingParts;

	public RulePackDef nameMaker;

	public RulePackDef nameMakerFemale;

	public List<AbilityDef> abilities;

	public bool preventIdeo;

	public bool studiableAsPrisoner;

	public bool isBoss;

	public Dictionary<string, float> moveSpeedFactorByTerrainTag = new Dictionary<string, float>();

	public List<BackstoryDef> fixedChildBackstories = new List<BackstoryDef>();

	public List<BackstoryDef> fixedAdultBackstories = new List<BackstoryDef>();

	public float backstoryCryptosleepCommonality;

	public FloatRange? chronologicalAgeRange;

	public int minGenerationAge;

	public int maxGenerationAge = 999999;

	public bool factionLeader;

	public Gender? fixedGender;

	public bool allowOldAgeInjuries = true;

	public bool generateInitialNonFamilyRelations = true;

	public DevelopmentalStage? pawnGroupDevelopmentStage;

	public bool destroyGearOnDrop;

	public bool canStrip = true;

	public float defendPointRadius = -1f;

	public bool factionHostileOnKill;

	public bool factionHostileOnDeath;

	public bool hostileToAll;

	public FloatRange? initialResistanceRange;

	public FloatRange? initialWillRange;

	public bool forceNoDeathNotification;

	public bool skipResistant;

	public float controlGroupPortraitZoom = 1f;

	public float? overrideDeathOnDownedChance;

	public bool forceDeathOnDowned;

	public bool immuneToGameConditionEffects;

	public bool immuneToTraps;

	public bool collidesWithPawns = true;

	public bool ignoresPainShock;

	public bool canMeleeAttack = true;

	public float basePrisonBreakMtbDays = 60f;

	public bool useFixedRotation;

	public Rot4 fixedRotation;

	public bool showInDebugSpawner = true;

	public bool canOpenAnyDoor;

	public bool canOpenDoors = true;

	[NoTranslate]
	public string overrideDebugActionCategory;

	public float royalTitleChance;

	public RoyalTitleDef titleRequired;

	public RoyalTitleDef minTitleRequired;

	public List<RoyalTitleDef> titleSelectOne;

	public bool allowRoyalRoomRequirements = true;

	public bool allowRoyalApparelRequirements = true;

	public List<InfectionPathwayDef> meleeAttackInfectionPathways;

	public List<InfectionPathwayDef> rangedAttackInfectionPathways;

	public bool isFighter = true;

	public float combatPower = -1f;

	public bool canArriveManhunter = true;

	public bool canBeSapper;

	public bool isGoodBreacher;

	public bool allowInMechClusters = true;

	public int maxPerGroup = int.MaxValue;

	public bool isGoodPsychicRitualInvoker;

	public bool canBeScattered = true;

	public bool appearsRandomlyInCombatGroups = true;

	public bool aiAvoidCover;

	public FloatRange fleeHealthThresholdRange = new FloatRange(-0.4f, 0.4f);

	public float acceptArrestChanceFactor = 1f;

	public bool canUseAvoidGrid;

	public QualityCategory itemQuality = QualityCategory.Normal;

	public QualityCategory? forceWeaponQuality;

	public bool forceNormalGearQuality;

	public FloatRange gearHealthRange = FloatRange.One;

	public FloatRange weaponMoney = FloatRange.Zero;

	[NoTranslate]
	public List<string> weaponTags;

	public ThingDef weaponStuffOverride;

	public ThingStyleDef weaponStyleDef;

	public FloatRange apparelMoney = FloatRange.Zero;

	public List<ThingDef> apparelRequired;

	[NoTranslate]
	public List<string> apparelTags;

	[NoTranslate]
	public List<string> apparelDisallowTags;

	public float apparelAllowHeadgearChance = 1f;

	public bool ignoreApparelAllowChance;

	public bool apparelIgnoreSeasons;

	public bool apparelIgnorePollution;

	public bool ignoreFactionApparelStuffRequirements;

	public Color apparelColor = Color.white;

	public Color? skinColorOverride;

	public ColorDef favoriteColor;

	public bool ignoreIdeoApparelColors;

	public List<SpecificApparelRequirement> specificApparelRequirements;

	public List<ThingDef> techHediffsRequired;

	public FloatRange techHediffsMoney = FloatRange.Zero;

	[NoTranslate]
	public List<string> techHediffsTags;

	[NoTranslate]
	public List<string> techHediffsDisallowTags;

	public float techHediffsChance;

	public int techHediffsMaxAmount = 1;

	public float biocodeWeaponChance;

	public float humanPregnancyChance = 0.03f;

	public List<StartingHediff> startingHediffs;

	public float nakedChance;

	public List<MiscDamage> existingDamage = new List<MiscDamage>();

	public QualityCategory minApparelQuality;

	public QualityCategory maxApparelQuality = QualityCategory.Excellent;

	public List<ThingDefCountClass> fixedInventory = new List<ThingDefCountClass>();

	public PawnInventoryOption inventoryOptions;

	public float invNutrition;

	public ThingDef invFoodDef;

	public float chemicalAddictionChance;

	public float combatEnhancingDrugsChance;

	public IntRange combatEnhancingDrugsCount = IntRange.Zero;

	public List<ChemicalDef> forcedAddictions = new List<ChemicalDef>();

	public bool trader;

	public List<SkillRange> skills;

	public WorkTags requiredWorkTags;

	public WorkTags disabledWorkTags;

	public int extraSkillLevels;

	public int minTotalSkillLevels;

	public int minBestSkillLevel;

	[MustTranslate]
	public string labelMale;

	[MustTranslate]
	public string labelMalePlural;

	[MustTranslate]
	public string labelFemale;

	[MustTranslate]
	public string labelFemalePlural;

	public IntRange wildGroupSize = IntRange.One;

	public float ecoSystemWeight = 1f;

	[NoTranslate]
	public string flyingAnimationFramePathPrefix;

	[NoTranslate]
	public string flyingAnimationFramePathPrefixFemale;

	public int flyingAnimationFrameCount;

	public int flyingAnimationTicksPerFrame = -1;

	public float flyingAnimationDrawSize = 1f;

	public bool flyingAnimationDrawSizeIsMultiplier;

	public bool flyingAnimationInheritColors;

	private const int MaxWeaponMoney = 999999;

	public RaceProperties RaceProps => race.race;

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		for (int i = 0; i < lifeStages.Count; i++)
		{
			lifeStages[i].ResolveReferences();
		}
	}

	public string GetLabelPlural(int count = -1)
	{
		if (!labelPlural.NullOrEmpty())
		{
			return labelPlural;
		}
		return Find.ActiveLanguageWorker.Pluralize(label, count);
	}

	public string GetLabelGendered(Gender gender)
	{
		if (gender == Gender.Female && !labelFemale.NullOrEmpty())
		{
			return labelFemale;
		}
		if (gender == Gender.Male && !labelMale.NullOrEmpty())
		{
			return labelMale;
		}
		return label;
	}

	public RulePackDef GetNameMaker(Gender gender)
	{
		if (gender == Gender.Female && nameMakerFemale != null)
		{
			return nameMakerFemale;
		}
		if (nameMaker != null)
		{
			return nameMaker;
		}
		return null;
	}

	public override void PostLoad()
	{
		if (backstoryCategories != null && backstoryCategories.Count > 0)
		{
			if (backstoryFilters == null)
			{
				backstoryFilters = new List<BackstoryCategoryFilter>();
			}
			backstoryFilters.Add(new BackstoryCategoryFilter
			{
				categories = backstoryCategories
			});
		}
		LongEventHandler.ExecuteWhenFinished(delegate
		{
			foreach (PawnKindLifeStage lifeStage in lifeStages)
			{
				if (lifeStage.swimmingGraphicData != null && lifeStage.swimmingGraphicData.shaderType == null)
				{
					lifeStage.swimmingGraphicData.shaderType = ShaderTypeDefOf.Transparent;
				}
				if (lifeStage.femaleSwimmingGraphicData != null && lifeStage.femaleSwimmingGraphicData.shaderType == null)
				{
					lifeStage.femaleSwimmingGraphicData.shaderType = ShaderTypeDefOf.Transparent;
				}
				if (lifeStage.stationaryGraphicData != null && lifeStage.stationaryGraphicData.shaderType == null)
				{
					lifeStage.stationaryGraphicData.shaderType = ShaderTypeDefOf.Transparent;
				}
				if (lifeStage.femaleStationaryGraphicData != null && lifeStage.femaleStationaryGraphicData.shaderType == null)
				{
					lifeStage.femaleStationaryGraphicData.shaderType = ShaderTypeDefOf.Transparent;
				}
			}
		});
	}

	public float GetAnimalPointsToHuntOrSlaughter()
	{
		return combatPower * 5f * (1f + RaceProps.manhunterOnDamageChance * 0.5f) * (1f + RaceProps.manhunterOnTameFailChance * 0.2f) * (1f + race.GetStatValueAbstract(StatDefOf.Wildness)) + race.BaseMarketValue;
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__156))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__156(-2)
		{
			_003C_003E4__this = this
		};
	}

	public static PawnKindDef Named(string defName)
	{
		return DefDatabase<PawnKindDef>.GetNamed(defName);
	}
}
