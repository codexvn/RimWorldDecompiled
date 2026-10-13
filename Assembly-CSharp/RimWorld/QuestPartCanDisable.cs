using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CCE RID: 7374
	public abstract class QuestPartCanDisable : QuestPart
	{
		// Token: 0x0600A46B RID: 42091 RVA: 0x002F1778 File Offset: 0x002EF978
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (this.state != QuestPartState.Enabled)
			{
				return;
			}
			if (signal.tag == this.inSignalDisable)
			{
				this.Disable();
				return;
			}
			this.ProcessQuestSignal(signal);
		}

		// Token: 0x0600A46C RID: 42092 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void ProcessQuestSignal(Signal signal)
		{
		}

		// Token: 0x0600A46D RID: 42093 RVA: 0x002F17A5 File Offset: 0x002EF9A5
		public void Disable()
		{
			this.state = QuestPartState.Disabled;
		}

		// Token: 0x0600A46E RID: 42094 RVA: 0x002F17AE File Offset: 0x002EF9AE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignalDisable, "inSignalDisable", null, false);
			Scribe_Values.Look<QuestPartState>(ref this.state, "state", QuestPartState.NeverEnabled, false);
		}

		// Token: 0x0400717C RID: 29052
		public string inSignalDisable;

		// Token: 0x0400717D RID: 29053
		public QuestPartState state = QuestPartState.Enabled;
	}
}
