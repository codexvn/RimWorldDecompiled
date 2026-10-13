using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003871 RID: 14449
	public static class GeneTuning
	{
		// Token: 0x0400DC96 RID: 56470
		public static readonly SimpleCurve ComplexityToCreationHoursCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 3f),
				true
			},
			{
				new CurvePoint(4f, 5f),
				true
			},
			{
				new CurvePoint(8f, 8f),
				true
			},
			{
				new CurvePoint(12f, 12f),
				true
			},
			{
				new CurvePoint(16f, 17f),
				true
			},
			{
				new CurvePoint(20f, 23f),
				true
			}
		};

		// Token: 0x0400DC97 RID: 56471
		public static readonly SimpleCurve MetabolismToFoodConsumptionFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(-5f, 2.25f),
				true
			},
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(5f, 0.5f),
				true
			}
		};

		// Token: 0x0400DC98 RID: 56472
		public static readonly IntRange BiostatRange = new IntRange(-5, 5);

		// Token: 0x0400DC99 RID: 56473
		public const int BaseMaxComplexity = 6;

		// Token: 0x0400DC9A RID: 56474
		public const float ChemicalDependencyDurationDays_Deficiency = 5f;

		// Token: 0x0400DC9B RID: 56475
		public const float ChemicalDependencyDurationDays_Coma = 30f;

		// Token: 0x0400DC9C RID: 56476
		public const float ChemicalDependencyDurationDays_Death = 60f;

		// Token: 0x0400DC9D RID: 56477
		public const float HemogenGainPerCannibalNutrition = 0.0375f;

		// Token: 0x0400DC9E RID: 56478
		public static readonly FloatRange GeneExtractorRegrowingDurationDaysRange = new FloatRange(12f, 20f);

		// Token: 0x0400DC9F RID: 56479
		public static readonly FloatRange SkinColorValueRange = new FloatRange(0.23f, 0.94f);

		// Token: 0x0400DCA0 RID: 56480
		public static readonly FloatRange HairColorValueRange = new FloatRange(0.1f, 0.98f);
	}
}
