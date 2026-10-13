using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200186C RID: 6252
	public class ThoughtWorker_TranshumanistAppreciation : ThoughtWorker
	{
		// Token: 0x06008FBE RID: 36798 RVA: 0x002A6FC0 File Offset: 0x002A51C0
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!p.story.traits.HasTrait(TraitDefOf.Transhumanist))
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
			int num = GeneUtility.AddedAndImplantedPartsWithXenogenesCount(other);
			if (num > 0)
			{
				return ThoughtState.ActiveAtStage(num - 1);
			}
			return false;
		}
	}
}
