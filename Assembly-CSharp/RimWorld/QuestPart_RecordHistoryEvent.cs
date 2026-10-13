using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C7B RID: 7291
	public class QuestPart_RecordHistoryEvent : QuestPart
	{
		// Token: 0x0600A27E RID: 41598 RVA: 0x002EB1BE File Offset: 0x002E93BE
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(this.historyEvent), true);
			}
		}

		// Token: 0x0600A27F RID: 41599 RVA: 0x002EB1F0 File Offset: 0x002E93F0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Defs.Look<HistoryEventDef>(ref this.historyEvent, "historyEvent");
		}

		// Token: 0x04007049 RID: 28745
		public string inSignal;

		// Token: 0x0400704A RID: 28746
		public HistoryEventDef historyEvent;
	}
}
