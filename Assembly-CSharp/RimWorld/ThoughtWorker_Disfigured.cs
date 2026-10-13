using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200185C RID: 6236
	public class ThoughtWorker_Disfigured : ThoughtWorker
	{
		// Token: 0x06008F9E RID: 36766 RVA: 0x002A67D0 File Offset: 0x002A49D0
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike || other.Dead)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (!RelationsUtility.IsDisfigured(other, pawn, false))
			{
				return false;
			}
			if (PawnUtility.IsBiologicallyBlind(pawn))
			{
				return false;
			}
			if (!pawn.story.CaresAboutOthersAppearance)
			{
				return false;
			}
			if (pawn.Ideo != null && pawn.Ideo.IdeoApprovesOfBlindness() && !RelationsUtility.IsDisfigured(other, pawn, true) && (PawnUtility.IsBiologicallyBlind(other) || ThoughtWorker_Precept_HalfBlind.IsHalfBlind(other)))
			{
				return false;
			}
			return true;
		}
	}
}
