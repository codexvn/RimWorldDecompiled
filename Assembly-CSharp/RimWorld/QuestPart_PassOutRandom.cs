using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B90 RID: 7056
	public class QuestPart_PassOutRandom : QuestPart
	{
		// Token: 0x06009D84 RID: 40324 RVA: 0x002DA7A4 File Offset: 0x002D89A4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			string tag;
			if (signal.tag == this.inSignal && this.outSignals.TryRandomElement(out tag))
			{
				Find.SignalManager.SendSignal(new Signal(tag, signal.args, false));
			}
		}

		// Token: 0x06009D85 RID: 40325 RVA: 0x002DA7F1 File Offset: 0x002D89F1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<string>(ref this.outSignals, "outSignals", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06009D86 RID: 40326 RVA: 0x002DA824 File Offset: 0x002D8A24
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			for (int i = 0; i < 3; i++)
			{
				this.outSignals.Add("DebugSignal" + Rand.Int.ToString());
			}
		}

		// Token: 0x04006D2A RID: 27946
		public string inSignal;

		// Token: 0x04006D2B RID: 27947
		public List<string> outSignals = new List<string>();
	}
}
