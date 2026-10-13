using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200188D RID: 6285
	public class ThoughtWorker_DrugDesireSatisfied : ThoughtWorker
	{
		// Token: 0x0600900A RID: 36874 RVA: 0x002A7F0C File Offset: 0x002A610C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Need_Chemical_Any drugsDesire = p.needs.drugsDesire;
			if (drugsDesire == null)
			{
				return false;
			}
			int moodBuffForCurrentLevel = (int)drugsDesire.MoodBuffForCurrentLevel;
			if (moodBuffForCurrentLevel > 3)
			{
				return ThoughtState.ActiveAtStage(moodBuffForCurrentLevel - 3 - 1);
			}
			return false;
		}

		// Token: 0x04005C35 RID: 23605
		private const int Neutral = 3;
	}
}
