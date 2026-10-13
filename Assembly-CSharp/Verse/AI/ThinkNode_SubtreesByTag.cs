using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI
{
	// Token: 0x02000FFF RID: 4095
	public class ThinkNode_SubtreesByTag : ThinkNode
	{
		// Token: 0x06006493 RID: 25747 RVA: 0x001FDBA7 File Offset: 0x001FBDA7
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_SubtreesByTag thinkNode_SubtreesByTag = (ThinkNode_SubtreesByTag)base.DeepCopy(resolve);
			thinkNode_SubtreesByTag.insertTag = this.insertTag;
			return thinkNode_SubtreesByTag;
		}

		// Token: 0x06006494 RID: 25748 RVA: 0x000026EA File Offset: 0x000008EA
		protected override void ResolveSubnodes()
		{
		}

		// Token: 0x06006495 RID: 25749 RVA: 0x001FDBC4 File Offset: 0x001FBDC4
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			if (this.matchedTrees == null)
			{
				this.matchedTrees = new List<ThinkTreeDef>();
				foreach (ThinkTreeDef thinkTreeDef in DefDatabase<ThinkTreeDef>.AllDefs)
				{
					if (thinkTreeDef.insertTag == this.insertTag)
					{
						this.matchedTrees.Add(thinkTreeDef);
					}
				}
				this.matchedTrees = (from tDef in this.matchedTrees
				orderby tDef.insertPriority descending
				select tDef).ToList<ThinkTreeDef>();
			}
			for (int i = 0; i < this.matchedTrees.Count; i++)
			{
				ThinkResult result = this.matchedTrees[i].thinkRoot.TryIssueJobPackage(pawn, jobParams);
				if (result.IsValid)
				{
					return result;
				}
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x040047EE RID: 18414
		[NoTranslate]
		public string insertTag;

		// Token: 0x040047EF RID: 18415
		[Unsaved(false)]
		private List<ThinkTreeDef> matchedTrees;
	}
}
