using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001873 RID: 6259
	public class ThoughtWorker_Joyous : ThoughtWorker
	{
		// Token: 0x06008FCD RID: 36813 RVA: 0x002A7318 File Offset: 0x002A5518
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				return false;
			}
			return other.story.traits.HasTrait(TraitDefOf.Joyous);
		}
	}
}
