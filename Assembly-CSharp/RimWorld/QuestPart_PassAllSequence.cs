using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B8A RID: 7050
	public class QuestPart_PassAllSequence : QuestPart
	{
		// Token: 0x1700183B RID: 6203
		// (get) Token: 0x06009D6A RID: 40298 RVA: 0x002DA0DB File Offset: 0x002D82DB
		private bool AllSignalsReceived
		{
			get
			{
				return this.ptr >= this.inSignals.Count - 1;
			}
		}

		// Token: 0x06009D6B RID: 40299 RVA: 0x002DA0F8 File Offset: 0x002D82F8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (!this.AllSignalsReceived && this.inSignals.IndexOf(signal.tag) == this.ptr + 1)
			{
				this.ptr++;
				if (this.AllSignalsReceived)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignal, false));
				}
			}
		}

		// Token: 0x06009D6C RID: 40300 RVA: 0x002DA154 File Offset: 0x002D8354
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Values.Look<int>(ref this.ptr, "ptr", 0, false);
		}

		// Token: 0x06009D6D RID: 40301 RVA: 0x002DA1A4 File Offset: 0x002D83A4
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

		// Token: 0x04006D1C RID: 27932
		public List<string> inSignals = new List<string>();

		// Token: 0x04006D1D RID: 27933
		public string outSignal;

		// Token: 0x04006D1E RID: 27934
		private int ptr = -1;
	}
}
