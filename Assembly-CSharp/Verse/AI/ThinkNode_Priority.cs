using System;

namespace Verse.AI
{
	// Token: 0x02000FF7 RID: 4087
	public class ThinkNode_Priority : ThinkNode
	{
		// Token: 0x0600647C RID: 25724 RVA: 0x001FD63C File Offset: 0x001FB83C
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			int count = this.subNodes.Count;
			for (int i = 0; i < count; i++)
			{
				ThinkResult result = ThinkResult.NoJob;
				try
				{
					result = this.subNodes[i].TryIssueJobPackage(pawn, jobParams);
				}
				catch (Exception ex)
				{
					string str = "Exception in ";
					Type type = base.GetType();
					Log.Error(str + ((type != null) ? type.ToString() : null) + " TryIssueJobPackage: " + ex.ToString());
				}
				if (result.IsValid)
				{
					return result;
				}
			}
			return ThinkResult.NoJob;
		}
	}
}
