using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000FFB RID: 4091
	public class ThinkNode_PrioritySorter : ThinkNode
	{
		// Token: 0x06006484 RID: 25732 RVA: 0x001FD807 File Offset: 0x001FBA07
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_PrioritySorter thinkNode_PrioritySorter = (ThinkNode_PrioritySorter)base.DeepCopy(resolve);
			thinkNode_PrioritySorter.minPriority = this.minPriority;
			return thinkNode_PrioritySorter;
		}

		// Token: 0x06006485 RID: 25733 RVA: 0x001FD824 File Offset: 0x001FBA24
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkNode_PrioritySorter.workingNodes.Clear();
			int count = this.subNodes.Count;
			for (int i = 0; i < count; i++)
			{
				ThinkNode_PrioritySorter.workingNodes.Insert(Rand.Range(0, ThinkNode_PrioritySorter.workingNodes.Count - 1), this.subNodes[i]);
			}
			while (ThinkNode_PrioritySorter.workingNodes.Count > 0)
			{
				float num = 0f;
				int num2 = -1;
				for (int j = 0; j < ThinkNode_PrioritySorter.workingNodes.Count; j++)
				{
					float num3 = 0f;
					try
					{
						num3 = ThinkNode_PrioritySorter.workingNodes[j].GetPriority(pawn);
					}
					catch (Exception ex)
					{
						string str = "Exception in ";
						Type type = base.GetType();
						Log.Error(str + ((type != null) ? type.ToString() : null) + " GetPriority: " + ex.ToString());
					}
					if (num3 > 0f && num3 >= this.minPriority && num3 > num)
					{
						num = num3;
						num2 = j;
					}
				}
				if (num2 == -1)
				{
					break;
				}
				ThinkResult result = ThinkResult.NoJob;
				try
				{
					result = ThinkNode_PrioritySorter.workingNodes[num2].TryIssueJobPackage(pawn, jobParams);
				}
				catch (Exception ex2)
				{
					string str2 = "Exception in ";
					Type type2 = base.GetType();
					Log.Error(str2 + ((type2 != null) ? type2.ToString() : null) + " TryIssueJobPackage: " + ex2.ToString());
				}
				if (result.IsValid)
				{
					return result;
				}
				ThinkNode_PrioritySorter.workingNodes.RemoveAt(num2);
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x040047E8 RID: 18408
		public float minPriority;

		// Token: 0x040047E9 RID: 18409
		private static List<ThinkNode> workingNodes = new List<ThinkNode>();
	}
}
