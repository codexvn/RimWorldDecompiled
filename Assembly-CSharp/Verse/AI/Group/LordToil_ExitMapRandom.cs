using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010A6 RID: 4262
	public class LordToil_ExitMapRandom : LordToil
	{
		// Token: 0x060068A7 RID: 26791 RVA: 0x00211090 File Offset: 0x0020F290
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty duty = new PawnDuty(DutyDefOf.ExitMapRandom);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}
	}
}
