using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020039ED RID: 14829
	public class QuestNode_IsSet : QuestNode
	{
		// Token: 0x0601456B RID: 83307 RVA: 0x00614860 File Offset: 0x00612A60
		protected override bool TestRunInt(Slate slate)
		{
			if (slate.Exists(this.name.GetValue(slate), false))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0601456C RID: 83308 RVA: 0x006148B0 File Offset: 0x00612AB0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestGen.slate.Exists(this.name.GetValue(slate), false))
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

		// Token: 0x0400E06D RID: 57453
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x0400E06E RID: 57454
		public QuestNode node;

		// Token: 0x0400E06F RID: 57455
		public QuestNode elseNode;
	}
}
