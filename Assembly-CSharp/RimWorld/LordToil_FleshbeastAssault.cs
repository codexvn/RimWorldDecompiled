using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016FB RID: 5883
	public class LordToil_FleshbeastAssault : LordToil
	{
		// Token: 0x06008AB7 RID: 35511 RVA: 0x00295E98 File Offset: 0x00294098
		public override void UpdateAllDuties()
		{
			foreach (Pawn pawn in this.lord.ownedPawns)
			{
				pawn.mindState.duty = new PawnDuty(DutyDefOf.FleshbeastAssault);
			}
		}
	}
}
