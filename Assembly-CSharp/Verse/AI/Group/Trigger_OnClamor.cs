using System;

namespace Verse.AI.Group
{
	// Token: 0x020010CA RID: 4298
	public class Trigger_OnClamor : Trigger
	{
		// Token: 0x06006911 RID: 26897 RVA: 0x00212389 File Offset: 0x00210589
		public Trigger_OnClamor(ClamorDef clamorType)
		{
			this.clamorType = clamorType;
		}

		// Token: 0x06006912 RID: 26898 RVA: 0x00212398 File Offset: 0x00210598
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Clamor && signal.clamorType == this.clamorType;
		}

		// Token: 0x04004A65 RID: 19045
		private ClamorDef clamorType;
	}
}
