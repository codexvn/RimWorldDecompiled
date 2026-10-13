using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A9B RID: 15003
	public class QuestNode_IsFreeWorldPawn : QuestNode
	{
		// Token: 0x060147B9 RID: 83897 RVA: 0x0061DD30 File Offset: 0x0061BF30
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsFreeWorldPawn(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x060147BA RID: 83898 RVA: 0x0061DD68 File Offset: 0x0061BF68
		protected override void RunInt()
		{
			if (this.IsFreeWorldPawn(QuestGen.slate))
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

		// Token: 0x060147BB RID: 83899 RVA: 0x0061DD9E File Offset: 0x0061BF9E
		private bool IsFreeWorldPawn(Slate slate)
		{
			return this.pawn.GetValue(slate) != null && Find.WorldPawns.GetSituation(this.pawn.GetValue(slate)) == WorldPawnSituation.Free;
		}

		// Token: 0x0400E2D6 RID: 58070
		public SlateRef<Pawn> pawn;

		// Token: 0x0400E2D7 RID: 58071
		public QuestNode node;

		// Token: 0x0400E2D8 RID: 58072
		public QuestNode elseNode;
	}
}
