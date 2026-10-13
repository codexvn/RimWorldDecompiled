using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CEC RID: 7404
	public class QuestPart_GiveTechprints : QuestPart
	{
		// Token: 0x0600A54C RID: 42316 RVA: 0x002F5714 File Offset: 0x002F3914
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.amount; i++)
				{
					Find.ResearchManager.ApplyTechprint(this.project, null);
				}
			}
		}

		// Token: 0x0600A54D RID: 42317 RVA: 0x002F5760 File Offset: 0x002F3960
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ResearchProjectDef>(ref this.project, "project");
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalWasGiven, "outSignalWasGiven", null, false);
		}

		// Token: 0x04007232 RID: 29234
		public const string WasGivenSignal = "AddedTechprints";

		// Token: 0x04007233 RID: 29235
		public string inSignal;

		// Token: 0x04007234 RID: 29236
		public string outSignalWasGiven;

		// Token: 0x04007235 RID: 29237
		public ResearchProjectDef project;

		// Token: 0x04007236 RID: 29238
		public int amount;
	}
}
