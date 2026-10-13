using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000FA6 RID: 4006
	public class MentalStateWorker_Jailbreaker : MentalStateWorker
	{
		// Token: 0x06006296 RID: 25238 RVA: 0x001F156A File Offset: 0x001EF76A
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && JailbreakerMentalStateUtility.FindPrisoner(pawn) != null;
		}
	}
}
