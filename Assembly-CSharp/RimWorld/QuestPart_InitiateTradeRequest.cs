using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C4C RID: 7244
	public class QuestPart_InitiateTradeRequest : QuestPart
	{
		// Token: 0x170018CA RID: 6346
		// (get) Token: 0x0600A145 RID: 41285 RVA: 0x002E60E8 File Offset: 0x002E42E8
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.settlement != null)
				{
					yield return this.settlement;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x170018CB RID: 6347
		// (get) Token: 0x0600A146 RID: 41286 RVA: 0x002E60F8 File Offset: 0x002E42F8
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				Settlement settlement = this.settlement;
				if (((settlement != null) ? settlement.Faction : null) != null)
				{
					yield return this.settlement.Faction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x170018CC RID: 6348
		// (get) Token: 0x0600A147 RID: 41287 RVA: 0x002E6108 File Offset: 0x002E4308
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				foreach (Dialog_InfoCard.Hyperlink hyperlink in base.Hyperlinks)
				{
					yield return hyperlink;
				}
				IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
				yield return new Dialog_InfoCard.Hyperlink(this.requestedThingDef, -1);
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A148 RID: 41288 RVA: 0x002E6118 File Offset: 0x002E4318
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				TradeRequestComp component = this.settlement.GetComponent<TradeRequestComp>();
				if (component != null)
				{
					if (component.ActiveRequest)
					{
						Log.Error("Settlement " + this.settlement.Label + " already has an active trade request.");
						return;
					}
					component.requestThingDef = this.requestedThingDef;
					component.requestCount = this.requestedCount;
					component.expiration = Find.TickManager.TicksGame + this.requestDuration;
				}
			}
		}

		// Token: 0x0600A149 RID: 41289 RVA: 0x002E61A8 File Offset: 0x002E43A8
		public override void Cleanup()
		{
			base.Cleanup();
			if (!this.keepAfterQuestEnds)
			{
				TradeRequestComp component = this.settlement.GetComponent<TradeRequestComp>();
				if (component != null && component.ActiveRequest)
				{
					component.Disable();
				}
			}
		}

		// Token: 0x0600A14A RID: 41290 RVA: 0x002E61E0 File Offset: 0x002E43E0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
			Scribe_Defs.Look<ThingDef>(ref this.requestedThingDef, "requestedThingDef");
			Scribe_Values.Look<int>(ref this.requestedCount, "requestedCount", 0, false);
			Scribe_Values.Look<int>(ref this.requestDuration, "requestDuration", 0, false);
			Scribe_Values.Look<bool>(ref this.keepAfterQuestEnds, "keepAfterQuestEnds", false, false);
		}

		// Token: 0x0600A14B RID: 41291 RVA: 0x002E625C File Offset: 0x002E445C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.settlement = Find.WorldObjects.Settlements.Where(delegate(Settlement x)
			{
				TradeRequestComp component = x.GetComponent<TradeRequestComp>();
				return component != null && !component.ActiveRequest && x.Faction != Faction.OfPlayer;
			}).RandomElementWithFallback(null);
			if (this.settlement == null)
			{
				this.settlement = Find.WorldObjects.Settlements.RandomElementWithFallback(null);
			}
			this.requestedThingDef = ThingDefOf.Silver;
			this.requestedCount = 100;
			this.requestDuration = 60000;
		}

		// Token: 0x04006F76 RID: 28534
		public string inSignal;

		// Token: 0x04006F77 RID: 28535
		public Settlement settlement;

		// Token: 0x04006F78 RID: 28536
		public ThingDef requestedThingDef;

		// Token: 0x04006F79 RID: 28537
		public int requestedCount;

		// Token: 0x04006F7A RID: 28538
		public int requestDuration;

		// Token: 0x04006F7B RID: 28539
		public bool keepAfterQuestEnds;
	}
}
