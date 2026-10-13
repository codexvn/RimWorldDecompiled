using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016DF RID: 5855
	public class LordToil_PrepareCaravan_Wait : LordToil
	{
		// Token: 0x17001530 RID: 5424
		// (get) Token: 0x06008A34 RID: 35380 RVA: 0x00293245 File Offset: 0x00291445
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x17001531 RID: 5425
		// (get) Token: 0x06008A35 RID: 35381 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008A36 RID: 35382 RVA: 0x00293A34 File Offset: 0x00291C34
		public LordToil_PrepareCaravan_Wait(IntVec3 meetingPoint)
		{
			this.meetingPoint = meetingPoint;
		}

		// Token: 0x06008A37 RID: 35383 RVA: 0x00293A44 File Offset: 0x00291C44
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait, this.meetingPoint, -1f);
			}
		}

		// Token: 0x04005A18 RID: 23064
		private IntVec3 meetingPoint;
	}
}
