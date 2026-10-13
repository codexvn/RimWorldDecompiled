using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020023F7 RID: 9207
	public class InteractionWorker_KindWords : InteractionWorker
	{
		// Token: 0x0600C79D RID: 51101 RVA: 0x003A30D4 File Offset: 0x003A12D4
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			if (initiator.Inhumanized())
			{
				return 0f;
			}
			Trait trait = initiator.story.traits.GetTrait(TraitDefOf.Kind);
			if (trait != null && !trait.Suppressed)
			{
				return 0.01f;
			}
			return 0f;
		}
	}
}
