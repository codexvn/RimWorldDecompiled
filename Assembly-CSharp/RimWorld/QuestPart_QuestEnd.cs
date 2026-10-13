using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B93 RID: 7059
	public class QuestPart_QuestEnd : QuestPart
	{
		// Token: 0x06009D8F RID: 40335 RVA: 0x002DA9B8 File Offset: 0x002D8BB8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag != this.inSignal)
			{
				return;
			}
			QuestEndOutcome questEndOutcome;
			if (this.outcome != null)
			{
				questEndOutcome = this.outcome.Value;
			}
			else if (!signal.args.TryGetArg<QuestEndOutcome>("OUTCOME", out questEndOutcome))
			{
				questEndOutcome = QuestEndOutcome.Unknown;
			}
			this.quest.End(questEndOutcome, this.sendLetter, this.playSound);
		}

		// Token: 0x06009D90 RID: 40336 RVA: 0x002DAA2C File Offset: 0x002D8C2C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<QuestEndOutcome?>(ref this.outcome, "outcome", null, false);
			Scribe_Values.Look<bool>(ref this.sendLetter, "sendLetter", false, false);
			Scribe_Values.Look<bool>(ref this.playSound, "playSound", false, false);
		}

		// Token: 0x06009D91 RID: 40337 RVA: 0x002DAA90 File Offset: 0x002D8C90
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
		}

		// Token: 0x04006D2F RID: 27951
		public string inSignal;

		// Token: 0x04006D30 RID: 27952
		public QuestEndOutcome? outcome;

		// Token: 0x04006D31 RID: 27953
		public bool sendLetter;

		// Token: 0x04006D32 RID: 27954
		public bool playSound;
	}
}
