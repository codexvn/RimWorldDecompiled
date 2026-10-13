using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016F8 RID: 5880
	public class LordToil_ExitMapAndDefendSelf : LordToil
	{
		// Token: 0x06008AA8 RID: 35496 RVA: 0x002958E8 File Offset: 0x00293AE8
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.ExitMapBestAndDefendSelf);
			}
		}
	}
}
