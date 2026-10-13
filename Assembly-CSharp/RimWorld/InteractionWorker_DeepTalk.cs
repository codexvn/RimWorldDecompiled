using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020023FA RID: 9210
	public class InteractionWorker_DeepTalk : InteractionWorker
	{
		// Token: 0x0600C7A5 RID: 51109 RVA: 0x003A3556 File Offset: 0x003A1756
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			if (initiator.Inhumanized())
			{
				return 0f;
			}
			return 0.075f * InteractionWorker_DeepTalk.CompatibilityFactorCurve.Evaluate(initiator.relations.CompatibilityWith(recipient));
		}

		// Token: 0x04008833 RID: 34867
		private const float BaseSelectionWeight = 0.075f;

		// Token: 0x04008834 RID: 34868
		private static readonly SimpleCurve CompatibilityFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(-1.5f, 0f),
				true
			},
			{
				new CurvePoint(-0.5f, 0.1f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 1.8f),
				true
			},
			{
				new CurvePoint(2f, 3f),
				true
			}
		};
	}
}
