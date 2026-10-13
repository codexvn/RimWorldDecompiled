using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AFA RID: 15098
	public class QuestNode_ShuttleDelay : QuestNode
	{
		// Token: 0x060148EE RID: 84206 RVA: 0x00621B3F File Offset: 0x0061FD3F
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x060148EF RID: 84207 RVA: 0x00621B58 File Offset: 0x0061FD58
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ShuttleDelay questPart_ShuttleDelay = new QuestPart_ShuttleDelay();
			questPart_ShuttleDelay.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ShuttleDelay.delayTicks = this.delayTicks.GetValue(slate);
			if (this.lodgers.GetValue(slate) != null)
			{
				questPart_ShuttleDelay.lodgers.AddRange(this.lodgers.GetValue(slate));
			}
			questPart_ShuttleDelay.expiryInfoPart = "ShuttleArrivesIn".Translate();
			questPart_ShuttleDelay.expiryInfoPartTip = "ShuttleArrivesOn".Translate();
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_ShuttleDelay);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_ShuttleDelay.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_ShuttleDelay);
		}

		// Token: 0x0400E42B RID: 58411
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400E42C RID: 58412
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x0400E42D RID: 58413
		public SlateRef<int> delayTicks;

		// Token: 0x0400E42E RID: 58414
		public SlateRef<IEnumerable<Pawn>> lodgers;

		// Token: 0x0400E42F RID: 58415
		public QuestNode node;
	}
}
