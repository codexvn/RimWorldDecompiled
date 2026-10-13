using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A9E RID: 15006
	public class QuestNode_IsPermanentEnemy : QuestNode
	{
		// Token: 0x060147C5 RID: 83909 RVA: 0x0061DF16 File Offset: 0x0061C116
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsPermanentEnemy(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x060147C6 RID: 83910 RVA: 0x0061DF4E File Offset: 0x0061C14E
		protected override void RunInt()
		{
			if (this.IsPermanentEnemy(QuestGen.slate))
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

		// Token: 0x060147C7 RID: 83911 RVA: 0x0061DF84 File Offset: 0x0061C184
		private bool IsPermanentEnemy(Slate slate)
		{
			Thing value = this.thing.GetValue(slate);
			return value != null && value.Faction != null && value.Faction.def.permanentEnemy;
		}

		// Token: 0x0400E2E0 RID: 58080
		public SlateRef<Thing> thing;

		// Token: 0x0400E2E1 RID: 58081
		public QuestNode node;

		// Token: 0x0400E2E2 RID: 58082
		public QuestNode elseNode;
	}
}
