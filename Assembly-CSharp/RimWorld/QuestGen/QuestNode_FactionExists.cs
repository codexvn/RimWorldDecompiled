using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A97 RID: 14999
	public class QuestNode_FactionExists : QuestNode
	{
		// Token: 0x060147A9 RID: 83881 RVA: 0x0061DA7B File Offset: 0x0061BC7B
		protected override bool TestRunInt(Slate slate)
		{
			if (this.FactionExists(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x060147AA RID: 83882 RVA: 0x0061DAB3 File Offset: 0x0061BCB3
		protected override void RunInt()
		{
			if (this.FactionExists(QuestGen.slate))
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

		// Token: 0x060147AB RID: 83883 RVA: 0x0061DAE9 File Offset: 0x0061BCE9
		private bool FactionExists(Slate slate)
		{
			return this.faction.GetValue(slate) != null;
		}

		// Token: 0x0400E2C9 RID: 58057
		public SlateRef<Faction> faction;

		// Token: 0x0400E2CA RID: 58058
		public QuestNode node;

		// Token: 0x0400E2CB RID: 58059
		public QuestNode elseNode;
	}
}
