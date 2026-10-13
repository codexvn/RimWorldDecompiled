using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013FF RID: 5119
	public class JobGiver_AIDefendEscortee : JobGiver_AIDefendPawn
	{
		// Token: 0x06007CC9 RID: 31945 RVA: 0x0025960F File Offset: 0x0025780F
		protected override Pawn GetDefendee(Pawn pawn)
		{
			return ((Thing)pawn.mindState.duty.focus) as Pawn;
		}

		// Token: 0x06007CCA RID: 31946 RVA: 0x00255E10 File Offset: 0x00254010
		protected override float GetFlagRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}
	}
}
