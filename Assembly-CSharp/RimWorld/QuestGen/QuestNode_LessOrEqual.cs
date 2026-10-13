using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020039F2 RID: 14834
	public class QuestNode_LessOrEqual : QuestNode
	{
		// Token: 0x0601457A RID: 83322 RVA: 0x00614B90 File Offset: 0x00612D90
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value1.GetValue(slate) <= this.value2.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0601457B RID: 83323 RVA: 0x00614BE4 File Offset: 0x00612DE4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) <= this.value2.GetValue(slate))
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

		// Token: 0x0400E07D RID: 57469
		public SlateRef<double> value1;

		// Token: 0x0400E07E RID: 57470
		public SlateRef<double> value2;

		// Token: 0x0400E07F RID: 57471
		public QuestNode node;

		// Token: 0x0400E080 RID: 57472
		public QuestNode elseNode;
	}
}
