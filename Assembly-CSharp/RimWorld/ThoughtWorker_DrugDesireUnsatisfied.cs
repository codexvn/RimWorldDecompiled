using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200188E RID: 6286
	public class ThoughtWorker_DrugDesireUnsatisfied : ThoughtWorker
	{
		// Token: 0x0600900C RID: 36876 RVA: 0x002A7F4C File Offset: 0x002A614C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Need_Chemical_Any drugsDesire = p.needs.drugsDesire;
			if (drugsDesire == null)
			{
				return false;
			}
			int moodBuffForCurrentLevel = (int)drugsDesire.MoodBuffForCurrentLevel;
			if (moodBuffForCurrentLevel < 3)
			{
				return ThoughtState.ActiveAtStage(3 - moodBuffForCurrentLevel - 1);
			}
			return false;
		}

		// Token: 0x04005C36 RID: 23606
		private const int Neutral = 3;
	}
}
