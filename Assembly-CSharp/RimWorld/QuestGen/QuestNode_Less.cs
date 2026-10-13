using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020039F1 RID: 14833
	public class QuestNode_Less : QuestNode
	{
		// Token: 0x06014577 RID: 83319 RVA: 0x00614AE8 File Offset: 0x00612CE8
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value1.GetValue(slate) < this.value2.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06014578 RID: 83320 RVA: 0x00614B3C File Offset: 0x00612D3C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) < this.value2.GetValue(slate))
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

		// Token: 0x0400E079 RID: 57465
		public SlateRef<double> value1;

		// Token: 0x0400E07A RID: 57466
		public SlateRef<double> value2;

		// Token: 0x0400E07B RID: 57467
		public QuestNode node;

		// Token: 0x0400E07C RID: 57468
		public QuestNode elseNode;
	}
}
