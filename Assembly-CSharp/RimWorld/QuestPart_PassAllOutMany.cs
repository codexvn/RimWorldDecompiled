using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B89 RID: 7049
	public class QuestPart_PassAllOutMany : QuestPart
	{
		// Token: 0x1700183A RID: 6202
		// (get) Token: 0x06009D65 RID: 40293 RVA: 0x002D9F46 File Offset: 0x002D8146
		private bool AllSignalsReceived
		{
			get
			{
				return PassAllQuestPartUtility.AllReceived(this.inSignals, this.signalsReceived);
			}
		}

		// Token: 0x06009D66 RID: 40294 RVA: 0x002D9F5C File Offset: 0x002D815C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (!this.AllSignalsReceived)
			{
				int num = this.inSignals.IndexOf(signal.tag);
				if (num >= 0)
				{
					while (this.signalsReceived.Count <= num)
					{
						this.signalsReceived.Add(false);
					}
					this.signalsReceived[num] = true;
					if (this.AllSignalsReceived)
					{
						for (int i = 0; i < this.outSignals.Count; i++)
						{
							Find.SignalManager.SendSignal(new Signal(this.outSignals[i], false));
						}
					}
				}
			}
		}

		// Token: 0x06009D67 RID: 40295 RVA: 0x002D9FEC File Offset: 0x002D81EC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.outSignals, "outSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<bool>(ref this.signalsReceived, "signalsReceived", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06009D68 RID: 40296 RVA: 0x002DA044 File Offset: 0x002D8244
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int.ToString());
				this.outSignals.Add("DebugSignal" + Rand.Int.ToString());
			}
		}

		// Token: 0x04006D19 RID: 27929
		public List<string> inSignals = new List<string>();

		// Token: 0x04006D1A RID: 27930
		public List<string> outSignals = new List<string>();

		// Token: 0x04006D1B RID: 27931
		private List<bool> signalsReceived = new List<bool>();
	}
}
