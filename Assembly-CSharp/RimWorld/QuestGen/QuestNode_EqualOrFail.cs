using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020039E6 RID: 14822
	public class QuestNode_EqualOrFail : QuestNode
	{
		// Token: 0x06014556 RID: 83286 RVA: 0x0061445C File Offset: 0x0061265C
		protected override bool TestRunInt(Slate slate)
		{
			return QuestNodeEqualUtility.Equal(this.value1.GetValue(slate), this.value2.GetValue(slate), this.compareAs.GetValue(slate)) && (this.node == null || this.node.TestRun(slate));
		}

		// Token: 0x06014557 RID: 83287 RVA: 0x006144AC File Offset: 0x006126AC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestNodeEqualUtility.Equal(this.value1.GetValue(slate), this.value2.GetValue(slate), this.compareAs.GetValue(slate)) && this.node != null)
			{
				this.node.Run();
			}
		}

		// Token: 0x0400E055 RID: 57429
		public SlateRef<object> value1;

		// Token: 0x0400E056 RID: 57430
		public SlateRef<object> value2;

		// Token: 0x0400E057 RID: 57431
		public SlateRef<Type> compareAs;

		// Token: 0x0400E058 RID: 57432
		public QuestNode node;
	}
}
