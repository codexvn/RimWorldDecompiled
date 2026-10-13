using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200256E RID: 9582
	public static class DiplomacyTuning
	{
		// Token: 0x04008E44 RID: 36420
		public const int MaxGoodwill = 100;

		// Token: 0x04008E45 RID: 36421
		public const int MinGoodwill = -100;

		// Token: 0x04008E46 RID: 36422
		public const int NaturalEnemyNaturalGoodwill = -130;

		// Token: 0x04008E47 RID: 36423
		public const int NaturalEnemyInitialGoodwill = -80;

		// Token: 0x04008E48 RID: 36424
		public const int NaturalGoodwillRange = 50;

		// Token: 0x04008E49 RID: 36425
		public const float NaturalGoodwillDailyChange = 0.2f;

		// Token: 0x04008E4A RID: 36426
		public const float GoodwillChangeTowardsNaturalGoodwillFactor = 1.25f;

		// Token: 0x04008E4B RID: 36427
		public const int BecomeHostileThreshold = -75;

		// Token: 0x04008E4C RID: 36428
		public const int BecomeNeutralThreshold = 0;

		// Token: 0x04008E4D RID: 36429
		public const int BecomeAllyThreshold = 75;

		// Token: 0x04008E4E RID: 36430
		public const int InitialHostileThreshold = -10;

		// Token: 0x04008E4F RID: 36431
		public const int InitialAllyThreshold = 75;

		// Token: 0x04008E50 RID: 36432
		public static readonly IntRange ForcedStartingEnemyGoodwillRange = new IntRange(-100, -40);

		// Token: 0x04008E51 RID: 36433
		public const int MinGoodwillToRequestAICoreQuest = 40;

		// Token: 0x04008E52 RID: 36434
		public const int RequestAICoreQuestSilverCost = 1500;

		// Token: 0x04008E53 RID: 36435
		public static readonly FloatRange RansomFeeMarketValueFactorRange = new FloatRange(1.2f, 2.2f);

		// Token: 0x04008E54 RID: 36436
		public const int Goodwill_NaturalChangeStep = 10;

		// Token: 0x04008E55 RID: 36437
		public const float Goodwill_PerDirectDamageToPawn = -1.3f;

		// Token: 0x04008E56 RID: 36438
		public const float Goodwill_PerDirectDamageToBuilding = -1f;

		// Token: 0x04008E57 RID: 36439
		public const int Goodwill_MemberCrushed_Humanlike = -25;

		// Token: 0x04008E58 RID: 36440
		public const int Goodwill_MemberCrushed_Animal = -15;

		// Token: 0x04008E59 RID: 36441
		public const int Goodwill_MemberNeutrallyDied_Humanlike = -5;

		// Token: 0x04008E5A RID: 36442
		public const int Goodwill_MemberNeutrallyDied_Animal = -3;

		// Token: 0x04008E5B RID: 36443
		public const int Goodwill_BodyPartRemovalViolation = -70;

		// Token: 0x04008E5C RID: 36444
		public const int Goodwill_HemogenPackExtracted = -1;

		// Token: 0x04008E5D RID: 36445
		public const int Goodwill_SurgicalInspection = -1;

		// Token: 0x04008E5E RID: 36446
		public const int Goodwill_MemberEuthanized = -100;

		// Token: 0x04008E5F RID: 36447
		public const int Goodwill_AttackedSettlement = -50;

		// Token: 0x04008E60 RID: 36448
		public const int Goodwill_MilitaryAidRequested = -25;

		// Token: 0x04008E61 RID: 36449
		public const int Goodwill_TraderRequested = -15;

		// Token: 0x04008E62 RID: 36450
		public const int Goodwill_OrbitalTraderRequested = -30;

		// Token: 0x04008E63 RID: 36451
		public const int Goodwill_MemberStripped = -40;

		// Token: 0x04008E64 RID: 36452
		public const int Goodwill_MemberKidnappedOnGravship = -10;

		// Token: 0x04008E65 RID: 36453
		public const int Goodwill_MemberXenogermAbsorbed = -50;

		// Token: 0x04008E66 RID: 36454
		public const int Goodwill_MemberMutated = -50;

		// Token: 0x04008E67 RID: 36455
		public static readonly SimpleCurve Goodwill_PerQuadrumFromSettlementProximity = new SimpleCurve
		{
			{
				new CurvePoint(2f, -30f),
				true
			},
			{
				new CurvePoint(3f, -20f),
				true
			},
			{
				new CurvePoint(4f, -10f),
				true
			},
			{
				new CurvePoint(5f, 0f),
				true
			}
		};

		// Token: 0x04008E68 RID: 36456
		public const float Goodwill_BaseGiftSilverForOneGoodwill = 40f;

		// Token: 0x04008E69 RID: 36457
		public static readonly SimpleCurve GiftGoodwillFactorRelationsCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(75f, 0.25f),
				true
			}
		};

		// Token: 0x04008E6A RID: 36458
		public const float Goodwill_GiftPrisonerOfTheirFactionValueFactor = 2f;

		// Token: 0x04008E6B RID: 36459
		public const float Goodwill_TradedMarketValueforOneGoodwill = 600f;

		// Token: 0x04008E6C RID: 36460
		public const int Goodwill_DestroyedMutualEnemyBase = 20;

		// Token: 0x04008E6D RID: 36461
		public const int Goodwill_MemberExitedMapHealthy = 12;

		// Token: 0x04008E6E RID: 36462
		public const int Goodwill_MemberExitedMapHealthy_LeaderBonus = 40;

		// Token: 0x04008E6F RID: 36463
		public const float Goodwill_PerTend = 1f;

		// Token: 0x04008E70 RID: 36464
		public const int Goodwill_MaxTimesTendedTo = 10;

		// Token: 0x04008E71 RID: 36465
		public const int Goodwill_QuestTradeRequestCompleted = 12;

		// Token: 0x04008E72 RID: 36466
		public static readonly IntRange Goodwill_PeaceTalksDisasterRange = new IntRange(-50, -40);

		// Token: 0x04008E73 RID: 36467
		public static readonly IntRange Goodwill_PeaceTalksBackfireRange = new IntRange(-20, -10);

		// Token: 0x04008E74 RID: 36468
		public static readonly IntRange Goodwill_PeaceTalksSuccessRange = new IntRange(60, 70);

		// Token: 0x04008E75 RID: 36469
		public static readonly IntRange Goodwill_PeaceTalksTriumphRange = new IntRange(100, 110);

		// Token: 0x04008E76 RID: 36470
		public static readonly IntRange RoyalFavor_PeaceTalksSuccessRange = new IntRange(1, 4);

		// Token: 0x04008E77 RID: 36471
		public const float VisitorGiftChanceBase = 0.25f;

		// Token: 0x04008E78 RID: 36472
		public static readonly SimpleCurve VisitorGiftChanceFactorFromPlayerWealthCurve = new SimpleCurve
		{
			{
				new CurvePoint(30000f, 1f),
				true
			},
			{
				new CurvePoint(80000f, 0.1f),
				true
			},
			{
				new CurvePoint(300000f, 0f),
				true
			}
		};

		// Token: 0x04008E79 RID: 36473
		public static readonly SimpleCurve VisitorGiftChanceFactorFromGoodwillCurve = new SimpleCurve
		{
			{
				new CurvePoint(-30f, 0f),
				true
			},
			{
				new CurvePoint(0f, 1f),
				true
			}
		};

		// Token: 0x04008E7A RID: 36474
		public static readonly FloatRange VisitorGiftTotalMarketValueRangeBase = new FloatRange(100f, 500f);

		// Token: 0x04008E7B RID: 36475
		public static readonly SimpleCurve VisitorGiftTotalMarketValueFactorFromPlayerWealthCurve = new SimpleCurve
		{
			{
				new CurvePoint(10000f, 0.25f),
				true
			},
			{
				new CurvePoint(100000f, 1f),
				true
			}
		};

		// Token: 0x04008E7C RID: 36476
		public static readonly FloatRange RequestedMilitaryAidPointsRange = new FloatRange(800f, 1000f);
	}
}
