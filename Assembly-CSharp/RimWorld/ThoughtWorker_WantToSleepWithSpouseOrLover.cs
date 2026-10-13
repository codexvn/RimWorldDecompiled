using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001858 RID: 6232
	public class ThoughtWorker_WantToSleepWithSpouseOrLover : ThoughtWorker
	{
		// Token: 0x06008F96 RID: 36758 RVA: 0x002A6540 File Offset: 0x002A4740
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			List<DirectPawnRelation> list = LovePartnerRelationUtility.ExistingLovePartners(p, false);
			bool isSlaveOfColony = p.IsSlaveOfColony;
			if (list.NullOrEmpty<DirectPawnRelation>())
			{
				return false;
			}
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].otherPawn.IsColonist && !list[i].otherPawn.IsWorldPawn() && list[i].otherPawn.relations.everSeenByPlayer && isSlaveOfColony == list[i].otherPawn.IsSlaveOfColony)
				{
					if (p.ownership.OwnedBed != null && p.ownership.OwnedBed == list[i].otherPawn.ownership.OwnedBed)
					{
						return false;
					}
					if (p.ownership.OwnedRoom != null && p.ownership.OwnedRoom == list[i].otherPawn.ownership.OwnedRoom)
					{
						return false;
					}
					HistoryEventDef def = (list[i].def == PawnRelationDefOf.Spouse) ? HistoryEventDefOf.SharedBed_Spouse : HistoryEventDefOf.SharedBed_NonSpouse;
					if (new HistoryEvent(def, p.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
					{
						num++;
					}
				}
			}
			return num > 0;
		}
	}
}
