using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B91 RID: 7057
	public class QuestPart_PassWhileActive : QuestPartActivable
	{
		// Token: 0x06009D88 RID: 40328 RVA: 0x002DA895 File Offset: 0x002D8A95
		protected override void ProcessQuestSignal(Signal signal)
		{
			base.ProcessQuestSignal(signal);
			if (signal.tag == this.inSignal)
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignal, signal.args, false));
			}
		}

		// Token: 0x06009D89 RID: 40329 RVA: 0x002DA8CD File Offset: 0x002D8ACD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
		}

		// Token: 0x04006D2C RID: 27948
		public string inSignal;

		// Token: 0x04006D2D RID: 27949
		public string outSignal;
	}
}
