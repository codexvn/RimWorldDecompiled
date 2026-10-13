using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B8F RID: 7055
	public class QuestPart_PassOutMany : QuestPartCanDisable
	{
		// Token: 0x06009D80 RID: 40320 RVA: 0x002DA6A8 File Offset: 0x002D88A8
		protected override void ProcessQuestSignal(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.outSignals.Count; i++)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignals[i], signal.args, false));
				}
			}
		}

		// Token: 0x06009D81 RID: 40321 RVA: 0x002DA700 File Offset: 0x002D8900
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<string>(ref this.outSignals, "outSignals", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06009D82 RID: 40322 RVA: 0x002DA730 File Offset: 0x002D8930
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			for (int i = 0; i < 3; i++)
			{
				this.outSignals.Add("DebugSignal" + Rand.Int.ToString());
			}
		}

		// Token: 0x04006D28 RID: 27944
		public string inSignal;

		// Token: 0x04006D29 RID: 27945
		public List<string> outSignals = new List<string>();
	}
}
