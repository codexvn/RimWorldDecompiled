using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B83 RID: 7043
	public class QuestPart_Pass : QuestPartCanDisable
	{
		// Token: 0x06009D4C RID: 40268 RVA: 0x002D9998 File Offset: 0x002D7B98
		protected override void ProcessQuestSignal(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				SignalArgs args = new SignalArgs(signal.args);
				if (this.outSignalOutcomeArg != null)
				{
					args.Add(this.outSignalOutcomeArg.Value.Named("OUTCOME"));
				}
				Find.SignalManager.SendSignal(new Signal(this.outSignal, args, false));
			}
		}

		// Token: 0x06009D4D RID: 40269 RVA: 0x002D9A0C File Offset: 0x002D7C0C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Values.Look<QuestEndOutcome?>(ref this.outSignalOutcomeArg, "outSignalOutcomeArg", null, false);
		}

		// Token: 0x06009D4E RID: 40270 RVA: 0x002D9A60 File Offset: 0x002D7C60
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.outSignal = "DebugSignal" + Rand.Int.ToString();
		}

		// Token: 0x04006D0C RID: 27916
		public string inSignal;

		// Token: 0x04006D0D RID: 27917
		public string outSignal;

		// Token: 0x04006D0E RID: 27918
		public QuestEndOutcome? outSignalOutcomeArg;
	}
}
