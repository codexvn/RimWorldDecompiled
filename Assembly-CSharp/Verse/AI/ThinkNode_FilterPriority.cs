using System;

namespace Verse.AI
{
	// Token: 0x02000FF8 RID: 4088
	public class ThinkNode_FilterPriority : ThinkNode
	{
		// Token: 0x0600647E RID: 25726 RVA: 0x001FD6D4 File Offset: 0x001FB8D4
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_FilterPriority thinkNode_FilterPriority = (ThinkNode_FilterPriority)base.DeepCopy(resolve);
			thinkNode_FilterPriority.minPriority = this.minPriority;
			return thinkNode_FilterPriority;
		}

		// Token: 0x0600647F RID: 25727 RVA: 0x001FD6F0 File Offset: 0x001FB8F0
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			int count = this.subNodes.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.subNodes[i].GetPriority(pawn) > this.minPriority)
				{
					ThinkResult result = this.subNodes[i].TryIssueJobPackage(pawn, jobParams);
					if (result.IsValid)
					{
						return result;
					}
				}
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x040047D6 RID: 18390
		public float minPriority = 0.5f;
	}
}
