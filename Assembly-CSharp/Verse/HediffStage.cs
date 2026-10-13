using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200028F RID: 655
	public class HediffStage
	{
		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06001064 RID: 4196 RVA: 0x0004D0AC File Offset: 0x0004B2AC
		public bool AffectsMemory
		{
			get
			{
				return this.forgetMemoryThoughtMtbDays > 0f || this.pctConditionalThoughtsNullified > 0f;
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06001065 RID: 4197 RVA: 0x0004D0CA File Offset: 0x0004B2CA
		public bool AffectsSocialInteractions
		{
			get
			{
				return this.opinionOfOthersFactor != 1f;
			}
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x0004D0DC File Offset: 0x0004B2DC
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x0004D0EA File Offset: 0x0004B2EA
		public IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			return HediffStatsUtility.SpecialDisplayStats(this, null);
		}

		// Token: 0x04000D48 RID: 3400
		public float minSeverity;

		// Token: 0x04000D49 RID: 3401
		[MustTranslate]
		public string label;

		// Token: 0x04000D4A RID: 3402
		[MustTranslate]
		public string overrideLabel;

		// Token: 0x04000D4B RID: 3403
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;

		// Token: 0x04000D4C RID: 3404
		public bool becomeVisible = true;

		// Token: 0x04000D4D RID: 3405
		public bool lifeThreatening;

		// Token: 0x04000D4E RID: 3406
		public TaleDef tale;

		// Token: 0x04000D4F RID: 3407
		public float vomitMtbDays = -1f;

		// Token: 0x04000D50 RID: 3408
		public float deathMtbDays = -1f;

		// Token: 0x04000D51 RID: 3409
		public bool mtbDeathDestroysBrain;

		// Token: 0x04000D52 RID: 3410
		public float painFactor = 1f;

		// Token: 0x04000D53 RID: 3411
		public float painOffset;

		// Token: 0x04000D54 RID: 3412
		public float totalBleedFactor = 1f;

		// Token: 0x04000D55 RID: 3413
		public float naturalHealingFactor = -1f;

		// Token: 0x04000D56 RID: 3414
		public float regeneration = -1f;

		// Token: 0x04000D57 RID: 3415
		public bool showRegenerationStat = true;

		// Token: 0x04000D58 RID: 3416
		public float forgetMemoryThoughtMtbDays = -1f;

		// Token: 0x04000D59 RID: 3417
		public float pctConditionalThoughtsNullified;

		// Token: 0x04000D5A RID: 3418
		public float pctAllThoughtNullification;

		// Token: 0x04000D5B RID: 3419
		public float opinionOfOthersFactor = 1f;

		// Token: 0x04000D5C RID: 3420
		public float fertilityFactor = 1f;

		// Token: 0x04000D5D RID: 3421
		public float hungerRateFactor = 1f;

		// Token: 0x04000D5E RID: 3422
		public float hungerRateFactorOffset;

		// Token: 0x04000D5F RID: 3423
		public float restFallFactor = 1f;

		// Token: 0x04000D60 RID: 3424
		public float restFallFactorOffset;

		// Token: 0x04000D61 RID: 3425
		public float socialFightChanceFactor = 1f;

		// Token: 0x04000D62 RID: 3426
		public float foodPoisoningChanceFactor = 1f;

		// Token: 0x04000D63 RID: 3427
		public float mentalBreakMtbDays = -1f;

		// Token: 0x04000D64 RID: 3428
		public string mentalBreakExplanation;

		// Token: 0x04000D65 RID: 3429
		public bool blocksMentalBreaks;

		// Token: 0x04000D66 RID: 3430
		public bool blocksInspirations;

		// Token: 0x04000D67 RID: 3431
		public float overrideMoodBase = -1f;

		// Token: 0x04000D68 RID: 3432
		public float severityGainFactor = 1f;

		// Token: 0x04000D69 RID: 3433
		public List<MentalBreakIntensity> allowedMentalBreakIntensities;

		// Token: 0x04000D6A RID: 3434
		public List<HediffDef> makeImmuneTo;

		// Token: 0x04000D6B RID: 3435
		public List<PawnCapacityModifier> capMods = new List<PawnCapacityModifier>();

		// Token: 0x04000D6C RID: 3436
		public List<HediffGiver> hediffGivers;

		// Token: 0x04000D6D RID: 3437
		public List<MentalStateGiver> mentalStateGivers;

		// Token: 0x04000D6E RID: 3438
		public List<StatModifier> statOffsets;

		// Token: 0x04000D6F RID: 3439
		public List<StatModifier> statFactors;

		// Token: 0x04000D70 RID: 3440
		public List<StatModifierBySeverity> statOffsetsBySeverity;

		// Token: 0x04000D71 RID: 3441
		public List<StatModifierBySeverity> statFactorsBySeverity;

		// Token: 0x04000D72 RID: 3442
		public List<DamageFactor> damageFactors = new List<DamageFactor>();

		// Token: 0x04000D73 RID: 3443
		public List<NeedDef> enablesNeeds;

		// Token: 0x04000D74 RID: 3444
		public List<NeedDef> disablesNeeds;

		// Token: 0x04000D75 RID: 3445
		public bool multiplyStatChangesBySeverity;

		// Token: 0x04000D76 RID: 3446
		public StatDef statOffsetEffectMultiplier;

		// Token: 0x04000D77 RID: 3447
		public StatDef statFactorEffectMultiplier;

		// Token: 0x04000D78 RID: 3448
		public StatDef capacityFactorEffectMultiplier;

		// Token: 0x04000D79 RID: 3449
		public WorkTags disabledWorkTags;

		// Token: 0x04000D7A RID: 3450
		[MustTranslate]
		public string overrideTooltip;

		// Token: 0x04000D7B RID: 3451
		[MustTranslate]
		public string extraTooltip;

		// Token: 0x04000D7C RID: 3452
		public bool blocksSleeping;

		// Token: 0x04000D7D RID: 3453
		public float partEfficiencyOffset;

		// Token: 0x04000D7E RID: 3454
		public bool partIgnoreMissingHP;

		// Token: 0x04000D7F RID: 3455
		public bool destroyPart;
	}
}
