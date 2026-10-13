using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200182B RID: 6187
	public class ThoughtWorker_AgeReversalDemanded : ThoughtWorker_Precept
	{
		// Token: 0x06008F1B RID: 36635 RVA: 0x002A4EC8 File Offset: 0x002A30C8
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.Faction != Faction.OfPlayer)
			{
				return ThoughtState.Inactive;
			}
			if (p.IsSlave)
			{
				return ThoughtState.Inactive;
			}
			if (p.ageTracker == null)
			{
				return ThoughtState.Inactive;
			}
			if (p.ageTracker.AgeBiologicalYears < 25)
			{
				return ThoughtState.Inactive;
			}
			long ageReversalDemandedDeadlineTicks = p.ageTracker.AgeReversalDemandedDeadlineTicks;
			if (ageReversalDemandedDeadlineTicks > 0L)
			{
				return ThoughtState.ActiveAtStage(3);
			}
			long num = -ageReversalDemandedDeadlineTicks / 60000L;
			int stageIndex;
			if (num <= 15L)
			{
				stageIndex = 0;
			}
			else if (num <= 30L)
			{
				stageIndex = 1;
			}
			else
			{
				stageIndex = 2;
			}
			return ThoughtState.ActiveAtStage(stageIndex);
		}

		// Token: 0x06008F1C RID: 36636 RVA: 0x002A4F58 File Offset: 0x002A3158
		public static bool CanHaveThought(Pawn pawn)
		{
			return ModLister.CheckIdeology("Age Reversal") && ThoughtDefOf.AgeReversalDemanded.Worker.CurrentState(pawn).Active;
		}
	}
}
