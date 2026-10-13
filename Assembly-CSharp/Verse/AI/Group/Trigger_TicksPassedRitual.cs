using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010EA RID: 4330
	public class Trigger_TicksPassedRitual : Trigger
	{
		// Token: 0x1700103D RID: 4157
		// (get) Token: 0x06006959 RID: 26969 RVA: 0x00213288 File Offset: 0x00211488
		protected TriggerData_TicksPassedRitual Data
		{
			get
			{
				return (TriggerData_TicksPassedRitual)this.data;
			}
		}

		// Token: 0x0600695A RID: 26970 RVA: 0x00213295 File Offset: 0x00211495
		public Trigger_TicksPassedRitual(int tickLimit, RitualStage stage)
		{
			this.data = new TriggerData_TicksPassedRitual();
			this.duration = tickLimit;
			this.stage = stage;
		}

		// Token: 0x0600695B RID: 26971 RVA: 0x002132C0 File Offset: 0x002114C0
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				LordJob_Ritual ritual = lord.LordJob as LordJob_Ritual;
				TriggerData_TicksPassedRitual data = this.Data;
				data.ticksPassed += this.stage.ProgressPerTick(ritual);
				return data.ticksPassed > (float)this.duration;
			}
			return false;
		}

		// Token: 0x0600695C RID: 26972 RVA: 0x00213311 File Offset: 0x00211511
		public override void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			if (!transition.sources.Contains(previousToil))
			{
				this.Data.ticksPassed = 0f;
			}
		}

		// Token: 0x04004A84 RID: 19076
		private int duration = 100;

		// Token: 0x04004A85 RID: 19077
		private RitualStage stage;
	}
}
