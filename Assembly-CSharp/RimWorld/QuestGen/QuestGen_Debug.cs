using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020039B9 RID: 14777
	public static class QuestGen_Debug
	{
		// Token: 0x06014479 RID: 83065 RVA: 0x0060E630 File Offset: 0x0060C830
		public static void Log(this Quest quest, string message, string inSignal = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			quest.AddPart(new QuestPart_Log
			{
				inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				signalListenMode = signalListenMode,
				message = message
			});
		}
	}
}
