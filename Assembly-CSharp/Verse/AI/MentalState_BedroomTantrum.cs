using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F93 RID: 3987
	public class MentalState_BedroomTantrum : MentalState_TantrumRandom
	{
		// Token: 0x06006254 RID: 25172 RVA: 0x001F0B24 File Offset: 0x001EED24
		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			outThings.Clear();
			Building_Bed ownedBed = this.pawn.ownership.OwnedBed;
			if (ownedBed == null)
			{
				return;
			}
			if (ownedBed.GetRoom(RegionType.Set_All) != null && !ownedBed.GetRoom(RegionType.Set_All).PsychologicallyOutdoors)
			{
				TantrumMentalStateUtility.GetSmashableThingsIn(ownedBed.GetRoom(RegionType.Set_All), this.pawn, outThings, this.GetCustomValidator(), 0);
				return;
			}
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, ownedBed.Position, outThings, this.GetCustomValidator(), 0, 8);
		}
	}
}
