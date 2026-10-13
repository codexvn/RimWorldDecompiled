using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B18 RID: 6936
	public class TraitDegreeData : IRenderNodePropertiesParent
	{
		// Token: 0x170017D5 RID: 6101
		// (get) Token: 0x06009AF7 RID: 39671 RVA: 0x002CFF33 File Offset: 0x002CE133
		public string LabelCap
		{
			get
			{
				if (this.cachedLabelCap == null)
				{
					this.cachedLabelCap = this.label.CapitalizeFirst();
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x170017D6 RID: 6102
		// (get) Token: 0x06009AF8 RID: 39672 RVA: 0x002CFF54 File Offset: 0x002CE154
		public TraitMentalStateGiver MentalStateGiver
		{
			get
			{
				if (this.mentalStateGiverInt == null)
				{
					this.mentalStateGiverInt = (TraitMentalStateGiver)Activator.CreateInstance(this.mentalStateGiverClass);
					this.mentalStateGiverInt.traitDegreeData = this;
				}
				return this.mentalStateGiverInt;
			}
		}

		// Token: 0x170017D7 RID: 6103
		// (get) Token: 0x06009AF9 RID: 39673 RVA: 0x002CFF86 File Offset: 0x002CE186
		public bool HasDefinedGraphicProperties
		{
			get
			{
				return !this.renderNodeProperties.NullOrEmpty<PawnRenderNodeProperties>();
			}
		}

		// Token: 0x170017D8 RID: 6104
		// (get) Token: 0x06009AFA RID: 39674 RVA: 0x002CFF96 File Offset: 0x002CE196
		public List<PawnRenderNodeProperties> RenderNodeProperties
		{
			get
			{
				return this.renderNodeProperties;
			}
		}

		// Token: 0x06009AFB RID: 39675 RVA: 0x002CFF9E File Offset: 0x002CE19E
		public string GetLabelFor(Pawn pawn)
		{
			return this.GetLabelFor((pawn != null) ? pawn.gender : Gender.None);
		}

		// Token: 0x06009AFC RID: 39676 RVA: 0x002CFFB2 File Offset: 0x002CE1B2
		public string GetLabelCapFor(Pawn pawn)
		{
			return this.GetLabelCapFor((pawn != null) ? pawn.gender : Gender.None);
		}

		// Token: 0x06009AFD RID: 39677 RVA: 0x002CFFC8 File Offset: 0x002CE1C8
		public string GetLabelFor(Gender gender)
		{
			if (gender != Gender.Male)
			{
				if (gender != Gender.Female)
				{
					return this.label;
				}
				if (!this.labelFemale.NullOrEmpty())
				{
					return this.labelFemale;
				}
				return this.label;
			}
			else
			{
				if (!this.labelMale.NullOrEmpty())
				{
					return this.labelMale;
				}
				return this.label;
			}
		}

		// Token: 0x06009AFE RID: 39678 RVA: 0x002D001C File Offset: 0x002CE21C
		public string GetLabelCapFor(Gender gender)
		{
			if (gender != Gender.Male)
			{
				if (gender != Gender.Female)
				{
					return this.LabelCap;
				}
				if (this.labelFemale.NullOrEmpty())
				{
					return this.LabelCap;
				}
				if (this.cachedLabelFemaleCap == null)
				{
					this.cachedLabelFemaleCap = this.labelFemale.CapitalizeFirst();
				}
				return this.cachedLabelFemaleCap;
			}
			else
			{
				if (this.labelMale.NullOrEmpty())
				{
					return this.LabelCap;
				}
				if (this.cachedLabelMaleCap == null)
				{
					this.cachedLabelMaleCap = this.labelMale.CapitalizeFirst();
				}
				return this.cachedLabelMaleCap;
			}
		}

		// Token: 0x06009AFF RID: 39679 RVA: 0x002D00A4 File Offset: 0x002CE2A4
		public List<IssueDef> GetAffectedIssues(TraitDef def)
		{
			if (this.affectedIssuesCached == null)
			{
				this.affectedIssuesCached = new List<IssueDef>();
				List<PreceptDef> allDefsListForReading = DefDatabase<PreceptDef>.AllDefsListForReading;
				Predicate<TraitRequirement> <>9__0;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (!this.affectedIssuesCached.Contains(allDefsListForReading[i].issue))
					{
						List<TraitRequirement> traitsAffecting = allDefsListForReading[i].TraitsAffecting;
						Predicate<TraitRequirement> predicate;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((TraitRequirement x) => x.def == def && x.degree.GetValueOrDefault() == this.degree));
						}
						if (traitsAffecting.Any(predicate))
						{
							this.affectedIssuesCached.Add(allDefsListForReading[i].issue);
						}
					}
				}
			}
			return this.affectedIssuesCached;
		}

		// Token: 0x06009B00 RID: 39680 RVA: 0x002D015C File Offset: 0x002CE35C
		public List<MemeDef> GetAffectedMemes(TraitDef def, bool agreeable)
		{
			if (agreeable)
			{
				if (this.agreeableMemesCached == null)
				{
					this.agreeableMemesCached = new List<MemeDef>();
					List<MemeDef> allDefsListForReading = DefDatabase<MemeDef>.AllDefsListForReading;
					Predicate<TraitRequirement> <>9__0;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (!allDefsListForReading[i].agreeableTraits.NullOrEmpty<TraitRequirement>())
						{
							List<TraitRequirement> agreeableTraits = allDefsListForReading[i].agreeableTraits;
							Predicate<TraitRequirement> predicate;
							if ((predicate = <>9__0) == null)
							{
								predicate = (<>9__0 = ((TraitRequirement x) => x.def == def && x.degree.GetValueOrDefault() == this.degree));
							}
							if (agreeableTraits.Any(predicate))
							{
								this.agreeableMemesCached.Add(allDefsListForReading[i]);
							}
						}
					}
				}
				return this.agreeableMemesCached;
			}
			if (this.disagreeableMemesCached == null)
			{
				this.disagreeableMemesCached = new List<MemeDef>();
				List<MemeDef> allDefsListForReading2 = DefDatabase<MemeDef>.AllDefsListForReading;
				Predicate<TraitRequirement> <>9__1;
				for (int j = 0; j < allDefsListForReading2.Count; j++)
				{
					if (!allDefsListForReading2[j].disagreeableTraits.NullOrEmpty<TraitRequirement>())
					{
						List<TraitRequirement> disagreeableTraits = allDefsListForReading2[j].disagreeableTraits;
						Predicate<TraitRequirement> predicate2;
						if ((predicate2 = <>9__1) == null)
						{
							predicate2 = (<>9__1 = ((TraitRequirement x) => x.def == def && x.degree.GetValueOrDefault() == this.degree));
						}
						if (disagreeableTraits.Any(predicate2))
						{
							this.disagreeableMemesCached.Add(allDefsListForReading2[j]);
						}
					}
				}
			}
			return this.disagreeableMemesCached;
		}

		// Token: 0x06009B01 RID: 39681 RVA: 0x002D02A4 File Offset: 0x002CE4A4
		public int AptitudeFor(SkillDef skill)
		{
			int num = 0;
			if (this.aptitudes.NullOrEmpty<Aptitude>())
			{
				return num;
			}
			for (int i = 0; i < this.aptitudes.Count; i++)
			{
				if (this.aptitudes[i].skill == skill)
				{
					num += this.aptitudes[i].level;
				}
			}
			return num;
		}

		// Token: 0x06009B02 RID: 39682 RVA: 0x002D0301 File Offset: 0x002CE501
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x06009B03 RID: 39683 RVA: 0x002D0310 File Offset: 0x002CE510
		public void ResolveReferences()
		{
			if (this.renderNodeProperties != null)
			{
				for (int i = 0; i < this.renderNodeProperties.Count; i++)
				{
					this.renderNodeProperties[i].ResolveReferencesRecursive();
				}
			}
		}

		// Token: 0x04006B16 RID: 27414
		[MustTranslate]
		public string label;

		// Token: 0x04006B17 RID: 27415
		[MustTranslate]
		public string labelMale;

		// Token: 0x04006B18 RID: 27416
		[MustTranslate]
		public string labelFemale;

		// Token: 0x04006B19 RID: 27417
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;

		// Token: 0x04006B1A RID: 27418
		[MustTranslate]
		public string description;

		// Token: 0x04006B1B RID: 27419
		public int degree;

		// Token: 0x04006B1C RID: 27420
		public float commonality = 1f;

		// Token: 0x04006B1D RID: 27421
		public List<StatModifier> statOffsets;

		// Token: 0x04006B1E RID: 27422
		public List<StatModifier> statFactors;

		// Token: 0x04006B1F RID: 27423
		public ThinkTreeDef thinkTree;

		// Token: 0x04006B20 RID: 27424
		public MentalStateDef randomMentalState;

		// Token: 0x04006B21 RID: 27425
		public SimpleCurve randomMentalStateMtbDaysMoodCurve;

		// Token: 0x04006B22 RID: 27426
		public MentalStateDef forcedMentalState;

		// Token: 0x04006B23 RID: 27427
		public float forcedMentalStateMtbDays = -1f;

		// Token: 0x04006B24 RID: 27428
		public List<MentalStateDef> disallowedMentalStates;

		// Token: 0x04006B25 RID: 27429
		public List<ThoughtDef> disallowedThoughts;

		// Token: 0x04006B26 RID: 27430
		public List<TraitIngestionThoughtsOverride> disallowedThoughtsFromIngestion;

		// Token: 0x04006B27 RID: 27431
		public List<TraitIngestionThoughtsOverride> extraThoughtsFromIngestion;

		// Token: 0x04006B28 RID: 27432
		public List<InspirationDef> disallowedInspirations;

		// Token: 0x04006B29 RID: 27433
		public List<InspirationDef> mentalBreakInspirationGainSet;

		// Token: 0x04006B2A RID: 27434
		public string mentalBreakInspirationGainReasonText;

		// Token: 0x04006B2B RID: 27435
		public List<MeditationFocusDef> allowedMeditationFocusTypes;

		// Token: 0x04006B2C RID: 27436
		public List<MeditationFocusDef> disallowedMeditationFocusTypes;

		// Token: 0x04006B2D RID: 27437
		public float mentalBreakInspirationGainChance;

		// Token: 0x04006B2E RID: 27438
		public List<MentalBreakDef> theOnlyAllowedMentalBreaks;

		// Token: 0x04006B2F RID: 27439
		public List<SkillGain> skillGains = new List<SkillGain>();

		// Token: 0x04006B30 RID: 27440
		public float socialFightChanceFactor = 1f;

		// Token: 0x04006B31 RID: 27441
		public float marketValueFactorOffset;

		// Token: 0x04006B32 RID: 27442
		public float randomDiseaseMtbDays;

		// Token: 0x04006B33 RID: 27443
		public float hungerRateFactor = 1f;

		// Token: 0x04006B34 RID: 27444
		public float painOffset;

		// Token: 0x04006B35 RID: 27445
		public float painFactor = 1f;

		// Token: 0x04006B36 RID: 27446
		public Type mentalStateGiverClass = typeof(TraitMentalStateGiver);

		// Token: 0x04006B37 RID: 27447
		public List<AbilityDef> abilities;

		// Token: 0x04006B38 RID: 27448
		public List<IngestibleModifiers> ingestibleModifiers;

		// Token: 0x04006B39 RID: 27449
		public List<Aptitude> aptitudes;

		// Token: 0x04006B3A RID: 27450
		public List<NeedDef> enablesNeeds;

		// Token: 0x04006B3B RID: 27451
		public List<NeedDef> disablesNeeds;

		// Token: 0x04006B3C RID: 27452
		private List<PawnRenderNodeProperties> renderNodeProperties;

		// Token: 0x04006B3D RID: 27453
		public List<PossessionThingDefCountClass> possessions = new List<PossessionThingDefCountClass>();

		// Token: 0x04006B3E RID: 27454
		[Unsaved(false)]
		private TraitMentalStateGiver mentalStateGiverInt;

		// Token: 0x04006B3F RID: 27455
		[Unsaved(false)]
		private string cachedLabelCap;

		// Token: 0x04006B40 RID: 27456
		[Unsaved(false)]
		private string cachedLabelMaleCap;

		// Token: 0x04006B41 RID: 27457
		[Unsaved(false)]
		private string cachedLabelFemaleCap;

		// Token: 0x04006B42 RID: 27458
		[Unsaved(false)]
		private List<IssueDef> affectedIssuesCached;

		// Token: 0x04006B43 RID: 27459
		[Unsaved(false)]
		private List<MemeDef> agreeableMemesCached;

		// Token: 0x04006B44 RID: 27460
		[Unsaved(false)]
		private List<MemeDef> disagreeableMemesCached;
	}
}
