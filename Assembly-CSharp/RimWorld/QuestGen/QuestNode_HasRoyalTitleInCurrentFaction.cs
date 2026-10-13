using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A98 RID: 15000
	public class QuestNode_HasRoyalTitleInCurrentFaction : QuestNode
	{
		// Token: 0x060147AD RID: 83885 RVA: 0x0061DAFA File Offset: 0x0061BCFA
		protected override bool TestRunInt(Slate slate)
		{
			if (this.HasRoyalTitleInCurrentFaction(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x060147AE RID: 83886 RVA: 0x0061DB32 File Offset: 0x0061BD32
		protected override void RunInt()
		{
			if (this.HasRoyalTitleInCurrentFaction(QuestGen.slate))
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

		// Token: 0x060147AF RID: 83887 RVA: 0x0061DB68 File Offset: 0x0061BD68
		private bool HasRoyalTitleInCurrentFaction(Slate slate)
		{
			Pawn value = this.pawn.GetValue(slate);
			return value != null && value.Faction != null && value.royalty != null && value.royalty.HasAnyTitleIn(value.Faction);
		}

		// Token: 0x0400E2CC RID: 58060
		public SlateRef<Pawn> pawn;

		// Token: 0x0400E2CD RID: 58061
		public QuestNode node;

		// Token: 0x0400E2CE RID: 58062
		public QuestNode elseNode;
	}
}
