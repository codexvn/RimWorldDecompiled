using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A29 RID: 6697
	public class FactionDef : Def
	{
		// Token: 0x17001710 RID: 5904
		// (get) Token: 0x060096E9 RID: 38633 RVA: 0x002C1BF4 File Offset: 0x002BFDF4
		public List<RoyalTitleDef> RoyalTitlesAwardableInSeniorityOrderForReading
		{
			get
			{
				if (this.royalTitlesAwardableInSeniorityOrderForReading == null)
				{
					this.royalTitlesAwardableInSeniorityOrderForReading = new List<RoyalTitleDef>();
					if (this.royalTitleTags != null && ModLister.RoyaltyInstalled)
					{
						foreach (RoyalTitleDef royalTitleDef in DefDatabase<RoyalTitleDef>.AllDefsListForReading)
						{
							if (royalTitleDef.Awardable && royalTitleDef.tags.SharesElementWith(this.royalTitleTags))
							{
								this.royalTitlesAwardableInSeniorityOrderForReading.Add(royalTitleDef);
							}
						}
						this.royalTitlesAwardableInSeniorityOrderForReading.SortBy((RoyalTitleDef t) => t.seniority);
					}
				}
				return this.royalTitlesAwardableInSeniorityOrderForReading;
			}
		}

		// Token: 0x17001711 RID: 5905
		// (get) Token: 0x060096EA RID: 38634 RVA: 0x002C1CC0 File Offset: 0x002BFEC0
		public List<RoyalTitleDef> RoyalTitlesAllInSeniorityOrderForReading
		{
			get
			{
				if (this.royalTitlesAllInSeniorityOrderForReading == null)
				{
					this.royalTitlesAllInSeniorityOrderForReading = new List<RoyalTitleDef>();
					if (this.royalTitleTags != null && ModLister.RoyaltyInstalled)
					{
						foreach (RoyalTitleDef royalTitleDef in DefDatabase<RoyalTitleDef>.AllDefsListForReading)
						{
							if (royalTitleDef.tags.SharesElementWith(this.royalTitleTags))
							{
								this.royalTitlesAllInSeniorityOrderForReading.Add(royalTitleDef);
							}
						}
						this.royalTitlesAllInSeniorityOrderForReading.SortBy((RoyalTitleDef t) => t.seniority);
					}
				}
				return this.royalTitlesAllInSeniorityOrderForReading;
			}
		}

		// Token: 0x17001712 RID: 5906
		// (get) Token: 0x060096EB RID: 38635 RVA: 0x002C1D80 File Offset: 0x002BFF80
		public RoyalTitleInheritanceWorker RoyalTitleInheritanceWorker
		{
			get
			{
				if (this.royalTitleInheritanceWorker == null && this.royalTitleInheritanceWorkerClass != null)
				{
					this.royalTitleInheritanceWorker = (RoyalTitleInheritanceWorker)Activator.CreateInstance(this.royalTitleInheritanceWorkerClass);
				}
				return this.royalTitleInheritanceWorker;
			}
		}

		// Token: 0x17001713 RID: 5907
		// (get) Token: 0x060096EC RID: 38636 RVA: 0x002C1DB4 File Offset: 0x002BFFB4
		public bool CanEverBeNonHostile
		{
			get
			{
				return !this.permanentEnemy;
			}
		}

		// Token: 0x17001714 RID: 5908
		// (get) Token: 0x060096ED RID: 38637 RVA: 0x002C1DC0 File Offset: 0x002BFFC0
		public Texture2D SettlementTexture
		{
			get
			{
				if (this.settlementTexture == null)
				{
					if (!this.settlementTexturePath.NullOrEmpty())
					{
						this.settlementTexture = ContentFinder<Texture2D>.Get(this.settlementTexturePath, true);
					}
					else
					{
						this.settlementTexture = BaseContent.BadTex;
					}
				}
				return this.settlementTexture;
			}
		}

		// Token: 0x17001715 RID: 5909
		// (get) Token: 0x060096EE RID: 38638 RVA: 0x002C1E10 File Offset: 0x002C0010
		public Texture2D FactionIcon
		{
			get
			{
				if (this.factionIcon == null)
				{
					if (!this.factionIconPath.NullOrEmpty())
					{
						this.factionIcon = ContentFinder<Texture2D>.Get(this.factionIconPath, true);
					}
					else
					{
						this.factionIcon = BaseContent.BadTex;
					}
				}
				return this.factionIcon;
			}
		}

		// Token: 0x17001716 RID: 5910
		// (get) Token: 0x060096EF RID: 38639 RVA: 0x002C1E5D File Offset: 0x002C005D
		public Texture2D RoyalFavorIcon
		{
			get
			{
				if (this.royalFavorIcon == null && !this.royalFavorIconPath.NullOrEmpty())
				{
					this.royalFavorIcon = ContentFinder<Texture2D>.Get(this.royalFavorIconPath, true);
				}
				return this.royalFavorIcon;
			}
		}

		// Token: 0x17001717 RID: 5911
		// (get) Token: 0x060096F0 RID: 38640 RVA: 0x002C1E92 File Offset: 0x002C0092
		public bool HasRoyalTitles
		{
			get
			{
				return this.RoyalTitlesAwardableInSeniorityOrderForReading.Count > 0;
			}
		}

		// Token: 0x17001718 RID: 5912
		// (get) Token: 0x060096F1 RID: 38641 RVA: 0x002C1EA2 File Offset: 0x002C00A2
		public Color DefaultColor
		{
			get
			{
				if (this.colorSpectrum.NullOrEmpty<Color>())
				{
					return Color.white;
				}
				return this.colorSpectrum[0];
			}
		}

		// Token: 0x17001719 RID: 5913
		// (get) Token: 0x060096F2 RID: 38642 RVA: 0x002C1EC3 File Offset: 0x002C00C3
		public float BaselinerChance
		{
			get
			{
				if (this.xenotypeSet != null)
				{
					return this.xenotypeSet.BaselinerChance;
				}
				return 1f;
			}
		}

		// Token: 0x1700171A RID: 5914
		// (get) Token: 0x060096F3 RID: 38643 RVA: 0x002C1EE0 File Offset: 0x002C00E0
		public string Description
		{
			get
			{
				if (this.cachedDescription == null)
				{
					if (this.description.NullOrEmpty())
					{
						this.description = string.Empty;
					}
					else
					{
						this.cachedDescription = this.description;
					}
					if (ModsConfig.BiotechActive && this.humanlikeFaction)
					{
						List<XenotypeChance> list = new List<XenotypeChance>();
						this.cachedDescription = this.cachedDescription + "\n\n" + ("MemberXenotypeChances".Translate() + ":").AsTipTitle() + "\n";
						if (this.BaselinerChance > 0f)
						{
							list.Add(new XenotypeChance(XenotypeDefOf.Baseliner, this.BaselinerChance));
						}
						if (this.xenotypeSet != null)
						{
							for (int i = 0; i < this.xenotypeSet.Count; i++)
							{
								if (this.xenotypeSet[i].xenotype != XenotypeDefOf.Baseliner)
								{
									list.Add(this.xenotypeSet[i]);
								}
							}
						}
						if (list.Any<XenotypeChance>())
						{
							list.SortBy((XenotypeChance x) => -x.chance);
							this.cachedDescription += (from x in list
							select string.Format("{0}: {1}", x.xenotype.LabelCap, Mathf.Min(x.chance, 1f).ToStringPercent())).ToLineList("  - ", false);
						}
					}
				}
				return this.cachedDescription;
			}
		}

		// Token: 0x060096F4 RID: 38644 RVA: 0x002C204C File Offset: 0x002C024C
		public bool PermanentlyHostileTo(FactionDef otherFactionDef)
		{
			return this.permanentEnemy || (this.permanentEnemyToEveryoneExcept != null && !this.permanentEnemyToEveryoneExcept.Contains(otherFactionDef)) || (this.permanentEnemyToEveryoneExceptPlayer && !otherFactionDef.isPlayer);
		}

		// Token: 0x060096F5 RID: 38645 RVA: 0x002C2084 File Offset: 0x002C0284
		public float MinPointsToGeneratePawnGroup(PawnGroupKindDef groupKind, PawnGroupMakerParms parms = null)
		{
			if (this.pawnGroupMakers == null)
			{
				return 0f;
			}
			IEnumerable<PawnGroupMaker> source = from x in this.pawnGroupMakers
			where x.kindDef == groupKind
			select x;
			if (!source.Any<PawnGroupMaker>())
			{
				return 0f;
			}
			return source.Min((PawnGroupMaker pgm) => pgm.MinPointsToGenerateAnything(this, parms));
		}

		// Token: 0x060096F6 RID: 38646 RVA: 0x002C20F2 File Offset: 0x002C02F2
		public bool CanUseStuffForApparel(ThingDef stuffDef)
		{
			return this.apparelStuffFilter == null || this.apparelStuffFilter.Allows(stuffDef);
		}

		// Token: 0x060096F7 RID: 38647 RVA: 0x002C210A File Offset: 0x002C030A
		public float RaidCommonalityFromPoints(float points)
		{
			if (points < 0f || this.raidCommonalityFromPointsCurve == null)
			{
				return 1f;
			}
			return this.raidCommonalityFromPointsCurve.Evaluate(points);
		}

		// Token: 0x060096F8 RID: 38648 RVA: 0x002C212E File Offset: 0x002C032E
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.apparelStuffFilter != null)
			{
				this.apparelStuffFilter.ResolveReferences();
			}
		}

		// Token: 0x060096F9 RID: 38649 RVA: 0x002C214C File Offset: 0x002C034C
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
		}

		// Token: 0x060096FA RID: 38650 RVA: 0x002C219E File Offset: 0x002C039E
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.pawnGroupMakers != null && this.maxPawnCostPerTotalPointsCurve == null)
			{
				yield return "has pawnGroupMakers but missing maxPawnCostPerTotalPointsCurve";
			}
			if (this.techLevel == TechLevel.Undefined)
			{
				yield return this.defName + " has no tech level.";
			}
			if (this.humanlikeFaction && this.backstoryFilters.NullOrEmpty<BackstoryCategoryFilter>())
			{
				yield return this.defName + " is humanlikeFaction but has no backstory categories.";
			}
			if (this.permanentEnemy && this.mustStartOneEnemy)
			{
				yield return "permanentEnemy has mustStartOneEnemy = true, which is redundant";
			}
			if (this.disallowedMemes != null && this.allowedMemes != null)
			{
				yield return "both disallowedMemes (black list) and allowedMemes (white list) are defined";
			}
			if (this.requiredMemes != null)
			{
				MemeDef memeDef = this.requiredMemes.FirstOrDefault((MemeDef x) => !IdeoUtility.IsMemeAllowedFor(x, this));
				if (memeDef != null)
				{
					yield return "has a required meme which is not allowed: " + memeDef.defName;
				}
			}
			if (this.raidLootValueFromPointsCurve == null)
			{
				yield return "raidLootValueFromPointsCurve must be defined";
			}
			if (this.dropPodActive == null != (this.dropPodIncoming == null))
			{
				yield return "Both drop pod and drop pod incoming must be defined or both must be undefined";
			}
			yield break;
			yield break;
		}

		// Token: 0x060096FB RID: 38651 RVA: 0x002C21AE File Offset: 0x002C03AE
		public static FactionDef Named(string defName)
		{
			return DefDatabase<FactionDef>.GetNamed(defName, true);
		}

		// Token: 0x04006371 RID: 25457
		public bool isPlayer;

		// Token: 0x04006372 RID: 25458
		public RulePackDef factionNameMaker;

		// Token: 0x04006373 RID: 25459
		public RulePackDef settlementNameMaker;

		// Token: 0x04006374 RID: 25460
		public RulePackDef playerInitialSettlementNameMaker;

		// Token: 0x04006375 RID: 25461
		[MustTranslate]
		public string fixedName;

		// Token: 0x04006376 RID: 25462
		public bool humanlikeFaction = true;

		// Token: 0x04006377 RID: 25463
		public bool hidden;

		// Token: 0x04006378 RID: 25464
		public float listOrderPriority;

		// Token: 0x04006379 RID: 25465
		public List<PawnGroupMaker> pawnGroupMakers;

		// Token: 0x0400637A RID: 25466
		public SimpleCurve raidCommonalityFromPointsCurve;

		// Token: 0x0400637B RID: 25467
		public bool autoFlee = true;

		// Token: 0x0400637C RID: 25468
		public FloatRange attackersDownPercentageRangeForAutoFlee = new FloatRange(0.4f, 0.7f);

		// Token: 0x0400637D RID: 25469
		public bool canSiege;

		// Token: 0x0400637E RID: 25470
		public bool canStageAttacks;

		// Token: 0x0400637F RID: 25471
		public bool canUseAvoidGrid = true;

		// Token: 0x04006380 RID: 25472
		public bool canPsychicRitualSiege;

		// Token: 0x04006381 RID: 25473
		public float earliestRaidDays;

		// Token: 0x04006382 RID: 25474
		public FloatRange allowedArrivalTemperatureRange = new FloatRange(-1000f, 1000f);

		// Token: 0x04006383 RID: 25475
		public SimpleCurve minSettlementTemperatureChanceCurve;

		// Token: 0x04006384 RID: 25476
		public PawnKindDef basicMemberKind;

		// Token: 0x04006385 RID: 25477
		public List<ResearchProjectTagDef> startingResearchTags;

		// Token: 0x04006386 RID: 25478
		public List<ResearchProjectTagDef> startingTechprintsResearchTags;

		// Token: 0x04006387 RID: 25479
		[NoTranslate]
		public List<string> recipePrerequisiteTags;

		// Token: 0x04006388 RID: 25480
		public bool rescueesCanJoin;

		// Token: 0x04006389 RID: 25481
		[MustTranslate]
		public string pawnSingular = "member";

		// Token: 0x0400638A RID: 25482
		[MustTranslate]
		public string pawnsPlural = "members";

		// Token: 0x0400638B RID: 25483
		[MustTranslate]
		public string leaderTitle = "leader";

		// Token: 0x0400638C RID: 25484
		[MustTranslate]
		public string leaderTitleFemale;

		// Token: 0x0400638D RID: 25485
		[MustTranslate]
		public string royalFavorLabel;

		// Token: 0x0400638E RID: 25486
		[NoTranslate]
		public string royalFavorIconPath;

		// Token: 0x0400638F RID: 25487
		public List<PawnKindDef> fixedLeaderKinds;

		// Token: 0x04006390 RID: 25488
		public bool leaderForceGenerateNewPawn;

		// Token: 0x04006391 RID: 25489
		public float forageabilityFactor = 1f;

		// Token: 0x04006392 RID: 25490
		public SimpleCurve maxPawnCostPerTotalPointsCurve;

		// Token: 0x04006393 RID: 25491
		public List<string> royalTitleTags;

		// Token: 0x04006394 RID: 25492
		[NoTranslate]
		public string categoryTag;

		// Token: 0x04006395 RID: 25493
		public bool hostileToFactionlessHumanlikes;

		// Token: 0x04006396 RID: 25494
		public ThingDef dropPodActive;

		// Token: 0x04006397 RID: 25495
		public ThingDef dropPodIncoming;

		// Token: 0x04006398 RID: 25496
		public bool raidsForbidden;

		// Token: 0x04006399 RID: 25497
		public bool animalsFleeDanger = true;

		// Token: 0x0400639A RID: 25498
		private PawnGroupKindDef defaultSettlementGroupKindDef;

		// Token: 0x0400639B RID: 25499
		public bool canRequestTraders = true;

		// Token: 0x0400639C RID: 25500
		public bool canRequestMilitaryAid = true;

		// Token: 0x0400639D RID: 25501
		public bool canRequestOrbitalTrader;

		// Token: 0x0400639E RID: 25502
		public bool canGenerateQuestSites = true;

		// Token: 0x0400639F RID: 25503
		public List<PawnsArrivalModeDef> arrivalModeWhitelist = new List<PawnsArrivalModeDef>();

		// Token: 0x040063A0 RID: 25504
		public List<PawnsArrivalModeDef> arrivalModeBlacklist = new List<PawnsArrivalModeDef>();

		// Token: 0x040063A1 RID: 25505
		public List<PlanetLayerDef> layerWhitelist = new List<PlanetLayerDef>();

		// Token: 0x040063A2 RID: 25506
		public List<PlanetLayerDef> layerBlacklist = new List<PlanetLayerDef>();

		// Token: 0x040063A3 RID: 25507
		public List<PlanetLayerDef> arrivalLayerWhitelist = new List<PlanetLayerDef>();

		// Token: 0x040063A4 RID: 25508
		public List<PlanetLayerDef> arrivalLayerBlacklist = new List<PlanetLayerDef>();

		// Token: 0x040063A5 RID: 25509
		public List<PlanetLayerDef> neutralArrivalLayerWhitelist = new List<PlanetLayerDef>();

		// Token: 0x040063A6 RID: 25510
		public List<PlanetLayerDef> neutralArrivalLayerBlacklist = new List<PlanetLayerDef>();

		// Token: 0x040063A7 RID: 25511
		public List<PlanetLayerDef> raidArrivalLayerWhitelist = new List<PlanetLayerDef>();

		// Token: 0x040063A8 RID: 25512
		public List<PlanetLayerDef> raidArrivalLayerBlacklist = new List<PlanetLayerDef>();

		// Token: 0x040063A9 RID: 25513
		public int requiredCountAtGameStart;

		// Token: 0x040063AA RID: 25514
		public float settlementGenerationWeight;

		// Token: 0x040063AB RID: 25515
		public bool generateNewLeaderFromMapMembersOnly;

		// Token: 0x040063AC RID: 25516
		public int maxConfigurableAtWorldCreation = -1;

		// Token: 0x040063AD RID: 25517
		public int startingCountAtWorldCreation = 1;

		// Token: 0x040063AE RID: 25518
		public int configurationListOrderPriority;

		// Token: 0x040063AF RID: 25519
		public FactionDef replacesFaction;

		// Token: 0x040063B0 RID: 25520
		public bool displayInFactionSelection = true;

		// Token: 0x040063B1 RID: 25521
		public TechLevel techLevel;

		// Token: 0x040063B2 RID: 25522
		[NoTranslate]
		public List<BackstoryCategoryFilter> backstoryFilters;

		// Token: 0x040063B3 RID: 25523
		[NoTranslate]
		private List<string> backstoryCategories;

		// Token: 0x040063B4 RID: 25524
		public ThingFilter apparelStuffFilter;

		// Token: 0x040063B5 RID: 25525
		public ThingSetMakerDef settlementLootMaker;

		// Token: 0x040063B6 RID: 25526
		public ThingSetMakerDef raidLootMaker;

		// Token: 0x040063B7 RID: 25527
		public SimpleCurve raidLootValueFromPointsCurve;

		// Token: 0x040063B8 RID: 25528
		public List<TraderKindDef> caravanTraderKinds = new List<TraderKindDef>();

		// Token: 0x040063B9 RID: 25529
		public List<TraderKindDef> orbitalTraderKinds = new List<TraderKindDef>();

		// Token: 0x040063BA RID: 25530
		public List<TraderKindDef> visitorTraderKinds = new List<TraderKindDef>();

		// Token: 0x040063BB RID: 25531
		public List<TraderKindDef> baseTraderKinds = new List<TraderKindDef>();

		// Token: 0x040063BC RID: 25532
		public XenotypeSet xenotypeSet;

		// Token: 0x040063BD RID: 25533
		public FloatRange melaninRange = FloatRange.ZeroToOne;

		// Token: 0x040063BE RID: 25534
		public List<RaidStrategyDef> disallowedRaidStrategies;

		// Token: 0x040063BF RID: 25535
		public List<RaidAgeRestrictionDef> disallowedRaidAgeRestrictions;

		// Token: 0x040063C0 RID: 25536
		public bool mustStartOneEnemy;

		// Token: 0x040063C1 RID: 25537
		public bool naturalEnemy;

		// Token: 0x040063C2 RID: 25538
		public bool permanentEnemy;

		// Token: 0x040063C3 RID: 25539
		public bool permanentEnemyToEveryoneExceptPlayer;

		// Token: 0x040063C4 RID: 25540
		public List<FactionDef> permanentEnemyToEveryoneExcept;

		// Token: 0x040063C5 RID: 25541
		[NoTranslate]
		public string settlementTexturePath;

		// Token: 0x040063C6 RID: 25542
		[NoTranslate]
		public string factionIconPath;

		// Token: 0x040063C7 RID: 25543
		public List<Color> colorSpectrum;

		// Token: 0x040063C8 RID: 25544
		public List<PawnRelationDef> royalTitleInheritanceRelations;

		// Token: 0x040063C9 RID: 25545
		public Type royalTitleInheritanceWorkerClass;

		// Token: 0x040063CA RID: 25546
		public List<RoyalImplantRule> royalImplantRules;

		// Token: 0x040063CB RID: 25547
		public string renounceTitleMessage;

		// Token: 0x040063CC RID: 25548
		public List<CultureDef> allowedCultures;

		// Token: 0x040063CD RID: 25549
		public List<MemeDef> requiredMemes;

		// Token: 0x040063CE RID: 25550
		public List<MemeDef> allowedMemes;

		// Token: 0x040063CF RID: 25551
		public List<MemeDef> disallowedMemes;

		// Token: 0x040063D0 RID: 25552
		public List<PreceptDef> disallowedPrecepts;

		// Token: 0x040063D1 RID: 25553
		public List<MemeWeight> structureMemeWeights;

		// Token: 0x040063D2 RID: 25554
		public bool classicIdeo;

		// Token: 0x040063D3 RID: 25555
		public bool fixedIdeo;

		// Token: 0x040063D4 RID: 25556
		public string ideoName;

		// Token: 0x040063D5 RID: 25557
		public bool hiddenIdeo;

		// Token: 0x040063D6 RID: 25558
		[MustTranslate]
		public string ideoDescription;

		// Token: 0x040063D7 RID: 25559
		public List<StyleCategoryDef> styles;

		// Token: 0x040063D8 RID: 25560
		public List<DeityPreset> deityPresets;

		// Token: 0x040063D9 RID: 25561
		public List<MemeDef> forcedMemes;

		// Token: 0x040063DA RID: 25562
		public bool requiredPreceptsOnly;

		// Token: 0x040063DB RID: 25563
		[Obsolete("Will be removed in future version.")]
		public int maxCountAtGameStart;

		// Token: 0x040063DC RID: 25564
		[Obsolete("Will be removed in future version.")]
		public bool canMakeRandomly;

		// Token: 0x040063DD RID: 25565
		[MayTranslate]
		public string dialogFactionGreetingHostile;

		// Token: 0x040063DE RID: 25566
		[MayTranslate]
		public string dialogFactionGreetingHostileAppreciative;

		// Token: 0x040063DF RID: 25567
		[MayTranslate]
		public string dialogFactionGreetingWary;

		// Token: 0x040063E0 RID: 25568
		[MayTranslate]
		public string dialogFactionGreetingWarm;

		// Token: 0x040063E1 RID: 25569
		[MayTranslate]
		public string dialogMilitaryAidSent;

		// Token: 0x040063E2 RID: 25570
		[MustTranslate]
		public string messageDefendersAttacking;

		// Token: 0x040063E3 RID: 25571
		[Unsaved(false)]
		private Texture2D factionIcon;

		// Token: 0x040063E4 RID: 25572
		[Unsaved(false)]
		private Texture2D settlementTexture;

		// Token: 0x040063E5 RID: 25573
		[Unsaved(false)]
		private Texture2D royalFavorIcon;

		// Token: 0x040063E6 RID: 25574
		[Unsaved(false)]
		private string cachedDescription;

		// Token: 0x040063E7 RID: 25575
		[Unsaved(false)]
		private List<RoyalTitleDef> royalTitlesAwardableInSeniorityOrderForReading;

		// Token: 0x040063E8 RID: 25576
		[Unsaved(false)]
		private List<RoyalTitleDef> royalTitlesAllInSeniorityOrderForReading;

		// Token: 0x040063E9 RID: 25577
		[Unsaved(false)]
		private RoyalTitleInheritanceWorker royalTitleInheritanceWorker;
	}
}
