using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200170B RID: 5899
	public class LordToil_LoadAndEnterTransporters : LordToil
	{
		// Token: 0x17001554 RID: 5460
		// (get) Token: 0x06008AF2 RID: 35570 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008AF3 RID: 35571 RVA: 0x00296CA0 File Offset: 0x00294EA0
		public LordToil_LoadAndEnterTransporters(int transportersGroup)
		{
			this.transportersGroup = transportersGroup;
		}

		// Token: 0x06008AF4 RID: 35572 RVA: 0x00296CB8 File Offset: 0x00294EB8
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.LoadAndEnterTransporters);
				pawnDuty.transportersGroup = this.transportersGroup;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x04005A5E RID: 23134
		public int transportersGroup = -1;
	}
}
