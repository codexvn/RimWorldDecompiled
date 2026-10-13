using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001532 RID: 5426
	public class JobGiver_ReachOutside : ThinkNode_JobGiver
	{
		// Token: 0x060080FB RID: 33019 RVA: 0x0026D704 File Offset: 0x0026B904
		protected override Job TryGiveJob(Pawn pawn)
		{
			District district = pawn.GetDistrict(RegionType.Set_Passable);
			if (district.Room.PsychologicallyOutdoors && district.TouchesMapEdge)
			{
				return null;
			}
			if (!pawn.CanReachMapEdge())
			{
				return null;
			}
			IntVec3 intVec;
			if (!RCellFinder.TryFindRandomSpotJustOutsideColony(pawn, out intVec))
			{
				return null;
			}
			if (intVec == pawn.Position)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Goto, intVec);
		}
	}
}
