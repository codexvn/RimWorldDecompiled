using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200305B RID: 12379
	public class Alert_MonolithAutoActivating : Alert
	{
		// Token: 0x06011629 RID: 71209 RVA: 0x004FCCEC File Offset: 0x004FAEEC
		public Alert_MonolithAutoActivating()
		{
			this.defaultExplanation = "MonolithTwistingExplanation".Translate();
			this.requireAnomaly = true;
		}

		// Token: 0x0601162A RID: 71210 RVA: 0x004FCD1C File Offset: 0x004FAF1C
		public override string GetLabel()
		{
			return "MonolithTwistingAlert".Translate() + ": " + Find.Anomaly.monolith.TicksUntilAutoActivate.ToStringTicksToPeriodVerbose(true, true);
		}

		// Token: 0x0601162B RID: 71211 RVA: 0x004FCD54 File Offset: 0x004FAF54
		public override AlertReport GetReport()
		{
			if (Find.Anomaly.monolith == null || !Find.Anomaly.monolith.IsAutoActivating)
			{
				return false;
			}
			this.targets.Clear();
			this.targets.Add(Find.Anomaly.monolith);
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0400B3F4 RID: 46068
		private List<Thing> targets = new List<Thing>(1);
	}
}
