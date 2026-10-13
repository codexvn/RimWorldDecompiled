using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000FFC RID: 4092
	public class ThinkNode_Random : ThinkNode
	{
		// Token: 0x06006488 RID: 25736 RVA: 0x001FD9B4 File Offset: 0x001FBBB4
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkNode_Random.tempList.Clear();
			for (int i = 0; i < this.subNodes.Count; i++)
			{
				ThinkNode_Random.tempList.Add(this.subNodes[i]);
			}
			ThinkNode_Random.tempList.Shuffle<ThinkNode>();
			for (int j = 0; j < ThinkNode_Random.tempList.Count; j++)
			{
				ThinkResult result = ThinkNode_Random.tempList[j].TryIssueJobPackage(pawn, jobParams);
				if (result.IsValid)
				{
					return result;
				}
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x040047EA RID: 18410
		private static List<ThinkNode> tempList = new List<ThinkNode>();
	}
}
