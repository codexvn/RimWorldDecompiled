using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017E6 RID: 6118
	public class ThoughtWorker_Precept_Scarification : ThoughtWorker_Precept
	{
		// Token: 0x06008E3E RID: 36414 RVA: 0x002A2748 File Offset: 0x002A0948
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!p.IsColonist)
			{
				return false;
			}
			int hediffCount = p.health.hediffSet.GetHediffCount(HediffDefOf.Scarification);
			int requiredScars = p.ideo.Ideo.RequiredScars;
			if (hediffCount >= requiredScars)
			{
				return this.ProcessedState(0, p);
			}
			if (hediffCount == 0)
			{
				return this.ProcessedState(1, p);
			}
			if (hediffCount < requiredScars)
			{
				return this.ProcessedState(2, p);
			}
			return false;
		}

		// Token: 0x06008E3F RID: 36415 RVA: 0x002A27B8 File Offset: 0x002A09B8
		private ThoughtState ProcessedState(int index, Pawn p)
		{
			if (this.def.stages[index].baseMoodEffect < 0f && this.def.minExpectationForNegativeThought != null && p.MapHeld != null && ExpectationsUtility.CurrentExpectationFor(p.MapHeld).order < this.def.minExpectationForNegativeThought.order)
			{
				return false;
			}
			return ThoughtState.ActiveAtStage(index);
		}
	}
}
