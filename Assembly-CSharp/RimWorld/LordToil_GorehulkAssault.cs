using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016FC RID: 5884
	public class LordToil_GorehulkAssault : LordToil
	{
		// Token: 0x06008AB9 RID: 35513 RVA: 0x00295EFC File Offset: 0x002940FC
		public override void UpdateAllDuties()
		{
			foreach (Pawn pawn in this.lord.ownedPawns)
			{
				pawn.mindState.duty = new PawnDuty(DutyDefOf.GorehulkAssault);
			}
		}
	}
}
