using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016F2 RID: 5874
	public class LordToil_DevourerAssault : LordToil
	{
		// Token: 0x06008A8C RID: 35468 RVA: 0x002951B4 File Offset: 0x002933B4
		public override void UpdateAllDuties()
		{
			foreach (Pawn pawn in this.lord.ownedPawns)
			{
				pawn.mindState.duty = new PawnDuty(DutyDefOf.DevourerAssault);
			}
		}
	}
}
