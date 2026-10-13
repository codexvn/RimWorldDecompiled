using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014C9 RID: 5321
	public class JobGiver_SeekSafeTemperature : ThinkNode_JobGiver
	{
		// Token: 0x06007F4C RID: 32588 RVA: 0x0026549C File Offset: 0x0026369C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (this.requiresInjury && !pawn.health.hediffSet.HasTemperatureInjury(TemperatureInjuryStage.Serious))
			{
				return null;
			}
			FloatRange tempRange = pawn.ComfortableTemperatureRange();
			if (tempRange.Includes(pawn.AmbientTemperature))
			{
				if (!this.waitInSafeTemp)
				{
					return null;
				}
				return JobMaker.MakeJob(JobDefOf.Wait_SafeTemperature, 500, true);
			}
			else
			{
				Region region = JobGiver_SeekSafeTemperature.ClosestRegionWithinTemperatureRange(pawn.Position, pawn.MapHeld, pawn, tempRange, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), RegionType.Set_Passable, this.maxRadius);
				if (region != null)
				{
					IntVec3 c;
					JobGiver_SeekSafeTemperature.TryGetAllowedCellInRegion(region, pawn, out c, -1f);
					return JobMaker.MakeJob(JobDefOf.GotoSafeTemperature, c);
				}
				return null;
			}
		}

		// Token: 0x06007F4D RID: 32589 RVA: 0x00265544 File Offset: 0x00263744
		public static Region ClosestRegionWithinTemperatureRange(IntVec3 root, Map map, Pawn pawn, FloatRange tempRange, TraverseParms traverseParms, RegionType traversableRegionTypes = RegionType.Set_Passable, float maxRadius = -1f)
		{
			Region region = root.GetRegion(map, traversableRegionTypes);
			if (region == null)
			{
				return null;
			}
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(traverseParms, false);
			Region foundReg = null;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				if (r.IsDoorway)
				{
					return false;
				}
				IntVec3 intVec;
				if (!JobGiver_SeekSafeTemperature.TryGetAllowedCellInRegion(r, pawn, out intVec, -1f))
				{
					return false;
				}
				if (tempRange.Includes(r.Room.Temperature))
				{
					foundReg = r;
					return true;
				}
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, 9999, traversableRegionTypes);
			return foundReg;
		}

		// Token: 0x06007F4E RID: 32590 RVA: 0x002655B4 File Offset: 0x002637B4
		public static bool TryGetAllowedCellInRegion(Region region, Pawn pawn, out IntVec3 cell, float maxRadius = -1f)
		{
			cell = IntVec3.Invalid;
			for (int i = 0; i < 100; i++)
			{
				IntVec3 randomCell = region.RandomCell;
				if (randomCell.InAllowedArea(pawn) && (maxRadius < 0f || randomCell.InHorDistOf(pawn.Position, maxRadius)))
				{
					cell = randomCell;
					return true;
				}
			}
			foreach (IntVec3 intVec in region.Cells.InRandomOrder(JobGiver_SeekSafeTemperature.tmpWorkingCellList))
			{
				if (intVec.InAllowedArea(pawn) && (maxRadius < 0f || intVec.InHorDistOf(pawn.Position, maxRadius)))
				{
					cell = intVec;
					return true;
				}
			}
			return false;
		}

		// Token: 0x04005538 RID: 21816
		public float maxRadius = -1f;

		// Token: 0x04005539 RID: 21817
		public bool requiresInjury = true;

		// Token: 0x0400553A RID: 21818
		public bool waitInSafeTemp = true;

		// Token: 0x0400553B RID: 21819
		public static List<IntVec3> tmpWorkingCellList = new List<IntVec3>();
	}
}
