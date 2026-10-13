using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001894 RID: 6292
	public class ThoughtWorker_Greedy : ThoughtWorker
	{
		// Token: 0x06009019 RID: 36889 RVA: 0x002A80B0 File Offset: 0x002A62B0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.IsColonist)
			{
				return false;
			}
			Room ownedRoom = p.ownership.OwnedRoom;
			if (ownedRoom == null)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			int num = RoomStatDefOf.Impressiveness.GetScoreStageIndex(ownedRoom.GetStat(RoomStatDefOf.Impressiveness)) + 1;
			if (this.def.stages[num] != null)
			{
				return ThoughtState.ActiveAtStage(num);
			}
			return ThoughtState.Inactive;
		}
	}
}
