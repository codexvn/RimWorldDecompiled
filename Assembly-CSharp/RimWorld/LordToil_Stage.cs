using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001728 RID: 5928
	public class LordToil_Stage : LordToil
	{
		// Token: 0x17001572 RID: 5490
		// (get) Token: 0x06008B7F RID: 35711 RVA: 0x00298FD4 File Offset: 0x002971D4
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.stagingPoint;
			}
		}

		// Token: 0x17001573 RID: 5491
		// (get) Token: 0x06008B80 RID: 35712 RVA: 0x00298FE1 File Offset: 0x002971E1
		private LordToilData_Stage Data
		{
			get
			{
				return (LordToilData_Stage)this.data;
			}
		}

		// Token: 0x17001574 RID: 5492
		// (get) Token: 0x06008B81 RID: 35713 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008B82 RID: 35714 RVA: 0x00298FEE File Offset: 0x002971EE
		public LordToil_Stage(IntVec3 stagingLoc)
		{
			this.data = new LordToilData_Stage();
			this.Data.stagingPoint = stagingLoc;
		}

		// Token: 0x06008B83 RID: 35715 RVA: 0x00299010 File Offset: 0x00297210
		public override void UpdateAllDuties()
		{
			LordToilData_Stage data = this.Data;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.Defend, data.stagingPoint, -1f);
				this.lord.ownedPawns[i].mindState.duty.radius = 28f;
			}
		}
	}
}
