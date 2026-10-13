using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020039FF RID: 14847
	public class QuestNode_End : QuestNode
	{
		// Token: 0x060145A3 RID: 83363 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060145A4 RID: 83364 RVA: 0x006159CC File Offset: 0x00613BCC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int value = this.goodwillChangeAmount.GetValue(slate);
			Thing value2 = this.goodwillChangeFactionOf.GetValue(slate);
			if (value != 0 && value2 != null && value2.Faction != null)
			{
				QuestPart_FactionGoodwillChange questPart_FactionGoodwillChange = new QuestPart_FactionGoodwillChange();
				questPart_FactionGoodwillChange.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
				questPart_FactionGoodwillChange.faction = value2.Faction;
				questPart_FactionGoodwillChange.change = value;
				questPart_FactionGoodwillChange.historyEvent = this.goodwillChangeReason.GetValue(slate);
				slate.Set<string>("goodwillPenalty", Mathf.Abs(value).ToString(), false);
				QuestGen.quest.AddPart(questPart_FactionGoodwillChange);
			}
			QuestPart_QuestEnd questPart_QuestEnd = new QuestPart_QuestEnd();
			questPart_QuestEnd.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_QuestEnd.outcome = new QuestEndOutcome?(this.outcome.GetValue(slate));
			questPart_QuestEnd.signalListenMode = this.signalListenMode.GetValue(slate).GetValueOrDefault();
			questPart_QuestEnd.sendLetter = this.sendStandardLetter.GetValue(slate).GetValueOrDefault();
			QuestGen.quest.AddPart(questPart_QuestEnd);
		}

		// Token: 0x0400E0B5 RID: 57525
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E0B6 RID: 57526
		public SlateRef<QuestEndOutcome> outcome;

		// Token: 0x0400E0B7 RID: 57527
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		// Token: 0x0400E0B8 RID: 57528
		public SlateRef<bool?> sendStandardLetter;

		// Token: 0x0400E0B9 RID: 57529
		public SlateRef<int> goodwillChangeAmount;

		// Token: 0x0400E0BA RID: 57530
		public SlateRef<Thing> goodwillChangeFactionOf;

		// Token: 0x0400E0BB RID: 57531
		public SlateRef<HistoryEventDef> goodwillChangeReason;
	}
}
