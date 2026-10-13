using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020039EA RID: 14826
	public class QuestNode_HasGravEngine : QuestNode
	{
		// Token: 0x06014562 RID: 83298 RVA: 0x006146C4 File Offset: 0x006128C4
		protected override bool TestRunInt(Slate slate)
		{
			if (GravshipUtility.PlayerHasGravEngine())
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06014563 RID: 83299 RVA: 0x006146FA File Offset: 0x006128FA
		protected override void RunInt()
		{
			if (GravshipUtility.PlayerHasGravEngine())
			{
				QuestNode questNode = this.node;
				if (questNode == null)
				{
					return;
				}
				questNode.Run();
				return;
			}
			else
			{
				QuestNode questNode2 = this.elseNode;
				if (questNode2 == null)
				{
					return;
				}
				questNode2.Run();
				return;
			}
		}

		// Token: 0x0400E064 RID: 57444
		public QuestNode node;

		// Token: 0x0400E065 RID: 57445
		public QuestNode elseNode;
	}
}
