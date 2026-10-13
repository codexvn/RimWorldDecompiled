using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BCA RID: 7114
	public class QuestPart_GiveHediff : QuestPart
	{
		// Token: 0x06009E62 RID: 40546 RVA: 0x002DD018 File Offset: 0x002DB218
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal && !this.aboutPawn.DestroyedOrNull())
			{
				this.aboutPawn.health.AddHediff(this.hediff, null, null, null);
			}
		}

		// Token: 0x06009E63 RID: 40547 RVA: 0x002DD067 File Offset: 0x002DB267
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.aboutPawn, "aboutPawn", false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Defs.Look<HediffDef>(ref this.hediff, "hediff");
		}

		// Token: 0x04006DA4 RID: 28068
		public Pawn aboutPawn;

		// Token: 0x04006DA5 RID: 28069
		public string inSignal;

		// Token: 0x04006DA6 RID: 28070
		public HediffDef hediff;
	}
}
