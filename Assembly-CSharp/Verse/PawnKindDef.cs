using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002B9 RID: 697
	public class PawnKindDef : Def
	{
		// Token: 0x170002DF RID: 735
		// (get) Token: 0x0600110A RID: 4362 RVA: 0x0004FDEB File Offset: 0x0004DFEB
		public RaceProperties RaceProps
		{
			get
			{
				return this.race.race;
			}
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x0004FDF8 File Offset: 0x0004DFF8
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.lifeStages.Count; i++)
			{
				this.lifeStages[i].ResolveReferences();
			}
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x0004FE32 File Offset: 0x0004E032
		public string GetLabelPlural(int count = -1)
		{
			if (!this.labelPlural.NullOrEmpty())
			{
				return this.labelPlural;
			}
			return Find.ActiveLanguageWorker.Pluralize(this.label, count);
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x0004FE59 File Offset: 0x0004E059
		public string GetLabelGendered(Gender gender)
		{
			if (gender == Gender.Female && !this.labelFemale.NullOrEmpty())
			{
				return this.labelFemale;
			}
			if (gender == Gender.Male && !this.labelMale.NullOrEmpty())
			{
				return this.labelMale;
			}
			return this.label;
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x0004FE91 File Offset: 0x0004E091
		public RulePackDef GetNameMaker(Gender gender)
		{
			if (gender == Gender.Female && this.nameMakerFemale != null)
			{
				return this.nameMakerFemale;
			}
			if (this.nameMaker != null)
			{
				return this.nameMaker;
			}
			return null;
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x0004FEB8 File Offset: 0x0004E0B8
		public override void PostLoad()
		{
			if (this.backstoryCategories != null && this.backstoryCategories.Count > 0)
			{
				if (this.backstoryFilters == null)
				{
					this.backstoryFilters = new List<BackstoryCategoryFilter>();
				}
				this.backstoryFilters.Add(new BackstoryCategoryFilter
				{
					categories = this.backstoryCategories
				});
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				foreach (PawnKindLifeStage pawnKindLifeStage in this.lifeStages)
				{
					if (pawnKindLifeStage.swimmingGraphicData != null && pawnKindLifeStage.swimmingGraphicData.shaderType == null)
					{
						pawnKindLifeStage.swimmingGraphicData.shaderType = ShaderTypeDefOf.Transparent;
					}
					if (pawnKindLifeStage.femaleSwimmingGraphicData != null && pawnKindLifeStage.femaleSwimmingGraphicData.shaderType == null)
					{
						pawnKindLifeStage.femaleSwimmingGraphicData.shaderType = ShaderTypeDefOf.Transparent;
					}
					if (pawnKindLifeStage.stationaryGraphicData != null && pawnKindLifeStage.stationaryGraphicData.shaderType == null)
					{
						pawnKindLifeStage.stationaryGraphicData.shaderType = ShaderTypeDefOf.Transparent;
					}
					if (pawnKindLifeStage.femaleStationaryGraphicData != null && pawnKindLifeStage.femaleStationaryGraphicData.shaderType == null)
					{
						pawnKindLifeStage.femaleStationaryGraphicData.shaderType = ShaderTypeDefOf.Transparent;
					}
				}
			});
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x0004FF1C File Offset: 0x0004E11C
		public float GetAnimalPointsToHuntOrSlaughter()
		{
			return this.combatPower * 5f * (1f + this.RaceProps.manhunterOnDamageChance * 0.5f) * (1f + this.RaceProps.manhunterOnTameFailChance * 0.2f) * (1f + this.race.GetStatValueAbstract(StatDefOf.Wildness, null)) + this.race.BaseMarketValue;
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x0004FF89 File Offset: 0x0004E189
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.backstoryFilters != null && this.backstoryFiltersOverride != null)
			{
				yield return "both backstoryCategories and backstoryCategoriesOverride are defined";
			}
			if (this.race == null)
			{
				yield return "no race";
			}
			if (this.combatPower < 0f)
			{
				yield return this.defName + " has no combatPower.";
			}
			if (this.weaponMoney != FloatRange.Zero)
			{
				if (this.weaponTags == null)
				{
					yield return "weaponMoney is set but weaponTags is not.";
				}
				else
				{
					float num = 999999f;
					int i;
					Func<ThingDef, bool> <>9__0;
					int num2;
					for (i = 0; i < this.weaponTags.Count; i = num2 + 1)
					{
						IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
						Func<ThingDef, bool> predicate;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((ThingDef d) => d.weaponTags != null && d.weaponTags.Contains(this.weaponTags[i])));
						}
						IEnumerable<ThingDef> source = allDefs.Where(predicate);
						if (source.Any<ThingDef>())
						{
							num = Mathf.Min(num, source.Min(new Func<ThingDef, float>(PawnWeaponGenerator.CheapestNonDerpPriceFor)));
						}
						num2 = i;
					}
					if (num < 999999f && num > this.weaponMoney.min)
					{
						yield return string.Concat(new string[]
						{
							"Cheapest weapon with one of my weaponTags costs ",
							num.ToString(),
							" but weaponMoney min is ",
							this.weaponMoney.min.ToString(),
							", so could end up weaponless."
						});
					}
				}
			}
			if (!this.RaceProps.Humanlike && this.lifeStages.Count != this.RaceProps.lifeStageAges.Count)
			{
				yield return "PawnKindDef defines " + this.lifeStages.Count.ToString() + " lifeStages while race def defines " + this.RaceProps.lifeStageAges.Count.ToString();
			}
			if (this.apparelRequired != null)
			{
				int num2;
				for (int k = 0; k < this.apparelRequired.Count; k = num2 + 1)
				{
					for (int j = k + 1; j < this.apparelRequired.Count; j = num2 + 1)
					{
						if (!ApparelUtility.CanWearTogether(this.apparelRequired[k], this.apparelRequired[j], this.race.race.body))
						{
							string[] array = new string[5];
							array[0] = "required apparel can't be worn together (";
							int num3 = 1;
							ThingDef thingDef = this.apparelRequired[k];
							array[num3] = ((thingDef != null) ? thingDef.ToString() : null);
							array[2] = ", ";
							int num4 = 3;
							ThingDef thingDef2 = this.apparelRequired[j];
							array[num4] = ((thingDef2 != null) ? thingDef2.ToString() : null);
							array[4] = ")";
							yield return string.Concat(array);
						}
						num2 = j;
					}
					num2 = k;
				}
			}
			if (this.alternateGraphics != null)
			{
				using (List<AlternateGraphic>.Enumerator enumerator2 = this.alternateGraphics.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.Weight < 0f)
						{
							yield return "alternate graphic has negative weight.";
						}
					}
				}
				List<AlternateGraphic>.Enumerator enumerator2 = default(List<AlternateGraphic>.Enumerator);
			}
			if (this.RaceProps.Humanlike && this.initialResistanceRange == null)
			{
				yield return "initial resistance range is undefined for humanlike pawn kind.";
			}
			if (this.RaceProps.Humanlike && this.initialWillRange == null)
			{
				yield return "initial will range is undefined for humanlike pawn kind.";
			}
			if (this.startingHediffs != null)
			{
				foreach (StartingHediff startingHediff in this.startingHediffs)
				{
					if (startingHediff.durationTicksRange != null && startingHediff.def.CompProps<HediffCompProperties_Disappears>() == null)
					{
						yield return "starting hediff " + startingHediff.def.defName + " has duration ticks set but doesn't have Disappears comp.";
					}
				}
				List<StartingHediff>.Enumerator enumerator3 = default(List<StartingHediff>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x0004FF99 File Offset: 0x0004E199
		public static PawnKindDef Named(string defName)
		{
			return DefDatabase<PawnKindDef>.GetNamed(defName, true);
		}

		// Token: 0x04000E85 RID: 3717
		public ThingDef race;

		// Token: 0x04000E86 RID: 3718
		[LoadAlias("defaultFactionType")]
		public FactionDef defaultFactionDef;

		// Token: 0x04000E87 RID: 3719
		[NoTranslate]
		public List<BackstoryCategoryFilter> backstoryFilters;

		// Token: 0x04000E88 RID: 3720
		[NoTranslate]
		public List<BackstoryCategoryFilter> backstoryFiltersOverride;

		// Token: 0x04000E89 RID: 3721
		[NoTranslate]
		public List<string> backstoryCategories;

		// Token: 0x04000E8A RID: 3722
		[MustTranslate]
		public string labelPlural;

		// Token: 0x04000E8B RID: 3723
		public List<PawnKindLifeStage> lifeStages = new List<PawnKindLifeStage>();

		// Token: 0x04000E8C RID: 3724
		public List<AlternateGraphic> alternateGraphics;

		// Token: 0x04000E8D RID: 3725
		[XmlInheritanceAllowDuplicateNodes]
		public List<TraitRequirement> forcedTraits;

		// Token: 0x04000E8E RID: 3726
		[XmlInheritanceAllowDuplicateNodes]
		public List<TraitRequirement> disallowedTraitsWithDegree;

		// Token: 0x04000E8F RID: 3727
		public List<TraitDef> disallowedTraits;

		// Token: 0x04000E90 RID: 3728
		public float alternateGraphicChance;

		// Token: 0x04000E91 RID: 3729
		public MutantDef mutant;

		// Token: 0x04000E92 RID: 3730
		public XenotypeSet xenotypeSet;

		// Token: 0x04000E93 RID: 3731
		public bool useFactionXenotypes = true;

		// Token: 0x04000E94 RID: 3732
		[LoadAlias("hairTags")]
		public List<StyleItemTagWeighted> styleItemTags;

		// Token: 0x04000E95 RID: 3733
		public HairDef forcedHair;

		// Token: 0x04000E96 RID: 3734
		public Color? forcedHairColor;

		// Token: 0x04000E97 RID: 3735
		public List<MissingPart> missingParts;

		// Token: 0x04000E98 RID: 3736
		public RulePackDef nameMaker;

		// Token: 0x04000E99 RID: 3737
		public RulePackDef nameMakerFemale;

		// Token: 0x04000E9A RID: 3738
		public List<AbilityDef> abilities;

		// Token: 0x04000E9B RID: 3739
		public bool preventIdeo;

		// Token: 0x04000E9C RID: 3740
		public bool studiableAsPrisoner;

		// Token: 0x04000E9D RID: 3741
		public bool isBoss;

		// Token: 0x04000E9E RID: 3742
		public Dictionary<string, float> moveSpeedFactorByTerrainTag = new Dictionary<string, float>();

		// Token: 0x04000E9F RID: 3743
		public List<BackstoryDef> fixedChildBackstories = new List<BackstoryDef>();

		// Token: 0x04000EA0 RID: 3744
		public List<BackstoryDef> fixedAdultBackstories = new List<BackstoryDef>();

		// Token: 0x04000EA1 RID: 3745
		public float backstoryCryptosleepCommonality;

		// Token: 0x04000EA2 RID: 3746
		public FloatRange? chronologicalAgeRange;

		// Token: 0x04000EA3 RID: 3747
		public int minGenerationAge;

		// Token: 0x04000EA4 RID: 3748
		public int maxGenerationAge = 999999;

		// Token: 0x04000EA5 RID: 3749
		public bool factionLeader;

		// Token: 0x04000EA6 RID: 3750
		public Gender? fixedGender;

		// Token: 0x04000EA7 RID: 3751
		public bool allowOldAgeInjuries = true;

		// Token: 0x04000EA8 RID: 3752
		public bool generateInitialNonFamilyRelations = true;

		// Token: 0x04000EA9 RID: 3753
		public DevelopmentalStage? pawnGroupDevelopmentStage;

		// Token: 0x04000EAA RID: 3754
		public bool destroyGearOnDrop;

		// Token: 0x04000EAB RID: 3755
		public bool canStrip = true;

		// Token: 0x04000EAC RID: 3756
		public float defendPointRadius = -1f;

		// Token: 0x04000EAD RID: 3757
		public bool factionHostileOnKill;

		// Token: 0x04000EAE RID: 3758
		public bool factionHostileOnDeath;

		// Token: 0x04000EAF RID: 3759
		public bool hostileToAll;

		// Token: 0x04000EB0 RID: 3760
		public FloatRange? initialResistanceRange;

		// Token: 0x04000EB1 RID: 3761
		public FloatRange? initialWillRange;

		// Token: 0x04000EB2 RID: 3762
		public bool forceNoDeathNotification;

		// Token: 0x04000EB3 RID: 3763
		public bool skipResistant;

		// Token: 0x04000EB4 RID: 3764
		public float controlGroupPortraitZoom = 1f;

		// Token: 0x04000EB5 RID: 3765
		public float? overrideDeathOnDownedChance;

		// Token: 0x04000EB6 RID: 3766
		public bool forceDeathOnDowned;

		// Token: 0x04000EB7 RID: 3767
		public bool immuneToGameConditionEffects;

		// Token: 0x04000EB8 RID: 3768
		public bool immuneToTraps;

		// Token: 0x04000EB9 RID: 3769
		public bool collidesWithPawns = true;

		// Token: 0x04000EBA RID: 3770
		public bool ignoresPainShock;

		// Token: 0x04000EBB RID: 3771
		public bool canMeleeAttack = true;

		// Token: 0x04000EBC RID: 3772
		public float basePrisonBreakMtbDays = 60f;

		// Token: 0x04000EBD RID: 3773
		public bool useFixedRotation;

		// Token: 0x04000EBE RID: 3774
		public Rot4 fixedRotation;

		// Token: 0x04000EBF RID: 3775
		public bool showInDebugSpawner = true;

		// Token: 0x04000EC0 RID: 3776
		public bool canOpenAnyDoor;

		// Token: 0x04000EC1 RID: 3777
		public bool canOpenDoors = true;

		// Token: 0x04000EC2 RID: 3778
		[NoTranslate]
		public string overrideDebugActionCategory;

		// Token: 0x04000EC3 RID: 3779
		public float royalTitleChance;

		// Token: 0x04000EC4 RID: 3780
		public RoyalTitleDef titleRequired;

		// Token: 0x04000EC5 RID: 3781
		public RoyalTitleDef minTitleRequired;

		// Token: 0x04000EC6 RID: 3782
		public List<RoyalTitleDef> titleSelectOne;

		// Token: 0x04000EC7 RID: 3783
		public bool allowRoyalRoomRequirements = true;

		// Token: 0x04000EC8 RID: 3784
		public bool allowRoyalApparelRequirements = true;

		// Token: 0x04000EC9 RID: 3785
		public List<InfectionPathwayDef> meleeAttackInfectionPathways;

		// Token: 0x04000ECA RID: 3786
		public List<InfectionPathwayDef> rangedAttackInfectionPathways;

		// Token: 0x04000ECB RID: 3787
		public bool isFighter = true;

		// Token: 0x04000ECC RID: 3788
		public float combatPower = -1f;

		// Token: 0x04000ECD RID: 3789
		public bool canArriveManhunter = true;

		// Token: 0x04000ECE RID: 3790
		public bool canBeSapper;

		// Token: 0x04000ECF RID: 3791
		public bool isGoodBreacher;

		// Token: 0x04000ED0 RID: 3792
		public bool allowInMechClusters = true;

		// Token: 0x04000ED1 RID: 3793
		public int maxPerGroup = int.MaxValue;

		// Token: 0x04000ED2 RID: 3794
		public bool isGoodPsychicRitualInvoker;

		// Token: 0x04000ED3 RID: 3795
		public bool canBeScattered = true;

		// Token: 0x04000ED4 RID: 3796
		public bool appearsRandomlyInCombatGroups = true;

		// Token: 0x04000ED5 RID: 3797
		public bool aiAvoidCover;

		// Token: 0x04000ED6 RID: 3798
		public FloatRange fleeHealthThresholdRange = new FloatRange(-0.4f, 0.4f);

		// Token: 0x04000ED7 RID: 3799
		public float acceptArrestChanceFactor = 1f;

		// Token: 0x04000ED8 RID: 3800
		public bool canUseAvoidGrid;

		// Token: 0x04000ED9 RID: 3801
		public QualityCategory itemQuality = QualityCategory.Normal;

		// Token: 0x04000EDA RID: 3802
		public QualityCategory? forceWeaponQuality;

		// Token: 0x04000EDB RID: 3803
		public bool forceNormalGearQuality;

		// Token: 0x04000EDC RID: 3804
		public FloatRange gearHealthRange = FloatRange.One;

		// Token: 0x04000EDD RID: 3805
		public FloatRange weaponMoney = FloatRange.Zero;

		// Token: 0x04000EDE RID: 3806
		[NoTranslate]
		public List<string> weaponTags;

		// Token: 0x04000EDF RID: 3807
		public ThingDef weaponStuffOverride;

		// Token: 0x04000EE0 RID: 3808
		public ThingStyleDef weaponStyleDef;

		// Token: 0x04000EE1 RID: 3809
		public FloatRange apparelMoney = FloatRange.Zero;

		// Token: 0x04000EE2 RID: 3810
		public List<ThingDef> apparelRequired;

		// Token: 0x04000EE3 RID: 3811
		[NoTranslate]
		public List<string> apparelTags;

		// Token: 0x04000EE4 RID: 3812
		[NoTranslate]
		public List<string> apparelDisallowTags;

		// Token: 0x04000EE5 RID: 3813
		public float apparelAllowHeadgearChance = 1f;

		// Token: 0x04000EE6 RID: 3814
		public bool ignoreApparelAllowChance;

		// Token: 0x04000EE7 RID: 3815
		public bool apparelIgnoreSeasons;

		// Token: 0x04000EE8 RID: 3816
		public bool apparelIgnorePollution;

		// Token: 0x04000EE9 RID: 3817
		public bool ignoreFactionApparelStuffRequirements;

		// Token: 0x04000EEA RID: 3818
		public Color apparelColor = Color.white;

		// Token: 0x04000EEB RID: 3819
		public Color? skinColorOverride;

		// Token: 0x04000EEC RID: 3820
		public ColorDef favoriteColor;

		// Token: 0x04000EED RID: 3821
		public bool ignoreIdeoApparelColors;

		// Token: 0x04000EEE RID: 3822
		public List<SpecificApparelRequirement> specificApparelRequirements;

		// Token: 0x04000EEF RID: 3823
		public List<ThingDef> techHediffsRequired;

		// Token: 0x04000EF0 RID: 3824
		public FloatRange techHediffsMoney = FloatRange.Zero;

		// Token: 0x04000EF1 RID: 3825
		[NoTranslate]
		public List<string> techHediffsTags;

		// Token: 0x04000EF2 RID: 3826
		[NoTranslate]
		public List<string> techHediffsDisallowTags;

		// Token: 0x04000EF3 RID: 3827
		public float techHediffsChance;

		// Token: 0x04000EF4 RID: 3828
		public int techHediffsMaxAmount = 1;

		// Token: 0x04000EF5 RID: 3829
		public float biocodeWeaponChance;

		// Token: 0x04000EF6 RID: 3830
		public float humanPregnancyChance = 0.03f;

		// Token: 0x04000EF7 RID: 3831
		public List<StartingHediff> startingHediffs;

		// Token: 0x04000EF8 RID: 3832
		public float nakedChance;

		// Token: 0x04000EF9 RID: 3833
		public List<MiscDamage> existingDamage = new List<MiscDamage>();

		// Token: 0x04000EFA RID: 3834
		public QualityCategory minApparelQuality;

		// Token: 0x04000EFB RID: 3835
		public QualityCategory maxApparelQuality = QualityCategory.Excellent;

		// Token: 0x04000EFC RID: 3836
		public List<ThingDefCountClass> fixedInventory = new List<ThingDefCountClass>();

		// Token: 0x04000EFD RID: 3837
		public PawnInventoryOption inventoryOptions;

		// Token: 0x04000EFE RID: 3838
		public float invNutrition;

		// Token: 0x04000EFF RID: 3839
		public ThingDef invFoodDef;

		// Token: 0x04000F00 RID: 3840
		public float chemicalAddictionChance;

		// Token: 0x04000F01 RID: 3841
		public float combatEnhancingDrugsChance;

		// Token: 0x04000F02 RID: 3842
		public IntRange combatEnhancingDrugsCount = IntRange.Zero;

		// Token: 0x04000F03 RID: 3843
		public List<ChemicalDef> forcedAddictions = new List<ChemicalDef>();

		// Token: 0x04000F04 RID: 3844
		public bool trader;

		// Token: 0x04000F05 RID: 3845
		public List<SkillRange> skills;

		// Token: 0x04000F06 RID: 3846
		public WorkTags requiredWorkTags;

		// Token: 0x04000F07 RID: 3847
		public WorkTags disabledWorkTags;

		// Token: 0x04000F08 RID: 3848
		public int extraSkillLevels;

		// Token: 0x04000F09 RID: 3849
		public int minTotalSkillLevels;

		// Token: 0x04000F0A RID: 3850
		public int minBestSkillLevel;

		// Token: 0x04000F0B RID: 3851
		[MustTranslate]
		public string labelMale;

		// Token: 0x04000F0C RID: 3852
		[MustTranslate]
		public string labelMalePlural;

		// Token: 0x04000F0D RID: 3853
		[MustTranslate]
		public string labelFemale;

		// Token: 0x04000F0E RID: 3854
		[MustTranslate]
		public string labelFemalePlural;

		// Token: 0x04000F0F RID: 3855
		public IntRange wildGroupSize = IntRange.One;

		// Token: 0x04000F10 RID: 3856
		public float ecoSystemWeight = 1f;

		// Token: 0x04000F11 RID: 3857
		[NoTranslate]
		public string flyingAnimationFramePathPrefix;

		// Token: 0x04000F12 RID: 3858
		[NoTranslate]
		public string flyingAnimationFramePathPrefixFemale;

		// Token: 0x04000F13 RID: 3859
		public int flyingAnimationFrameCount;

		// Token: 0x04000F14 RID: 3860
		public int flyingAnimationTicksPerFrame = -1;

		// Token: 0x04000F15 RID: 3861
		public float flyingAnimationDrawSize = 1f;

		// Token: 0x04000F16 RID: 3862
		public bool flyingAnimationDrawSizeIsMultiplier;

		// Token: 0x04000F17 RID: 3863
		public bool flyingAnimationInheritColors;

		// Token: 0x04000F18 RID: 3864
		private const int MaxWeaponMoney = 999999;
	}
}
