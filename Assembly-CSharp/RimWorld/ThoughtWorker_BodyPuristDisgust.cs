using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200186D RID: 6253
	public class ThoughtWorker_BodyPuristDisgust : ThoughtWorker
	{
		// Token: 0x06008FC0 RID: 36800 RVA: 0x002A7040 File Offset: 0x002A5240
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!p.story.traits.HasTrait(TraitDefOf.BodyPurist))
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
