using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200155D RID: 5469
	public class WorkGiver_ConstructRemoveFoundation : WorkGiver_ConstructAffectFloor
	{
		// Token: 0x170013D7 RID: 5079
		// (get) Token: 0x060081CC RID: 33228 RVA: 0x00222DF2 File Offset: 0x00220FF2
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.RemoveFoundation;
			}
		}

		// Token: 0x060081CD RID: 33229 RVA: 0x00270D94 File Offset: 0x0026EF94
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			Job job = this.RemoveFloorJob(pawn, c);
			if (job != null)
			{
				return job;
			}
			Job job2 = this.RemoveFoundationJob(pawn, c);
			if (job2 != null)
			{
				return job2;
			}
			return JobMaker.MakeJob(JobDefOf.RemoveFoundation, c);
		}

		// Token: 0x060081CE RID: 33230 RVA: 0x00270DCD File Offset: 0x0026EFCD
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return base.HasJobOnCell(pawn, c, forced) && pawn.Map.terrainGrid.CanRemoveFoundationAt(c) && !WorkGiver_ConstructRemoveFoundation.AnyBuildingBlockingFoundationRemoval(c, pawn.Map);
		}

		// Token: 0x060081CF RID: 33231 RVA: 0x00270E04 File Offset: 0x0026F004
		public static bool AnyBuildingBlockingFoundationRemoval(IntVec3 c, Map map)
		{
			if (!map.terrainGrid.CanRemoveFoundationAt(c))
			{
				return false;
			}
			foreach (Thing thing in c.GetThingList(map))
			{
				ThingDef thingDef = thing.def;
				Blueprint_Build blueprint_Build = thing as Blueprint_Build;
				if (blueprint_Build != null)
				{
					thingDef = blueprint_Build.BuildDef;
				}
				else
				{
					Blueprint_Install blueprint_Install = thing as Blueprint_Install;
					if (blueprint_Install != null)
					{
						thingDef = blueprint_Install.MiniToInstallOrBuildingToReinstall.def;
					}
					else
					{
						Frame frame = thing as Frame;
						if (frame != null)
						{
							thingDef = frame.BuildDef;
						}
					}
				}
				if (thingDef.terrainAffordanceNeeded != null && ((map.terrainGrid.UnderTerrainAt(c) != null) ? (!map.terrainGrid.UnderTerrainAt(c).affordances.Contains(thingDef.terrainAffordanceNeeded)) : (!map.terrainGrid.TopTerrainAt(c).affordances.Contains(thingDef.terrainAffordanceNeeded))))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060081D0 RID: 33232 RVA: 0x00270F14 File Offset: 0x0026F114
		private Job RemoveFloorJob(Pawn pawn, IntVec3 c)
		{
			if (pawn.Map.terrainGrid.UnderTerrainAt(c) == null || !pawn.Map.terrainGrid.CanRemoveTopLayerAt(c))
			{
				return null;
			}
			if (!pawn.CanReserve(c, 1, -1, ReservationLayerDefOf.Floor, false))
			{
				return null;
			}
			if (pawn.WorkTypeIsDisabled(WorkGiverDefOf.ConstructRemoveFloors.workType))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.RemoveFloor, c);
			job.ignoreDesignations = true;
			return job;
		}

		// Token: 0x060081D1 RID: 33233 RVA: 0x00270F8C File Offset: 0x0026F18C
		private Job RemoveFoundationJob(Pawn pawn, IntVec3 c)
		{
			if (!pawn.Map.terrainGrid.CanRemoveFoundationAt(c))
			{
				return null;
			}
			if (!pawn.CanReserve(c, 1, -1, ReservationLayerDefOf.Floor, false))
			{
				return null;
			}
			if (pawn.WorkTypeIsDisabled(WorkGiverDefOf.ConstructRemoveFloors.workType))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.RemoveFoundation, c);
			job.ignoreDesignations = true;
			return job;
		}
	}
}
