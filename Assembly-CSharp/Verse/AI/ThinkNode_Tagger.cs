using System;

namespace Verse.AI
{
	// Token: 0x02000FFD RID: 4093
	public class ThinkNode_Tagger : ThinkNode_Priority
	{
		// Token: 0x0600648B RID: 25739 RVA: 0x001FDA45 File Offset: 0x001FBC45
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Tagger thinkNode_Tagger = (ThinkNode_Tagger)base.DeepCopy(resolve);
			thinkNode_Tagger.tagToGive = this.tagToGive;
			return thinkNode_Tagger;
		}

		// Token: 0x0600648C RID: 25740 RVA: 0x001FDA60 File Offset: 0x001FBC60
		public override float GetPriority(Pawn pawn)
		{
			if (this.priority >= 0f)
			{
				return this.priority;
			}
			if (this.subNodes.Any<ThinkNode>())
			{
				return this.subNodes[0].GetPriority(pawn);
			}
			Log.ErrorOnce("ThinkNode_PrioritySorter has child node which didn't give a priority: " + ((this != null) ? this.ToString() : null), this.GetHashCode());
			return 0f;
		}

		// Token: 0x0600648D RID: 25741 RVA: 0x001FDAC8 File Offset: 0x001FBCC8
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkResult result = base.TryIssueJobPackage(pawn, jobParams);
			if (result.IsValid && result.Tag == null)
			{
				result = new ThinkResult(result.Job, result.SourceNode, new JobTag?(this.tagToGive), false);
			}
			return result;
		}

		// Token: 0x040047EB RID: 18411
		private JobTag tagToGive;
	}
}
