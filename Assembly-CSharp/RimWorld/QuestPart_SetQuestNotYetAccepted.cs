using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B7E RID: 7038
	public class QuestPart_SetQuestNotYetAccepted : QuestPart
	{
		// Token: 0x06009D27 RID: 40231 RVA: 0x002D9224 File Offset: 0x002D7424
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.quest.SetNotYetAccepted();
			}
		}

		// Token: 0x06009D28 RID: 40232 RVA: 0x002D924C File Offset: 0x002D744C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.revertOngoingQuestAfterLoad, "revertOngoingQuestAfterLoad", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.revertOngoingQuestAfterLoad && this.quest.State == QuestState.Ongoing)
			{
				this.quest.SetNotYetAccepted();
			}
		}

		// Token: 0x06009D29 RID: 40233 RVA: 0x002D92AC File Offset: 0x002D74AC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
		}

		// Token: 0x04006CF6 RID: 27894
		public string inSignal;

		// Token: 0x04006CF7 RID: 27895
		public bool revertOngoingQuestAfterLoad;
	}
}
