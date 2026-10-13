using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016ED RID: 5869
	public class LordToil_DefendBase : LordToil
	{
		// Token: 0x17001540 RID: 5440
		// (get) Token: 0x06008A79 RID: 35449 RVA: 0x00294C3C File Offset: 0x00292E3C
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.baseCenter;
			}
		}

		// Token: 0x06008A7A RID: 35450 RVA: 0x00294C44 File Offset: 0x00292E44
		public LordToil_DefendBase(IntVec3 baseCenter)
		{
			this.baseCenter = baseCenter;
		}

		// Token: 0x06008A7B RID: 35451 RVA: 0x00294C54 File Offset: 0x00292E54
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.DefendBase, this.baseCenter, -1f);
			}
		}

		// Token: 0x04005A36 RID: 23094
		public IntVec3 baseCenter;
	}
}
