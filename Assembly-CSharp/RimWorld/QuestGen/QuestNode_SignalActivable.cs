using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A0B RID: 14859
	public class QuestNode_SignalActivable : QuestNode
	{
		// Token: 0x060145CD RID: 83405 RVA: 0x0061647C File Offset: 0x0061467C
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x060145CE RID: 83406 RVA: 0x00616494 File Offset: 0x00614694
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if ((this.outSignals.GetValue(slate) != null && this.outSignals.GetValue(slate).Count<string>() != 0) + this.node == null)
			{
				return;
			}
			QuestPart_PassActivable questPart_PassActivable = new QuestPart_PassActivable();
			QuestGen.quest.AddPart(questPart_PassActivable);
			questPart_PassActivable.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_PassActivable.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			questPart_PassActivable.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate));
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_PassActivable.OutSignalCompleted);
			}
			IEnumerable<string> value = this.outSignals.GetValue(slate);
			if (value != null)
			{
				foreach (string signal in value)
				{
					questPart_PassActivable.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
				}
			}
			questPart_PassActivable.signalListenMode = this.signalListenMode.GetValue(slate).GetValueOrDefault();
		}

		// Token: 0x0400E0E0 RID: 57568
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400E0E1 RID: 57569
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x0400E0E2 RID: 57570
		[NoTranslate]
		[TranslationHandle(Priority = 100)]
		public SlateRef<string> inSignal;

		// Token: 0x0400E0E3 RID: 57571
		[NoTranslate]
		public SlateRef<IEnumerable<string>> outSignals;

		// Token: 0x0400E0E4 RID: 57572
		public QuestNode node;

		// Token: 0x0400E0E5 RID: 57573
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;
	}
}
