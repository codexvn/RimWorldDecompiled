using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001638 RID: 5688
	public class LordToil_BestowingCeremony_MoveInPlace : LordToil
	{
		// Token: 0x0600860B RID: 34315 RVA: 0x002807FF File Offset: 0x0027E9FF
		public LordToil_BestowingCeremony_MoveInPlace(IntVec3 spot, Pawn target)
		{
			this.spot = spot;
			this.target = target;
		}

		// Token: 0x0600860C RID: 34316 RVA: 0x00280818 File Offset: 0x0027EA18
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.BestowingCeremony_MoveInPlace, this.spot, -1f);
				pawnDuty.locomotion = LocomotionUrgency.Walk;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x04005798 RID: 22424
		public bool? pass;

		// Token: 0x04005799 RID: 22425
		public IntVec3 spot;

		// Token: 0x0400579A RID: 22426
		public Pawn target;
	}
}
