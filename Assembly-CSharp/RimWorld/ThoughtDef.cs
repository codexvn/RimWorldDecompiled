using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B11 RID: 6929
	public class ThoughtDef : Def
	{
		// Token: 0x170017C1 RID: 6081
		// (get) Token: 0x06009ABA RID: 39610 RVA: 0x002CF230 File Offset: 0x002CD430
		public string Label
		{
			get
			{
				if (!this.label.NullOrEmpty())
				{
					return this.label;
				}
				if (!this.stages.NullOrEmpty<ThoughtStage>())
				{
					if (!this.stages[0].label.NullOrEmpty())
					{
						return this.stages[0].label;
					}
					if (!this.stages[0].labelSocial.NullOrEmpty())
					{
						return this.stages[0].labelSocial;
					}
				}
				Log.Error("Cannot get good label for ThoughtDef " + this.defName);
				return this.defName;
			}
		}

		// Token: 0x170017C2 RID: 6082
		// (get) Token: 0x06009ABB RID: 39611 RVA: 0x002CF2CD File Offset: 0x002CD4CD
		public override TaggedString LabelCap
		{
			get
			{
				if (this.Label.NullOrEmpty())
				{
					return null;
				}
				if (this.cachedLabelCap.NullOrEmpty())
				{
					this.cachedLabelCap = this.Label.CapitalizeFirst();
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x170017C3 RID: 6083
		// (get) Token: 0x06009ABC RID: 39612 RVA: 0x002CF30C File Offset: 0x002CD50C
		public int DurationTicks
		{
			get
			{
				return (int)(this.durationDays * 60000f);
			}
		}

		// Token: 0x170017C4 RID: 6084
		// (get) Token: 0x06009ABD RID: 39613 RVA: 0x002CF31C File Offset: 0x002CD51C
		public bool IsMemory
		{
			get
			{
				if (this.isMemoryCached == BoolUnknown.Unknown)
				{
					this.isMemoryCached = ((this.durationDays > 0f || typeof(Thought_Memory).IsAssignableFrom(this.thoughtClass)) ? BoolUnknown.True : BoolUnknown.False);
				}
				return this.isMemoryCached == BoolUnknown.True;
			}
		}

		// Token: 0x170017C5 RID: 6085
		// (get) Token: 0x06009ABE RID: 39614 RVA: 0x002CF369 File Offset: 0x002CD569
		public bool IsSituational
		{
			get
			{
				return this.Worker != null;
			}
		}

		// Token: 0x170017C6 RID: 6086
		// (get) Token: 0x06009ABF RID: 39615 RVA: 0x002CF374 File Offset: 0x002CD574
		public bool IsSocial
		{
			get
			{
				return typeof(ISocialThought).IsAssignableFrom(this.ThoughtClass);
			}
		}

		// Token: 0x170017C7 RID: 6087
		// (get) Token: 0x06009AC0 RID: 39616 RVA: 0x002CF38B File Offset: 0x002CD58B
		public bool RequiresSpecificTraitsDegree
		{
			get
			{
				return this.requiredTraitsDegree != int.MinValue;
			}
		}

		// Token: 0x170017C8 RID: 6088
		// (get) Token: 0x06009AC1 RID: 39617 RVA: 0x002CF39D File Offset: 0x002CD59D
		public ThoughtWorker Worker
		{
			get
			{
				if (this.workerInt == null && this.workerClass != null)
				{
					this.workerInt = (ThoughtWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x170017C9 RID: 6089
		// (get) Token: 0x06009AC2 RID: 39618 RVA: 0x002CF3DD File Offset: 0x002CD5DD
		public Type ThoughtClass
		{
			get
			{
				if (this.thoughtClass != null)
				{
					return this.thoughtClass;
				}
				if (this.IsMemory)
				{
					return typeof(Thought_Memory);
				}
				return typeof(Thought_Situational);
			}
		}

		// Token: 0x170017CA RID: 6090
		// (get) Token: 0x06009AC3 RID: 39619 RVA: 0x002CF411 File Offset: 0x002CD611
		public Texture2D Icon
		{
			get
			{
				if (this.iconInt == null)
				{
					if (this.icon == null)
					{
						return null;
					}
					this.iconInt = ContentFinder<Texture2D>.Get(this.icon, true);
				}
				return this.iconInt;
			}
		}

		// Token: 0x06009AC4 RID: 39620 RVA: 0x002CF443 File Offset: 0x002CD643
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.stages.NullOrEmpty<ThoughtStage>())
			{
				yield return "no stages";
			}
			if (this.workerClass != null && this.nextThought != null)
			{
				yield return "has a nextThought but also has a workerClass. nextThought only works for memories";
			}
			if (this.IsMemory && this.workerClass != null)
			{
				yield return "has a workerClass but is a memory. workerClass only works for situational thoughts, not memories";
			}
			if (!this.IsMemory && this.workerClass == null && this.IsSituational)
			{
				yield return "is a situational thought but has no workerClass. Situational thoughts require workerClasses to analyze the situation";
			}
			int num;
			for (int i = 0; i < this.stages.Count; i = num + 1)
			{
				if (this.stages[i] != null)
				{
					foreach (string text2 in this.stages[i].ConfigErrors())
					{
						yield return text2;
					}
					enumerator = null;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06009AC5 RID: 39621 RVA: 0x002CF453 File Offset: 0x002CD653
		public static ThoughtDef Named(string defName)
		{
			return DefDatabase<ThoughtDef>.GetNamed(defName, true);
		}

		// Token: 0x04006AB8 RID: 27320
		public Type thoughtClass;

		// Token: 0x04006AB9 RID: 27321
		public Type workerClass;

		// Token: 0x04006ABA RID: 27322
		public List<ThoughtStage> stages = new List<ThoughtStage>();

		// Token: 0x04006ABB RID: 27323
		public int stackLimit = 1;

		// Token: 0x04006ABC RID: 27324
		public float stackedEffectMultiplier = 0.75f;

		// Token: 0x04006ABD RID: 27325
		public bool stagesStack;

		// Token: 0x04006ABE RID: 27326
		public float durationDays;

		// Token: 0x04006ABF RID: 27327
		public bool invert;

		// Token: 0x04006AC0 RID: 27328
		public bool validWhileDespawned;

		// Token: 0x04006AC1 RID: 27329
		public ThoughtDef nextThought;

		// Token: 0x04006AC2 RID: 27330
		public ThoughtDef producesMemoryThought;

		// Token: 0x04006AC3 RID: 27331
		public List<TraitDef> nullifyingTraits;

		// Token: 0x04006AC4 RID: 27332
		public List<TraitDef> neverNullifyIfAnyTrait;

		// Token: 0x04006AC5 RID: 27333
		[XmlInheritanceAllowDuplicateNodes]
		public List<TraitRequirement> nullifyingTraitDegrees;

		// Token: 0x04006AC6 RID: 27334
		public List<TaleDef> nullifyingOwnTales;

		// Token: 0x04006AC7 RID: 27335
		public List<PreceptDef> nullifyingPrecepts;

		// Token: 0x04006AC8 RID: 27336
		public List<HediffDef> nullifyingHediffs;

		// Token: 0x04006AC9 RID: 27337
		public List<GeneDef> nullifyingGenes;

		// Token: 0x04006ACA RID: 27338
		public List<TraitDef> requiredTraits;

		// Token: 0x04006ACB RID: 27339
		public List<GeneDef> requiredGenes;

		// Token: 0x04006ACC RID: 27340
		public int requiredTraitsDegree = int.MinValue;

		// Token: 0x04006ACD RID: 27341
		public List<HediffDef> requiredHediffs;

		// Token: 0x04006ACE RID: 27342
		public StatDef effectMultiplyingStat;

		// Token: 0x04006ACF RID: 27343
		public SimpleCurve effectMultiplyingStatCurve;

		// Token: 0x04006AD0 RID: 27344
		public HediffDef hediff;

		// Token: 0x04006AD1 RID: 27345
		public MentalStateDef mentalState;

		// Token: 0x04006AD2 RID: 27346
		public GameConditionDef gameCondition;

		// Token: 0x04006AD3 RID: 27347
		public bool nullifiedIfNotColonist;

		// Token: 0x04006AD4 RID: 27348
		public ThoughtDef thoughtToMake;

		// Token: 0x04006AD5 RID: 27349
		[NoTranslate]
		private string icon;

		// Token: 0x04006AD6 RID: 27350
		public bool showBubble;

		// Token: 0x04006AD7 RID: 27351
		public ExpectationDef minExpectation;

		// Token: 0x04006AD8 RID: 27352
		public ExpectationDef minExpectationForNegativeThought;

		// Token: 0x04006AD9 RID: 27353
		public List<ThoughtDef> replaceThoughts;

		// Token: 0x04006ADA RID: 27354
		public DevelopmentalStage developmentalStageFilter = DevelopmentalStage.Child | DevelopmentalStage.Adult;

		// Token: 0x04006ADB RID: 27355
		public ChemicalDef chemicalDef;

		// Token: 0x04006ADC RID: 27356
		public bool lerpMoodToZero;

		// Token: 0x04006ADD RID: 27357
		public int stackLimitForSameOtherPawn = -1;

		// Token: 0x04006ADE RID: 27358
		public float lerpOpinionToZeroAfterDurationPct = 0.7f;

		// Token: 0x04006ADF RID: 27359
		public DevelopmentalStage socialTargetDevelopmentalStageFilter = DevelopmentalStage.Child | DevelopmentalStage.Adult;

		// Token: 0x04006AE0 RID: 27360
		public bool ignoreSubhumans = true;

		// Token: 0x04006AE1 RID: 27361
		public bool doNotApplyToQuestLodgers;

		// Token: 0x04006AE2 RID: 27362
		public float maxCumulatedOpinionOffset = float.MaxValue;

		// Token: 0x04006AE3 RID: 27363
		public TaleDef taleDef;

		// Token: 0x04006AE4 RID: 27364
		public Gender gender;

		// Token: 0x04006AE5 RID: 27365
		[MustTranslate]
		public string labelInBracketsExtraForHediff;

		// Token: 0x04006AE6 RID: 27366
		[Unsaved(false)]
		private ThoughtWorker workerInt;

		// Token: 0x04006AE7 RID: 27367
		[Unsaved(false)]
		private BoolUnknown isMemoryCached = BoolUnknown.Unknown;

		// Token: 0x04006AE8 RID: 27368
		private Texture2D iconInt;
	}
}
