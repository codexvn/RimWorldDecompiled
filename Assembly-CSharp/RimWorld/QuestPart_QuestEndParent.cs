using System;

namespace RimWorld
{
	// Token: 0x02001B94 RID: 7060
	public class QuestPart_QuestEndParent : QuestPart_QuestEnd
	{
		// Token: 0x06009D93 RID: 40339 RVA: 0x002DAAC0 File Offset: 0x002D8CC0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag != this.inSignal)
			{
				return;
			}
			QuestEndOutcome outcome;
			if (this.outcome != null)
			{
				outcome = this.outcome.Value;
			}
			else if (!signal.args.TryGetArg<QuestEndOutcome>("OUTCOME", out outcome))
			{
				outcome = QuestEndOutcome.Unknown;
			}
			this.quest.parent.End(outcome, this.sendLetter, this.playSound);
		}
	}
}
