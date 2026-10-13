using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CDF RID: 7391
	[StaticConstructorOnStartup]
	public class QuestPart_FactionGoodwillChange : QuestPart
	{
		// Token: 0x17001968 RID: 6504
		// (get) Token: 0x0600A4D6 RID: 42198 RVA: 0x002F38C7 File Offset: 0x002F1AC7
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				yield return this.lookTarget;
				yield break;
				yield break;
			}
		}

		// Token: 0x17001969 RID: 6505
		// (get) Token: 0x0600A4D7 RID: 42199 RVA: 0x002F38D7 File Offset: 0x002F1AD7
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

		// Token: 0x0600A4D8 RID: 42200 RVA: 0x002F38E8 File Offset: 0x002F1AE8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.faction != null && this.faction != Faction.OfPlayer)
			{
				if (this.lookTarget.IsValid)
				{
					GlobalTargetInfo globalTargetInfo = this.lookTarget;
				}
				else if (this.getLookTargetFromSignal)
				{
					LookTargets lookTargets;
					if (SignalArgsUtility.TryGetLookTargets(signal.args, "SUBJECT", out lookTargets))
					{
						lookTargets.TryGetPrimaryTarget();
					}
					else
					{
						GlobalTargetInfo invalid = GlobalTargetInfo.Invalid;
					}
				}
				else
				{
					GlobalTargetInfo invalid2 = GlobalTargetInfo.Invalid;
				}
				FactionRelationKind playerRelationKind = this.faction.PlayerRelationKind;
				int num = 0;
				if (!signal.args.TryGetArg<int>("GOODWILL", out num))
				{
					num = this.change;
				}
				if (this.ensureMakesHostile)
				{
					num = Mathf.Min(num, Faction.OfPlayer.GoodwillToMakeHostile(this.faction));
				}
				Faction.OfPlayer.TryAffectGoodwillWith(this.faction, num, this.canSendMessage, this.canSendHostilityLetter, (num >= 0) ? (this.historyEvent ?? HistoryEventDefOf.QuestGoodwillReward) : this.historyEvent, null);
				TaggedString t = "";
				this.faction.TryAppendRelationKindChangedInfo(ref t, playerRelationKind, this.faction.PlayerRelationKind, null);
				if (!t.NullOrEmpty())
				{
					t = "\n\n" + t;
				}
			}
		}

		// Token: 0x0600A4D9 RID: 42201 RVA: 0x002F3A3C File Offset: 0x002F1C3C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<HistoryEventDef>(ref this.historyEvent, "historyEvent");
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<int>(ref this.change, "change", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.canSendMessage, "canSendMessage", true, false);
			Scribe_Values.Look<bool>(ref this.canSendHostilityLetter, "canSendHostilityLetter", true, false);
			Scribe_Values.Look<bool>(ref this.getLookTargetFromSignal, "getLookTargetFromSignal", true, false);
			Scribe_TargetInfo.Look(ref this.lookTarget, "lookTarget");
			Scribe_Values.Look<bool>(ref this.ensureMakesHostile, "ensureMakesHostile", false, false);
		}

		// Token: 0x0600A4DA RID: 42202 RVA: 0x002F3AEC File Offset: 0x002F1CEC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.change = -15;
			this.faction = Find.FactionManager.RandomNonHostileFaction(false, false, false, TechLevel.Undefined);
		}

		// Token: 0x040071D9 RID: 29145
		public HistoryEventDef historyEvent;

		// Token: 0x040071DA RID: 29146
		public string inSignal;

		// Token: 0x040071DB RID: 29147
		public int change;

		// Token: 0x040071DC RID: 29148
		public Faction faction;

		// Token: 0x040071DD RID: 29149
		public bool canSendMessage = true;

		// Token: 0x040071DE RID: 29150
		public bool canSendHostilityLetter = true;

		// Token: 0x040071DF RID: 29151
		public bool getLookTargetFromSignal = true;

		// Token: 0x040071E0 RID: 29152
		public GlobalTargetInfo lookTarget;

		// Token: 0x040071E1 RID: 29153
		public bool ensureMakesHostile;
	}
}
