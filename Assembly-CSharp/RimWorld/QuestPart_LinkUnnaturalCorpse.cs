using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BD1 RID: 7121
	public class QuestPart_LinkUnnaturalCorpse : QuestPart
	{
		// Token: 0x06009E90 RID: 40592 RVA: 0x002DDA67 File Offset: 0x002DBC67
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal && !this.aboutPawn.DestroyedOrNull())
			{
				Find.Anomaly.RegisterUnnaturalCorpse(this.aboutPawn, this.corpse);
			}
		}

		// Token: 0x06009E91 RID: 40593 RVA: 0x002DDA9F File Offset: 0x002DBC9F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.aboutPawn, "aboutPawn", false);
			Scribe_References.Look<UnnaturalCorpse>(ref this.corpse, "corpse", false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
		}

		// Token: 0x04006DBF RID: 28095
		public Pawn aboutPawn;

		// Token: 0x04006DC0 RID: 28096
		public UnnaturalCorpse corpse;

		// Token: 0x04006DC1 RID: 28097
		public string inSignal;
	}
}
