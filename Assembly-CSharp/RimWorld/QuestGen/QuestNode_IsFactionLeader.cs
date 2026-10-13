using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A9A RID: 15002
	public class QuestNode_IsFactionLeader : QuestNode
	{
		// Token: 0x060147B5 RID: 83893 RVA: 0x0061DC6B File Offset: 0x0061BE6B
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsFactionLeader(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x060147B6 RID: 83894 RVA: 0x0061DCA3 File Offset: 0x0061BEA3
		protected override void RunInt()
		{
			if (this.IsFactionLeader(QuestGen.slate))
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

		// Token: 0x060147B7 RID: 83895 RVA: 0x0061DCDC File Offset: 0x0061BEDC
		private bool IsFactionLeader(Slate slate)
		{
			return this.pawn.GetValue(slate) != null && this.pawn.GetValue(slate).Faction != null && this.pawn.GetValue(slate).Faction.leader == this.pawn.GetValue(slate);
		}

		// Token: 0x0400E2D3 RID: 58067
		public SlateRef<Pawn> pawn;

		// Token: 0x0400E2D4 RID: 58068
		public QuestNode node;

		// Token: 0x0400E2D5 RID: 58069
		public QuestNode elseNode;
	}
}
