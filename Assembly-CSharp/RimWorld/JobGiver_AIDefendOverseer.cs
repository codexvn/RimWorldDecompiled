using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001402 RID: 5122
	public class JobGiver_AIDefendOverseer : JobGiver_AIDefendPawn
	{
		// Token: 0x06007CD2 RID: 31954 RVA: 0x00259676 File Offset: 0x00257876
		protected override Pawn GetDefendee(Pawn pawn)
		{
			return pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Overseer, null);
		}

		// Token: 0x06007CD3 RID: 31955 RVA: 0x00259689 File Offset: 0x00257889
		protected override float GetFlagRadius(Pawn pawn)
		{
			return 5f;
		}
	}
}
