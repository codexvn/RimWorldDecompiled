using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014E8 RID: 5352
	public class JoyGiver_GoForWalk : JoyGiver
	{
		// Token: 0x06007FCA RID: 32714 RVA: 0x0026736C File Offset: 0x0026556C
		public override Job TryGiveJob(Pawn pawn)
		{
			JoyGiver_GoForWalk.<>c__DisplayClass0_0 CS$<>8__locals1 = new JoyGiver_GoForWalk.<>c__DisplayClass0_0();
			CS$<>8__locals1.pawn = pawn;
			if (!JoyUtility.EnjoyableOutsideNow(CS$<>8__locals1.pawn, null))
			{
				return null;
			}
			if (PawnUtility.WillSoonHaveBasicNeed(CS$<>8__locals1.pawn, 0.05f))
			{
				return null;
			}
			Region reg;
			if (!CellFinder.TryFindClosestRegionWith(CS$<>8__locals1.pawn.GetRegion(RegionType.Set_Passable), TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), new Predicate<Region>(CS$<>8__locals1.<TryGiveJob>g__RegionValidator|1), 100, out reg, RegionType.Set_Passable))
			{
				return null;
			}
			IntVec3 root;
			if (!reg.TryFindRandomCellInRegionUnforbidden(CS$<>8__locals1.pawn, new Predicate<IntVec3>(CS$<>8__locals1.<TryGiveJob>g__CellValidator|0), out root))
			{
				return null;
			}
			List<IntVec3> list;
			if (!WalkPathFinder.TryFindWalkPath(CS$<>8__locals1.pawn, root, out list))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(this.def.jobDef, list[0]);
			job.targetQueueA = new List<LocalTargetInfo>();
			for (int i = 1; i < list.Count; i++)
			{
				job.targetQueueA.Add(list[i]);
			}
			job.locomotionUrgency = LocomotionUrgency.Walk;
			return job;
		}
	}
}
