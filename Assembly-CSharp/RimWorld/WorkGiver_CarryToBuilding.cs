using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200159E RID: 5534
	public abstract class WorkGiver_CarryToBuilding : WorkGiver_Scanner
	{
		// Token: 0x17001406 RID: 5126
		// (get) Token: 0x060082E7 RID: 33511 RVA: 0x000E9647 File Offset: 0x000E7847
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x060082E8 RID: 33512 RVA: 0x0027500C File Offset: 0x0027320C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!pawn.CanReserveAndReach(t, PathEndMode.InteractionCell, Danger.Deadly, 1, -1, null, forced))
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
			{
				return false;
			}
			Building_Enterable building_Enterable = t as Building_Enterable;
			if (building_Enterable == null)
			{
				return false;
			}
			Pawn selectedPawn = building_Enterable.SelectedPawn;
			return selectedPawn != null && selectedPawn.Map == pawn.Map && building_Enterable.CanAcceptPawn(selectedPawn) && (selectedPawn.IsPrisonerOfColony || selectedPawn.Downed || !selectedPawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving) || (this.def.workType != null && selectedPawn.WorkTypeIsDisabled(this.def.workType))) && pawn.CanReserveAndReach(selectedPawn, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, forced);
		}

		// Token: 0x060082E9 RID: 33513 RVA: 0x002750D8 File Offset: 0x002732D8
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_Enterable building_Enterable = t as Building_Enterable;
			if (building_Enterable == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.CarryToBuilding, building_Enterable, building_Enterable.SelectedPawn);
			job.count = 1;
			return job;
		}
	}
}
