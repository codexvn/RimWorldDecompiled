using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020039FA RID: 14842
	public class QuestNode_AnySignalActivable : QuestNode
	{
		// Token: 0x06014592 RID: 83346 RVA: 0x00615418 File Offset: 0x00613618
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x06014593 RID: 83347 RVA: 0x00615430 File Offset: 0x00613630
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if ((this.outSignals.GetValue(slate) != null && this.outSignals.GetValue(slate).Count<string>() != 0) + this.node == null)
			{
				return;
			}
			QuestPart_PassAnyActivable questPart_PassAnyActivable = new QuestPart_PassAnyActivable();
			QuestGen.quest.AddPart(questPart_PassAnyActivable);
			questPart_PassAnyActivable.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_PassAnyActivable.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			foreach (string signal in this.inSignals.GetValue(slate))
			{
				questPart_PassAnyActivable.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
			}
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_PassAnyActivable.OutSignalCompleted);
			}
			IEnumerable<string> value = this.outSignals.GetValue(slate);
			if (value != null)
			{
				foreach (string signal2 in value)
				{
					questPart_PassAnyActivable.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal2));
				}
			}
			questPart_PassAnyActivable.signalListenMode = this.signalListenMode.GetValue(slate).GetValueOrDefault();
		}

		// Token: 0x0400E09A RID: 57498
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400E09B RID: 57499
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x0400E09C RID: 57500
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignals;

		// Token: 0x0400E09D RID: 57501
		[NoTranslate]
		public SlateRef<IEnumerable<string>> outSignals;

		// Token: 0x0400E09E RID: 57502
		public QuestNode node;

		// Token: 0x0400E09F RID: 57503
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;
	}
}
