using System;

namespace Verse.AI.Group
{
	// Token: 0x020010EF RID: 4335
	public class Trigger_TicksPassedAfterConditionMet : Trigger_TicksPassed
	{
		// Token: 0x1700103E RID: 4158
		// (get) Token: 0x06006965 RID: 26981 RVA: 0x002133DD File Offset: 0x002115DD
		protected new TriggerData_TicksPassedAfterConditionMet Data
		{
			get
			{
				return (TriggerData_TicksPassedAfterConditionMet)this.data;
			}
		}

		// Token: 0x06006966 RID: 26982 RVA: 0x002133EA File Offset: 0x002115EA
		public Trigger_TicksPassedAfterConditionMet(int tickLimit, Func<bool> condition, int checkEveryTicks = 1) : base(tickLimit)
		{
			this.condition = condition;
			this.checkEveryTicks = checkEveryTicks;
			this.data = new TriggerData_TicksPassedAfterConditionMet();
		}

		// Token: 0x06006967 RID: 26983 RVA: 0x0021340C File Offset: 0x0021160C
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (!this.Data.conditionMet && signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.checkEveryTicks == 0)
			{
				this.Data.conditionMet = this.condition();
			}
			return this.Data.conditionMet && base.ActivateOn(lord, signal);
		}

		// Token: 0x04004A89 RID: 19081
		private Func<bool> condition;

		// Token: 0x04004A8A RID: 19082
		private int checkEveryTicks;
	}
}
