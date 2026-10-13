using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A9F RID: 15007
	public class QuestNode_ModIsActive : QuestNode
	{
		// Token: 0x060147C9 RID: 83913 RVA: 0x0061DFBC File Offset: 0x0061C1BC
		protected override bool TestRunInt(Slate slate)
		{
			if (ModsConfig.IsActive(this.modID.GetValue(slate)))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x060147CA RID: 83914 RVA: 0x0061E009 File Offset: 0x0061C209
		protected override void RunInt()
		{
			if (ModsConfig.IsActive(this.modID.GetValue(QuestGen.slate)))
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

		// Token: 0x0400E2E3 RID: 58083
		public SlateRef<string> modID;

		// Token: 0x0400E2E4 RID: 58084
		public QuestNode node;

		// Token: 0x0400E2E5 RID: 58085
		public QuestNode elseNode;
	}
}
