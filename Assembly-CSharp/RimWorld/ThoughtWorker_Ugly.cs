using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001867 RID: 6247
	public class ThoughtWorker_Ugly : ThoughtWorker
	{
		// Token: 0x06008FB4 RID: 36788 RVA: 0x002A6D30 File Offset: 0x002A4F30
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (PawnUtility.IsBiologicallyOrArtificiallyBlind(pawn))
			{
				return false;
			}
			if (!pawn.story.CaresAboutOthersAppearance)
			{
				return false;
			}
			float statValue = other.GetStatValue(StatDefOf.PawnBeauty, true, -1);
			if (statValue <= -2f)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (statValue <= -1f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return false;
		}
	}
}
