using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200172B RID: 5931
	public class LordToil_TakeWoundedGuest : LordToil
	{
		// Token: 0x17001578 RID: 5496
		// (get) Token: 0x06008B8B RID: 35723 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001579 RID: 5497
		// (get) Token: 0x06008B8C RID: 35724 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008B8D RID: 35725 RVA: 0x00299134 File Offset: 0x00297334
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.TakeWoundedGuest);
			}
		}
	}
}
