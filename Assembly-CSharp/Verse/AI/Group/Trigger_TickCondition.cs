using System;

namespace Verse.AI.Group
{
	// Token: 0x020010C5 RID: 4293
	public class Trigger_TickCondition : Trigger
	{
		// Token: 0x1700103A RID: 4154
		// (get) Token: 0x06006906 RID: 26886 RVA: 0x00212257 File Offset: 0x00210457
		private TriggerData_Reason Data
		{
			get
			{
				return (TriggerData_Reason)this.data;
			}
		}

		// Token: 0x06006907 RID: 26887 RVA: 0x00212264 File Offset: 0x00210464
		public Trigger_TickCondition(Func<bool> condition, int checkEveryTicks = 1)
		{
			this.condition = condition;
			this.checkEveryTicks = checkEveryTicks;
		}

		// Token: 0x06006908 RID: 26888 RVA: 0x00212281 File Offset: 0x00210481
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.checkEveryTicks == 0 && this.condition();
		}

		// Token: 0x04004A5E RID: 19038
		private Func<bool> condition;

		// Token: 0x04004A5F RID: 19039
		private int checkEveryTicks = 1;
	}
}
