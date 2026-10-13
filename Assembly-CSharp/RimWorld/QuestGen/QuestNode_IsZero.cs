using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020039F0 RID: 14832
	public class QuestNode_IsZero : QuestNode
	{
		// Token: 0x06014574 RID: 83316 RVA: 0x00614A40 File Offset: 0x00612C40
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate) == 0.0)
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06014575 RID: 83317 RVA: 0x00614A94 File Offset: 0x00612C94
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value.GetValue(slate) == 0.0)
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

		// Token: 0x0400E076 RID: 57462
		public SlateRef<double> value;

		// Token: 0x0400E077 RID: 57463
		public QuestNode node;

		// Token: 0x0400E078 RID: 57464
		public QuestNode elseNode;
	}
}
