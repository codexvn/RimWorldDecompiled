using System;

namespace RimWorld
{
	// Token: 0x020035B3 RID: 13747
	[DefOf]
	public static class StatDefOf
	{
		// Token: 0x060133A1 RID: 78753 RVA: 0x005B5C3C File Offset: 0x005B3E3C
		static StatDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StatDefOf));
		}

		// Token: 0x0400D043 RID: 53315
		public static StatDef MaxHitPoints;

		// Token: 0x0400D044 RID: 53316
		public static StatDef MarketValue;

		// Token: 0x0400D045 RID: 53317
		public static StatDef MarketValueIgnoreHp;

		// Token: 0x0400D046 RID: 53318
		public static StatDef RoyalFavorValue;

		// Token: 0x0400D047 RID: 53319
		public static StatDef SellPriceFactor;

		// Token: 0x0400D048 RID: 53320
		public static StatDef Beauty;

		// Token: 0x0400D049 RID: 53321
		public static StatDef BeautyOutdoors;

		// Token: 0x0400D04A RID: 53322
		public static StatDef Cleanliness;

		// Token: 0x0400D04B RID: 53323
		public static StatDef Flammability;

		// Token: 0x0400D04C RID: 53324
		public static StatDef DeteriorationRate;

		// Token: 0x0400D04D RID: 53325
		public static StatDef WorkToMake;

		// Token: 0x0400D04E RID: 53326
		public static StatDef WorkToBuild;

		// Token: 0x0400D04F RID: 53327
		public static StatDef Mass;

		// Token: 0x0400D050 RID: 53328
		public static StatDef ConstructionSpeedFactor;

		// Token: 0x0400D051 RID: 53329
		public static StatDef Nutrition;

		// Token: 0x0400D052 RID: 53330
		public static StatDef FoodPoisonChanceFixedHuman;

		// Token: 0x0400D053 RID: 53331
		public static StatDef MoveSpeed;

		// Token: 0x0400D054 RID: 53332
		public static StatDef CrawlSpeed;

		// Token: 0x0400D055 RID: 53333
		public static StatDef GlobalLearningFactor;

		// Token: 0x0400D056 RID: 53334
		public static StatDef BedHungerRateFactor;

		// Token: 0x0400D057 RID: 53335
		public static StatDef RestRateMultiplier;

		// Token: 0x0400D058 RID: 53336
		public static StatDef PsychicSensitivity;

		// Token: 0x0400D059 RID: 53337
		public static StatDef ToxicResistance;

		// Token: 0x0400D05A RID: 53338
		public static StatDef MentalBreakThreshold;

		// Token: 0x0400D05B RID: 53339
		public static StatDef EatingSpeed;

		// Token: 0x0400D05C RID: 53340
		public static StatDef ComfyTemperatureMin;

		// Token: 0x0400D05D RID: 53341
		public static StatDef ComfyTemperatureMax;

		// Token: 0x0400D05E RID: 53342
		public static StatDef Comfort;

		// Token: 0x0400D05F RID: 53343
		public static StatDef MeatAmount;

		// Token: 0x0400D060 RID: 53344
		public static StatDef LeatherAmount;

		// Token: 0x0400D061 RID: 53345
		public static StatDef MinimumHandlingSkill;

		// Token: 0x0400D062 RID: 53346
		public static StatDef MeleeDPS;

		// Token: 0x0400D063 RID: 53347
		public static StatDef PainShockThreshold;

		// Token: 0x0400D064 RID: 53348
		public static StatDef ForagedNutritionPerDay;

		// Token: 0x0400D065 RID: 53349
		public static StatDef FilthRate;

		// Token: 0x0400D066 RID: 53350
		public static StatDef MaxNutrition;

		// Token: 0x0400D067 RID: 53351
		public static StatDef LifespanFactor;

		// Token: 0x0400D068 RID: 53352
		public static StatDef MaxFlightTime;

		// Token: 0x0400D069 RID: 53353
		public static StatDef FlightCooldown;

		// Token: 0x0400D06A RID: 53354
		public static StatDef Wildness;

		// Token: 0x0400D06B RID: 53355
		[MayRequireRoyalty]
		public static StatDef PsychicEntropyMax;

		// Token: 0x0400D06C RID: 53356
		[MayRequireRoyalty]
		public static StatDef PsychicEntropyRecoveryRate;

		// Token: 0x0400D06D RID: 53357
		[MayRequireRoyalty]
		public static StatDef PsychicEntropyGain;

		// Token: 0x0400D06E RID: 53358
		[MayRequireRoyalty]
		public static StatDef MeditationFocusGain;

		// Token: 0x0400D06F RID: 53359
		[MayRequireIdeology]
		public static StatDef CertaintyLossFactor;

		// Token: 0x0400D070 RID: 53360
		[MayRequireIdeology]
		public static StatDef SocialIdeoSpreadFrequencyFactor;

		// Token: 0x0400D071 RID: 53361
		[MayRequireIdeology]
		public static StatDef SuppressionPower;

		// Token: 0x0400D072 RID: 53362
		[MayRequireIdeology]
		public static StatDef SlaveSuppressionFallRate;

		// Token: 0x0400D073 RID: 53363
		public static StatDef AnimalsLearningFactor;

		// Token: 0x0400D074 RID: 53364
		public static StatDef BondAnimalChanceFactor;

		// Token: 0x0400D075 RID: 53365
		public static StatDef CaravanRidingSpeedFactor;

		// Token: 0x0400D076 RID: 53366
		public static StatDef CaravanBonusSpeedFactor;

		// Token: 0x0400D077 RID: 53367
		public static StatDef RestFallRateFactor;

		// Token: 0x0400D078 RID: 53368
		public static StatDef JoyFallRateFactor;

		// Token: 0x0400D079 RID: 53369
		[MayRequireIdeology]
		public static StatDef BiosculpterOccupantSpeed;

		// Token: 0x0400D07A RID: 53370
		[MayRequireBiotech]
		public static StatDef MechEnergyUsageFactor;

		// Token: 0x0400D07B RID: 53371
		public static StatDef MeleeDamageFactor;

		// Token: 0x0400D07C RID: 53372
		public static StatDef MeleeCooldownFactor;

		// Token: 0x0400D07D RID: 53373
		public static StatDef ToxicEnvironmentResistance;

		// Token: 0x0400D07E RID: 53374
		[MayRequireBiotech]
		public static StatDef HemogenGainFactor;

		// Token: 0x0400D07F RID: 53375
		[MayRequireBiotech]
		public static StatDef RawNutritionFactor;

		// Token: 0x0400D080 RID: 53376
		[MayRequireBiotech]
		public static StatDef CancerRate;

		// Token: 0x0400D081 RID: 53377
		[MayRequireBiotech]
		public static StatDef WastepacksPerRecharge;

		// Token: 0x0400D082 RID: 53378
		[MayRequireBiotech]
		public static StatDef MechEnergyLossPerHP;

		// Token: 0x0400D083 RID: 53379
		[MayRequireBiotech]
		public static StatDef LearningRateFactor;

		// Token: 0x0400D084 RID: 53380
		[MayRequireBiotech]
		public static StatDef GrowthVatOccupantSpeed;

		// Token: 0x0400D085 RID: 53381
		[MayRequireAnomaly]
		public static StatDef MinimumContainmentStrength;

		// Token: 0x0400D086 RID: 53382
		[MayRequireAnomaly]
		public static StatDef PsychicRitualQualityOffset;

		// Token: 0x0400D087 RID: 53383
		[MayRequireOdyssey]
		public static StatDef VacuumResistance;

		// Token: 0x0400D088 RID: 53384
		[MayRequireOdyssey]
		public static StatDef PilotingAbility;

		// Token: 0x0400D089 RID: 53385
		public static StatDef WorkSpeedGlobal;

		// Token: 0x0400D08A RID: 53386
		public static StatDef MiningSpeed;

		// Token: 0x0400D08B RID: 53387
		public static StatDef DeepDrillingSpeed;

		// Token: 0x0400D08C RID: 53388
		public static StatDef MiningYield;

		// Token: 0x0400D08D RID: 53389
		public static StatDef ResearchSpeed;

		// Token: 0x0400D08E RID: 53390
		public static StatDef ConstructionSpeed;

		// Token: 0x0400D08F RID: 53391
		public static StatDef HuntingStealth;

		// Token: 0x0400D090 RID: 53392
		public static StatDef PlantWorkSpeed;

		// Token: 0x0400D091 RID: 53393
		public static StatDef SmoothingSpeed;

		// Token: 0x0400D092 RID: 53394
		public static StatDef FoodPoisonChance;

		// Token: 0x0400D093 RID: 53395
		public static StatDef CarryingCapacity;

		// Token: 0x0400D094 RID: 53396
		public static StatDef PlantHarvestYield;

		// Token: 0x0400D095 RID: 53397
		public static StatDef DrugHarvestYield;

		// Token: 0x0400D096 RID: 53398
		public static StatDef FixBrokenDownBuildingSuccessChance;

		// Token: 0x0400D097 RID: 53399
		public static StatDef ConstructSuccessChance;

		// Token: 0x0400D098 RID: 53400
		public static StatDef GeneralLaborSpeed;

		// Token: 0x0400D099 RID: 53401
		public static StatDef CleaningSpeed;

		// Token: 0x0400D09A RID: 53402
		public static StatDef ReadingSpeed;

		// Token: 0x0400D09B RID: 53403
		public static StatDef HackingSpeed;

		// Token: 0x0400D09C RID: 53404
		public static StatDef HackingStealth;

		// Token: 0x0400D09D RID: 53405
		[MayRequireIdeology]
		public static StatDef PruningSpeed;

		// Token: 0x0400D09E RID: 53406
		[MayRequireBiotech]
		public static StatDef MechBandwidth;

		// Token: 0x0400D09F RID: 53407
		[MayRequireBiotech]
		public static StatDef MechRemoteRepairDistance;

		// Token: 0x0400D0A0 RID: 53408
		[MayRequireBiotech]
		public static StatDef MechRemoteShieldEnergy;

		// Token: 0x0400D0A1 RID: 53409
		[MayRequireBiotech]
		public static StatDef MechControlGroups;

		// Token: 0x0400D0A2 RID: 53410
		[MayRequireBiotech]
		public static StatDef BandwidthCost;

		// Token: 0x0400D0A3 RID: 53411
		[MayRequireBiotech]
		public static StatDef MechRepairSpeed;

		// Token: 0x0400D0A4 RID: 53412
		[MayRequireBiotech]
		public static StatDef ControlTakingTime;

		// Token: 0x0400D0A5 RID: 53413
		[MayRequireAnomaly]
		public static StatDef EntityStudyRate;

		// Token: 0x0400D0A6 RID: 53414
		[MayRequireAnomaly]
		public static StatDef ActivitySuppressionRate;

		// Token: 0x0400D0A7 RID: 53415
		[MayRequireAnomaly]
		public static StatDef StudyEfficiency;

		// Token: 0x0400D0A8 RID: 53416
		[MayRequireOdyssey]
		public static StatDef FishingSpeed;

		// Token: 0x0400D0A9 RID: 53417
		[MayRequireOdyssey]
		public static StatDef FishingYield;

		// Token: 0x0400D0AA RID: 53418
		public static StatDef MedicalTendSpeed;

		// Token: 0x0400D0AB RID: 53419
		public static StatDef MedicalTendQuality;

		// Token: 0x0400D0AC RID: 53420
		public static StatDef MedicalSurgerySuccessChance;

		// Token: 0x0400D0AD RID: 53421
		public static StatDef NegotiationAbility;

		// Token: 0x0400D0AE RID: 53422
		public static StatDef ArrestSuccessChance;

		// Token: 0x0400D0AF RID: 53423
		public static StatDef TradePriceImprovement;

		// Token: 0x0400D0B0 RID: 53424
		public static StatDef DrugSellPriceImprovement;

		// Token: 0x0400D0B1 RID: 53425
		public static StatDef SocialImpact;

		// Token: 0x0400D0B2 RID: 53426
		public static StatDef PawnBeauty;

		// Token: 0x0400D0B3 RID: 53427
		[MayRequireIdeology]
		public static StatDef AnimalProductsSellImprovement;

		// Token: 0x0400D0B4 RID: 53428
		[MayRequireIdeology]
		public static StatDef ConversionPower;

		// Token: 0x0400D0B5 RID: 53429
		[MayRequireBiotech]
		public static StatDef BabyPlayGainFactor;

		// Token: 0x0400D0B6 RID: 53430
		[MayRequireBiotech]
		public static StatDef Fertility;

		// Token: 0x0400D0B7 RID: 53431
		public static StatDef AnimalGatherSpeed;

		// Token: 0x0400D0B8 RID: 53432
		public static StatDef AnimalGatherYield;

		// Token: 0x0400D0B9 RID: 53433
		public static StatDef TameAnimalChance;

		// Token: 0x0400D0BA RID: 53434
		public static StatDef TrainAnimalChance;

		// Token: 0x0400D0BB RID: 53435
		public static StatDef ShootingAccuracyPawn;

		// Token: 0x0400D0BC RID: 53436
		public static StatDef ShootingAccuracyTurret;

		// Token: 0x0400D0BD RID: 53437
		public static StatDef MortarMissRadiusFactor;

		// Token: 0x0400D0BE RID: 53438
		public static StatDef AimingDelayFactor;

		// Token: 0x0400D0BF RID: 53439
		[MayRequireIdeology]
		public static StatDef ShootingAccuracyOutdoorsDarkOffset;

		// Token: 0x0400D0C0 RID: 53440
		[MayRequireIdeology]
		public static StatDef ShootingAccuracyOutdoorsLitOffset;

		// Token: 0x0400D0C1 RID: 53441
		[MayRequireIdeology]
		public static StatDef ShootingAccuracyIndoorsDarkOffset;

		// Token: 0x0400D0C2 RID: 53442
		[MayRequireIdeology]
		public static StatDef ShootingAccuracyIndoorsLitOffset;

		// Token: 0x0400D0C3 RID: 53443
		[MayRequireIdeology]
		public static StatDef MeleeHitChanceOutdoorsDarkOffset;

		// Token: 0x0400D0C4 RID: 53444
		[MayRequireIdeology]
		public static StatDef MeleeHitChanceOutdoorsLitOffset;

		// Token: 0x0400D0C5 RID: 53445
		[MayRequireIdeology]
		public static StatDef MeleeHitChanceIndoorsDarkOffset;

		// Token: 0x0400D0C6 RID: 53446
		[MayRequireIdeology]
		public static StatDef MeleeHitChanceIndoorsLitOffset;

		// Token: 0x0400D0C7 RID: 53447
		[MayRequireIdeology]
		public static StatDef MeleeDodgeChanceOutdoorsDarkOffset;

		// Token: 0x0400D0C8 RID: 53448
		[MayRequireIdeology]
		public static StatDef MeleeDodgeChanceOutdoorsLitOffset;

		// Token: 0x0400D0C9 RID: 53449
		[MayRequireIdeology]
		public static StatDef MeleeDodgeChanceIndoorsDarkOffset;

		// Token: 0x0400D0CA RID: 53450
		[MayRequireIdeology]
		public static StatDef MeleeDodgeChanceIndoorsLitOffset;

		// Token: 0x0400D0CB RID: 53451
		[MayRequireBiotech]
		public static StatDef StaggerDurationFactor;

		// Token: 0x0400D0CC RID: 53452
		public static StatDef RangedCooldownFactor;

		// Token: 0x0400D0CD RID: 53453
		public static StatDef MeleeHitChance;

		// Token: 0x0400D0CE RID: 53454
		public static StatDef MeleeDodgeChance;

		// Token: 0x0400D0CF RID: 53455
		[MayRequireBiotech]
		public static StatDef MeleeDoorDamageFactor;

		// Token: 0x0400D0D0 RID: 53456
		public static StatDef PawnTrapSpringChance;

		// Token: 0x0400D0D1 RID: 53457
		public static StatDef IncomingDamageFactor;

		// Token: 0x0400D0D2 RID: 53458
		public static StatDef MeleeWeapon_AverageDPS;

		// Token: 0x0400D0D3 RID: 53459
		public static StatDef MeleeWeapon_DamageMultiplier;

		// Token: 0x0400D0D4 RID: 53460
		public static StatDef MeleeWeapon_CooldownMultiplier;

		// Token: 0x0400D0D5 RID: 53461
		public static StatDef SharpDamageMultiplier;

		// Token: 0x0400D0D6 RID: 53462
		public static StatDef BluntDamageMultiplier;

		// Token: 0x0400D0D7 RID: 53463
		public static StatDef StuffPower_Armor_Sharp;

		// Token: 0x0400D0D8 RID: 53464
		public static StatDef StuffPower_Armor_Blunt;

		// Token: 0x0400D0D9 RID: 53465
		public static StatDef StuffPower_Armor_Heat;

		// Token: 0x0400D0DA RID: 53466
		public static StatDef StuffPower_Insulation_Cold;

		// Token: 0x0400D0DB RID: 53467
		public static StatDef StuffPower_Insulation_Heat;

		// Token: 0x0400D0DC RID: 53468
		public static StatDef RangedWeapon_Cooldown;

		// Token: 0x0400D0DD RID: 53469
		public static StatDef RangedWeapon_DamageMultiplier;

		// Token: 0x0400D0DE RID: 53470
		public static StatDef RangedWeapon_ArmorPenetrationMultiplier;

		// Token: 0x0400D0DF RID: 53471
		public static StatDef RangedWeapon_RangeMultiplier;

		// Token: 0x0400D0E0 RID: 53472
		public static StatDef RangedWeapon_WarmupMultiplier;

		// Token: 0x0400D0E1 RID: 53473
		public static StatDef AccuracyTouch;

		// Token: 0x0400D0E2 RID: 53474
		public static StatDef AccuracyShort;

		// Token: 0x0400D0E3 RID: 53475
		public static StatDef AccuracyMedium;

		// Token: 0x0400D0E4 RID: 53476
		public static StatDef AccuracyLong;

		// Token: 0x0400D0E5 RID: 53477
		public static StatDef ShootingAccuracyFactor_Touch;

		// Token: 0x0400D0E6 RID: 53478
		public static StatDef ShootingAccuracyFactor_Short;

		// Token: 0x0400D0E7 RID: 53479
		public static StatDef ShootingAccuracyFactor_Medium;

		// Token: 0x0400D0E8 RID: 53480
		public static StatDef ShootingAccuracyFactor_Long;

		// Token: 0x0400D0E9 RID: 53481
		public static StatDef StuffEffectMultiplierArmor;

		// Token: 0x0400D0EA RID: 53482
		public static StatDef StuffEffectMultiplierInsulation_Cold;

		// Token: 0x0400D0EB RID: 53483
		public static StatDef StuffEffectMultiplierInsulation_Heat;

		// Token: 0x0400D0EC RID: 53484
		public static StatDef ArmorRating_Sharp;

		// Token: 0x0400D0ED RID: 53485
		public static StatDef ArmorRating_Blunt;

		// Token: 0x0400D0EE RID: 53486
		public static StatDef ArmorRating_Heat;

		// Token: 0x0400D0EF RID: 53487
		public static StatDef Insulation_Cold;

		// Token: 0x0400D0F0 RID: 53488
		public static StatDef Insulation_Heat;

		// Token: 0x0400D0F1 RID: 53489
		public static StatDef EnergyShieldRechargeRate;

		// Token: 0x0400D0F2 RID: 53490
		public static StatDef EnergyShieldEnergyMax;

		// Token: 0x0400D0F3 RID: 53491
		public static StatDef PackRadius;

		// Token: 0x0400D0F4 RID: 53492
		[MayRequireRoyalty]
		public static StatDef JumpRange;

		// Token: 0x0400D0F5 RID: 53493
		public static StatDef EquipDelay;

		// Token: 0x0400D0F6 RID: 53494
		public static StatDef MedicalPotency;

		// Token: 0x0400D0F7 RID: 53495
		public static StatDef MedicalQualityMax;

		// Token: 0x0400D0F8 RID: 53496
		public static StatDef ImmunityGainSpeed;

		// Token: 0x0400D0F9 RID: 53497
		public static StatDef ImmunityGainSpeedFactor;

		// Token: 0x0400D0FA RID: 53498
		public static StatDef InjuryHealingFactor;

		// Token: 0x0400D0FB RID: 53499
		public static StatDef DoorOpenSpeed;

		// Token: 0x0400D0FC RID: 53500
		public static StatDef BedRestEffectiveness;

		// Token: 0x0400D0FD RID: 53501
		public static StatDef TrapMeleeDamage;

		// Token: 0x0400D0FE RID: 53502
		public static StatDef TrapSpringChance;

		// Token: 0x0400D0FF RID: 53503
		public static StatDef ResearchSpeedFactor;

		// Token: 0x0400D100 RID: 53504
		public static StatDef MedicalTendQualityOffset;

		// Token: 0x0400D101 RID: 53505
		public static StatDef WorkTableWorkSpeedFactor;

		// Token: 0x0400D102 RID: 53506
		public static StatDef WorkTableEfficiencyFactor;

		// Token: 0x0400D103 RID: 53507
		public static StatDef JoyGainFactor;

		// Token: 0x0400D104 RID: 53508
		public static StatDef SurgerySuccessChanceFactor;

		// Token: 0x0400D105 RID: 53509
		[MayRequireIdeology]
		public static StatDef StyleDominance;

		// Token: 0x0400D106 RID: 53510
		[MayRequireIdeology]
		public static StatDef BiosculpterPodSpeedFactor;

		// Token: 0x0400D107 RID: 53511
		[MayRequireRoyalty]
		public static StatDef MeditationPlantGrowthOffset;

		// Token: 0x0400D108 RID: 53512
		[MayRequireBiotech]
		public static StatDef GeneticComplexityIncrease;

		// Token: 0x0400D109 RID: 53513
		[MayRequireBiotech]
		public static StatDef AssemblySpeedFactor;

		// Token: 0x0400D10A RID: 53514
		[MayRequireAnomaly]
		public static StatDef PsychicRitualQuality;

		// Token: 0x0400D10B RID: 53515
		[MayRequireAnomaly]
		public static StatDef ContainmentStrength;

		// Token: 0x0400D10C RID: 53516
		[MayRequireOdyssey]
		public static StatDef GravshipRange;

		// Token: 0x0400D10D RID: 53517
		[MayRequireOdyssey]
		public static StatDef SubstructureSupport;

		// Token: 0x0400D10E RID: 53518
		public static StatDef Ability_CastingTime;

		// Token: 0x0400D10F RID: 53519
		public static StatDef Ability_EntropyGain;

		// Token: 0x0400D110 RID: 53520
		public static StatDef Ability_PsyfocusCost;

		// Token: 0x0400D111 RID: 53521
		public static StatDef Ability_Duration;

		// Token: 0x0400D112 RID: 53522
		public static StatDef Ability_Range;

		// Token: 0x0400D113 RID: 53523
		public static StatDef Ability_EffectRadius;

		// Token: 0x0400D114 RID: 53524
		public static StatDef Ability_RequiredPsylink;

		// Token: 0x0400D115 RID: 53525
		public static StatDef Ability_GoodwillImpact;

		// Token: 0x0400D116 RID: 53526
		public static StatDef Ability_DetectChancePerEntropy;

		// Token: 0x0400D117 RID: 53527
		public static StatDef MeditationFocusStrength;

		// Token: 0x0400D118 RID: 53528
		[MayRequireIdeology]
		public static StatDef TerrorSource;

		// Token: 0x0400D119 RID: 53529
		[MayRequireIdeology]
		public static StatDef Terror;

		// Token: 0x0400D11A RID: 53530
		public static StatDef FilthMultiplier;

		// Token: 0x0400D11B RID: 53531
		public static StatDef CleaningTimeFactor;
	}
}
