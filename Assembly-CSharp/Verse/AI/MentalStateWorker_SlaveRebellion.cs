using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000FAC RID: 4012
	public class MentalStateWorker_SlaveRebellion : MentalStateWorker
	{
		// Token: 0x060062A2 RID: 25250 RVA: 0x001F1613 File Offset: 0x001EF813
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && SlaveRebellionUtility.FindSlaveForRebellion(pawn) != null;
		}
	}
}
