using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018D2 RID: 6354
	public class ThoughtWorker_Recluse : ThoughtWorker
	{
		// Token: 0x060090CA RID: 37066 RVA: 0x002AA3F8 File Offset: 0x002A85F8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.Spawned)
			{
				return ThoughtState.Inactive;
			}
			if (ThoughtUtility.ThoughtNullified(p, this.def))
			{
				return ThoughtState.Inactive;
			}
			int freeColonistsAndPrisonersSpawnedCount = p.Map.mapPawns.FreeColonistsAndPrisonersSpawnedCount;
			if (freeColonistsAndPrisonersSpawnedCount <= 1)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (freeColonistsAndPrisonersSpawnedCount <= 4)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (freeColonistsAndPrisonersSpawnedCount <= 10)
			{
				return ThoughtState.Inactive;
			}
			if (freeColonistsAndPrisonersSpawnedCount <= 15)
			{
				return ThoughtState.ActiveAtStage(2);
			}
			return ThoughtState.ActiveAtStage(3);
		}
	}
}
