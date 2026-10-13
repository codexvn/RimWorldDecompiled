using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BAC RID: 7084
	public class QuestPart_Filter_AnyPawnHasHediff : QuestPart_Filter
	{
		// Token: 0x06009DDD RID: 40413 RVA: 0x002DB680 File Offset: 0x002D9880
		protected override bool Pass(SignalArgs args)
		{
			if (this.pawns.NullOrEmpty<Pawn>())
			{
				return false;
			}
			foreach (Pawn pawn in this.pawns)
			{
				if (pawn != null && pawn.health.hediffSet.HasHediff(this.hediff, false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009DDE RID: 40414 RVA: 0x002DB700 File Offset: 0x002D9900
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			Pawn item;
			if (!this.inSignalRemovePawn.NullOrEmpty() && signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
		}

		// Token: 0x06009DDF RID: 40415 RVA: 0x002DB764 File Offset: 0x002D9964
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Defs.Look<HediffDef>(ref this.hediff, "hediff");
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04006D54 RID: 27988
		public List<Pawn> pawns;

		// Token: 0x04006D55 RID: 27989
		public HediffDef hediff;

		// Token: 0x04006D56 RID: 27990
		public string inSignalRemovePawn;
	}
}
