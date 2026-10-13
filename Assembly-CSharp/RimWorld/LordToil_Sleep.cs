using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001726 RID: 5926
	public class LordToil_Sleep : LordToil
	{
		// Token: 0x06008B79 RID: 35705 RVA: 0x00298D08 File Offset: 0x00296F08
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.SleepForever);
			}
		}
	}
}
