using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A9C RID: 15004
	public class QuestNode_IsOfFaction : QuestNode
	{
		// Token: 0x060147BD RID: 83901 RVA: 0x0061DDC9 File Offset: 0x0061BFC9
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsOfFaction(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x060147BE RID: 83902 RVA: 0x0061DE01 File Offset: 0x0061C001
		protected override void RunInt()
		{
			if (this.IsOfFaction(QuestGen.slate))
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

		// Token: 0x060147BF RID: 83903 RVA: 0x0061DE37 File Offset: 0x0061C037
		private bool IsOfFaction(Slate slate)
		{
			return this.thing.GetValue(slate) != null && this.thing.GetValue(slate).Faction == this.faction.GetValue(slate);
		}

		// Token: 0x0400E2D9 RID: 58073
		public SlateRef<Thing> thing;

		// Token: 0x0400E2DA RID: 58074
		public SlateRef<Faction> faction;

		// Token: 0x0400E2DB RID: 58075
		public QuestNode node;

		// Token: 0x0400E2DC RID: 58076
		public QuestNode elseNode;
	}
}
