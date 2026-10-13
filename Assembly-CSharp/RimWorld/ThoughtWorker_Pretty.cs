using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001866 RID: 6246
	public class ThoughtWorker_Pretty : ThoughtWorker
	{
		// Token: 0x06008FB2 RID: 36786 RVA: 0x002A6CB4 File Offset: 0x002A4EB4
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (RelationsUtility.IsDisfigured(other, pawn, false))
			{
				return false;
			}
			if (PawnUtility.IsBiologicallyOrArtificiallyBlind(pawn))
			{
				return false;
			}
			float statValue = other.GetStatValue(StatDefOf.PawnBeauty, true, -1);
			if (statValue >= 2f)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (statValue >= 1f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return false;
		}
	}
}
