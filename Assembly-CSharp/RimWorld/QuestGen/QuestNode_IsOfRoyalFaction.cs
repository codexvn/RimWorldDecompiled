using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A9D RID: 15005
	public class QuestNode_IsOfRoyalFaction : QuestNode
	{
		// Token: 0x060147C1 RID: 83905 RVA: 0x0061DE68 File Offset: 0x0061C068
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsOfRoyalFaction(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x060147C2 RID: 83906 RVA: 0x0061DEA0 File Offset: 0x0061C0A0
		protected override void RunInt()
		{
			if (this.IsOfRoyalFaction(QuestGen.slate))
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

		// Token: 0x060147C3 RID: 83907 RVA: 0x0061DED6 File Offset: 0x0061C0D6
		private bool IsOfRoyalFaction(Slate slate)
		{
			return this.thing.GetValue(slate) != null && this.thing.GetValue(slate).Faction != null && this.thing.GetValue(slate).Faction.def.HasRoyalTitles;
		}

		// Token: 0x0400E2DD RID: 58077
		public SlateRef<Thing> thing;

		// Token: 0x0400E2DE RID: 58078
		public QuestNode node;

		// Token: 0x0400E2DF RID: 58079
		public QuestNode elseNode;
	}
}
