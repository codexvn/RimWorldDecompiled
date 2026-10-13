using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D43 RID: 15683
	public static class CaravanCarryUtility
	{
		// Token: 0x060155DC RID: 87516 RVA: 0x00663840 File Offset: 0x00661A40
		public static bool CarriedByCaravan(this Pawn p)
		{
			Caravan caravan = p.GetCaravan();
			return caravan != null && caravan.carryTracker.IsCarried(p);
		}

		// Token: 0x060155DD RID: 87517 RVA: 0x00663865 File Offset: 0x00661A65
		public static bool WouldBenefitFromBeingCarried(Pawn p)
		{
			return CaravanBedUtility.WouldBenefitFromRestingInBed(p);
		}
	}
}
