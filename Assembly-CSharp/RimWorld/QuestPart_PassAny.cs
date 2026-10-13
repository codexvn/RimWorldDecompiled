using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B8B RID: 7051
	public class QuestPart_PassAny : QuestPart
	{
		// Token: 0x06009D6F RID: 40303 RVA: 0x002DA227 File Offset: 0x002D8427
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (this.inSignals.Contains(signal.tag))
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignal, signal.args, false));
			}
		}

		// Token: 0x06009D70 RID: 40304 RVA: 0x002DA25F File Offset: 0x002D845F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
		}

		// Token: 0x06009D71 RID: 40305 RVA: 0x002DA290 File Offset: 0x002D8490
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int.ToString());
			}
			this.outSignal = "DebugSignal" + Rand.Int.ToString();
		}

		// Token: 0x04006D1F RID: 27935
		public List<string> inSignals = new List<string>();

		// Token: 0x04006D20 RID: 27936
		public string outSignal;
	}
}
