using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001400 RID: 5120
	public class JobGiver_AIDefendSelf : JobGiver_AIDefendPawn
	{
		// Token: 0x06007CCC RID: 31948 RVA: 0x00009C2B File Offset: 0x00007E2B
		protected override Pawn GetDefendee(Pawn pawn)
		{
			return pawn;
		}

		// Token: 0x06007CCD RID: 31949 RVA: 0x00255E10 File Offset: 0x00254010
		protected override float GetFlagRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}
	}
}
