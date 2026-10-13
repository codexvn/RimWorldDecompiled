using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BA8 RID: 7080
	public abstract class QuestPart_Filter_AnyPawn : QuestPart_Filter
	{
		// Token: 0x1700183C RID: 6204
		// (get) Token: 0x06009DD1 RID: 40401
		protected abstract int PawnsCount { get; }

		// Token: 0x06009DD2 RID: 40402 RVA: 0x002DB4B9 File Offset: 0x002D96B9
		protected override bool Pass(SignalArgs args)
		{
			return this.PawnsCount > 0;
		}

		// Token: 0x06009DD3 RID: 40403 RVA: 0x002DB4C4 File Offset: 0x002D96C4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			if (signal.tag == this.inSignal)
			{
				signal.args.Add(this.PawnsCount.Named("PAWNSALIVECOUNT"));
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x06009DD4 RID: 40404 RVA: 0x002DB550 File Offset: 0x002D9750
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04006D50 RID: 27984
		public List<Pawn> pawns;

		// Token: 0x04006D51 RID: 27985
		public string inSignalRemovePawn;
	}
}
