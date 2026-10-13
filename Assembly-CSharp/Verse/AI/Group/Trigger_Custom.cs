using System;

namespace Verse.AI.Group
{
	// Token: 0x020010C3 RID: 4291
	public class Trigger_Custom : Trigger
	{
		// Token: 0x06006902 RID: 26882 RVA: 0x0021221E File Offset: 0x0021041E
		public Trigger_Custom(Func<TriggerSignal, bool> condition)
		{
			this.condition = condition;
		}

		// Token: 0x06006903 RID: 26883 RVA: 0x0021222D File Offset: 0x0021042D
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return this.condition(signal);
		}

		// Token: 0x04004A5C RID: 19036
		private Func<TriggerSignal, bool> condition;
	}
}
