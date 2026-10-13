using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BDF RID: 15327
	public abstract class QuestNode_Filter : QuestNode
	{
		// Token: 0x06014CE3 RID: 85219 RVA: 0x0063B9EC File Offset: 0x00639BEC
		protected override bool TestRunInt(Slate slate)
		{
			return (this.node == null || this.node.TestRun(slate)) && (this.elseNode == null || this.elseNode.TestRun(slate));
		}

		// Token: 0x06014CE4 RID: 85220
		protected abstract QuestPart_Filter MakeFilterQuestPart();

		// Token: 0x06014CE5 RID: 85221 RVA: 0x0063BA20 File Offset: 0x00639C20
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Filter questPart_Filter = this.MakeFilterQuestPart();
			questPart_Filter.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			if (this.node != null)
			{
				questPart_Filter.outSignal = QuestGen.GenerateNewSignal("FilterPass", true);
				QuestGenUtility.RunInnerNode(this.node, questPart_Filter.outSignal);
			}
			if (this.elseNode != null)
			{
				questPart_Filter.outSignalElse = QuestGen.GenerateNewSignal("FilterNoPass", true);
				QuestGenUtility.RunInnerNode(this.elseNode, questPart_Filter.outSignalElse);
			}
			QuestGen.quest.AddPart(questPart_Filter);
		}

		// Token: 0x0400E79E RID: 59294
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E79F RID: 59295
		public QuestNode node;

		// Token: 0x0400E7A0 RID: 59296
		public QuestNode elseNode;

		// Token: 0x0400E7A1 RID: 59297
		private const string FilterPassSignal = "FilterPass";

		// Token: 0x0400E7A2 RID: 59298
		private const string FilterNoPassSignal = "FilterNoPass";
	}
}
