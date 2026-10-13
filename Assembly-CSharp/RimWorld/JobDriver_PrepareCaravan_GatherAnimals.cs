using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001131 RID: 4401
	public class JobDriver_PrepareCaravan_GatherAnimals : JobDriver_RopeToDestination
	{
		// Token: 0x06006B66 RID: 27494 RVA: 0x0021CFD0 File Offset: 0x0021B1D0
		protected override bool HasRopeeArrived(Pawn ropee, bool roperWaitingAtDest)
		{
			PawnDuty duty = this.pawn.mindState.duty;
			IntVec3 intVec = (duty != null) ? duty.focus.Cell : IntVec3.Invalid;
			if (!intVec.IsValid)
			{
				return false;
			}
			if (!this.pawn.Position.InHorDistOf(intVec, 2f))
			{
				return false;
			}
			District district = intVec.GetDistrict(this.pawn.Map, RegionType.Set_Passable);
			return district == this.pawn.GetDistrict(RegionType.Set_Passable) && district == ropee.GetDistrict(RegionType.Set_Passable);
		}

		// Token: 0x06006B67 RID: 27495 RVA: 0x0021D060 File Offset: 0x0021B260
		protected override void ProcessArrivedRopee(Pawn ropee)
		{
			PawnDuty duty = ropee.mindState.duty;
			IntVec3 spot = (duty != null) ? duty.focus.Cell : IntVec3.Invalid;
			if (spot.IsValid)
			{
				ropee.roping.RopeToSpot(spot);
			}
		}

		// Token: 0x06006B68 RID: 27496 RVA: 0x0021D0A3 File Offset: 0x0021B2A3
		protected override bool ShouldOpportunisticallyRopeAnimal(Pawn animal)
		{
			return JobGiver_PrepareCaravan_GatherPawns.DoesAnimalNeedGathering(this.pawn, animal);
		}
	}
}
