using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C5A RID: 7258
	public class QuestPart_LeavePlayer : QuestPart
	{
		// Token: 0x0600A1A9 RID: 41385 RVA: 0x002E76EC File Offset: 0x002E58EC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i].Faction == Faction.OfPlayer)
					{
						this.pawns[i].SetFaction(this.replacementFaction, null);
					}
				}
			}
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
		}

		// Token: 0x0600A1AA RID: 41386 RVA: 0x002E77C7 File Offset: 0x002E59C7
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.replacementFaction == faction)
			{
				this.replacementFaction = null;
			}
		}

		// Token: 0x0600A1AB RID: 41387 RVA: 0x002E77D9 File Offset: 0x002E59D9
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x0600A1AC RID: 41388 RVA: 0x002E77E7 File Offset: 0x002E59E7
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x0600A1AD RID: 41389 RVA: 0x002E77F8 File Offset: 0x002E59F8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.replacementFaction, "replacementFaction", false);
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04006FB1 RID: 28593
		public string inSignal;

		// Token: 0x04006FB2 RID: 28594
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04006FB3 RID: 28595
		public Faction replacementFaction;

		// Token: 0x04006FB4 RID: 28596
		public string inSignalRemovePawn;
	}
}
