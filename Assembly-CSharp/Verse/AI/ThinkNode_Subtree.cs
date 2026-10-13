using System;

namespace Verse.AI
{
	// Token: 0x02000FFE RID: 4094
	public class ThinkNode_Subtree : ThinkNode
	{
		// Token: 0x0600648F RID: 25743 RVA: 0x001FDB1C File Offset: 0x001FBD1C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Subtree thinkNode_Subtree = (ThinkNode_Subtree)base.DeepCopy(false);
			thinkNode_Subtree.treeDef = this.treeDef;
			if (resolve)
			{
				thinkNode_Subtree.ResolveSubnodesAndRecur();
				thinkNode_Subtree.subtreeNode = thinkNode_Subtree.subNodes[this.subNodes.IndexOf(this.subtreeNode)];
			}
			return thinkNode_Subtree;
		}

		// Token: 0x06006490 RID: 25744 RVA: 0x001FDB6E File Offset: 0x001FBD6E
		protected override void ResolveSubnodes()
		{
			this.subtreeNode = this.treeDef.thinkRoot.DeepCopy(true);
			this.subNodes.Add(this.subtreeNode);
		}

		// Token: 0x06006491 RID: 25745 RVA: 0x001FDB98 File Offset: 0x001FBD98
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			return this.subtreeNode.TryIssueJobPackage(pawn, jobParams);
		}

		// Token: 0x040047EC RID: 18412
		private ThinkTreeDef treeDef;

		// Token: 0x040047ED RID: 18413
		[Unsaved(false)]
		public ThinkNode subtreeNode;
	}
}
