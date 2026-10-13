using System;

namespace RimWorld
{
	// Token: 0x02001864 RID: 6244
	public class Thought_ChemicalInterestVsTeetotaler : Thought_SituationalSocial
	{
		// Token: 0x06008FAE RID: 36782 RVA: 0x002A6BBC File Offset: 0x002A4DBC
		public override float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			int num = this.pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire);
			if (this.otherPawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) >= 0)
			{
				return 0f;
			}
			if (num == 1)
			{
				return -20f;
			}
			return -30f;
		}
	}
}
