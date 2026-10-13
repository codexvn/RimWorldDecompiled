using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AE4 RID: 15076
	public class QuestNode_RecordHistoryEvent : QuestNode
	{
		// Token: 0x060148A4 RID: 84132 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060148A5 RID: 84133 RVA: 0x00621014 File Offset: 0x0061F214
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			HistoryEventDef value = this.historyDef.GetValue(slate);
			if (value == null)
			{
				return;
			}
			quest.AddPart(new QuestPart_RecordHistoryEvent
			{
				inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false)),
				historyEvent = value
			});
		}

		// Token: 0x0400E3F1 RID: 58353
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E3F2 RID: 58354
		public SlateRef<HistoryEventDef> historyDef;
	}
}
