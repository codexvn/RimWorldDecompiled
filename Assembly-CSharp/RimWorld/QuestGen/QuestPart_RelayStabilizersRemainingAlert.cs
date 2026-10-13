using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B14 RID: 15124
	public class QuestPart_RelayStabilizersRemainingAlert : QuestPart_Alert
	{
		// Token: 0x06014940 RID: 84288 RVA: 0x00623C51 File Offset: 0x00621E51
		public QuestPart_RelayStabilizersRemainingAlert()
		{
		}

		// Token: 0x06014941 RID: 84289 RVA: 0x00623C64 File Offset: 0x00621E64
		public QuestPart_RelayStabilizersRemainingAlert(Site site, string inSignalEnable, string inSignalDisable, string inSignalStabilizerDealtWith, int stabilizerCount)
		{
			this.site = site;
			this.inSignalEnable = inSignalEnable;
			this.inSignalDisable = inSignalDisable;
			this.inSignalStabilizerDealtWith = inSignalStabilizerDealtWith;
			this.stabilizerCount = stabilizerCount;
			this.stabilizersRemaining = stabilizerCount;
		}

		// Token: 0x1700334C RID: 13132
		// (get) Token: 0x06014942 RID: 84290 RVA: 0x00623CA4 File Offset: 0x00621EA4
		public override string AlertLabel
		{
			get
			{
				return string.Format("{0}: {1}/{2}", "RelayStabilizersRemainingAlertLabel".Translate(), this.stabilizersRemaining, this.stabilizerCount);
			}
		}

		// Token: 0x1700334D RID: 13133
		// (get) Token: 0x06014943 RID: 84291 RVA: 0x00623CD5 File Offset: 0x00621ED5
		public override string AlertExplanation
		{
			get
			{
				return "RelayStabilizersRemainingAlertDescription".Translate();
			}
		}

		// Token: 0x1700334E RID: 13134
		// (get) Token: 0x06014944 RID: 84292 RVA: 0x00623CE6 File Offset: 0x00621EE6
		public override AlertReport AlertReport
		{
			get
			{
				if (this.stabilizersRemaining <= 0)
				{
					return AlertReport.Inactive;
				}
				return AlertReport.Active;
			}
		}

		// Token: 0x06014945 RID: 84293 RVA: 0x00623CFC File Offset: 0x00621EFC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			this.stabilizersRemaining = this.stabilizerCount;
			for (int i = 0; i < this.things.Count; i++)
			{
				if (this.things[i].DestroyedOrNull() || this.things[i].IsHacked())
				{
					this.stabilizersRemaining--;
				}
			}
		}

		// Token: 0x06014946 RID: 84294 RVA: 0x00623D68 File Offset: 0x00621F68
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
			Scribe_Values.Look<string>(ref this.inSignalStabilizerDealtWith, "inSignalStabilizerDealtWith", null, false);
			Scribe_Values.Look<int>(ref this.stabilizerCount, "stabilizerCount", 0, false);
			Scribe_Values.Look<int>(ref this.stabilizersRemaining, "stabilizersRemaining", 0, false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x0400E489 RID: 58505
		private Site site;

		// Token: 0x0400E48A RID: 58506
		private string inSignalStabilizerDealtWith;

		// Token: 0x0400E48B RID: 58507
		private int stabilizerCount;

		// Token: 0x0400E48C RID: 58508
		private int stabilizersRemaining;

		// Token: 0x0400E48D RID: 58509
		public List<Thing> things = new List<Thing>();
	}
}
