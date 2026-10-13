using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015F9 RID: 5625
	public class WorkGiver_RemovePaintFloor : WorkGiver_Scanner
	{
		// Token: 0x1700145A RID: 5210
		// (get) Token: 0x060084C6 RID: 33990 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060084C7 RID: 33991 RVA: 0x0027C613 File Offset: 0x0027A813
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.RemovePaintFloor);
		}

		// Token: 0x060084C8 RID: 33992 RVA: 0x0027C62D File Offset: 0x0027A82D
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.RemovePaintFloor))
			{
				yield return designation.target.Cell;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060084C9 RID: 33993 RVA: 0x0027C640 File Offset: 0x0027A840
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			if (!pawn.Map.terrainGrid.TerrainAt(c).isPaintable)
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationAt(c, DesignationDefOf.RemovePaintFloor) == null)
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationAt(c, DesignationDefOf.PaintFloor) != null)
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationAt(c, DesignationDefOf.RemoveFloor) != null)
			{
				return false;
			}
			Building firstBuilding = c.GetFirstBuilding(pawn.Map);
			TerrainDef terrainDef = pawn.Map.terrainGrid.UnderTerrainAt(c);
			return (firstBuilding == null || firstBuilding.def.terrainAffordanceNeeded == null || terrainDef == null || terrainDef.affordances.Contains(firstBuilding.def.terrainAffordanceNeeded)) && pawn.CanReserve(c, 1, -1, ReservationLayerDefOf.Floor, forced);
		}

		// Token: 0x060084CA RID: 33994 RVA: 0x0027C71C File Offset: 0x0027A91C
		public override Job JobOnCell(Pawn pawn, IntVec3 cell, bool forced = false)
		{
			Job job = JobMaker.MakeJob(JobDefOf.RemovePaintFloor);
			job.AddQueuedTarget(TargetIndex.A, cell);
			for (int i = 0; i < 100; i++)
			{
				IntVec3 c = cell + GenRadial.RadialPattern[i];
				if (c.InBounds(pawn.Map) && !c.Fogged(pawn.Map) && pawn.CanReach(c, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && !job.targetQueueA.Contains(c))
				{
					job.AddQueuedTarget(TargetIndex.A, c);
					if (job.GetTargetQueue(TargetIndex.A).Count >= 10)
					{
						break;
					}
				}
			}
			if (job.targetQueueA != null && job.targetQueueA.Count >= 5)
			{
				job.targetQueueA.SortBy((LocalTargetInfo targ) => targ.Cell.DistanceToSquared(pawn.Position));
			}
			return job;
		}
	}
}
