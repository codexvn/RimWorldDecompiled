using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A1A RID: 6682
	public sealed class DifficultyDef : Def
	{
		// Token: 0x040062F9 RID: 25337
		[Obsolete]
		public Color drawColor = Color.white;

		// Token: 0x040062FA RID: 25338
		[Obsolete]
		public bool isExtreme;

		// Token: 0x040062FB RID: 25339
		public bool isCustom;

		// Token: 0x040062FC RID: 25340
		[Obsolete]
		public int difficulty = -1;

		// Token: 0x040062FD RID: 25341
		public float threatScale = 1f;

		// Token: 0x040062FE RID: 25342
		public bool allowBigThreats = true;

		// Token: 0x040062FF RID: 25343
		public bool allowIntroThreats = true;

		// Token: 0x04006300 RID: 25344
		public bool allowCaveHives = true;

		// Token: 0x04006301 RID: 25345
		public bool peacefulTemples;

		// Token: 0x04006302 RID: 25346
		public bool allowViolentQuests = true;

		// Token: 0x04006303 RID: 25347
		public bool predatorsHuntHumanlikes = true;

		// Token: 0x04006304 RID: 25348
		public bool babiesAreHealthy;

		// Token: 0x04006305 RID: 25349
		public bool noBabiesOrChildren;

		// Token: 0x04006306 RID: 25350
		public float scariaRotChance;

		// Token: 0x04006307 RID: 25351
		public float colonistMoodOffset;

		// Token: 0x04006308 RID: 25352
		public float tradePriceFactorLoss;

		// Token: 0x04006309 RID: 25353
		public float cropYieldFactor = 1f;

		// Token: 0x0400630A RID: 25354
		public float mineYieldFactor = 1f;

		// Token: 0x0400630B RID: 25355
		public float butcherYieldFactor = 1f;

		// Token: 0x0400630C RID: 25356
		public float fishingYieldFactor = 1f;

		// Token: 0x0400630D RID: 25357
		public float researchSpeedFactor = 1f;

		// Token: 0x0400630E RID: 25358
		public float diseaseIntervalFactor = 1f;

		// Token: 0x0400630F RID: 25359
		public float enemyReproductionRateFactor = 1f;

		// Token: 0x04006310 RID: 25360
		public float playerPawnInfectionChanceFactor = 1f;

		// Token: 0x04006311 RID: 25361
		public float manhunterChanceOnDamageFactor = 1f;

		// Token: 0x04006312 RID: 25362
		public float deepDrillInfestationChanceFactor = 1f;

		// Token: 0x04006313 RID: 25363
		public float wastepackInfestationChanceFactor = 1f;

		// Token: 0x04006314 RID: 25364
		public float nomadicMineableResourcesFactor = 1f;

		// Token: 0x04006315 RID: 25365
		public float foodPoisonChanceFactor = 1f;

		// Token: 0x04006316 RID: 25366
		[Obsolete]
		public float threatsGeneratorThreatCountFactor = 1f;

		// Token: 0x04006317 RID: 25367
		public float maintenanceCostFactor = 1f;

		// Token: 0x04006318 RID: 25368
		public float enemyDeathOnDownedChanceFactor = 1f;

		// Token: 0x04006319 RID: 25369
		public float adaptationGrowthRateFactorOverZero = 1f;

		// Token: 0x0400631A RID: 25370
		public float adaptationEffectFactor = 1f;

		// Token: 0x0400631B RID: 25371
		public float questRewardValueFactor = 1f;

		// Token: 0x0400631C RID: 25372
		public float raidLootPointsFactor = 1f;

		// Token: 0x0400631D RID: 25373
		public bool allowTraps = true;

		// Token: 0x0400631E RID: 25374
		public bool allowTurrets = true;

		// Token: 0x0400631F RID: 25375
		public bool allowMortars = true;

		// Token: 0x04006320 RID: 25376
		public bool classicMortars;

		// Token: 0x04006321 RID: 25377
		public bool allowExtremeWeatherIncidents = true;

		// Token: 0x04006322 RID: 25378
		public bool fixedWealthMode;

		// Token: 0x04006323 RID: 25379
		public float lowPopConversionBoost = 3f;

		// Token: 0x04006324 RID: 25380
		public float minThreatPointsRangeCeiling = 70f;

		// Token: 0x04006325 RID: 25381
		public float childAgingRate = 4f;

		// Token: 0x04006326 RID: 25382
		public float adultAgingRate = 1f;

		// Token: 0x04006327 RID: 25383
		public bool unwaveringPrisoners = true;

		// Token: 0x04006328 RID: 25384
		public bool childRaidersAllowed;

		// Token: 0x04006329 RID: 25385
		public float anomalyThreatsInactiveFraction = 0.08f;

		// Token: 0x0400632A RID: 25386
		public float anomalyThreatsActiveFraction = 0.3f;

		// Token: 0x0400632B RID: 25387
		public float studyEfficiencyFactor = 1f;
	}
}
