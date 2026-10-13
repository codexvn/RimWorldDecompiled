using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FDF RID: 12255
	public class Alert_DormanyWakeUpDelay : Alert_ActionDelay
	{
		// Token: 0x0601143E RID: 70718 RVA: 0x004F26AF File Offset: 0x004F08AF
		public Alert_DormanyWakeUpDelay()
		{
		}

		// Token: 0x0601143F RID: 70719 RVA: 0x004F26B7 File Offset: 0x004F08B7
		public Alert_DormanyWakeUpDelay(SignalAction_DormancyWakeUp wakeUpDelay)
		{
			this.wakeUpDelay = wakeUpDelay;
		}

		// Token: 0x06011440 RID: 70720 RVA: 0x004F26C8 File Offset: 0x004F08C8
		public override AlertReport GetReport()
		{
			if (this.wakeUpDelay == null)
			{
				return AlertReport.Inactive;
			}
			SignalAction_DormancyWakeUp signalAction_DormancyWakeUp = this.wakeUpDelay;
			if (((signalAction_DormancyWakeUp != null) ? signalAction_DormancyWakeUp.lord : null) != null && !this.wakeUpDelay.lord.ownedPawns.NullOrEmpty<Pawn>())
			{
				return AlertReport.CulpritsAre(this.wakeUpDelay.lord.ownedPawns);
			}
			return AlertReport.Active;
		}

		// Token: 0x06011441 RID: 70721 RVA: 0x004F272C File Offset: 0x004F092C
		public override string GetLabel()
		{
			return "AlertHostilesWakingUp".Translate(this.wakeUpDelay.lord.faction, this.wakeUpDelay.delayTicks.ToStringTicksToPeriodVerbose(true, true)).CapitalizeFirst();
		}

		// Token: 0x06011442 RID: 70722 RVA: 0x004F277C File Offset: 0x004F097C
		public override TaggedString GetExplanation()
		{
			return "AlertHostilesWakingUpDesc".Translate(this.wakeUpDelay.lord.faction).CapitalizeFirst();
		}

		// Token: 0x0400B317 RID: 45847
		private SignalAction_DormancyWakeUp wakeUpDelay;
	}
}
