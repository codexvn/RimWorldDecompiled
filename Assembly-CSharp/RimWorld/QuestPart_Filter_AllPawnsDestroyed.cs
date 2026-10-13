using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B99 RID: 7065
	public class QuestPart_Filter_AllPawnsDestroyed : QuestPart_Filter
	{
		// Token: 0x06009DA4 RID: 40356 RVA: 0x002DADC0 File Offset: 0x002D8FC0
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
					if (!enumerator.Current.Destroyed)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06009DA5 RID: 40357 RVA: 0x002DAE28 File Offset: 0x002D9028
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x06009DA6 RID: 40358 RVA: 0x002DAE80 File Offset: 0x002D9080
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

		// Token: 0x04006D3B RID: 27963
		public List<Pawn> pawns;

		// Token: 0x04006D3C RID: 27964
		public string inSignalRemovePawn;
	}
}
