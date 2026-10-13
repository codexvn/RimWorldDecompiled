using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AD0 RID: 15056
	public class QuestNode_GuardianShipDelay : QuestNode
	{
		// Token: 0x06014867 RID: 84071 RVA: 0x0061FE39 File Offset: 0x0061E039
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x06014868 RID: 84072 RVA: 0x0061FE54 File Offset: 0x0061E054
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_GuardianShipDelay questPart_GuardianShipDelay = new QuestPart_GuardianShipDelay();
			questPart_GuardianShipDelay.pawn = this.pawn.GetValue(slate);
			questPart_GuardianShipDelay.delayTicks = this.delayTicks.GetValue(slate);
			questPart_GuardianShipDelay.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_GuardianShipDelay.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			questPart_GuardianShipDelay.reactivatable = true;
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_GuardianShipDelay);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_GuardianShipDelay.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_GuardianShipDelay);
		}

		// Token: 0x0400E38F RID: 58255
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400E390 RID: 58256
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x0400E391 RID: 58257
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x0400E392 RID: 58258
		public SlateRef<Pawn> pawn;

		// Token: 0x0400E393 RID: 58259
		public SlateRef<int> delayTicks;

		// Token: 0x0400E394 RID: 58260
		public QuestNode node;
	}
}
