using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AFB RID: 15099
	public class QuestNode_ShuttleLeaveDelay : QuestNode
	{
		// Token: 0x060148F1 RID: 84209 RVA: 0x00621C4C File Offset: 0x0061FE4C
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x060148F2 RID: 84210 RVA: 0x00621C64 File Offset: 0x0061FE64
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ShuttleLeaveDelay questPart_ShuttleLeaveDelay = new QuestPart_ShuttleLeaveDelay();
			questPart_ShuttleLeaveDelay.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ShuttleLeaveDelay.delayTicks = this.delayTicks.GetValue(slate);
			questPart_ShuttleLeaveDelay.shuttle = this.shuttle.GetValue(slate);
			questPart_ShuttleLeaveDelay.expiryInfoPart = "ShuttleDepartsIn".Translate();
			questPart_ShuttleLeaveDelay.expiryInfoPartTip = "ShuttleDepartsOn".Translate();
			if (this.inSignalsDisable.GetValue(slate) != null)
			{
				foreach (string signal in this.inSignalsDisable.GetValue(slate))
				{
					questPart_ShuttleLeaveDelay.inSignalsDisable.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
				}
			}
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_ShuttleLeaveDelay);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_ShuttleLeaveDelay.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_ShuttleLeaveDelay);
		}

		// Token: 0x0400E430 RID: 58416
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400E431 RID: 58417
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x0400E432 RID: 58418
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignalsDisable;

		// Token: 0x0400E433 RID: 58419
		public SlateRef<int> delayTicks;

		// Token: 0x0400E434 RID: 58420
		public SlateRef<Thing> shuttle;

		// Token: 0x0400E435 RID: 58421
		public QuestNode node;
	}
}
