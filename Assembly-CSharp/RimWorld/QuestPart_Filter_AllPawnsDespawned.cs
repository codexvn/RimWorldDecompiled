using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B97 RID: 7063
	public class QuestPart_Filter_AllPawnsDespawned : QuestPart_Filter
	{
		// Token: 0x06009D9D RID: 40349 RVA: 0x002DAC84 File Offset: 0x002D8E84
		protected override bool Pass(SignalArgs args)
		{
			if (this.pawns.NullOrEmpty<Pawn>())
			{
				return false;
			}
			using (List<Pawn>.Enumerator enumerator = this.pawns.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Spawned)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06009D9E RID: 40350 RVA: 0x002DACEC File Offset: 0x002D8EEC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x06009D9F RID: 40351 RVA: 0x002DAD44 File Offset: 0x002D8F44
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

		// Token: 0x04006D37 RID: 27959
		public List<Pawn> pawns;

		// Token: 0x04006D38 RID: 27960
		public string inSignalRemovePawn;
	}
}
