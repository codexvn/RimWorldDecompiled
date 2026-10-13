using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016EA RID: 5866
	public class LordToil_ChimeraAttack : LordToil
	{
		// Token: 0x06008A72 RID: 35442 RVA: 0x00294A40 File Offset: 0x00292C40
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.ChimeraAttack);
			}
		}
	}
}
