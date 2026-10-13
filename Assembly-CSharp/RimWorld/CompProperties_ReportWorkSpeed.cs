using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DE4 RID: 11748
	public class CompProperties_ReportWorkSpeed : CompProperties
	{
		// Token: 0x0601088B RID: 67723 RVA: 0x004C45BA File Offset: 0x004C27BA
		public CompProperties_ReportWorkSpeed()
		{
			this.compClass = typeof(CompReportWorkSpeed);
		}

		// Token: 0x0400AC6D RID: 44141
		public StatDef workSpeedStat;
	}
}
