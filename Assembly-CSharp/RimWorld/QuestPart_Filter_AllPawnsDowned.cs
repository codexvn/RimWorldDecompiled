using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B9B RID: 7067
	public class QuestPart_Filter_AllPawnsDowned : QuestPart_Filter
	{
		// Token: 0x06009DAB RID: 40363 RVA: 0x002DAEFC File Offset: 0x002D90FC
		protected override bool Pass(SignalArgs args)
		{
			if (this.pawns.NullOrEmpty<Pawn>())
			{
				return false;
			}
			foreach (Pawn pawn in this.pawns)
			{
				if (!pawn.Dead && !pawn.Downed)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06009DAC RID: 40364 RVA: 0x002DAF70 File Offset: 0x002D9170
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x06009DAD RID: 40365 RVA: 0x002DAFC8 File Offset: 0x002D91C8
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

		// Token: 0x04006D3F RID: 27967
		public List<Pawn> pawns;

		// Token: 0x04006D40 RID: 27968
		public string inSignalRemovePawn;
	}
}
