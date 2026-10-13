using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200186A RID: 6250
	public class ThoughtWorker_Man : ThoughtWorker
	{
		// Token: 0x06008FBA RID: 36794 RVA: 0x002A6EC8 File Offset: 0x002A50C8
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!p.story.traits.HasTrait(TraitDefOf.DislikesMen))
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				return false;
			}
			if (other.def != p.def)
			{
				return false;
			}
			if (other.gender != Gender.Male)
			{
				return false;
			}
			return true;
		}
	}
}
