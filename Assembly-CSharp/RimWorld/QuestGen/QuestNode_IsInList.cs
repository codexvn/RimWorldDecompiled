using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020039EB RID: 14827
	public class QuestNode_IsInList : QuestNode
	{
		// Token: 0x06014565 RID: 83301 RVA: 0x00614724 File Offset: 0x00612924
		protected override bool TestRunInt(Slate slate)
		{
			if (QuestGenUtility.IsInList(slate, this.name.GetValue(slate), this.value.GetValue(slate)))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06014566 RID: 83302 RVA: 0x00614780 File Offset: 0x00612980
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestGenUtility.IsInList(slate, this.name.GetValue(slate), this.value.GetValue(slate)))
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

		// Token: 0x0400E066 RID: 57446
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x0400E067 RID: 57447
		public SlateRef<object> value;

		// Token: 0x0400E068 RID: 57448
		public QuestNode node;

		// Token: 0x0400E069 RID: 57449
		public QuestNode elseNode;
	}
}
