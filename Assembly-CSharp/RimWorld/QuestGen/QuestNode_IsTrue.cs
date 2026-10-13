using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020039EE RID: 14830
	public class QuestNode_IsTrue : QuestNode
	{
		// Token: 0x0601456E RID: 83310 RVA: 0x00614903 File Offset: 0x00612B03
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0601456F RID: 83311 RVA: 0x00614940 File Offset: 0x00612B40
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value.GetValue(slate))
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

		// Token: 0x0400E070 RID: 57456
		public SlateRef<bool> value;

		// Token: 0x0400E071 RID: 57457
		public QuestNode node;

		// Token: 0x0400E072 RID: 57458
		public QuestNode elseNode;
	}
}
