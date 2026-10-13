using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017E2 RID: 6114
	public class ThoughtWorker_Precept_SlabBed_Preferred : ThoughtWorker_Precept
	{
		// Token: 0x06008E35 RID: 36405 RVA: 0x002A2649 File Offset: 0x002A0849
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return p.mindState.lastBedDefSleptIn != null && p.mindState.lastBedDefSleptIn.building != null && p.mindState.lastBedDefSleptIn.building.bed_slabBed;
		}
	}
}
