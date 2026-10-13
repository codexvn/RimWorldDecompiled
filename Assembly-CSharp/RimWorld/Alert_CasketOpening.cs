using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FE1 RID: 12257
	public class Alert_CasketOpening : Alert_ActionDelay
	{
		// Token: 0x06011448 RID: 70728 RVA: 0x004F284D File Offset: 0x004F0A4D
		public Alert_CasketOpening()
		{
		}

		// Token: 0x06011449 RID: 70729 RVA: 0x004F2860 File Offset: 0x004F0A60
		public Alert_CasketOpening(SignalAction_OpenCasket openCasketAction)
		{
			this.openCasketAction = openCasketAction;
			this.culprints.AddRange(openCasketAction.caskets);
		}

		// Token: 0x0601144A RID: 70730 RVA: 0x004F288B File Offset: 0x004F0A8B
		public override AlertReport GetReport()
		{
			if (this.openCasketAction == null)
			{
				return AlertReport.Inactive;
			}
			return AlertReport.CulpritsAre(this.culprints);
		}

		// Token: 0x0601144B RID: 70731 RVA: 0x004F28A6 File Offset: 0x004F0AA6
		public override string GetLabel()
		{
			return "AlertCasketOpening".Translate(this.openCasketAction.delayTicks.ToStringTicksToPeriodVerbose(true, true));
		}

		// Token: 0x0601144C RID: 70732 RVA: 0x004F28CE File Offset: 0x004F0ACE
		public override TaggedString GetExplanation()
		{
			return "AlertCasketOpeningDesc".Translate();
		}

		// Token: 0x0400B319 RID: 45849
		private SignalAction_OpenCasket openCasketAction;

		// Token: 0x0400B31A RID: 45850
		private List<Thing> culprints = new List<Thing>();
	}
}
