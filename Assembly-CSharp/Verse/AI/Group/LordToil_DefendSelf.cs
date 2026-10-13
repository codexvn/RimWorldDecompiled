using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200109F RID: 4255
	public class LordToil_DefendSelf : LordToil
	{
		// Token: 0x06006890 RID: 26768 RVA: 0x00210D9C File Offset: 0x0020EF9C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.Defend, this.lord.ownedPawns[i].Position, -1f);
				this.lord.ownedPawns[i].mindState.duty.radius = 28f;
			}
		}
	}
}
