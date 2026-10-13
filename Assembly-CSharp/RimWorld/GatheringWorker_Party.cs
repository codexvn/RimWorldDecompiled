using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001A3E RID: 6718
	public class GatheringWorker_Party : GatheringWorker
	{
		// Token: 0x06009751 RID: 38737 RVA: 0x002C3344 File Offset: 0x002C1544
		protected override LordJob CreateLordJob(IntVec3 spot, Pawn organizer)
		{
			return new LordJob_Joinable_Party(spot, organizer, this.def);
		}

		// Token: 0x06009752 RID: 38738 RVA: 0x002C3353 File Offset: 0x002C1553
		protected override bool TryFindGatherSpot(Pawn organizer, out IntVec3 spot)
		{
			return RCellFinder.TryFindGatheringSpot(organizer, this.def, false, out spot);
		}
	}
}
