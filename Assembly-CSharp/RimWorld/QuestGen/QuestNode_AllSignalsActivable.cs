using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020039F8 RID: 14840
	public class QuestNode_AllSignalsActivable : QuestNode
	{
		// Token: 0x0601458C RID: 83340 RVA: 0x00615064 File Offset: 0x00613264
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0601458D RID: 83341 RVA: 0x0061507C File Offset: 0x0061327C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if ((this.outSignals.GetValue(slate) != null && this.outSignals.GetValue(slate).Count<string>() != 0) + this.node == null)
			{
				return;
			}
			QuestPart_PassAllActivable questPart_PassAllActivable = new QuestPart_PassAllActivable();
			QuestGen.quest.AddPart(questPart_PassAllActivable);
			questPart_PassAllActivable.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_PassAllActivable.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			foreach (string signal in this.inSignals.GetValue(slate))
			{
				questPart_PassAllActivable.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
			}
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_PassAllActivable.OutSignalCompleted);
			}
			IEnumerable<string> value = this.outSignals.GetValue(slate);
			if (value != null)
			{
				foreach (string signal2 in value)
				{
					questPart_PassAllActivable.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal2));
				}
			}
			questPart_PassAllActivable.signalListenMode = this.signalListenMode.GetValue(slate).GetValueOrDefault();
		}

		// Token: 0x0400E08F RID: 57487
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400E090 RID: 57488
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x0400E091 RID: 57489
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignals;

		// Token: 0x0400E092 RID: 57490
		[NoTranslate]
		public SlateRef<IEnumerable<string>> outSignals;

		// Token: 0x0400E093 RID: 57491
		public QuestNode node;

		// Token: 0x0400E094 RID: 57492
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;
	}
}
