using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CE2 RID: 7394
	public class QuestPart_FactionGoodwillChange_ShuttleSentThings : QuestPartActivable
	{
		// Token: 0x1700196E RID: 6510
		// (get) Token: 0x0600A4F0 RID: 42224 RVA: 0x002F3E93 File Offset: 0x002F2093
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.faction != null)
				{
					yield return this.faction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x1700196F RID: 6511
		// (get) Token: 0x0600A4F1 RID: 42225 RVA: 0x002F3EA3 File Offset: 0x002F20A3
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				foreach (Thing t in this.things)
				{
					yield return t;
				}
				List<Thing>.Enumerator enumerator2 = default(List<Thing>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A4F2 RID: 42226 RVA: 0x002F3EB4 File Offset: 0x002F20B4
		protected override void ProcessQuestSignal(Signal signal)
		{
			base.ProcessQuestSignal(signal);
			List<Thing> list;
			if (this.inSignalsShuttleSent.Contains(signal.tag) && signal.args.TryGetArg<List<Thing>>("SENT", out list))
			{
				int num = 0;
				for (int i = 0; i < this.things.Count; i++)
				{
					if (!list.Contains(this.things[i]))
					{
						num++;
					}
				}
				this.TryAffectGoodwill(num * this.changeNotOnShuttle);
				base.Complete();
			}
			if (signal.tag == this.inSignalShuttleDestroyed)
			{
				this.TryAffectGoodwill(this.things.Count * this.changeNotOnShuttle);
				base.Complete();
			}
		}

		// Token: 0x0600A4F3 RID: 42227 RVA: 0x002F3F65 File Offset: 0x002F2165
		public override void Cleanup()
		{
			if (base.State == QuestPartState.Enabled)
			{
				this.TryAffectGoodwill(this.things.Count * this.changeNotOnShuttle);
			}
		}

		// Token: 0x0600A4F4 RID: 42228 RVA: 0x002F3F88 File Offset: 0x002F2188
		private void TryAffectGoodwill(int goodwillChange)
		{
			if (goodwillChange != 0)
			{
				Faction.OfPlayer.TryAffectGoodwillWith(this.faction, goodwillChange, this.canSendMessage, this.canSendHostilityLetter, this.historyEvent, null);
			}
		}

		// Token: 0x0600A4F5 RID: 42229 RVA: 0x002F3FC8 File Offset: 0x002F21C8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignalsShuttleSent, "inSignalsShuttleSent", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.changeNotOnShuttle, "changeNotOnShuttle", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.canSendMessage, "canSendMessage", true, false);
			Scribe_Values.Look<bool>(ref this.canSendHostilityLetter, "canSendHostilityLetter", true, false);
			Scribe_Values.Look<string>(ref this.reason, "reason", null, false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignalShuttleDestroyed, "inSignalShuttleDestroyed", null, false);
			Scribe_Defs.Look<HistoryEventDef>(ref this.historyEvent, "historyEvent");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x040071EC RID: 29164
		public List<string> inSignalsShuttleSent = new List<string>();

		// Token: 0x040071ED RID: 29165
		public string inSignalShuttleDestroyed;

		// Token: 0x040071EE RID: 29166
		public int changeNotOnShuttle;

		// Token: 0x040071EF RID: 29167
		public Faction faction;

		// Token: 0x040071F0 RID: 29168
		public bool canSendMessage = true;

		// Token: 0x040071F1 RID: 29169
		public bool canSendHostilityLetter = true;

		// Token: 0x040071F2 RID: 29170
		public string reason;

		// Token: 0x040071F3 RID: 29171
		public List<Thing> things = new List<Thing>();

		// Token: 0x040071F4 RID: 29172
		public HistoryEventDef historyEvent;
	}
}
