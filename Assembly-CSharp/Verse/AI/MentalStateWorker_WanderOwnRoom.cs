using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F9D RID: 3997
	public class MentalStateWorker_WanderOwnRoom : MentalStateWorker
	{
		// Token: 0x0600627E RID: 25214 RVA: 0x001F12CC File Offset: 0x001EF4CC
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			Building_Bed ownedBed = pawn.ownership.OwnedBed;
			return ownedBed != null && ownedBed.GetRoom(RegionType.Set_All) != null && !ownedBed.GetRoom(RegionType.Set_All).PsychologicallyOutdoors;
		}
	}
}
