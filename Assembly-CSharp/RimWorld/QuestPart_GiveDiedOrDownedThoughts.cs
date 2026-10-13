using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BC9 RID: 7113
	public class QuestPart_GiveDiedOrDownedThoughts : QuestPart
	{
		// Token: 0x06009E5F RID: 40543 RVA: 0x002DCFA0 File Offset: 0x002DB1A0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(this.aboutPawn, null, this.thoughtsKind);
			}
		}

		// Token: 0x06009E60 RID: 40544 RVA: 0x002DCFDA File Offset: 0x002DB1DA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.aboutPawn, "aboutPawn", false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<PawnDiedOrDownedThoughtsKind>(ref this.thoughtsKind, "thoughtsKind", PawnDiedOrDownedThoughtsKind.Died, false);
		}

		// Token: 0x04006DA1 RID: 28065
		public Pawn aboutPawn;

		// Token: 0x04006DA2 RID: 28066
		public string inSignal;

		// Token: 0x04006DA3 RID: 28067
		public PawnDiedOrDownedThoughtsKind thoughtsKind;
	}
}
