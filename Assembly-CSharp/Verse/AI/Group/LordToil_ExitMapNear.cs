using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010A5 RID: 4261
	public class LordToil_ExitMapNear : LordToil
	{
		// Token: 0x17001032 RID: 4146
		// (get) Token: 0x060068A3 RID: 26787 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001033 RID: 4147
		// (get) Token: 0x060068A4 RID: 26788 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060068A5 RID: 26789 RVA: 0x00210FF2 File Offset: 0x0020F1F2
		public LordToil_ExitMapNear(IntVec3 near, float radius, LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false)
		{
			this.near = near;
			this.radius = radius;
			this.locomotion = locomotion;
			this.canDig = canDig;
		}

		// Token: 0x060068A6 RID: 26790 RVA: 0x00211018 File Offset: 0x0020F218
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.ExitMapNearDutyTarget, this.near, this.radius);
				pawnDuty.locomotion = this.locomotion;
				pawnDuty.canDig = this.canDig;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x04004A37 RID: 18999
		private IntVec3 near;

		// Token: 0x04004A38 RID: 19000
		private float radius;

		// Token: 0x04004A39 RID: 19001
		private LocomotionUrgency locomotion;

		// Token: 0x04004A3A RID: 19002
		private bool canDig;
	}
}
