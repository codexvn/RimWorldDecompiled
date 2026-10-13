using System;

namespace Verse
{
	// Token: 0x02000678 RID: 1656
	public class HealthTuning
	{
		// Token: 0x040021CB RID: 8651
		public const int StandardInterval = 60;

		// Token: 0x040021CC RID: 8652
		public const float SmallPawnFragmentedDamageHealthScaleThreshold = 0.5f;

		// Token: 0x040021CD RID: 8653
		public const int SmallPawnFragmentedDamageMinimumDamageAmount = 10;

		// Token: 0x040021CE RID: 8654
		public static float ChanceToAdditionallyDamageInnerSolidPart = 0.2f;

		// Token: 0x040021CF RID: 8655
		public const float MinBleedingRateToBleed = 0.1f;

		// Token: 0x040021D0 RID: 8656
		public const float BleedSeverityRecoveryPerInterval = 0.00033333333f;

		// Token: 0x040021D1 RID: 8657
		public const float BloodFilthDropChanceFactorStanding = 0.004f;

		// Token: 0x040021D2 RID: 8658
		public const float BloodFilthDropChanceFactorLaying = 0.0004f;

		// Token: 0x040021D3 RID: 8659
		public const float BloodSmearFilthDropChanceFactorCrawling = 0.015f;

		// Token: 0x040021D4 RID: 8660
		public const int BaseTicksAfterInjuryToStopBleeding = 90000;

		// Token: 0x040021D5 RID: 8661
		public const int TicksAfterMissingBodyPartToStopBeingFresh = 90000;

		// Token: 0x040021D6 RID: 8662
		public const float DefaultPainShockThreshold = 0.8f;

		// Token: 0x040021D7 RID: 8663
		public const int InjuryHealInterval = 600;

		// Token: 0x040021D8 RID: 8664
		public const int RapidRegenInjuryHealInterval = 15;

		// Token: 0x040021D9 RID: 8665
		public const float InjuryHealPerDay_Base = 8f;

		// Token: 0x040021DA RID: 8666
		public const float InjuryHealPerDayOffset_Laying = 4f;

		// Token: 0x040021DB RID: 8667
		public const float InjuryHealPerDayOffset_Tended = 8f;

		// Token: 0x040021DC RID: 8668
		public const int InjurySeverityTendedPerMedicine = 20;

		// Token: 0x040021DD RID: 8669
		public const float BaseTotalDamageLethalThreshold = 150f;

		// Token: 0x040021DE RID: 8670
		public const float BecomePermanentBaseChance = 0.02f;

		// Token: 0x040021DF RID: 8671
		public static readonly SimpleCurve BecomePermanentChanceFactorBySeverityCurve = new SimpleCurve
		{
			{
				new CurvePoint(4f, 0f),
				true
			},
			{
				new CurvePoint(14f, 1f),
				true
			}
		};

		// Token: 0x040021E0 RID: 8672
		public static readonly HealthTuning.PainCategoryWeighted[] InjuryPainCategories = new HealthTuning.PainCategoryWeighted[]
		{
			new HealthTuning.PainCategoryWeighted(PainCategory.Painless, 0.5f),
			new HealthTuning.PainCategoryWeighted(PainCategory.LowPain, 0.2f),
			new HealthTuning.PainCategoryWeighted(PainCategory.MediumPain, 0.2f),
			new HealthTuning.PainCategoryWeighted(PainCategory.HighPain, 0.1f)
		};

		// Token: 0x040021E1 RID: 8673
		public const float MinDamagePartPctForInfection = 0.2f;

		// Token: 0x040021E2 RID: 8674
		public static readonly IntRange InfectionDelayRange = new IntRange(15000, 45000);

		// Token: 0x040021E3 RID: 8675
		public const float AnimalsInfectionChanceFactor = 0.1f;

		// Token: 0x040021E4 RID: 8676
		public const float HypothermiaGrowthPerDegreeUnder = 6.45E-05f;

		// Token: 0x040021E5 RID: 8677
		public const float HeatstrokeGrowthPerDegreeOver = 6.45E-05f;

		// Token: 0x040021E6 RID: 8678
		public const float MinHeatstrokeProgressPerInterval = 0.000375f;

		// Token: 0x040021E7 RID: 8679
		public const float MinHypothermiaProgress = 0.00075f;

		// Token: 0x040021E8 RID: 8680
		public const float HarmfulTemperatureOffset = 10f;

		// Token: 0x040021E9 RID: 8681
		public const float MinTempOverComfyMaxForBurn = 150f;

		// Token: 0x040021EA RID: 8682
		public const float BurnDamagePerTempOverage = 0.06f;

		// Token: 0x040021EB RID: 8683
		public const int MinBurnDamage = 3;

		// Token: 0x040021EC RID: 8684
		public const float ImmunityGainRandomFactorMin = 0.8f;

		// Token: 0x040021ED RID: 8685
		public const float ImmunityGainRandomFactorMax = 1.2f;

		// Token: 0x040021EE RID: 8686
		public const float ImpossibleToFallSickIfAboveThisImmunityLevel = 0.6f;

		// Token: 0x040021EF RID: 8687
		public const int HediffGiverUpdateInterval = 60;

		// Token: 0x040021F0 RID: 8688
		public const int VomitCheckInterval = 600;

		// Token: 0x040021F1 RID: 8689
		public const int DeathCheckInterval = 200;

		// Token: 0x040021F2 RID: 8690
		public const int ForgetRandomMemoryThoughtCheckInterval = 400;

		// Token: 0x040021F3 RID: 8691
		public const float PawnBaseHealthForSummary = 75f;

		// Token: 0x040021F4 RID: 8692
		public const float DeathOnDownedChance_NonColonyAnimal = 0.5f;

		// Token: 0x040021F5 RID: 8693
		public const float DeathOnDownedChance_NonColonyMechanoid = 1f;

		// Token: 0x040021F6 RID: 8694
		public static readonly SimpleCurve DeathOnDownedChance_EntityFromThreatCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 0f),
				true
			},
			{
				new CurvePoint(1000f, 0.5f),
				true
			},
			{
				new CurvePoint(5000f, 0.9f),
				true
			}
		};

		// Token: 0x040021F7 RID: 8695
		public static readonly SimpleCurve DeathOnDownedChance_NonColonyHumanlikeFromPopulationIntentCurve = new SimpleCurve
		{
			{
				new CurvePoint(-1f, 0.8667f),
				true
			},
			{
				new CurvePoint(0f, 0.8f),
				true
			},
			{
				new CurvePoint(1f, 0.5657f),
				true
			},
			{
				new CurvePoint(2f, 0.5f),
				true
			},
			{
				new CurvePoint(8f, 0.2632f),
				true
			}
		};

		// Token: 0x040021F8 RID: 8696
		public static readonly SimpleCurve DeathOnDownedChance_NonColonyHumanlikeFromPopulationIntentCurve_WaveringPrisoners = new SimpleCurve
		{
			{
				new CurvePoint(-1f, 0.92f),
				true
			},
			{
				new CurvePoint(0f, 0.85f),
				true
			},
			{
				new CurvePoint(1f, 0.62f),
				true
			},
			{
				new CurvePoint(2f, 0.55f),
				true
			},
			{
				new CurvePoint(8f, 0.3f),
				true
			}
		};

		// Token: 0x040021F9 RID: 8697
		public const int MaxPopulationAlwaysRecruitable = 3;

		// Token: 0x040021FA RID: 8698
		public static readonly SimpleCurve NonRecruitableChanceOverPopulationIntentCurve = new SimpleCurve
		{
			{
				new CurvePoint(-1f, 0.4f),
				true
			},
			{
				new CurvePoint(0f, 0.25f),
				true
			},
			{
				new CurvePoint(1f, 0.13f),
				true
			},
			{
				new CurvePoint(2f, 0.1f),
				true
			},
			{
				new CurvePoint(8f, 0.05f),
				true
			}
		};

		// Token: 0x040021FB RID: 8699
		public const float TendPriority_LifeThreateningDisease = 1f;

		// Token: 0x040021FC RID: 8700
		public const float TendPriority_PerBleedRate = 1.5f;

		// Token: 0x040021FD RID: 8701
		public const float TendPriority_DiseaseSeverityDecreasesWhenTended = 0.025f;

		// Token: 0x02000679 RID: 1657
		public struct PainCategoryWeighted
		{
			// Token: 0x06002988 RID: 10632 RVA: 0x000DB0A0 File Offset: 0x000D92A0
			public PainCategoryWeighted(PainCategory category, float weight)
			{
				this.category = category;
				this.weight = weight;
			}

			// Token: 0x040021FE RID: 8702
			public PainCategory category;

			// Token: 0x040021FF RID: 8703
			public float weight;
		}
	}
}
