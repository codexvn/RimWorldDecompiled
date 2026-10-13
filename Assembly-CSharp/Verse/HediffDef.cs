using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000286 RID: 646
	public class HediffDef : Def, IRenderNodePropertiesParent
	{
		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06001033 RID: 4147 RVA: 0x0004C102 File Offset: 0x0004A302
		public bool HasDefinedGraphicProperties
		{
			get
			{
				return !this.renderNodeProperties.NullOrEmpty<PawnRenderNodeProperties>() || this.skinShader != null;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06001034 RID: 4148 RVA: 0x0004C11C File Offset: 0x0004A31C
		public List<PawnRenderNodeProperties> RenderNodeProperties
		{
			get
			{
				return this.renderNodeProperties ?? PawnRenderUtility.EmptyRenderNodeProperties;
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06001035 RID: 4149 RVA: 0x0004C12D File Offset: 0x0004A32D
		public bool IsAddiction
		{
			get
			{
				return typeof(Hediff_Addiction).IsAssignableFrom(this.hediffClass);
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06001036 RID: 4150 RVA: 0x0004C144 File Offset: 0x0004A344
		public bool AlwaysAllowMothball
		{
			get
			{
				if (!this.alwaysAllowMothballCached)
				{
					this.alwaysAllowMothball = true;
					if (this.comps != null && this.comps.Count > 0)
					{
						this.alwaysAllowMothball = false;
					}
					if (this.stages != null)
					{
						for (int i = 0; i < this.stages.Count; i++)
						{
							HediffStage hediffStage = this.stages[i];
							if (hediffStage.deathMtbDays > 0f || (hediffStage.hediffGivers != null && hediffStage.hediffGivers.Count > 0))
							{
								this.alwaysAllowMothball = false;
							}
						}
					}
					this.alwaysAllowMothballCached = true;
				}
				return this.alwaysAllowMothball;
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06001037 RID: 4151 RVA: 0x0004C1E4 File Offset: 0x0004A3E4
		public Hediff ConcreteExample
		{
			get
			{
				Hediff result;
				if ((result = this.concreteExampleInt) == null)
				{
					result = (this.concreteExampleInt = HediffMaker.Debug_MakeConcreteExampleHediff(this));
				}
				return result;
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06001038 RID: 4152 RVA: 0x0004C20A File Offset: 0x0004A40A
		public string Description
		{
			get
			{
				if (this.descriptionCached == null)
				{
					if (!this.descriptionShort.NullOrEmpty())
					{
						this.descriptionCached = this.descriptionShort;
					}
					else
					{
						this.descriptionCached = this.description;
					}
				}
				return this.descriptionCached;
			}
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0004C244 File Offset: 0x0004A444
		public bool HasComp(Type compClass)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].compClass == compClass)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0004C28C File Offset: 0x0004A48C
		public HediffCompProperties CompPropsFor(Type compClass)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].compClass == compClass)
					{
						return this.comps[i];
					}
				}
			}
			return null;
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x0004C2E0 File Offset: 0x0004A4E0
		public T CompProps<T>() where T : HediffCompProperties
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					T t = this.comps[i] as T;
					if (t != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x0004C338 File Offset: 0x0004A538
		public bool PossibleToDevelopImmunityNaturally()
		{
			HediffCompProperties_Immunizable hediffCompProperties_Immunizable = this.CompProps<HediffCompProperties_Immunizable>();
			return hediffCompProperties_Immunizable != null && (hediffCompProperties_Immunizable.immunityPerDayNotSick > 0f || hediffCompProperties_Immunizable.immunityPerDaySick > 0f);
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x0004C36C File Offset: 0x0004A56C
		public string PrettyTextForPart(BodyPartRecord bodyPart)
		{
			if (this.labelNounPretty.NullOrEmpty())
			{
				return null;
			}
			return this.labelNounPretty.Formatted(this.label, bodyPart.Label);
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x0004C3A4 File Offset: 0x0004A5A4
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].ResolveReferences(this);
				}
			}
			if (this.renderNodeProperties != null)
			{
				for (int j = 0; j < this.renderNodeProperties.Count; j++)
				{
					this.renderNodeProperties[j].ResolveReferencesRecursive();
				}
			}
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x0004C418 File Offset: 0x0004A618
		public int StageAtSeverity(float severity)
		{
			if (this.stages == null)
			{
				return 0;
			}
			for (int i = this.stages.Count - 1; i >= 0; i--)
			{
				if (severity >= this.stages[i].minSeverity)
				{
					return i;
				}
			}
			return 0;
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x0004C460 File Offset: 0x0004A660
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

		// Token: 0x06001041 RID: 4161 RVA: 0x0004C4BD File Offset: 0x0004A6BD
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.hediffClass == null)
			{
				yield return "hediffClass is null";
			}
			if (!this.comps.NullOrEmpty<HediffCompProperties>() && !typeof(HediffWithComps).IsAssignableFrom(this.hediffClass))
			{
				yield return "has comps but hediffClass is not HediffWithComps or subclass thereof";
			}
			if (this.minSeverity > this.initialSeverity)
			{
				yield return "minSeverity is greater than initialSeverity";
			}
			if (this.maxSeverity < this.initialSeverity)
			{
				yield return "maxSeverity is lower than initialSeverity";
			}
			if (!this.tendable && this.HasComp(typeof(HediffComp_TendDuration)))
			{
				yield return "has HediffComp_TendDuration but tendable = false";
			}
			if (string.IsNullOrEmpty(this.description))
			{
				yield return "Hediff with defName " + this.defName + " has no description!";
			}
			if (this.possiblePathways != null)
			{
				int num;
				for (int i = 0; i < this.possiblePathways.Count - 1; i = num + 1)
				{
					HediffInfectionPathway vector = this.possiblePathways[i];
					for (int k = i + 1; k < this.possiblePathways.Count; k++)
					{
						HediffInfectionPathway hediffInfectionPathway = this.possiblePathways[k];
						if (vector.PathwayDef == hediffInfectionPathway.PathwayDef)
						{
							yield return "Multiple possible infection vectors of type: " + vector.PathwayDef.defName;
							break;
						}
					}
					if (string.IsNullOrEmpty(vector.Explanation))
					{
						yield return "Missing explanation for possible infection vector: " + vector.PathwayDef.defName;
					}
					vector = null;
					num = i;
				}
			}
			if (this.comps != null)
			{
				int num;
				for (int i = 0; i < this.comps.Count; i = num + 1)
				{
					foreach (string arg in this.comps[i].ConfigErrors(this))
					{
						yield return string.Format("{0}: {1}", this.comps[i], arg);
					}
					enumerator = null;
					num = i;
				}
			}
			if (this.stages != null)
			{
				int num;
				if (!typeof(Hediff_Addiction).IsAssignableFrom(this.hediffClass))
				{
					for (int i = 0; i < this.stages.Count; i = num + 1)
					{
						if (i >= 1 && this.stages[i].minSeverity <= this.stages[i - 1].minSeverity)
						{
							yield return "stages are not in order of minSeverity";
						}
						num = i;
					}
				}
				for (int i = 0; i < this.stages.Count; i = num + 1)
				{
					if (this.stages[i].hediffGivers != null)
					{
						for (int j = 0; j < this.stages[i].hediffGivers.Count; j = num + 1)
						{
							foreach (string text2 in this.stages[i].hediffGivers[j].ConfigErrors())
							{
								yield return text2;
							}
							enumerator = null;
							num = j;
						}
					}
					if (this.stages[i].minSeverity > this.maxSeverity)
					{
						yield return string.Format("minSeverity of stage {0} is greater than maxSeverity.", i);
					}
					num = i;
				}
			}
			if (this.hediffGivers != null)
			{
				int num;
				for (int i = 0; i < this.hediffGivers.Count; i = num + 1)
				{
					foreach (string text3 in this.hediffGivers[i].ConfigErrors())
					{
						yield return text3;
					}
					enumerator = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x0004C4CD File Offset: 0x0004A6CD
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			if (this.stages != null && this.stages.Count == 1)
			{
				foreach (StatDrawEntry statDrawEntry in this.stages[0].SpecialDisplayStats())
				{
					yield return statDrawEntry;
				}
				IEnumerator<StatDrawEntry> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x0004C4E0 File Offset: 0x0004A6E0
		public bool TryGetReportStringOverrideFor(JobDef def, out string str)
		{
			if (this.reportStringOverrides.NullOrEmpty<HediffDef.ReportStringOverride>())
			{
				str = null;
				return false;
			}
			if (this.reportStringOverridesDict == null)
			{
				this.reportStringOverridesDict = new Dictionary<JobDef, string>();
				if (this.reportStringOverrides != null)
				{
					foreach (HediffDef.ReportStringOverride reportStringOverride in this.reportStringOverrides)
					{
						this.reportStringOverridesDict[reportStringOverride.jobDef] = reportStringOverride.reportString;
					}
				}
			}
			return this.reportStringOverridesDict.TryGetValue(def, out str);
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x0004C580 File Offset: 0x0004A780
		public static HediffDef Named(string defName)
		{
			return DefDatabase<HediffDef>.GetNamed(defName, true);
		}

		// Token: 0x04000CCE RID: 3278
		public Type hediffClass = typeof(Hediff);

		// Token: 0x04000CCF RID: 3279
		public List<HediffCompProperties> comps;

		// Token: 0x04000CD0 RID: 3280
		[MustTranslate]
		public string descriptionShort;

		// Token: 0x04000CD1 RID: 3281
		[NoTranslate]
		public string debugLabelExtra;

		// Token: 0x04000CD2 RID: 3282
		public float initialSeverity = 0.5f;

		// Token: 0x04000CD3 RID: 3283
		public float lethalSeverity = -1f;

		// Token: 0x04000CD4 RID: 3284
		public List<HediffStage> stages;

		// Token: 0x04000CD5 RID: 3285
		public bool tendable;

		// Token: 0x04000CD6 RID: 3286
		public bool isBad = true;

		// Token: 0x04000CD7 RID: 3287
		public ThingDef spawnThingOnRemoved;

		// Token: 0x04000CD8 RID: 3288
		public float chanceToCauseNoPain;

		// Token: 0x04000CD9 RID: 3289
		public bool canApplyDodChanceForCapacityChanges;

		// Token: 0x04000CDA RID: 3290
		public bool makesSickThought;

		// Token: 0x04000CDB RID: 3291
		public bool makesAlert = true;

		// Token: 0x04000CDC RID: 3292
		public NeedDef chemicalNeed;

		// Token: 0x04000CDD RID: 3293
		public float minSeverity;

		// Token: 0x04000CDE RID: 3294
		public float maxSeverity = float.MaxValue;

		// Token: 0x04000CDF RID: 3295
		public bool scenarioCanAdd;

		// Token: 0x04000CE0 RID: 3296
		public List<HediffGiver> hediffGivers;

		// Token: 0x04000CE1 RID: 3297
		public bool cureAllAtOnceIfCuredByItem;

		// Token: 0x04000CE2 RID: 3298
		public TaleDef taleOnVisible;

		// Token: 0x04000CE3 RID: 3299
		public bool recordDownedTale = true;

		// Token: 0x04000CE4 RID: 3300
		public bool everCurableByItem = true;

		// Token: 0x04000CE5 RID: 3301
		public List<string> tags;

		// Token: 0x04000CE6 RID: 3302
		public bool priceImpact;

		// Token: 0x04000CE7 RID: 3303
		public float priceOffset;

		// Token: 0x04000CE8 RID: 3304
		public bool chronic;

		// Token: 0x04000CE9 RID: 3305
		public bool keepOnBodyPartRestoration;

		// Token: 0x04000CEA RID: 3306
		public bool countsAsAddedPartOrImplant;

		// Token: 0x04000CEB RID: 3307
		public bool blocksSocialInteraction;

		// Token: 0x04000CEC RID: 3308
		public bool blocksSleeping;

		// Token: 0x04000CED RID: 3309
		[MustTranslate]
		public string overrideTooltip;

		// Token: 0x04000CEE RID: 3310
		[MustTranslate]
		public string extraTooltip;

		// Token: 0x04000CEF RID: 3311
		[MustTranslate]
		public string inspectString;

		// Token: 0x04000CF0 RID: 3312
		public bool levelIsQuantity;

		// Token: 0x04000CF1 RID: 3313
		public bool removeOnDeathrestStart;

		// Token: 0x04000CF2 RID: 3314
		public bool preventsCrawling;

		// Token: 0x04000CF3 RID: 3315
		public bool preventsPregnancy;

		// Token: 0x04000CF4 RID: 3316
		public bool preventsLungRot;

		// Token: 0x04000CF5 RID: 3317
		public bool pregnant;

		// Token: 0x04000CF6 RID: 3318
		public bool allowMothballIfLowPriorityWorldPawn;

		// Token: 0x04000CF7 RID: 3319
		public List<string> removeWithTags;

		// Token: 0x04000CF8 RID: 3320
		public List<BodyPartDef> onlyLifeThreateningTo;

		// Token: 0x04000CF9 RID: 3321
		public bool canAffectBionicOrImplant = true;

		// Token: 0x04000CFA RID: 3322
		public bool alwaysShowSeverity;

		// Token: 0x04000CFB RID: 3323
		public bool showGizmosOnCorpse;

		// Token: 0x04000CFC RID: 3324
		public BodyPartDef defaultInstallPart;

		// Token: 0x04000CFD RID: 3325
		public Color? hairColorOverride;

		// Token: 0x04000CFE RID: 3326
		public List<HediffInfectionPathway> possiblePathways;

		// Token: 0x04000CFF RID: 3327
		public List<InfectionPathwayDef> givesInfectionPathways;

		// Token: 0x04000D00 RID: 3328
		public bool duplicationAllowed = true;

		// Token: 0x04000D01 RID: 3329
		public bool preventsDeath;

		// Token: 0x04000D02 RID: 3330
		public List<MeditationFocusDef> allowedMeditationFocusTypes;

		// Token: 0x04000D03 RID: 3331
		public List<AbilityDef> abilities;

		// Token: 0x04000D04 RID: 3332
		public bool isInfection;

		// Token: 0x04000D05 RID: 3333
		public bool forceRemoveOnResurrection;

		// Token: 0x04000D06 RID: 3334
		public bool organicAddedBodypart;

		// Token: 0x04000D07 RID: 3335
		public bool deprioritizeHealing;

		// Token: 0x04000D08 RID: 3336
		public bool clearsEgo;

		// Token: 0x04000D09 RID: 3337
		public List<Aptitude> aptitudes;

		// Token: 0x04000D0A RID: 3338
		public SimpleCurve removeOnRedressChanceByDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, 0f),
				true
			}
		};

		// Token: 0x04000D0B RID: 3339
		public bool removeOnQuestLodgers;

		// Token: 0x04000D0C RID: 3340
		public List<PawnKindDef> removeOnRedressIfNotOfKind;

		// Token: 0x04000D0D RID: 3341
		public bool displayWound;

		// Token: 0x04000D0E RID: 3342
		public float? woundAnchorRange;

		// Token: 0x04000D0F RID: 3343
		public Color defaultLabelColor = Color.white;

		// Token: 0x04000D10 RID: 3344
		private List<PawnRenderNodeProperties> renderNodeProperties;

		// Token: 0x04000D11 RID: 3345
		public Color? skinColorOverride;

		// Token: 0x04000D12 RID: 3346
		public Color? skinColorTint;

		// Token: 0x04000D13 RID: 3347
		public float skinColorTintStrength = 0.5f;

		// Token: 0x04000D14 RID: 3348
		public ShaderTypeDef skinShader;

		// Token: 0x04000D15 RID: 3349
		public bool forceRenderTreeRecache;

		// Token: 0x04000D16 RID: 3350
		public InjuryProps injuryProps;

		// Token: 0x04000D17 RID: 3351
		public AddedBodyPartProps addedPartProps;

		// Token: 0x04000D18 RID: 3352
		private List<HediffDef.ReportStringOverride> reportStringOverrides;

		// Token: 0x04000D19 RID: 3353
		[MustTranslate]
		public string labelNoun;

		// Token: 0x04000D1A RID: 3354
		[MustTranslate]
		public string battleStateLabel;

		// Token: 0x04000D1B RID: 3355
		[MustTranslate]
		public string labelNounPretty;

		// Token: 0x04000D1C RID: 3356
		[MustTranslate]
		public string targetPrefix;

		// Token: 0x04000D1D RID: 3357
		private bool alwaysAllowMothballCached;

		// Token: 0x04000D1E RID: 3358
		private bool alwaysAllowMothball;

		// Token: 0x04000D1F RID: 3359
		private string descriptionCached;

		// Token: 0x04000D20 RID: 3360
		private Dictionary<JobDef, string> reportStringOverridesDict;

		// Token: 0x04000D21 RID: 3361
		private Hediff concreteExampleInt;

		// Token: 0x02000287 RID: 647
		private class ReportStringOverride
		{
			// Token: 0x04000D22 RID: 3362
			public JobDef jobDef;

			// Token: 0x04000D23 RID: 3363
			[MustTranslate]
			public string reportString;
		}
	}
}
