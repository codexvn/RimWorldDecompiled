using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016F7 RID: 5879
	public class LordToil_EscortPawn : LordToil
	{
		// Token: 0x06008AA5 RID: 35493 RVA: 0x00295867 File Offset: 0x00293A67
		public LordToil_EscortPawn(Pawn escortee, float followRadius = 7f)
		{
			this.escortee = escortee;
			this.followRadius = followRadius;
		}

		// Token: 0x06008AA6 RID: 35494 RVA: 0x00295888 File Offset: 0x00293A88
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty duty = new PawnDuty(DutyDefOf.Escort, this.escortee, this.followRadius);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}

		// Token: 0x04005A47 RID: 23111
		public Pawn escortee;

		// Token: 0x04005A48 RID: 23112
		public float followRadius = 7f;
	}
}
