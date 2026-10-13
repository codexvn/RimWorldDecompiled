using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013CC RID: 5068
	public class JobGiver_MineRandom : ThinkNode_JobGiver
	{
		// Token: 0x06007C19 RID: 31769 RVA: 0x00255F34 File Offset: 0x00254134
		protected override Job TryGiveJob(Pawn pawn)
		{
			Region region = pawn.GetRegion(RegionType.Set_Passable);
			if (region == null)
			{
				return null;
			}
			for (int i = 0; i < 40; i++)
			{
				IntVec3 randomCell = region.RandomCell;
				for (int j = 0; j < 4; j++)
				{
					IntVec3 c = randomCell + GenAdj.CardinalDirections[j];
					if (c.InBounds(pawn.Map))
					{
						Building edifice = c.GetEdifice(pawn.Map);
						if (edifice != null && (edifice.def.passability == Traversability.Impassable || edifice.def.IsDoor) && edifice.def.size == IntVec2.One && edifice.def != ThingDefOf.CollapsedRocks && pawn.CanReserve(edifice, 1, -1, null, false))
						{
							Job job = JobMaker.MakeJob(JobDefOf.Mine, edifice);
							job.ignoreDesignations = true;
							return job;
						}
					}
				}
			}
			return null;
		}
	}
}
