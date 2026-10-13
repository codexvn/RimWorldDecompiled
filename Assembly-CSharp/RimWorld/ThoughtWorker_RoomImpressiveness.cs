using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200184D RID: 6221
	public abstract class ThoughtWorker_RoomImpressiveness : ThoughtWorker
	{
		// Token: 0x06008F7B RID: 36731 RVA: 0x002A6090 File Offset: 0x002A4290
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.story.traits.HasTrait(TraitDefOf.Ascetic))
			{
				return ThoughtState.Inactive;
			}
			Room room = p.GetRoom(RegionType.Set_All);
			if (room == null)
			{
				return ThoughtState.Inactive;
			}
			int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
			if (this.def.stages[scoreStageIndex] == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(scoreStageIndex);
		}
	}
}
