using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B84 RID: 7044
	public class QuestPart_PassActivable : QuestPartActivable
	{
		// Token: 0x06009D50 RID: 40272 RVA: 0x002D9AB5 File Offset: 0x002D7CB5
		protected override void ProcessQuestSignal(Signal signal)
		{
			base.ProcessQuestSignal(signal);
			if (signal.tag == this.inSignal)
			{
				this.Complete(signal.args);
			}
		}

		// Token: 0x06009D51 RID: 40273 RVA: 0x002D9ADD File Offset: 0x002D7CDD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
		}

		// Token: 0x06009D52 RID: 40274 RVA: 0x002D9AF8 File Offset: 0x002D7CF8
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
		}

		// Token: 0x04006D0F RID: 27919
		public string inSignal;
	}
}
