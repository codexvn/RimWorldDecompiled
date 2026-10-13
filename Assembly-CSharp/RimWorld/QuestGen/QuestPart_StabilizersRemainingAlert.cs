using System;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B18 RID: 15128
	public class QuestPart_StabilizersRemainingAlert : QuestPart_Alert
	{
		// Token: 0x17003350 RID: 13136
		// (get) Token: 0x06014953 RID: 84307 RVA: 0x0062421C File Offset: 0x0062241C
		private CompCerebrexCore CoreComp
		{
			get
			{
				if (this.coreComp == null)
				{
					this.coreComp = this.core.TryGetComp<CompCerebrexCore>();
				}
				return this.coreComp;
			}
		}

		// Token: 0x06014954 RID: 84308 RVA: 0x0062423D File Offset: 0x0062243D
		public QuestPart_StabilizersRemainingAlert()
		{
		}

		// Token: 0x06014955 RID: 84309 RVA: 0x00624245 File Offset: 0x00622445
		public QuestPart_StabilizersRemainingAlert(Site site, string inSignalEnable, string inSignalDisable)
		{
			this.site = site;
			this.inSignalEnable = inSignalEnable;
			this.inSignalDisable = inSignalDisable;
		}

		// Token: 0x17003351 RID: 13137
		// (get) Token: 0x06014956 RID: 84310 RVA: 0x00624262 File Offset: 0x00622462
		public override string AlertLabel
		{
			get
			{
				return string.Format("{0}: {1}/{2}", "StabilizersRemainingAlertLabel".Translate(), this.CoreComp.stabilizersRemaining, 3);
			}
		}

		// Token: 0x17003352 RID: 13138
		// (get) Token: 0x06014957 RID: 84311 RVA: 0x00624293 File Offset: 0x00622493
		public override string AlertExplanation
		{
			get
			{
				return "StabilizersRemainingAlertDescription".Translate();
			}
		}

		// Token: 0x17003353 RID: 13139
		// (get) Token: 0x06014958 RID: 84312 RVA: 0x006242A4 File Offset: 0x006224A4
		public override AlertReport AlertReport
		{
			get
			{
				CompCerebrexCore compCerebrexCore = this.CoreComp;
				if (compCerebrexCore == null || compCerebrexCore.stabilizersRemaining <= 0)
				{
					return AlertReport.Inactive;
				}
				return AlertReport.Active;
			}
		}

		// Token: 0x06014959 RID: 84313 RVA: 0x006242C8 File Offset: 0x006224C8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignalEnable)
			{
				this.core = this.site.Map.listerThings.ThingsOfDef(ThingDefOf.CerebrexCore).FirstOrDefault<Thing>();
			}
		}

		// Token: 0x0601495A RID: 84314 RVA: 0x00624314 File Offset: 0x00622514
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
			Scribe_References.Look<Thing>(ref this.core, "core", false);
		}

		// Token: 0x0400E495 RID: 58517
		private Site site;

		// Token: 0x0400E496 RID: 58518
		private Thing core;

		// Token: 0x0400E497 RID: 58519
		private CompCerebrexCore coreComp;
	}
}
