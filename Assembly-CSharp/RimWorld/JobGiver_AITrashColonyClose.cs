using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001415 RID: 5141
	public class JobGiver_AITrashColonyClose : ThinkNode_JobGiver
	{
		// Token: 0x06007D27 RID: 32039 RVA: 0x0025AD94 File Offset: 0x00258F94
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.HostileTo(Faction.OfPlayer))
			{
				return null;
			}
			bool flag = pawn.natives.IgniteVerb != null && pawn.natives.IgniteVerb.IsStillUsableBy(pawn) && pawn.HostileTo(Faction.OfPlayer);
			CellRect cellRect = CellRect.CenteredOn(pawn.Position, 5);
			for (int i = 0; i < 35; i++)
			{
				IntVec3 randomCell = cellRect.RandomCell;
				if (randomCell.InBounds(pawn.Map))
				{
					Building edifice = randomCell.GetEdifice(pawn.Map);
					if (edifice != null && TrashUtility.ShouldTrashBuilding(pawn, edifice, false) && GenSight.LineOfSight(pawn.Position, randomCell, pawn.Map))
					{
						if (DebugViewSettings.drawDestSearch && Find.CurrentMap == pawn.Map)
						{
							Find.CurrentMap.debugDrawer.FlashCell(randomCell, 1f, "trash bld", 50);
						}
						Job job = TrashUtility.TrashJob(pawn, edifice, false, false);
						if (job != null)
						{
							return job;
						}
					}
					if (flag)
					{
						Plant plant = randomCell.GetPlant(pawn.Map);
						if (plant != null && TrashUtility.ShouldTrashPlant(pawn, plant) && GenSight.LineOfSight(pawn.Position, randomCell, pawn.Map))
						{
							if (DebugViewSettings.drawDestSearch && Find.CurrentMap == pawn.Map)
							{
								Find.CurrentMap.debugDrawer.FlashCell(randomCell, 0.5f, "trash plant", 50);
							}
							Job job2 = TrashUtility.TrashJob(pawn, plant, false, false);
							if (job2 != null)
							{
								return job2;
							}
						}
					}
					if (DebugViewSettings.drawDestSearch && Find.CurrentMap == pawn.Map)
					{
						Find.CurrentMap.debugDrawer.FlashCell(randomCell, 0f, "trash no", 50);
					}
				}
			}
			return null;
		}

		// Token: 0x04005437 RID: 21559
		private const int CloseSearchRadius = 5;
	}
}
