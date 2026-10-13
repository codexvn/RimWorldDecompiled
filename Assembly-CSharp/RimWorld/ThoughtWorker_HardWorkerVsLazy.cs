using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200185D RID: 6237
	public class ThoughtWorker_HardWorkerVsLazy : ThoughtWorker
	{
		// Token: 0x06008FA0 RID: 36768 RVA: 0x002A687C File Offset: 0x002A4A7C
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (p.story.traits.DegreeOfTrait(TraitDefOf.Industriousness) <= 0)
			{
				return false;
			}
			if (!other.RaceProps.Humanlike)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				return false;
			}
			if (other.story.traits.DegreeOfTrait(TraitDefOf.Industriousness) > 0)
			{
				return false;
			}
			return true;
		}
	}
}
