using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200182A RID: 6186
	public class ThoughtWorker_NeedNeuralSupercharge : ThoughtWorker_Precept
	{
		// Token: 0x06008F19 RID: 36633 RVA: 0x002A4E6C File Offset: 0x002A306C
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.health == null)
			{
				return false;
			}
			if (!ResearchProjectDefOf.MicroelectronicsBasics.IsFinished)
			{
				return false;
			}
			int lastReceivedNeuralSuperchargeTick = p.health.lastReceivedNeuralSuperchargeTick;
			return Find.TickManager.TicksGame - lastReceivedNeuralSuperchargeTick >= 30000 || lastReceivedNeuralSuperchargeTick == -1;
		}

		// Token: 0x04005C25 RID: 23589
		public const int TicksUntilNeed = 30000;
	}
}
