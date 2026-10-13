using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020039C1 RID: 14785
	public static class QuestGen_HistoryEvents
	{
		// Token: 0x0601449D RID: 83101 RVA: 0x0060F768 File Offset: 0x0060D968
		public static QuestPart_RecordHistoryEvent RecordHistoryEvent(this Quest quest, HistoryEventDef def, string inSignal = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_RecordHistoryEvent questPart_RecordHistoryEvent = new QuestPart_RecordHistoryEvent();
			questPart_RecordHistoryEvent.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_RecordHistoryEvent.historyEvent = def;
			questPart_RecordHistoryEvent.signalListenMode = signalListenMode;
			quest.AddPart(questPart_RecordHistoryEvent);
			return questPart_RecordHistoryEvent;
		}
	}
}
