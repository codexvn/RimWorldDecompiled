using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020039FC RID: 14844
	public class QuestNode_Chance : QuestNode
	{
		// Token: 0x06014598 RID: 83352 RVA: 0x006155DC File Offset: 0x006137DC
		protected override bool TestRunInt(Slate slate)
		{
			if (this.node == null || this.elseNode == null)
			{
				return true;
			}
			if (this.node.TestRun(slate.DeepCopy()))
			{
				this.node.TestRun(slate);
				return true;
			}
			if (this.elseNode.TestRun(slate.DeepCopy()))
			{
				this.elseNode.TestRun(slate);
				return true;
			}
			return false;
		}

		// Token: 0x06014599 RID: 83353 RVA: 0x00615640 File Offset: 0x00613840
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (Rand.Chance(this.chance.GetValue(slate)))
			{
				if (this.node == null)
				{
					return;
				}
				if (this.node.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.node.Run();
					return;
				}
				if (this.elseNode != null && this.elseNode.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.elseNode.Run();
					return;
				}
			}
			else
			{
				if (this.elseNode == null)
				{
					return;
				}
				if (this.elseNode.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.elseNode.Run();
					return;
				}
				if (this.node != null && this.node.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.node.Run();
				}
			}
		}

		// Token: 0x0400E0A0 RID: 57504
		public SlateRef<float> chance;

		// Token: 0x0400E0A1 RID: 57505
		public QuestNode node;

		// Token: 0x0400E0A2 RID: 57506
		public QuestNode elseNode;
	}
}
