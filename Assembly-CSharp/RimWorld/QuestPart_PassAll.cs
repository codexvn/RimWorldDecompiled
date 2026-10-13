using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B85 RID: 7045
	public class QuestPart_PassAll : QuestPart
	{
		// Token: 0x17001836 RID: 6198
		// (get) Token: 0x06009D54 RID: 40276 RVA: 0x002D9B30 File Offset: 0x002D7D30
		private bool AllSignalsReceived
		{
			get
			{
				return PassAllQuestPartUtility.AllReceived(this.inSignals, this.signalsReceived);
			}
		}

		// Token: 0x06009D55 RID: 40277 RVA: 0x002D9B44 File Offset: 0x002D7D44
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
						Find.SignalManager.SendSignal(new Signal(this.outSignal, false));
					}
				}
			}
		}

		// Token: 0x06009D56 RID: 40278 RVA: 0x002D9BB8 File Offset: 0x002D7DB8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Collections.Look<bool>(ref this.signalsReceived, "signalsReceived", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06009D57 RID: 40279 RVA: 0x002D9C0C File Offset: 0x002D7E0C
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

		// Token: 0x04006D10 RID: 27920
		public List<string> inSignals = new List<string>();

		// Token: 0x04006D11 RID: 27921
		public string outSignal;

		// Token: 0x04006D12 RID: 27922
		private List<bool> signalsReceived = new List<bool>();
	}
}
