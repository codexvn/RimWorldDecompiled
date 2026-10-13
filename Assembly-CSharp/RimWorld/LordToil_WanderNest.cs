using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200173B RID: 5947
	public class LordToil_WanderNest : LordToil
	{
		// Token: 0x06008BD5 RID: 35797 RVA: 0x0029A0BC File Offset: 0x002982BC
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty duty = new PawnDuty(DutyDefOf.WanderNest);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}
	}
}
