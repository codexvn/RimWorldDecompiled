using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200173A RID: 5946
	public class LordToil_WanderNearby : LordToil
	{
		// Token: 0x06008BD4 RID: 35796 RVA: 0x0029A008 File Offset: 0x00298208
		public override void UpdateAllDuties()
		{
			if (this.cell == null && this.lord.ownedPawns.Any<Pawn>())
			{
				this.cell = new IntVec3?(this.lord.ownedPawns[0].Position);
			}
			if (this.cell == null)
			{
				return;
			}
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty duty = new PawnDuty(DutyDefOf.WanderClose, this.cell.Value, -1f);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}

		// Token: 0x04005ADE RID: 23262
		private IntVec3? cell;
	}
}
