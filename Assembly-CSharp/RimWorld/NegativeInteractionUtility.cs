using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020023FE RID: 9214
	public static class NegativeInteractionUtility
	{
		// Token: 0x0600C7AE RID: 51118 RVA: 0x003A3910 File Offset: 0x003A1B10
		public static float NegativeInteractionChanceFactor(Pawn initiator, Pawn recipient)
		{
			if (initiator.story.traits.HasTrait(TraitDefOf.Kind))
			{
				return 0f;
			}
			float num = 1f;
			num *= NegativeInteractionUtility.OpinionFactorCurve.Evaluate((float)initiator.relations.OpinionOf(recipient));
			num *= NegativeInteractionUtility.CompatibilityFactorCurve.Evaluate(initiator.relations.CompatibilityWith(recipient));
			if (initiator.story.traits.HasTrait(TraitDefOf.Abrasive))
			{
				num *= 2.3f;
			}
			return num;
		}

		// Token: 0x04008839 RID: 34873
		public const float AbrasiveSelectionChanceFactor = 2.3f;

		// Token: 0x0400883A RID: 34874
		private static readonly SimpleCurve CompatibilityFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(-2.5f, 4f),
				true
			},
			{
				new CurvePoint(-1.5f, 3f),
				true
			},
			{
				new CurvePoint(-0.5f, 2f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.75f),
				true
			},
			{
				new CurvePoint(2f, 0.5f),
				true
			},
			{
				new CurvePoint(3f, 0.4f),
				true
			}
		};

		// Token: 0x0400883B RID: 34875
		private static readonly SimpleCurve OpinionFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(-100f, 6f),
				true
			},
			{
				new CurvePoint(-50f, 4f),
				true
			},
			{
				new CurvePoint(-25f, 2f),
				true
			},
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(50f, 0.1f),
				true
			},
			{
				new CurvePoint(100f, 0f),
				true
			}
		};
	}
}
