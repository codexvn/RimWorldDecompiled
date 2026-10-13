using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200185A RID: 6234
	public class ThoughtWorker_Incestuous : ThoughtWorker
	{
		// Token: 0x06008F9A RID: 36762 RVA: 0x002A6750 File Offset: 0x002A4950
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (LovePartnerRelationUtility.IncestOpinionOffsetFor(other, pawn) == 0f)
			{
				return false;
			}
			return true;
		}
	}
}
