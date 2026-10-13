using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020039E8 RID: 14824
	public class QuestNode_GreaterOrEqual : QuestNode
	{
		// Token: 0x0601455C RID: 83292 RVA: 0x006145A8 File Offset: 0x006127A8
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value1.GetValue(slate) >= this.value2.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0601455D RID: 83293 RVA: 0x006145FC File Offset: 0x006127FC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) >= this.value2.GetValue(slate))
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

		// Token: 0x0400E05D RID: 57437
		public SlateRef<double> value1;

		// Token: 0x0400E05E RID: 57438
		public SlateRef<double> value2;

		// Token: 0x0400E05F RID: 57439
		public QuestNode node;

		// Token: 0x0400E060 RID: 57440
		public QuestNode elseNode;
	}
}
