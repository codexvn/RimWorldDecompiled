using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200160C RID: 5644
	public class WorkGiver_Repair : WorkGiver_Scanner
	{
		// Token: 0x1700146B RID: 5227
		// (get) Token: 0x06008527 RID: 34087 RVA: 0x00278AC2 File Offset: 0x00276CC2
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial);
			}
		}

		// Token: 0x1700146C RID: 5228
		// (get) Token: 0x06008528 RID: 34088 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06008529 RID: 34089 RVA: 0x00075162 File Offset: 0x00073362
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x0600852A RID: 34090 RVA: 0x0027D943 File Offset: 0x0027BB43
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerBuildingsRepairable.RepairableBuildings(pawn.Faction);
		}

		// Token: 0x0600852B RID: 34091 RVA: 0x0027D95B File Offset: 0x0027BB5B
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.listerBuildingsRepairable.RepairableBuildings(pawn.Faction).Count == 0;
		}

		// Token: 0x0600852C RID: 34092 RVA: 0x0027D97C File Offset: 0x0027BB7C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!RepairUtility.PawnCanRepairNow(pawn, t))
			{
				return false;
			}
			Building building = t as Building;
			if (pawn.Faction == Faction.OfPlayer && !pawn.Map.areaManager.Home[t.Position])
			{
				JobFailReason.Is(WorkGiver_FixBrokenDownBuilding.NotInHomeAreaTrans, null);
				return false;
			}
			return pawn.CanReserve(building, 1, -1, null, forced) && building.Map.designationManager.DesignationOn(building, DesignationDefOf.Deconstruct) == null && (!building.def.mineable || building.Map.designationManager.DesignationAt(building.Position, DesignationDefOf.Mine) == null) && (!building.def.mineable || building.Map.designationManager.DesignationAt(building.Position, DesignationDefOf.MineVein) == null) && !building.IsBurning();
		}

		// Token: 0x0600852D RID: 34093 RVA: 0x0027DA63 File Offset: 0x0027BC63
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Repair, t);
		}
	}
}
