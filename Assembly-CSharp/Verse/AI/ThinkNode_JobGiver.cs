using System;

namespace Verse.AI
{
	// Token: 0x02001014 RID: 4116
	public abstract class ThinkNode_JobGiver : ThinkNode
	{
		// Token: 0x17000FCD RID: 4045
		// (get) Token: 0x060064E5 RID: 25829 RVA: 0x001FEA07 File Offset: 0x001FCC07
		public virtual string CrawlingReportStringOverride
		{
			get
			{
				return this.crawlingReportStringOverride;
			}
		}

		// Token: 0x060064E6 RID: 25830 RVA: 0x001FEA0F File Offset: 0x001FCC0F
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_JobGiver thinkNode_JobGiver = (ThinkNode_JobGiver)base.DeepCopy(resolve);
			thinkNode_JobGiver.reportStringOverride = this.reportStringOverride;
			thinkNode_JobGiver.crawlingReportStringOverride = this.crawlingReportStringOverride;
			return thinkNode_JobGiver;
		}

		// Token: 0x060064E7 RID: 25831
		protected abstract Job TryGiveJob(Pawn pawn);

		// Token: 0x060064E8 RID: 25832 RVA: 0x001FEA38 File Offset: 0x001FCC38
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			Job job = this.TryGiveJob(pawn);
			if (job == null)
			{
				return ThinkResult.NoJob;
			}
			if (this.reportStringOverride != null)
			{
				job.reportStringOverride = this.reportStringOverride;
			}
			if (this.CrawlingReportStringOverride != null)
			{
				job.crawlingReportStringOverride = this.CrawlingReportStringOverride;
			}
			return new ThinkResult(job, this, null, false);
		}

		// Token: 0x04004817 RID: 18455
		[MustTranslate]
		protected string reportStringOverride;

		// Token: 0x04004818 RID: 18456
		[MustTranslate]
		protected string crawlingReportStringOverride;
	}
}
