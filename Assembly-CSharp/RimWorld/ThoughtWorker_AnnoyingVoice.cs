using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001868 RID: 6248
	public class ThoughtWorker_AnnoyingVoice : ThoughtWorker
	{
		// Token: 0x06008FB6 RID: 36790 RVA: 0x002A6DB0 File Offset: 0x002A4FB0
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (!other.story.traits.HasTrait(TraitDefOf.AnnoyingVoice))
			{
				return false;
			}
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Hearing))
			{
				return false;
			}
			if (pawn.story.traits.HasTrait(TraitDefOf.Kind))
			{
				return false;
			}
			return true;
		}
	}
}
