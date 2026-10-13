using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001878 RID: 6264
	public class ThoughtWorker_ExpectationsSlave : ThoughtWorker
	{
		// Token: 0x06008FD8 RID: 36824 RVA: 0x002A75BC File Offset: 0x002A57BC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.IsSlave)
			{
				return ThoughtState.Inactive;
			}
			ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(p);
			if (expectationDef == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(this.GetThoughtStageForExpectation(expectationDef));
		}

		// Token: 0x06008FD9 RID: 36825 RVA: 0x002A75F3 File Offset: 0x002A57F3
		private int GetThoughtStageForExpectation(ExpectationDef expectation)
		{
			if (expectation == ExpectationDefOf.ExtremelyLow)
			{
				return 0;
			}
			if (expectation == ExpectationDefOf.VeryLow)
			{
				return 1;
			}
			if (expectation == ExpectationDefOf.Low)
			{
				return 2;
			}
			return 3;
		}
	}
}
