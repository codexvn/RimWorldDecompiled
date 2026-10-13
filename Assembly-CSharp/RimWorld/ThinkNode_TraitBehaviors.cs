using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017D7 RID: 6103
	public class ThinkNode_TraitBehaviors : ThinkNode
	{
		// Token: 0x06008E05 RID: 36357 RVA: 0x002A1ACC File Offset: 0x0029FCCC
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			List<Trait> allTraits = pawn.story.traits.allTraits;
			for (int i = 0; i < allTraits.Count; i++)
			{
				if (!allTraits[i].Suppressed)
				{
					ThinkTreeDef thinkTree = allTraits[i].CurrentData.thinkTree;
					if (thinkTree != null)
					{
						return thinkTree.thinkRoot.TryIssueJobPackage(pawn, jobParams);
					}
				}
			}
			return ThinkResult.NoJob;
		}
	}
}
