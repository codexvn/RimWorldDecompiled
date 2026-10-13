using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010A1 RID: 4257
	public class LordToil_ExitMap : LordToil
	{
		// Token: 0x1700102B RID: 4139
		// (get) Token: 0x06006895 RID: 26773 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700102C RID: 4140
		// (get) Token: 0x06006896 RID: 26774 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700102D RID: 4141
		// (get) Token: 0x06006897 RID: 26775 RVA: 0x00210E39 File Offset: 0x0020F039
		public virtual DutyDef ExitDuty
		{
			get
			{
				return DutyDefOf.ExitMapBest;
			}
		}

		// Token: 0x1700102E RID: 4142
		// (get) Token: 0x06006898 RID: 26776 RVA: 0x00210E40 File Offset: 0x0020F040
		protected LordToilData_ExitMap Data
		{
			get
			{
				return (LordToilData_ExitMap)this.data;
			}
		}

		// Token: 0x06006899 RID: 26777 RVA: 0x00210E4D File Offset: 0x0020F04D
		public LordToil_ExitMap(LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false, bool interruptCurrentJob = false)
		{
			this.data = new LordToilData_ExitMap();
			this.Data.locomotion = locomotion;
			this.Data.canDig = canDig;
			this.Data.interruptCurrentJob = interruptCurrentJob;
		}

		// Token: 0x0600689A RID: 26778 RVA: 0x00210E84 File Offset: 0x0020F084
		public override void UpdateAllDuties()
		{
			LordToilData_ExitMap data = this.Data;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(this.ExitDuty);
				pawnDuty.locomotion = data.locomotion;
				pawnDuty.canDig = data.canDig;
				Pawn pawn = this.lord.ownedPawns[i];
				pawn.mindState.duty = pawnDuty;
				if (this.Data.interruptCurrentJob && pawn.jobs.curJob != null)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}
	}
}
