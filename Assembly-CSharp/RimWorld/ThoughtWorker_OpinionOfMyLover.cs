using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001851 RID: 6225
	public class ThoughtWorker_OpinionOfMyLover : ThoughtWorker
	{
		// Token: 0x06008F83 RID: 36739 RVA: 0x002A6218 File Offset: 0x002A4418
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(p, false);
			if (directPawnRelation == null)
			{
				return false;
			}
			if (!directPawnRelation.otherPawn.IsColonist || directPawnRelation.otherPawn.IsWorldPawn() || !directPawnRelation.otherPawn.relations.everSeenByPlayer)
			{
				return false;
			}
			return p.relations.OpinionOf(directPawnRelation.otherPawn) != 0;
		}
	}
}
