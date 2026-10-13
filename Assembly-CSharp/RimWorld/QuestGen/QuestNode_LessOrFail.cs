using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020039F3 RID: 14835
	public class QuestNode_LessOrFail : QuestNode
	{
		// Token: 0x0601457D RID: 83325 RVA: 0x00614C38 File Offset: 0x00612E38
		protected override bool TestRunInt(Slate slate)
		{
			return this.value1.GetValue(slate) < this.value2.GetValue(slate) && (this.node == null || this.node.TestRun(slate));
		}

		// Token: 0x0601457E RID: 83326 RVA: 0x00614C6C File Offset: 0x00612E6C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) < this.value2.GetValue(slate) && this.node != null)
			{
				this.node.Run();
			}
		}

		// Token: 0x0400E081 RID: 57473
		public SlateRef<double> value1;

		// Token: 0x0400E082 RID: 57474
		public SlateRef<double> value2;

		// Token: 0x0400E083 RID: 57475
		public QuestNode node;
	}
}
