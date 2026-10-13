using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200186B RID: 6251
	public class ThoughtWorker_Woman : ThoughtWorker
	{
		// Token: 0x06008FBC RID: 36796 RVA: 0x002A6F44 File Offset: 0x002A5144
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!p.story.traits.HasTrait(TraitDefOf.DislikesWomen))
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
			if (other.gender != Gender.Female)
			{
				return false;
			}
			return true;
		}
	}
}
