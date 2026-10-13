using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000E8F RID: 3727
	public class JobDriver_WaitDowned : JobDriver_Wait
	{
		// Token: 0x06005DF3 RID: 24051 RVA: 0x001DEF88 File Offset: 0x001DD188
		public override string GetReport()
		{
			if (this.pawn.Deathresting)
			{
				return this.ReportStringProcessed(SanguophageUtility.DeathrestJobReport(this.pawn));
			}
			if (!this.pawn.Awake())
			{
				return "DownedUnconscious".Translate();
			}
			if (!this.pawn.health.CanCrawl)
			{
				return "DownedCannotCrawl".Translate();
			}
			return base.GetReport();
		}

		// Token: 0x06005DF4 RID: 24052 RVA: 0x001DEFF9 File Offset: 0x001DD1F9
		public override void DecorateWaitToil(Toil wait)
		{
			base.DecorateWaitToil(wait);
			wait.AddFailCondition(() => !this.pawn.Downed);
		}
	}
}
