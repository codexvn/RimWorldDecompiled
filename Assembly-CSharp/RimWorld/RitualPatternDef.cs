using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002782 RID: 10114
	public class RitualPatternDef : Def
	{
		// Token: 0x170021EB RID: 8683
		// (get) Token: 0x0600DDB9 RID: 56761 RVA: 0x00414294 File Offset: 0x00412494
		public Texture2D Icon
		{
			get
			{
				if (this.icon == null)
				{
					if (this.iconPathOverride != null)
					{
						this.icon = ContentFinder<Texture2D>.Get(this.iconPathOverride, true);
					}
					else
					{
						this.icon = null;
					}
				}
				return this.icon;
			}
		}

		// Token: 0x0600DDBA RID: 56762 RVA: 0x004142D0 File Offset: 0x004124D0
		public void Fill(Precept_Ritual ritual)
		{
			ritual.nameMaker = this.nameMaker;
			if (!this.ritualObligationTriggers.NullOrEmpty<RitualObligationTriggerProperties>())
			{
				ritual.obligationTriggers.Clear();
				foreach (RitualObligationTriggerProperties ritualObligationTriggerProperties in this.ritualObligationTriggers)
				{
					RitualObligationTrigger instance = ritualObligationTriggerProperties.GetInstance(ritual);
					ritual.obligationTriggers.Add(instance);
					instance.Init(ritualObligationTriggerProperties);
				}
			}
			if (this.ritualOutcomeEffect != null)
			{
				ritual.outcomeEffect = this.ritualOutcomeEffect.GetInstance();
			}
			Precept_Ritual ritual2 = ritual;
			RitualObligationTargetFilterDef ritualObligationTargetFilterDef = this.ritualObligationTargetFilter;
			ritual2.obligationTargetFilter = ((ritualObligationTargetFilterDef != null) ? ritualObligationTargetFilterDef.GetInstance() : null);
			if (ritual.obligationTargetFilter != null)
			{
				ritual.obligationTargetFilter.parent = ritual;
			}
			Precept_Ritual ritual3 = ritual;
			RitualTargetFilterDef ritualTargetFilterDef = this.ritualTargetFilter;
			ritual3.targetFilter = ((ritualTargetFilterDef != null) ? ritualTargetFilterDef.GetInstance() : null);
			ritual.behavior = this.ritualBehavior.GetInstance();
			ritual.ritualOnlyForIdeoMembers = this.ritualOnlyForIdeoMembers;
			ritual.ritualExpectedDesc = this.ritualExpectedDesc;
			ritual.ritualExpectedDescNoAdjective = this.ritualExpectedDescNoAdjective;
			ritual.descOverride = this.descOverride;
			ritual.shortDescOverride = this.shortDescOverride;
			ritual.iconPathOverride = this.iconPathOverride;
			ritual.cancelIconPathOverride = this.cancelIconPathOverride;
			ritual.patternGroupTag = this.patternGroupTag;
			ritual.minTechLevel = this.minTechLevel;
			ritual.maxTechLevel = this.maxTechLevel;
			ritual.showIdeoIconsInDialog = this.showIdeoIconsInDialog;
			ritual.playsIdeoMusic = this.playsIdeoMusic;
			ritual.ritualExplanation = this.ritualExplanation;
			ritual.canBeAnytime = this.canStartAnytime;
			ritual.allowOtherInstances = this.allowOtherInstances;
			ritual.mergeGizmosForObligations = this.mergeGizmosForObligations;
			ritual.canMergeGizmosFromDifferentIdeos = this.canMergeGizmosFromDifferentIdeos;
			ritual.layerWhitelist = this.layerWhitelist;
			ritual.layerBlacklist = this.layerBlacklist;
			ritual.ignoreExtremeTemperatures = this.ignoreExtremeTemperatures;
			if (this.alwaysStartAnytime)
			{
				ritual.isAnytime = true;
			}
			else if (this.canStartAnytime)
			{
				Precept_Ritual precept_Ritual = ritual.ideo.PreceptsListForReading.OfType<Precept_Ritual>().FirstOrDefault((Precept_Ritual p) => p != ritual && p.behavior != null && p.behavior.def == this.ritualBehavior);
				int num = ritual.ideo.PreceptsListForReading.OfType<Precept_Ritual>().Count((Precept_Ritual r) => r != ritual && r.isAnytime && r.def.ritualPatternBase != null && r.def.ritualPatternBase.canStartAnytime);
				int num2 = ritual.ideo.PreceptsListForReading.OfType<Precept_Ritual>().Count((Precept_Ritual r) => r != ritual && !r.isAnytime && r.def.ritualPatternBase != null && r.def.ritualPatternBase.canStartAnytime);
				if (precept_Ritual != null)
				{
					ritual.isAnytime = false;
					precept_Ritual.isAnytime = false;
				}
				else if (num != num2)
				{
					ritual.isAnytime = (num2 > num);
				}
				else
				{
					ritual.isAnytime = Rand.Bool;
				}
			}
			else
			{
				ritual.isAnytime = false;
			}
			if (ritual.SupportsAttachableOutcomeEffect)
			{
				ritual.attachableOutcomeEffect = (from d in DefDatabase<RitualAttachableOutcomeEffectDef>.AllDefs
				where base.<Fill>g__ValidateAttachedOutcome|0(d, false)
				select d).RandomElementWithFallback(null);
				if (ritual.attachableOutcomeEffect == null)
				{
					ritual.attachableOutcomeEffect = (from d in DefDatabase<RitualAttachableOutcomeEffectDef>.AllDefs
					where base.<Fill>g__ValidateAttachedOutcome|0(d, true)
					select d).RandomElementWithFallback(null);
				}
			}
			ritual.generatedAttachedReward = true;
			ritual.sourcePattern = this;
		}

		// Token: 0x0600DDBB RID: 56763 RVA: 0x004146D8 File Offset: 0x004128D8
		public bool CanFactionUse(FactionDef faction)
		{
			return faction == null || (Find.IdeoManager != null && Find.IdeoManager.classicMode) || RitualPatternDef.CanUseWithTechLevel(faction.techLevel, this.minTechLevel, this.maxTechLevel);
		}

		// Token: 0x0600DDBC RID: 56764 RVA: 0x00414709 File Offset: 0x00412909
		public static bool CanUseWithTechLevel(TechLevel level, TechLevel min, TechLevel max)
		{
			return min <= level && (max == TechLevel.Undefined || max >= level);
		}

		// Token: 0x0400956B RID: 38251
		public RulePackDef nameMaker;

		// Token: 0x0400956C RID: 38252
		public FloatRange ritualFreeStartIntervalDaysRange;

		// Token: 0x0400956D RID: 38253
		public List<RitualObligationTriggerProperties> ritualObligationTriggers;

		// Token: 0x0400956E RID: 38254
		public RitualObligationTargetFilterDef ritualObligationTargetFilter;

		// Token: 0x0400956F RID: 38255
		public RitualTargetFilterDef ritualTargetFilter;

		// Token: 0x04009570 RID: 38256
		public RitualBehaviorDef ritualBehavior;

		// Token: 0x04009571 RID: 38257
		public RitualOutcomeEffectDef ritualOutcomeEffect;

		// Token: 0x04009572 RID: 38258
		public bool ritualOnlyForIdeoMembers = true;

		// Token: 0x04009573 RID: 38259
		public bool canStartAnytime;

		// Token: 0x04009574 RID: 38260
		public bool alwaysStartAnytime;

		// Token: 0x04009575 RID: 38261
		public bool allowOtherInstances;

		// Token: 0x04009576 RID: 38262
		public bool playsIdeoMusic = true;

		// Token: 0x04009577 RID: 38263
		public bool ignoreConsumableBuildingRequirement;

		// Token: 0x04009578 RID: 38264
		public bool mergeGizmosForObligations;

		// Token: 0x04009579 RID: 38265
		public bool canMergeGizmosFromDifferentIdeos = true;

		// Token: 0x0400957A RID: 38266
		public bool ignoreExtremeTemperatures;

		// Token: 0x0400957B RID: 38267
		public TechLevel minTechLevel;

		// Token: 0x0400957C RID: 38268
		public TechLevel maxTechLevel;

		// Token: 0x0400957D RID: 38269
		public bool showIdeoIconsInDialog = true;

		// Token: 0x0400957E RID: 38270
		[MustTranslate]
		public string ritualExpectedDesc;

		// Token: 0x0400957F RID: 38271
		[MustTranslate]
		public string ritualExpectedDescNoAdjective;

		// Token: 0x04009580 RID: 38272
		[MustTranslate]
		public string shortDescOverride;

		// Token: 0x04009581 RID: 38273
		[MustTranslate]
		public string descOverride;

		// Token: 0x04009582 RID: 38274
		[MustTranslate]
		public string ritualExplanation;

		// Token: 0x04009583 RID: 38275
		[MustTranslate]
		public string beginRitualOverride;

		// Token: 0x04009584 RID: 38276
		[NoTranslate]
		public string iconPathOverride;

		// Token: 0x04009585 RID: 38277
		[NoTranslate]
		public string cancelIconPathOverride;

		// Token: 0x04009586 RID: 38278
		[NoTranslate]
		public List<string> tags;

		// Token: 0x04009587 RID: 38279
		[NoTranslate]
		public string patternGroupTag;

		// Token: 0x04009588 RID: 38280
		public List<PlanetLayerDef> layerWhitelist;

		// Token: 0x04009589 RID: 38281
		public List<PlanetLayerDef> layerBlacklist;

		// Token: 0x0400958A RID: 38282
		private Texture2D icon;
	}
}
