using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200170C RID: 5900
	public class LordToil_ManClosestTurrets : LordToil
	{
		// Token: 0x06008AF5 RID: 35573 RVA: 0x00296D14 File Offset: 0x00294F14
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.ManClosestTurret, this.lord.ownedPawns[i].Position, -1f);
			}
		}
	}
}
