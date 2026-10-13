using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001869 RID: 6249
	public class ThoughtWorker_CreepyBreathing : ThoughtWorker
	{
		// Token: 0x06008FB8 RID: 36792 RVA: 0x002A6E3C File Offset: 0x002A503C
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (!other.story.traits.HasTrait(TraitDefOf.CreepyBreathing))
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
