using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200182C RID: 6188
	public class ThoughtWorker_Precept_TreeDensity : ThoughtWorker_Precept
	{
		// Token: 0x06008F1E RID: 36638 RVA: 0x002A4F8B File Offset: 0x002A318B
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(this.ThoughtStageIndex(p));
		}

		// Token: 0x06008F1F RID: 36639 RVA: 0x002A4FA8 File Offset: 0x002A31A8
		private int ThoughtStageIndex(Pawn p)
		{
			int playerResponsibleTreeDestructionCount = p.Map.treeDestructionTracker.PlayerResponsibleTreeDestructionCount;
			for (int i = ThoughtWorker_Precept_TreeDensity.treeDestructionThresholds.Length - 1; i >= 0; i--)
			{
				if (playerResponsibleTreeDestructionCount >= ThoughtWorker_Precept_TreeDensity.treeDestructionThresholds[i])
				{
					return i + 1;
				}
			}
			return 0;
		}

		// Token: 0x04005C26 RID: 23590
		private static readonly int[] treeDestructionThresholds = new int[]
		{
			2,
			5,
			8,
			10,
			13,
			16,
			20
		};
	}
}
