using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020039EF RID: 14831
	public class QuestNode_IsTrueOrUnset : QuestNode
	{
		// Token: 0x06014571 RID: 83313 RVA: 0x00614988 File Offset: 0x00612B88
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate) ?? true)
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06014572 RID: 83314 RVA: 0x006149E4 File Offset: 0x00612BE4
		protected override void RunInt()
		{
			if (this.value.GetValue(QuestGen.slate) ?? true)
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

		// Token: 0x0400E073 RID: 57459
		public SlateRef<bool?> value;

		// Token: 0x0400E074 RID: 57460
		public QuestNode node;

		// Token: 0x0400E075 RID: 57461
		public QuestNode elseNode;
	}
}
