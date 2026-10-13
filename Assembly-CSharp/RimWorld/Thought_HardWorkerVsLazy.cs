using System;

namespace RimWorld
{
	// Token: 0x0200185E RID: 6238
	public class Thought_HardWorkerVsLazy : Thought_SituationalSocial
	{
		// Token: 0x06008FA2 RID: 36770 RVA: 0x002A6908 File Offset: 0x002A4B08
		public override float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			int num = this.otherPawn.story.traits.DegreeOfTrait(TraitDefOf.Industriousness);
			if (num > 0)
			{
				return 0f;
			}
			if (num == 0)
			{
				return -5f;
			}
			if (num == -1)
			{
				return -20f;
			}
			return -30f;
		}
	}
}
