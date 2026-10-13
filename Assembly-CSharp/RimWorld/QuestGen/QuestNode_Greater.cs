using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020039E7 RID: 14823
	public class QuestNode_Greater : QuestNode
	{
		// Token: 0x06014559 RID: 83289 RVA: 0x00614500 File Offset: 0x00612700
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value1.GetValue(slate) > this.value2.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0601455A RID: 83290 RVA: 0x00614554 File Offset: 0x00612754
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) > this.value2.GetValue(slate))
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

		// Token: 0x0400E059 RID: 57433
		public SlateRef<double> value1;

		// Token: 0x0400E05A RID: 57434
		public SlateRef<double> value2;

		// Token: 0x0400E05B RID: 57435
		public QuestNode node;

		// Token: 0x0400E05C RID: 57436
		public QuestNode elseNode;
	}
}
