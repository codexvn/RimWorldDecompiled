using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A25 RID: 14885
	public class QuestPart_PawnsNoLongerRequiredForShuttleOnRescue : QuestPart
	{
		// Token: 0x06014627 RID: 83495 RVA: 0x006174A0 File Offset: 0x006156A0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignal && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				CompShuttle compShuttle = this.shuttle.TryGetComp<CompShuttle>();
				if (compShuttle != null)
				{
					compShuttle.requiredPawns.Remove(item);
				}
				this.pawns.Remove(item);
			}
		}

		// Token: 0x06014628 RID: 83496 RVA: 0x0061750C File Offset: 0x0061570C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0400E126 RID: 57638
		public string inSignal;

		// Token: 0x0400E127 RID: 57639
		public Thing shuttle;

		// Token: 0x0400E128 RID: 57640
		public List<Pawn> pawns = new List<Pawn>();
	}
}
