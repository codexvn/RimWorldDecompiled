using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001896 RID: 6294
	public class ThoughtWorker_Ascetic : ThoughtWorker
	{
		// Token: 0x0600901D RID: 36893 RVA: 0x002A8228 File Offset: 0x002A6428
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.IsColonist)
			{
				return false;
			}
			Room ownedRoom = p.ownership.OwnedRoom;
			if (ownedRoom == null)
			{
				return false;
			}
			int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(ownedRoom.GetStat(RoomStatDefOf.Impressiveness));
			if (this.def.stages[scoreStageIndex] != null)
			{
				return ThoughtState.ActiveAtStage(scoreStageIndex);
			}
			return ThoughtState.Inactive;
		}
	}
}
