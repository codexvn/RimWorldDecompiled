using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020039EC RID: 14828
	public class QuestNode_IsNull : QuestNode
	{
		// Token: 0x06014568 RID: 83304 RVA: 0x006147DA File Offset: 0x006129DA
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate) == null)
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06014569 RID: 83305 RVA: 0x00614818 File Offset: 0x00612A18
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value.GetValue(slate) == null)
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

		// Token: 0x0400E06A RID: 57450
		[NoTranslate]
		public SlateRef<object> value;

		// Token: 0x0400E06B RID: 57451
		public QuestNode node;

		// Token: 0x0400E06C RID: 57452
		public QuestNode elseNode;
	}
}
