using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003ADC RID: 15068
	public class QuestNode_MoodBelow : QuestNode
	{
		// Token: 0x0601488A RID: 84106 RVA: 0x0062073A File Offset: 0x0061E93A
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0601488B RID: 84107 RVA: 0x00620754 File Offset: 0x0061E954
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_MoodBelow questPart_MoodBelow = new QuestPart_MoodBelow();
			questPart_MoodBelow.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_MoodBelow.threshold = this.threshold.GetValue(slate);
			questPart_MoodBelow.minTicksBelowThreshold = 40000;
			questPart_MoodBelow.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_MoodBelow);
			}
			if (!this.outSignal.GetValue(slate).NullOrEmpty())
			{
				questPart_MoodBelow.outSignalsCompleted.Add(this.outSignal.GetValue(slate));
			}
			QuestGen.quest.AddPart(questPart_MoodBelow);
		}

		// Token: 0x0400E3C7 RID: 58311
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400E3C8 RID: 58312
		[NoTranslate]
		public SlateRef<string> outSignal;

		// Token: 0x0400E3C9 RID: 58313
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x0400E3CA RID: 58314
		public SlateRef<float> threshold;

		// Token: 0x0400E3CB RID: 58315
		public QuestNode node;

		// Token: 0x0400E3CC RID: 58316
		private const int MinTicksBelowMinMood = 40000;
	}
}
