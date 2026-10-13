using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B8D RID: 7053
	public class QuestPart_PassAnyOutMany : QuestPart
	{
		// Token: 0x06009D77 RID: 40311 RVA: 0x002DA3AC File Offset: 0x002D85AC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (this.inSignals.Contains(signal.tag))
			{
				for (int i = 0; i < this.outSignals.Count; i++)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignals[i], signal.args, false));
				}
			}
		}

		// Token: 0x06009D78 RID: 40312 RVA: 0x002DA40B File Offset: 0x002D860B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.outSignals, "outSignals", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06009D79 RID: 40313 RVA: 0x002DA440 File Offset: 0x002D8640
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			this.outSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int.ToString());
				this.outSignals.Add("DebugSignal" + Rand.Int.ToString());
			}
		}

		// Token: 0x04006D22 RID: 27938
		public List<string> inSignals = new List<string>();

		// Token: 0x04006D23 RID: 27939
		public List<string> outSignals = new List<string>();
	}
}
