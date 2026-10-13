using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000FA1 RID: 4001
	public class MentalStateWorker_BedroomTantrum : MentalStateWorker
	{
		// Token: 0x06006289 RID: 25225 RVA: 0x001F13E8 File Offset: 0x001EF5E8
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			Building_Bed ownedBed = pawn.ownership.OwnedBed;
			if (ownedBed == null || ownedBed.GetRoom(RegionType.Set_All) == null || ownedBed.GetRoom(RegionType.Set_All).PsychologicallyOutdoors)
			{
				return false;
			}
			MentalStateWorker_BedroomTantrum.tmpThings.Clear();
			TantrumMentalStateUtility.GetSmashableThingsIn(ownedBed.GetRoom(RegionType.Set_All), pawn, MentalStateWorker_BedroomTantrum.tmpThings, null, 0);
			bool result = MentalStateWorker_BedroomTantrum.tmpThings.Any<Thing>();
			MentalStateWorker_BedroomTantrum.tmpThings.Clear();
			return result;
		}

		// Token: 0x04004681 RID: 18049
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
