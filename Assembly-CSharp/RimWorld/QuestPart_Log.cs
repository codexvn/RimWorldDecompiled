using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BEA RID: 7146
	public class QuestPart_Log : QuestPart
	{
		// Token: 0x06009F33 RID: 40755 RVA: 0x002DF937 File Offset: 0x002DDB37
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				Log.Message(signal.args.GetFormattedText(this.message));
			}
		}

		// Token: 0x06009F34 RID: 40756 RVA: 0x002DF974 File Offset: 0x002DDB74
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.message, "message", null, false);
		}

		// Token: 0x06009F35 RID: 40757 RVA: 0x002DF9A0 File Offset: 0x002DDBA0
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.message = "DEV: Test";
		}

		// Token: 0x04006E13 RID: 28179
		public string inSignal;

		// Token: 0x04006E14 RID: 28180
		public string message;
	}
}
