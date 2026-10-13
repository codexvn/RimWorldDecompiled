using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001739 RID: 5945
	public class LordToil_WanderClose : LordToil
	{
		// Token: 0x06008BD1 RID: 35793 RVA: 0x00299F97 File Offset: 0x00298197
		public LordToil_WanderClose(IntVec3 location)
		{
			this.location = location;
		}

		// Token: 0x06008BD2 RID: 35794 RVA: 0x00299FA8 File Offset: 0x002981A8
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty duty = new PawnDuty(DutyDefOf.WanderClose, this.location, -1f);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}

		// Token: 0x04005ADD RID: 23261
		private IntVec3 location;
	}
}
