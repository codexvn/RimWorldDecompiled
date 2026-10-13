using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AB5 RID: 15029
	public class QuestPart_ChangeGoodwillForAlivePawnsMissingFromShuttle : QuestPart
	{
		// Token: 0x0601480C RID: 83980 RVA: 0x0061EACC File Offset: 0x0061CCCC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			IEnumerable<Thing> sentThings;
			if (signal.tag == this.inSignal && signal.args.TryGetArg<IEnumerable<Thing>>("SENT", out sentThings))
			{
				this.DoWork(sentThings);
			}
		}

		// Token: 0x0601480D RID: 83981 RVA: 0x0061EB08 File Offset: 0x0061CD08
		private void DoWork(IEnumerable<Thing> sentThings)
		{
			int num = 0;
			using (IEnumerator<Thing> enumerator = sentThings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current is Pawn)
					{
						num++;
					}
				}
			}
			int num2 = 0;
			using (List<Pawn>.Enumerator enumerator2 = this.pawns.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (!enumerator2.Current.Dead)
					{
						num2++;
					}
				}
			}
			if (num < num2)
			{
				Faction.OfPlayer.TryAffectGoodwillWith(this.faction, this.goodwillChange * (num2 - num), true, true, this.historyEvent, null);
			}
		}

		// Token: 0x0601480E RID: 83982 RVA: 0x0061EBD0 File Offset: 0x0061CDD0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<int>(ref this.goodwillChange, "goodwillChange", 0, false);
			Scribe_Defs.Look<HistoryEventDef>(ref this.historyEvent, "historyEvent");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0400E319 RID: 58137
		public string inSignal;

		// Token: 0x0400E31A RID: 58138
		public List<Pawn> pawns;

		// Token: 0x0400E31B RID: 58139
		public Faction faction;

		// Token: 0x0400E31C RID: 58140
		public int goodwillChange;

		// Token: 0x0400E31D RID: 58141
		public HistoryEventDef historyEvent;
	}
}
