using System;

namespace Verse.AI.Group
{
	// Token: 0x020010A2 RID: 4258
	public class LordToilData_ExitMap : LordToilData
	{
		// Token: 0x0600689B RID: 26779 RVA: 0x00210F21 File Offset: 0x0020F121
		public override void ExposeData()
		{
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.None, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<bool>(ref this.interruptCurrentJob, "interruptCurrentJob", false, false);
		}

		// Token: 0x04004A34 RID: 18996
		public LocomotionUrgency locomotion;

		// Token: 0x04004A35 RID: 18997
		public bool canDig;

		// Token: 0x04004A36 RID: 18998
		public bool interruptCurrentJob;
	}
}
