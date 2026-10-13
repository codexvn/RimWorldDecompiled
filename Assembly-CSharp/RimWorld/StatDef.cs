using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AEB RID: 6891
	public class StatDef : Def
	{
		// Token: 0x1700179D RID: 6045
		// (get) Token: 0x060099F6 RID: 39414 RVA: 0x002CC9FC File Offset: 0x002CABFC
		public StatWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					if (this.parts != null)
					{
						for (int i = 0; i < this.parts.Count; i++)
						{
							this.parts[i].parentStat = this;
						}
					}
					this.workerInt = (StatWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.InitSetStat(this);
				}
				return this.workerInt;
			}
		}

		// Token: 0x1700179E RID: 6046
		// (get) Token: 0x060099F7 RID: 39415 RVA: 0x002CCA69 File Offset: 0x002CAC69
		public ToStringStyle ToStringStyleUnfinalized
		{
			get
			{
				if (this.toStringStyleUnfinalized == null)
				{
					return this.toStringStyle;
				}
				return this.toStringStyleUnfinalized.Value;
			}
		}

		// Token: 0x1700179F RID: 6047
		// (get) Token: 0x060099F8 RID: 39416 RVA: 0x002CCA8A File Offset: 0x002CAC8A
		public string LabelForFullStatList
		{
			get
			{
				if (!this.labelForFullStatList.NullOrEmpty())
				{
					return this.labelForFullStatList;
				}
				return this.label;
			}
		}

		// Token: 0x170017A0 RID: 6048
		// (get) Token: 0x060099F9 RID: 39417 RVA: 0x002CCAA6 File Offset: 0x002CACA6
		public string LabelForFullStatListCap
		{
			get
			{
				return this.LabelForFullStatList.CapitalizeFirst(this);
			}
		}

		// Token: 0x170017A1 RID: 6049
		// (get) Token: 0x060099FA RID: 39418 RVA: 0x002CCAB4 File Offset: 0x002CACB4
		public string OffsetLabel
		{
			get
			{
				return this.offsetLabel ?? this.label;
			}
		}

		// Token: 0x170017A2 RID: 6050
		// (get) Token: 0x060099FB RID: 39419 RVA: 0x002CCAC6 File Offset: 0x002CACC6
		public string OffsetLabelCap
		{
			get
			{
				return this.OffsetLabel.CapitalizeFirst();
			}
		}

		// Token: 0x060099FC RID: 39420 RVA: 0x002CCAD3 File Offset: 0x002CACD3
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.capacityFactors != null)
			{
				using (List<PawnCapacityFactor>.Enumerator enumerator2 = this.capacityFactors.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.weight > 1f)
						{
							yield return this.defName + " has activity factor with weight > 1";
						}
					}
				}
				List<PawnCapacityFactor>.Enumerator enumerator2 = default(List<PawnCapacityFactor>.Enumerator);
			}
			if (this.parts != null)
			{
				int num;
				for (int i = 0; i < this.parts.Count; i = num + 1)
				{
					foreach (string text2 in this.parts[i].ConfigErrors())
					{
						yield return string.Concat(new string[]
						{
							this.defName,
							" has error in StatPart ",
							this.parts[i].ToString(),
							": ",
							text2
						});
					}
					enumerator = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x060099FD RID: 39421 RVA: 0x002CCAE3 File Offset: 0x002CACE3
		public string ValueToString(float val, ToStringNumberSense numberSense = ToStringNumberSense.Absolute, bool finalized = true)
		{
			return this.Worker.ValueToString(val, finalized, numberSense);
		}

		// Token: 0x060099FE RID: 39422 RVA: 0x002CCAF3 File Offset: 0x002CACF3
		public static StatDef Named(string defName)
		{
			return DefDatabase<StatDef>.GetNamed(defName, true);
		}

		// Token: 0x060099FF RID: 39423 RVA: 0x002CCAFC File Offset: 0x002CACFC
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.parts != null)
			{
				List<StatPart> partsCopy = this.parts.ToList<StatPart>();
				this.parts.SortBy((StatPart x) => -x.priority, (StatPart x) => partsCopy.IndexOf(x));
			}
		}

		// Token: 0x06009A00 RID: 39424 RVA: 0x002CCB64 File Offset: 0x002CAD64
		public T GetStatPart<T>() where T : StatPart
		{
			return this.parts.OfType<T>().FirstOrDefault<T>();
		}

		// Token: 0x06009A01 RID: 39425 RVA: 0x002CCB78 File Offset: 0x002CAD78
		public bool CanShowWithLoadedMods()
		{
			if (!this.showIfModsLoaded.NullOrEmpty<string>())
			{
				for (int i = 0; i < this.showIfModsLoaded.Count; i++)
				{
					if (!ModsConfig.IsActive(this.showIfModsLoaded[i]))
					{
						return false;
					}
				}
			}
			if (!this.showIfModsLoadedAny.NullOrEmpty<string>())
			{
				bool result = false;
				for (int j = 0; j < this.showIfModsLoadedAny.Count; j++)
				{
					if (ModsConfig.IsActive(this.showIfModsLoadedAny[j]))
					{
						result = true;
						break;
					}
				}
				return result;
			}
			return true;
		}

		// Token: 0x06009A02 RID: 39426 RVA: 0x002CCBFC File Offset: 0x002CADFC
		private static void PopulateMutableStats()
		{
			StatDef.mutableStats = new HashSet<StatDef>();
			foreach (TraitDef traitDef in DefDatabase<TraitDef>.AllDefsListForReading)
			{
				foreach (TraitDegreeData traitDegreeData in traitDef.degreeDatas)
				{
					StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(traitDegreeData.statOffsets);
					StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(traitDegreeData.statFactors);
				}
			}
			foreach (HediffDef hediffDef in DefDatabase<HediffDef>.AllDefsListForReading)
			{
				if (hediffDef.stages != null)
				{
					foreach (HediffStage hediffStage in hediffDef.stages)
					{
						StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(hediffStage.statOffsets);
						StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(hediffStage.statFactors);
						if (hediffStage.statOffsetEffectMultiplier != null)
						{
							StatDef.mutableStats.Add(hediffStage.statOffsetEffectMultiplier);
						}
						if (hediffStage.statFactorEffectMultiplier != null)
						{
							StatDef.mutableStats.Add(hediffStage.statFactorEffectMultiplier);
						}
						if (hediffStage.capacityFactorEffectMultiplier != null)
						{
							StatDef.mutableStats.Add(hediffStage.capacityFactorEffectMultiplier);
						}
					}
				}
			}
			foreach (PreceptDef preceptDef in DefDatabase<PreceptDef>.AllDefsListForReading)
			{
				StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(preceptDef.statOffsets);
				StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(preceptDef.statFactors);
				if (preceptDef.roleEffects != null)
				{
					foreach (RoleEffect roleEffect in preceptDef.roleEffects)
					{
						RoleEffect_PawnStatOffset roleEffect_PawnStatOffset = roleEffect as RoleEffect_PawnStatOffset;
						if (roleEffect_PawnStatOffset != null)
						{
							StatDef.mutableStats.Add(roleEffect_PawnStatOffset.statDef);
						}
						RoleEffect_PawnStatFactor roleEffect_PawnStatFactor = roleEffect as RoleEffect_PawnStatFactor;
						if (roleEffect_PawnStatFactor != null)
						{
							StatDef.mutableStats.Add(roleEffect_PawnStatFactor.statDef);
						}
					}
				}
				if (preceptDef.abilityStatFactors != null)
				{
					foreach (AbilityStatModifiers abilityStatModifiers in preceptDef.abilityStatFactors)
					{
						StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(abilityStatModifiers.modifiers);
					}
				}
			}
			foreach (GeneDef geneDef in DefDatabase<GeneDef>.AllDefsListForReading)
			{
				StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(geneDef.statOffsets);
				StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(geneDef.statFactors);
				if (geneDef.conditionalStatAffecters != null)
				{
					foreach (ConditionalStatAffecter conditionalStatAffecter in geneDef.conditionalStatAffecters)
					{
						StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(conditionalStatAffecter.statOffsets);
						StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(conditionalStatAffecter.statFactors);
					}
				}
			}
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
			{
				StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(thingDef.equippedStatOffsets);
				if (thingDef.HasAssignableCompFrom(typeof(CompFacility)))
				{
					CompProperties_Facility compProperties = thingDef.GetCompProperties<CompProperties_Facility>();
					if (((compProperties != null) ? compProperties.statOffsets : null) != null)
					{
						StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(compProperties.statOffsets);
					}
					CompProperties_FacilityQualityBased compProperties_FacilityQualityBased = compProperties as CompProperties_FacilityQualityBased;
					if (compProperties_FacilityQualityBased != null)
					{
						foreach (StatDef item in compProperties_FacilityQualityBased.statOffsetsPerQuality.Keys)
						{
							StatDef.mutableStats.Add(item);
						}
					}
				}
			}
			foreach (WeaponTraitDef weaponTraitDef in DefDatabase<WeaponTraitDef>.AllDefsListForReading)
			{
				StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(weaponTraitDef.equippedStatOffsets);
			}
			foreach (LifeStageDef lifeStageDef in DefDatabase<LifeStageDef>.AllDefsListForReading)
			{
				StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(lifeStageDef.statFactors);
				StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(lifeStageDef.statOffsets);
			}
			foreach (InspirationDef inspirationDef in DefDatabase<InspirationDef>.AllDefsListForReading)
			{
				StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(inspirationDef.statOffsets);
				StatDef.<PopulateMutableStats>g__AddStatsFromModifiers|84_0(inspirationDef.statFactors);
			}
		}

		// Token: 0x06009A03 RID: 39427 RVA: 0x002CD12C File Offset: 0x002CB32C
		public bool IsImmutable()
		{
			return !(this.workerClass != typeof(StatWorker)) && this.skillNeedOffsets.NullOrEmpty<SkillNeed>() && this.skillNeedFactors.NullOrEmpty<SkillNeed>() && this.capacityOffsets.NullOrEmpty<PawnCapacityOffset>() && this.capacityFactors.NullOrEmpty<PawnCapacityFactor>() && this.statFactors.NullOrEmpty<StatDef>() && this.parts.NullOrEmpty<StatPart>() && this.postProcessStatFactors.NullOrEmpty<StatDef>() && !StatDef.mutableStats.Contains(this);
		}

		// Token: 0x06009A04 RID: 39428 RVA: 0x002CD1C8 File Offset: 0x002CB3C8
		public static void SetImmutability()
		{
			StatDef.PopulateMutableStats();
			foreach (StatDef statDef in DefDatabase<StatDef>.AllDefsListForReading)
			{
				statDef.immutable = statDef.IsImmutable();
				statDef.Worker.SetCacheability(statDef.immutable);
			}
		}

		// Token: 0x06009A05 RID: 39429 RVA: 0x002CD238 File Offset: 0x002CB438
		public static void ResetStaticData()
		{
			StatDef.mutableStats = null;
			foreach (StatDef statDef in DefDatabase<StatDef>.AllDefsListForReading)
			{
				statDef.immutable = false;
				statDef.Worker.DeleteStatCache();
			}
		}

		// Token: 0x06009A08 RID: 39432 RVA: 0x002CD380 File Offset: 0x002CB580
		[CompilerGenerated]
		internal static void <PopulateMutableStats>g__AddStatsFromModifiers|84_0(List<StatModifier> mods)
		{
			if (mods != null)
			{
				StatDef.mutableStats.AddRange(from mod in mods
				select mod.stat);
			}
		}

		// Token: 0x040069B3 RID: 27059
		private static HashSet<StatDef> mutableStats;

		// Token: 0x040069B4 RID: 27060
		public StatCategoryDef category;

		// Token: 0x040069B5 RID: 27061
		public Type workerClass = typeof(StatWorker);

		// Token: 0x040069B6 RID: 27062
		public string labelForFullStatList;

		// Token: 0x040069B7 RID: 27063
		public bool forInformationOnly;

		// Token: 0x040069B8 RID: 27064
		[MustTranslate]
		private string offsetLabel;

		// Token: 0x040069B9 RID: 27065
		public float hideAtValue = -2.1474836E+09f;

		// Token: 0x040069BA RID: 27066
		public bool alwaysHide;

		// Token: 0x040069BB RID: 27067
		public bool showNonAbstract = true;

		// Token: 0x040069BC RID: 27068
		public bool showIfUndefined = true;

		// Token: 0x040069BD RID: 27069
		public bool showOnPawns = true;

		// Token: 0x040069BE RID: 27070
		public bool showOnHumanlikes = true;

		// Token: 0x040069BF RID: 27071
		public bool showOnNonWildManHumanlikes = true;

		// Token: 0x040069C0 RID: 27072
		public bool showOnAnimals = true;

		// Token: 0x040069C1 RID: 27073
		public bool showOnMechanoids = true;

		// Token: 0x040069C2 RID: 27074
		public bool showOnNonWorkTables = true;

		// Token: 0x040069C3 RID: 27075
		public bool showOnEntities = true;

		// Token: 0x040069C4 RID: 27076
		public bool showOnDrones = true;

		// Token: 0x040069C5 RID: 27077
		public bool showOnNonPowerPlants = true;

		// Token: 0x040069C6 RID: 27078
		public bool showOnDefaultValue = true;

		// Token: 0x040069C7 RID: 27079
		public bool showOnUnhaulables = true;

		// Token: 0x040069C8 RID: 27080
		public bool showOnUntradeables = true;

		// Token: 0x040069C9 RID: 27081
		public List<string> showIfModsLoaded;

		// Token: 0x040069CA RID: 27082
		public List<string> showIfModsLoadedAny;

		// Token: 0x040069CB RID: 27083
		public List<HediffDef> showIfHediffsPresent;

		// Token: 0x040069CC RID: 27084
		public bool neverDisabled;

		// Token: 0x040069CD RID: 27085
		public bool showZeroBaseValue;

		// Token: 0x040069CE RID: 27086
		public bool showOnSlavesOnly;

		// Token: 0x040069CF RID: 27087
		public bool showOnPlayerMechanoids;

		// Token: 0x040069D0 RID: 27088
		public DevelopmentalStage showDevelopmentalStageFilter = DevelopmentalStage.Baby | DevelopmentalStage.Child | DevelopmentalStage.Adult;

		// Token: 0x040069D1 RID: 27089
		public bool hideInClassicMode;

		// Token: 0x040069D2 RID: 27090
		public List<PawnKindDef> showOnPawnKind;

		// Token: 0x040069D3 RID: 27091
		public bool overridesHideStats;

		// Token: 0x040069D4 RID: 27092
		public int displayPriorityInCategory;

		// Token: 0x040069D5 RID: 27093
		public ToStringNumberSense toStringNumberSense = ToStringNumberSense.Absolute;

		// Token: 0x040069D6 RID: 27094
		public ToStringStyle toStringStyle;

		// Token: 0x040069D7 RID: 27095
		private ToStringStyle? toStringStyleUnfinalized;

		// Token: 0x040069D8 RID: 27096
		[MustTranslate]
		public string formatString;

		// Token: 0x040069D9 RID: 27097
		[MustTranslate]
		public string formatStringUnfinalized;

		// Token: 0x040069DA RID: 27098
		public bool finalizeEquippedStatOffset = true;

		// Token: 0x040069DB RID: 27099
		[MustTranslate]
		public string statFactorsExplanationHeader;

		// Token: 0x040069DC RID: 27100
		public float defaultBaseValue = 1f;

		// Token: 0x040069DD RID: 27101
		public List<SkillNeed> skillNeedOffsets;

		// Token: 0x040069DE RID: 27102
		public float noSkillOffset;

		// Token: 0x040069DF RID: 27103
		public List<PawnCapacityOffset> capacityOffsets;

		// Token: 0x040069E0 RID: 27104
		public List<StatDef> statFactors;

		// Token: 0x040069E1 RID: 27105
		public bool applyFactorsIfNegative = true;

		// Token: 0x040069E2 RID: 27106
		public List<SkillNeed> skillNeedFactors;

		// Token: 0x040069E3 RID: 27107
		public float noSkillFactor = 1f;

		// Token: 0x040069E4 RID: 27108
		public List<PawnCapacityFactor> capacityFactors;

		// Token: 0x040069E5 RID: 27109
		public SimpleCurve postProcessCurve;

		// Token: 0x040069E6 RID: 27110
		public List<StatDef> postProcessStatFactors;

		// Token: 0x040069E7 RID: 27111
		public float minValue = -9999999f;

		// Token: 0x040069E8 RID: 27112
		public float maxValue = 9999999f;

		// Token: 0x040069E9 RID: 27113
		public float valueIfMissing;

		// Token: 0x040069EA RID: 27114
		public bool roundValue;

		// Token: 0x040069EB RID: 27115
		public float roundToFiveOver = float.MaxValue;

		// Token: 0x040069EC RID: 27116
		public bool minifiedThingInherits;

		// Token: 0x040069ED RID: 27117
		public bool supressDisabledError;

		// Token: 0x040069EE RID: 27118
		public bool cacheable;

		// Token: 0x040069EF RID: 27119
		public bool displayMaxWhenAboveOrEqual;

		// Token: 0x040069F0 RID: 27120
		public bool scenarioRandomizable;

		// Token: 0x040069F1 RID: 27121
		public SkillDef disableIfSkillDisabled;

		// Token: 0x040069F2 RID: 27122
		public List<StatPart> parts;

		// Token: 0x040069F3 RID: 27123
		[Unsaved(false)]
		private StatWorker workerInt;

		// Token: 0x040069F4 RID: 27124
		[Unsaved(false)]
		public bool immutable;
	}
}
