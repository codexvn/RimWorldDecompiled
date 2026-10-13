using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020039E4 RID: 14820
	public class QuestNode_Equal : QuestNode
	{
		// Token: 0x06014552 RID: 83282 RVA: 0x00614310 File Offset: 0x00612510
		protected override bool TestRunInt(Slate slate)
		{
			if (QuestNodeEqualUtility.Equal(this.value1.GetValue(slate), this.value2.GetValue(slate), this.compareAs.GetValue(slate)))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06014553 RID: 83283 RVA: 0x00614378 File Offset: 0x00612578
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestNodeEqualUtility.Equal(this.value1.GetValue(slate), this.value2.GetValue(slate), this.compareAs.GetValue(slate)))
			{
				if (this.node != null)
				{
					this.node.Run();
					return;
				}
			}
			else if (this.elseNode != null)
			{
				this.elseNode.Run();
			}
		}

		// Token: 0x0400E050 RID: 57424
		[NoTranslate]
		public SlateRef<object> value1;

		// Token: 0x0400E051 RID: 57425
		[NoTranslate]
		public SlateRef<object> value2;

		// Token: 0x0400E052 RID: 57426
		public SlateRef<Type> compareAs;

		// Token: 0x0400E053 RID: 57427
		public QuestNode node;

		// Token: 0x0400E054 RID: 57428
		public QuestNode elseNode;
	}
}
