using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A99 RID: 15001
	public class QuestNode_IsFactionHostileToPlayer : QuestNode
	{
		// Token: 0x060147B1 RID: 83889 RVA: 0x0061DBA8 File Offset: 0x0061BDA8
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsHostile(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x060147B2 RID: 83890 RVA: 0x0061DBE0 File Offset: 0x0061BDE0
		protected override void RunInt()
		{
			if (this.IsHostile(QuestGen.slate))
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

		// Token: 0x060147B3 RID: 83891 RVA: 0x0061DC18 File Offset: 0x0061BE18
		private bool IsHostile(Slate slate)
		{
			Faction value = this.faction.GetValue(slate);
			if (value != null)
			{
				return value.HostileTo(Faction.OfPlayer);
			}
			Thing value2 = this.factionOf.GetValue(slate);
			return value2 != null && value2.Faction != null && value2.Faction.HostileTo(Faction.OfPlayer);
		}

		// Token: 0x0400E2CF RID: 58063
		public SlateRef<Faction> faction;

		// Token: 0x0400E2D0 RID: 58064
		public SlateRef<Thing> factionOf;

		// Token: 0x0400E2D1 RID: 58065
		public QuestNode node;

		// Token: 0x0400E2D2 RID: 58066
		public QuestNode elseNode;
	}
}
