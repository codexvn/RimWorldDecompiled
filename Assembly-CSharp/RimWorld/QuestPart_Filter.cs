using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B95 RID: 7061
	public abstract class QuestPart_Filter : QuestPart
	{
		// Token: 0x06009D95 RID: 40341
		protected abstract bool Pass(SignalArgs args);

		// Token: 0x06009D96 RID: 40342 RVA: 0x002DAB38 File Offset: 0x002D8D38
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				if (this.Pass(signal.args))
				{
					if (!this.outSignal.NullOrEmpty())
					{
						Find.SignalManager.SendSignal(new Signal(this.outSignal, signal.args, false));
						return;
					}
				}
				else if (!this.outSignalElse.NullOrEmpty())
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalElse, signal.args, false));
				}
			}
		}

		// Token: 0x06009D97 RID: 40343 RVA: 0x002DABC0 File Offset: 0x002D8DC0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalElse, "outSignalElse", null, false);
		}

		// Token: 0x06009D98 RID: 40344 RVA: 0x002DAC00 File Offset: 0x002D8E00
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.outSignal = "DebugSignal" + Rand.Int.ToString();
		}

		// Token: 0x04006D33 RID: 27955
		public string inSignal;

		// Token: 0x04006D34 RID: 27956
		public string outSignal;

		// Token: 0x04006D35 RID: 27957
		public string outSignalElse;
	}
}
