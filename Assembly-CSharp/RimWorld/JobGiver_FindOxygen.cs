using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013E0 RID: 5088
	public class JobGiver_FindOxygen : ThinkNode_JobGiver
	{
		// Token: 0x06007C47 RID: 31815 RVA: 0x00256CD4 File Offset: 0x00254ED4
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return null;
			}
			if (PawnUtility.PlayerForcedJobNowOrSoon(pawn))
			{
				return null;
			}
			if (!pawn.ConcernedByVacuum)
			{
				return null;
			}
			Hediff hediff;
			if (!pawn.health.hediffSet.TryGetHediff(HediffDefOf.VacuumExposure, out hediff) || hediff.CurStageIndex < 1)
			{
				return null;
			}
			float vacuum = pawn.Position.GetVacuum(pawn.Map);
			IntVec3 cell = this.GuessJobDestination(pawn);
			if (cell.IsValid)
			{
				vacuum = cell.GetVacuum(pawn.Map);
			}
			if (vacuum < 0.5f)
			{
				return null;
			}
			Region region = JobGiver_FindOxygen.ClosestOxygenatedRegion(pawn.Position, pawn.MapHeld, pawn, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), RegionType.Set_Passable);
			if (region != null)
			{
				IntVec3 c;
				JobGiver_FindOxygen.TryGetAllowedCellInRegion(region, pawn, out c, -1f);
				return JobMaker.MakeJob(JobDefOf.GotoOxygenatedArea, c);
			}
			return null;
		}

		// Token: 0x06007C48 RID: 31816 RVA: 0x00256DA4 File Offset: 0x00254FA4
		private IntVec3 GuessJobDestination(Pawn pawn)
		{
			if (pawn.CurJob == null)
			{
				return IntVec3.Invalid;
			}
			Job curJob = pawn.CurJob;
			if (!curJob.targetA.HasThing && curJob.targetA.Cell.IsValid)
			{
				return curJob.targetA.Cell;
			}
			if (!curJob.targetB.HasThing && curJob.targetB.Cell.IsValid)
			{
				return curJob.targetB.Cell;
			}
			if (!curJob.targetC.HasThing && curJob.targetC.Cell.IsValid)
			{
				return curJob.targetC.Cell;
			}
			if (curJob.targetA.HasThing && curJob.targetA.Cell.IsValid)
			{
				return curJob.targetA.Cell;
			}
			if (curJob.targetB.HasThing && curJob.targetB.Cell.IsValid)
			{
				return curJob.targetB.Cell;
			}
			if (curJob.targetC.HasThing && curJob.targetC.Cell.IsValid)
			{
				return curJob.targetC.Cell;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06007C49 RID: 31817 RVA: 0x00256EE0 File Offset: 0x002550E0
		private static Region ClosestOxygenatedRegion(IntVec3 root, Map map, Pawn pawn, TraverseParms traverseParms, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			Region region = root.GetRegion(map, traversableRegionTypes);
			if (region == null)
			{
				return null;
			}
			RegionEntryPredicate entryCondition = (Region _, Region r) => r.Allows(traverseParms, false);
			Region foundReg = null;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				if (r.IsDoorway)
				{
					return false;
				}
				IntVec3 intVec;
				if (!JobGiver_FindOxygen.TryGetAllowedCellInRegion(r, pawn, out intVec, -1f))
				{
					return false;
				}
				if (r.Room.Vacuum < 0.5f)
				{
					foundReg = r;
					return true;
				}
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, 9999, traversableRegionTypes);
			return foundReg;
		}

		// Token: 0x06007C4A RID: 31818 RVA: 0x00256F48 File Offset: 0x00255148
		private static bool TryGetAllowedCellInRegion(Region region, Pawn pawn, out IntVec3 cell, float maxRadius = -1f)
		{
			cell = IntVec3.Invalid;
			for (int i = 0; i < 100; i++)
			{
				IntVec3 randomCell = region.RandomCell;
				if (!randomCell.Fogged(pawn.Map) && randomCell.InAllowedArea(pawn) && (maxRadius < 0f || randomCell.InHorDistOf(pawn.Position, maxRadius)))
				{
					cell = randomCell;
					return true;
				}
			}
			foreach (IntVec3 intVec in region.Cells.InRandomOrder(JobGiver_FindOxygen.tmpWorkingCellList))
			{
				if (!intVec.Fogged(pawn.Map) && intVec.InAllowedArea(pawn) && (maxRadius < 0f || intVec.InHorDistOf(pawn.Position, maxRadius)))
				{
					cell = intVec;
					return true;
				}
			}
			return false;
		}

		// Token: 0x040053DB RID: 21467
		private static List<IntVec3> tmpWorkingCellList = new List<IntVec3>();
	}
}
