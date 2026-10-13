using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020039E9 RID: 14825
	public class QuestNode_GreaterOrFail : QuestNode
	{
		// Token: 0x0601455F RID: 83295 RVA: 0x00614650 File Offset: 0x00612850
		protected override bool TestRunInt(Slate slate)
		{
			return this.value1.GetValue(slate) > this.value2.GetValue(slate) && (this.node == null || this.node.TestRun(slate));
		}

		// Token: 0x06014560 RID: 83296 RVA: 0x00614684 File Offset: 0x00612884
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) > this.value2.GetValue(slate) && this.node != null)
			{
				this.node.Run();
			}
		}

		// Token: 0x0400E061 RID: 57441
		public SlateRef<double> value1;

		// Token: 0x0400E062 RID: 57442
		public SlateRef<double> value2;

		// Token: 0x0400E063 RID: 57443
		public QuestNode node;
	}
}
