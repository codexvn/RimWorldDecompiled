using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A00 RID: 14848
	public class QuestNode_ExpansionActive : QuestNode
	{
		// Token: 0x060145A6 RID: 83366 RVA: 0x00615B20 File Offset: 0x00613D20
		protected override bool TestRunInt(Slate slate)
		{
			if (ModsConfig.IsActive(this.modID.GetValue(slate)))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x060145A7 RID: 83367 RVA: 0x00615B70 File Offset: 0x00613D70
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (ModsConfig.IsActive(this.modID.GetValue(slate)))
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

		// Token: 0x0400E0BC RID: 57532
		[NoTranslate]
		public SlateRef<string> modID;

		// Token: 0x0400E0BD RID: 57533
		public QuestNode node;

		// Token: 0x0400E0BE RID: 57534
		public QuestNode elseNode;
	}
}
