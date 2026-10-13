using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016D9 RID: 5849
	public class LordToil_PrepareCaravan_Pause : LordToil
	{
		// Token: 0x06008A20 RID: 35360 RVA: 0x0029357C File Offset: 0x0029177C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Pause);
			}
		}
	}
}
