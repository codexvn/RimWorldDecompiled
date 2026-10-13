using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BAE RID: 7086
	public class QuestPart_Filter_AnyPawnPlayerControlled : QuestPart_Filter
	{
		// Token: 0x06009DE4 RID: 40420 RVA: 0x002DB7F0 File Offset: 0x002D99F0
		protected override bool Pass(SignalArgs args)
		{
			if (this.pawns.NullOrEmpty<Pawn>())
			{
				return false;
			}
			foreach (Pawn pawn in this.pawns)
			{
				if (!pawn.Destroyed && pawn.IsColonistPlayerControlled)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009DE5 RID: 40421 RVA: 0x002DB864 File Offset: 0x002D9A64
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x06009DE6 RID: 40422 RVA: 0x002DB8BC File Offset: 0x002D9ABC
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

		// Token: 0x04006D59 RID: 27993
		public List<Pawn> pawns;

		// Token: 0x04006D5A RID: 27994
		public string inSignalRemovePawn;
	}
}
