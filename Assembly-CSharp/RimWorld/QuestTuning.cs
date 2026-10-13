using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D08 RID: 7432
	public static class QuestTuning
	{
		// Token: 0x040072C1 RID: 29377
		public static readonly SimpleCurve IncreasesPopQuestChanceByPopIntentCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.05f),
				true
			},
			{
				new CurvePoint(1f, 0.3f),
				true
			},
			{
				new CurvePoint(3f, 0.45f),
				true
			}
		};

		// Token: 0x040072C2 RID: 29378
		public static readonly float[] RecentStoryWeightFactors = new float[]
		{
			0.01f,
			0.3f,
			0.5f,
			0.7f,
			0.9f
		};

		// Token: 0x040072C3 RID: 29379
		public static readonly SimpleCurve NonFavorQuestSelectionWeightFactorByDaysSinceFavorQuestCurve = new SimpleCurve
		{
			{
				new CurvePoint(10f, 1f),
				true
			},
			{
				new CurvePoint(25f, 0.01f),
				true
			}
		};

		// Token: 0x040072C4 RID: 29380
		public static readonly SimpleCurve PointsToRewardMarketValueCurve = new SimpleCurve
		{
			{
				new CurvePoint(300f, 800f),
				true
			},
			{
				new CurvePoint(700f, 1500f),
				true
			},
			{
				new CurvePoint(5000f, 4000f),
				true
			}
		};

		// Token: 0x040072C5 RID: 29381
		public const int MinFavorAtOnce = 1;

		// Token: 0x040072C6 RID: 29382
		public const int MaxFavorAtOnce = 12;

		// Token: 0x040072C7 RID: 29383
		public const int MaxGoodwillToAllowGoodwillReward = 92;

		// Token: 0x040072C8 RID: 29384
		public static readonly SimpleCurve PopIncreasingRewardWeightByPopIntentCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.05f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(3f, 2f),
				true
			}
		};

		// Token: 0x040072C9 RID: 29385
		public const float FutureResearchProjectTechprintSelectionWeightFactor = 0.02f;

		// Token: 0x040072CA RID: 29386
		public static readonly SimpleCurve DaysSincePsylinkAvailableToGuaranteedNeuroformerChance = new SimpleCurve
		{
			{
				new CurvePoint(45f, 0f),
				true
			},
			{
				new CurvePoint(60f, 1f),
				true
			}
		};

		// Token: 0x040072CB RID: 29387
		public const float MinDaysBetweenRaidSourceRaids = 1.5f;

		// Token: 0x040072CC RID: 29388
		public const float RaidSourceRaidThreatPointsFactor = 0.6f;

		// Token: 0x040072CD RID: 29389
		public static readonly SimpleCurve PointsToRaidSourceRaidsMTBDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(400f, 25f),
				true
			},
			{
				new CurvePoint(1500f, 10f),
				true
			},
			{
				new CurvePoint(5000f, 5f),
				true
			}
		};
	}
}
