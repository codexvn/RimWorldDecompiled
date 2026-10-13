using System;

namespace RimWorld
{
	// Token: 0x0200185B RID: 6235
	public class Thought_Incestuous : Thought_SituationalSocial
	{
		// Token: 0x06008F9C RID: 36764 RVA: 0x002A679C File Offset: 0x002A499C
		public override float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			return LovePartnerRelationUtility.IncestOpinionOffsetFor(this.otherPawn, this.pawn);
		}
	}
}
