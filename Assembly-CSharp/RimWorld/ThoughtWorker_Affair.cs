using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001859 RID: 6233
	public class ThoughtWorker_Affair : ThoughtWorker
	{
		// Token: 0x06008F98 RID: 36760 RVA: 0x002A6698 File Offset: 0x002A4898
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			if (!p.relations.DirectRelationExists(PawnRelationDefOf.Spouse, otherPawn))
			{
				return false;
			}
			List<DirectPawnRelation> directRelations = otherPawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (directRelations[i].otherPawn != p && !directRelations[i].otherPawn.Dead && (directRelations[i].def == PawnRelationDefOf.Lover || directRelations[i].def == PawnRelationDefOf.Fiance) && !new HistoryEvent(otherPawn.GetHistoryEventLoveRelationCount(), otherPawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
				{
					return true;
				}
			}
			return false;
		}
	}
}
