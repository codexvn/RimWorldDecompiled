using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200155C RID: 5468
	public class WorkGiver_ConstructRemoveFloor : WorkGiver_ConstructAffectFloor
	{
		// Token: 0x170013D6 RID: 5078
		// (get) Token: 0x060081C7 RID: 33223 RVA: 0x00222D56 File Offset: 0x00220F56
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.RemoveFloor;
			}
		}

		// Token: 0x060081C8 RID: 33224 RVA: 0x00270CBD File Offset: 0x0026EEBD
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.RemoveFloor, c);
		}

		// Token: 0x060081C9 RID: 33225 RVA: 0x00270CCF File Offset: 0x0026EECF
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return base.HasJobOnCell(pawn, c, false) && pawn.Map.terrainGrid.CanRemoveTopLayerAt(c) && !WorkGiver_ConstructRemoveFloor.AnyBuildingBlockingFloorRemoval(c, pawn.Map);
		}

		// Token: 0x060081CA RID: 33226 RVA: 0x00270D04 File Offset: 0x0026EF04
		public static bool AnyBuildingBlockingFloorRemoval(IntVec3 c, Map map)
		{
			if (!map.terrainGrid.CanRemoveTopLayerAt(c))
			{
				return false;
			}
			Building firstBuilding = c.GetFirstBuilding(map);
			if (firstBuilding == null || firstBuilding.def.terrainAffordanceNeeded == null)
			{
				return false;
			}
			if (map.terrainGrid.FoundationAt(c) == null)
			{
				return !map.terrainGrid.UnderTerrainAt(c).affordances.Contains(firstBuilding.def.terrainAffordanceNeeded);
			}
			return !map.terrainGrid.FoundationAt(c).affordances.Contains(firstBuilding.def.terrainAffordanceNeeded);
		}
	}
}
