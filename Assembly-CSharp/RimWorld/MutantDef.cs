using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AA2 RID: 6818
	public class MutantDef : Def, IRenderNodePropertiesParent
	{
		// Token: 0x17001763 RID: 5987
		// (get) Token: 0x060098A9 RID: 39081 RVA: 0x002C8513 File Offset: 0x002C6713
		public bool HasDefinedGraphicProperties
		{
			get
			{
				return !this.renderNodeProperties.NullOrEmpty<PawnRenderNodeProperties>();
			}
		}

		// Token: 0x17001764 RID: 5988
		// (get) Token: 0x060098AA RID: 39082 RVA: 0x002C8523 File Offset: 0x002C6723
		public List<PawnRenderNodeProperties> RenderNodeProperties
		{
			get
			{
				return this.renderNodeProperties;
			}
		}

		// Token: 0x17001765 RID: 5989
		// (get) Token: 0x060098AB RID: 39083 RVA: 0x002C852C File Offset: 0x002C672C
		public List<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				if (this.cachedDisabledWorkTypes == null)
				{
					this.cachedDisabledWorkTypes = new List<WorkTypeDef>();
					List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (!this.AllowsWorkType(allDefsListForReading[i]))
						{
							this.cachedDisabledWorkTypes.Add(allDefsListForReading[i]);
						}
					}
				}
				return this.cachedDisabledWorkTypes;
			}
		}

		// Token: 0x060098AC RID: 39084 RVA: 0x002C858C File Offset: 0x002C678C
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.tools != null)
			{
				for (int i = 0; i < this.tools.Count; i++)
				{
					this.tools[i].id = i.ToString();
				}
			}
		}

		// Token: 0x060098AD RID: 39085 RVA: 0x002C85D8 File Offset: 0x002C67D8
		public string GetBodyGraphicPath(Pawn pawn)
		{
			for (int i = 0; i < this.bodyTypeGraphicPaths.Count; i++)
			{
				if (this.bodyTypeGraphicPaths[i].bodyType == pawn.story.bodyType)
				{
					return this.bodyTypeGraphicPaths[i].texturePath;
				}
			}
			return null;
		}

		// Token: 0x060098AE RID: 39086 RVA: 0x002C862C File Offset: 0x002C682C
		public bool StyleItemAllowed(StyleItemDef styleItem)
		{
			if (!ModLister.AnomalyInstalled)
			{
				return true;
			}
			bool flag = styleItem is HairDef;
			bool flag2 = styleItem is BeardDef;
			if (!flag && !flag2)
			{
				return true;
			}
			if (flag)
			{
				if (this.hairTagFilter != null && !this.hairTagFilter.Allows(styleItem.styleTags))
				{
					return false;
				}
			}
			else if (flag2 && this.beardTagFilter != null && !this.beardTagFilter.Allows(styleItem.styleTags))
			{
				return false;
			}
			return true;
		}

		// Token: 0x060098AF RID: 39087 RVA: 0x002C869F File Offset: 0x002C689F
		private bool AllowsWorkType(WorkTypeDef workType)
		{
			return (this.workDisables & workType.workTags) == WorkTags.None;
		}

		// Token: 0x060098B0 RID: 39088 RVA: 0x002C86B4 File Offset: 0x002C68B4
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.renderNodeProperties != null)
			{
				for (int i = 0; i < this.renderNodeProperties.Count; i++)
				{
					this.renderNodeProperties[i].ResolveReferencesRecursive();
				}
			}
		}

		// Token: 0x0400670A RID: 26378
		public HediffDef hediff;

		// Token: 0x0400670B RID: 26379
		public ThinkTreeDef thinkTree;

		// Token: 0x0400670C RID: 26380
		public ThinkTreeDef thinkTreeConstant;

		// Token: 0x0400670D RID: 26381
		public DevelopmentalStage allowedDevelopmentalStages = DevelopmentalStage.Baby | DevelopmentalStage.Child | DevelopmentalStage.Adult;

		// Token: 0x0400670E RID: 26382
		public bool clearMutantStatusOnDeath;

		// Token: 0x0400670F RID: 26383
		public bool consideredSubhuman;

		// Token: 0x04006710 RID: 26384
		public WorkTags workDisables;

		// Token: 0x04006711 RID: 26385
		public bool disablesIdeo;

		// Token: 0x04006712 RID: 26386
		public bool disableAging;

		// Token: 0x04006713 RID: 26387
		public bool disableApparel;

		// Token: 0x04006714 RID: 26388
		public List<GeneDef> disablesGenes = new List<GeneDef>();

		// Token: 0x04006715 RID: 26389
		public bool preventsMentalBreaks;

		// Token: 0x04006716 RID: 26390
		public bool canOpenDoors = true;

		// Token: 0x04006717 RID: 26391
		public bool canOpenAnyDoor;

		// Token: 0x04006718 RID: 26392
		public bool canCarryPawns = true;

		// Token: 0x04006719 RID: 26393
		public bool canTend = true;

		// Token: 0x0400671A RID: 26394
		public bool canTravelInCaravan = true;

		// Token: 0x0400671B RID: 26395
		public bool respectsAllowedArea = true;

		// Token: 0x0400671C RID: 26396
		public bool canGainXP = true;

		// Token: 0x0400671D RID: 26397
		public bool disableEatingAtTable;

		// Token: 0x0400671E RID: 26398
		public bool disableOwnership;

		// Token: 0x0400671F RID: 26399
		public bool disablePolicies;

		// Token: 0x04006720 RID: 26400
		public bool disableTitles;

		// Token: 0x04006721 RID: 26401
		public bool disableFlying;

		// Token: 0x04006722 RID: 26402
		[MustTranslate]
		public string namePrefix = "";

		// Token: 0x04006723 RID: 26403
		public bool hideLabel;

		// Token: 0x04006724 RID: 26404
		public EntityCodexEntryDef codexEntry;

		// Token: 0x04006725 RID: 26405
		public bool showInScenarioEditor = true;

		// Token: 0x04006726 RID: 26406
		public bool overrideLabel;

		// Token: 0x04006727 RID: 26407
		public bool overrideInspectString;

		// Token: 0x04006728 RID: 26408
		public bool showJobReport = true;

		// Token: 0x04006729 RID: 26409
		public List<Type> whitelistedFloatMenuProviders;

		// Token: 0x0400672A RID: 26410
		public bool disableNeeds;

		// Token: 0x0400672B RID: 26411
		public List<NeedDef> needWhitelist;

		// Token: 0x0400672C RID: 26412
		public bool overrideFoodType;

		// Token: 0x0400672D RID: 26413
		public FoodTypeFlags foodType;

		// Token: 0x0400672E RID: 26414
		public bool allowEatingCorpses;

		// Token: 0x0400672F RID: 26415
		public bool useCorpseGraphics;

		// Token: 0x04006730 RID: 26416
		public Color? skinColorOverride;

		// Token: 0x04006731 RID: 26417
		public Color? skinColorTint;

		// Token: 0x04006732 RID: 26418
		public float skinColorTintStrength = 0.5f;

		// Token: 0x04006733 RID: 26419
		public Color? hairColorOverride;

		// Token: 0x04006734 RID: 26420
		public List<BodyTypeGraphicData> bodyTypeGraphicPaths = new List<BodyTypeGraphicData>();

		// Token: 0x04006735 RID: 26421
		public List<HeadTypeDef> forcedHeadTypes;

		// Token: 0x04006736 RID: 26422
		public TagFilter hairTagFilter;

		// Token: 0x04006737 RID: 26423
		public TagFilter beardTagFilter;

		// Token: 0x04006738 RID: 26424
		private List<PawnRenderNodeProperties> renderNodeProperties;

		// Token: 0x04006739 RID: 26425
		public AnimationDef standingAnimation;

		// Token: 0x0400673A RID: 26426
		public bool makesFootprints = true;

		// Token: 0x0400673B RID: 26427
		public bool canBleed = true;

		// Token: 0x0400673C RID: 26428
		public ThingDef bloodDef;

		// Token: 0x0400673D RID: 26429
		public ThingDef bloodSmearDef;

		// Token: 0x0400673E RID: 26430
		public bool entitledToMedicalCare = true;

		// Token: 0x0400673F RID: 26431
		public bool isImmuneToInfections;

		// Token: 0x04006740 RID: 26432
		public bool canUseDrugs;

		// Token: 0x04006741 RID: 26433
		public List<ThingDef> drugWhitelist = new List<ThingDef>();

		// Token: 0x04006742 RID: 26434
		public Color? woundColor;

		// Token: 0x04006743 RID: 26435
		public bool preventIllnesses = true;

		// Token: 0x04006744 RID: 26436
		public bool removeChronicIllnesses;

		// Token: 0x04006745 RID: 26437
		public bool removePermanentInjuries;

		// Token: 0x04006746 RID: 26438
		public bool removeAddictions;

		// Token: 0x04006747 RID: 26439
		public bool removeAllInjuries;

		// Token: 0x04006748 RID: 26440
		public bool restoreLegs;

		// Token: 0x04006749 RID: 26441
		public bool terminatePregnancy;

		// Token: 0x0400674A RID: 26442
		public bool partsCleanAndDroppable = true;

		// Token: 0x0400674B RID: 26443
		public bool clearsEgo;

		// Token: 0x0400674C RID: 26444
		public bool breathesAir = true;

		// Token: 0x0400674D RID: 26445
		public List<StartingHediff> givesHediffs = new List<StartingHediff>();

		// Token: 0x0400674E RID: 26446
		public List<HediffDef> removesHediffs = new List<HediffDef>();

		// Token: 0x0400674F RID: 26447
		public List<HediffGiver> hediffGivers;

		// Token: 0x04006750 RID: 26448
		[MustTranslate]
		public string deathLetter;

		// Token: 0x04006751 RID: 26449
		[MustTranslate]
		public string deathLetterExtra;

		// Token: 0x04006752 RID: 26450
		public SoundDef soundCall;

		// Token: 0x04006753 RID: 26451
		public SoundDef soundAttack;

		// Token: 0x04006754 RID: 26452
		public SoundDef soundWounded;

		// Token: 0x04006755 RID: 26453
		public SoundDef soundDeath;

		// Token: 0x04006756 RID: 26454
		public SoundDef soundAngry;

		// Token: 0x04006757 RID: 26455
		public List<VerbProperties> verbs;

		// Token: 0x04006758 RID: 26456
		public List<Tool> tools;

		// Token: 0x04006759 RID: 26457
		public List<AbilityDef> abilities = new List<AbilityDef>();

		// Token: 0x0400675A RID: 26458
		public List<AbilityDef> abilityWhitelist = new List<AbilityDef>();

		// Token: 0x0400675B RID: 26459
		public bool passive;

		// Token: 0x0400675C RID: 26460
		public bool canBeDrafted = true;

		// Token: 0x0400675D RID: 26461
		public bool canAttackWhileCrawling;

		// Token: 0x0400675E RID: 26462
		public float deathOnDownedChance = -1f;

		// Token: 0x0400675F RID: 26463
		public float soundAttackChance = -1f;

		// Token: 0x04006760 RID: 26464
		public List<ThingDefCountClass> killedLeavings;

		// Token: 0x04006761 RID: 26465
		public bool psychicShockUntargetable;

		// Token: 0x04006762 RID: 26466
		public bool disableHostilityResponse;

		// Token: 0x04006763 RID: 26467
		public FactionDef defaultFaction;

		// Token: 0x04006764 RID: 26468
		public bool isConsideredCorpse;

		// Token: 0x04006765 RID: 26469
		public ThoughtDef relativeTurnedThought;

		// Token: 0x04006766 RID: 26470
		public bool incapableOfSocialInteractions;

		// Token: 0x04006767 RID: 26471
		public bool tameable = true;

		// Token: 0x04006768 RID: 26472
		public bool canBeCapturedToHoldingPlatform;

		// Token: 0x04006769 RID: 26473
		public bool producesBioferrite;

		// Token: 0x0400676A RID: 26474
		public float anomalyKnowledgeOffset;

		// Token: 0x0400676B RID: 26475
		public KnowledgeCategoryDef knowledgeCategory;

		// Token: 0x0400676C RID: 26476
		private List<WorkTypeDef> cachedDisabledWorkTypes;
	}
}
