using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016E1 RID: 5857
	public class LordToil_AssaultColonyBossgroup : LordToil
	{
		// Token: 0x17001534 RID: 5428
		// (get) Token: 0x06008A3D RID: 35389 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001535 RID: 5429
		// (get) Token: 0x06008A3E RID: 35390 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008A40 RID: 35392 RVA: 0x00293BA8 File Offset: 0x00291DA8
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				if (this.lord.ownedPawns[i].RaceProps.dutyBoss != null)
				{
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(this.lord.ownedPawns[i].RaceProps.dutyBoss);
				}
				else
				{
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
				}
			}
		}
	}
}
