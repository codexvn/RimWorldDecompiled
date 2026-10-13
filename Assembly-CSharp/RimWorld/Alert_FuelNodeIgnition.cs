using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FE2 RID: 12258
	public class Alert_FuelNodeIgnition : Alert_ActionDelay
	{
		// Token: 0x0601144D RID: 70733 RVA: 0x004F26AF File Offset: 0x004F08AF
		public Alert_FuelNodeIgnition()
		{
		}

		// Token: 0x0601144E RID: 70734 RVA: 0x004F28DA File Offset: 0x004F0ADA
		public Alert_FuelNodeIgnition(SignalAction_StartWick startWickAction)
		{
			this.startWickAction = startWickAction;
		}

		// Token: 0x0601144F RID: 70735 RVA: 0x004F28E9 File Offset: 0x004F0AE9
		public override AlertReport GetReport()
		{
			if (this.startWickAction == null)
			{
				return AlertReport.Inactive;
			}
			return AlertReport.CulpritIs(this.startWickAction.thingWithWick);
		}

		// Token: 0x06011450 RID: 70736 RVA: 0x004F290E File Offset: 0x004F0B0E
		public override string GetLabel()
		{
			return "AlertFuelNodeIgniting".Translate(this.startWickAction.delayTicks.ToStringTicksToPeriodVerbose(true, true));
		}

		// Token: 0x06011451 RID: 70737 RVA: 0x004F2936 File Offset: 0x004F0B36
		public override TaggedString GetExplanation()
		{
			return "AlertFuelNodeIgnitingDesc".Translate();
		}

		// Token: 0x0400B31B RID: 45851
		private SignalAction_StartWick startWickAction;
	}
}
