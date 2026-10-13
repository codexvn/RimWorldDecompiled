using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015D3 RID: 5587
	public class WorkGiver_HaulGeneral : WorkGiver_Haul
	{
		// Token: 0x06008400 RID: 33792 RVA: 0x002798F2 File Offset: 0x00277AF2
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t is Corpse)
			{
				return null;
			}
			return base.JobOnThing(pawn, t, forced);
		}
	}
}
