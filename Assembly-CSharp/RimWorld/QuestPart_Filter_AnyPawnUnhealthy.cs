using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BB0 RID: 7088
	public class QuestPart_Filter_AnyPawnUnhealthy : QuestPart_Filter
	{
		// Token: 0x06009DEB RID: 40427 RVA: 0x002DB938 File Offset: 0x002D9B38
		protected override bool Pass(SignalArgs args)
		{
			if (this.pawns.NullOrEmpty<Pawn>())
			{
				return false;
			}
			foreach (Pawn pawn in this.pawns)
			{
				if (pawn.Destroyed || pawn.InMentalState || pawn.health.hediffSet.BleedRateTotal > 0.001f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009DEC RID: 40428 RVA: 0x002DB9C4 File Offset: 0x002D9BC4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x06009DED RID: 40429 RVA: 0x002DBA1C File Offset: 0x002D9C1C
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

		// Token: 0x04006D5D RID: 27997
		public List<Pawn> pawns;

		// Token: 0x04006D5E RID: 27998
		public string inSignalRemovePawn;
	}
}
