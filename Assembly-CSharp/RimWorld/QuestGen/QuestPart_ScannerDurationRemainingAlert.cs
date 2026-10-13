using System;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B9F RID: 15263
	public class QuestPart_ScannerDurationRemainingAlert : QuestPart_Alert
	{
		// Token: 0x06014B9F RID: 84895 RVA: 0x0062423D File Offset: 0x0062243D
		public QuestPart_ScannerDurationRemainingAlert()
		{
		}

		// Token: 0x06014BA0 RID: 84896 RVA: 0x00635117 File Offset: 0x00633317
		public QuestPart_ScannerDurationRemainingAlert(Site site, string inSignalEnable, string inSignalDisable, string completedSignal, string raidSignal, int duration, int fireRaidRemainingTicks)
		{
			this.site = site;
			this.inSignalEnable = inSignalEnable;
			this.inSignalDisable = inSignalDisable;
			this.completedSignal = completedSignal;
			this.raidSignal = raidSignal;
			this.duration = duration;
			this.fireRaidRemainingTicks = fireRaidRemainingTicks;
		}

		// Token: 0x17003371 RID: 13169
		// (get) Token: 0x06014BA1 RID: 84897 RVA: 0x00635154 File Offset: 0x00633354
		public override string AlertLabel
		{
			get
			{
				return string.Format("{0}: {1}", "SurveyScannerAlertLabel".Translate(), Mathf.Max(this.endTick - GenTicks.TicksGame, 0).ToStringTicksToPeriod(true, false, true, true, false));
			}
		}

		// Token: 0x17003372 RID: 13170
		// (get) Token: 0x06014BA2 RID: 84898 RVA: 0x0063518B File Offset: 0x0063338B
		public override string AlertExplanation
		{
			get
			{
				return "SurveyScannerAlertDescription".Translate();
			}
		}

		// Token: 0x17003373 RID: 13171
		// (get) Token: 0x06014BA3 RID: 84899 RVA: 0x0063519C File Offset: 0x0063339C
		public override AlertReport AlertReport
		{
			get
			{
				return base.State == QuestPartState.Enabled;
			}
		}

		// Token: 0x17003374 RID: 13172
		// (get) Token: 0x06014BA4 RID: 84900 RVA: 0x006351AC File Offset: 0x006333AC
		public override GlobalTargetInfo? AlertCulpritTarget
		{
			get
			{
				return new GlobalTargetInfo?(this.scanner);
			}
		}

		// Token: 0x06014BA5 RID: 84901 RVA: 0x006351C0 File Offset: 0x006333C0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignalEnable)
			{
				this.scanner = this.site.Map.listerThings.ThingsOfDef(ThingDefOf.SurveyScanner).FirstOrDefault<Thing>();
				this.endTick = GenTicks.TicksGame + this.duration;
				if (this.fireRaidRemainingTicks > 0)
				{
					this.fireRaidTick = Mathf.Max(GenTicks.TicksGame + 2500, this.endTick - this.fireRaidRemainingTicks);
				}
			}
		}

		// Token: 0x06014BA6 RID: 84902 RVA: 0x0063524C File Offset: 0x0063344C
		public override void QuestPartTick()
		{
			if (GenTicks.TicksGame >= this.endTick)
			{
				Find.SignalManager.SendSignal(new Signal(this.completedSignal, false));
				base.Complete();
				return;
			}
			if (GenTicks.TicksGame >= this.fireRaidTick && this.fireRaidTick > 0)
			{
				this.fireRaidTick = -1;
				Find.SignalManager.SendSignal(new Signal(this.raidSignal, false));
			}
		}

		// Token: 0x06014BA7 RID: 84903 RVA: 0x006352B8 File Offset: 0x006334B8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
			Scribe_References.Look<Thing>(ref this.scanner, "scanner", false);
			Scribe_Values.Look<string>(ref this.completedSignal, "completedSignal", null, false);
			Scribe_Values.Look<string>(ref this.raidSignal, "raidSignal", null, false);
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<int>(ref this.endTick, "endTick", 0, false);
			Scribe_Values.Look<int>(ref this.fireRaidTick, "fireRaidTick", 0, false);
			Scribe_Values.Look<int>(ref this.fireRaidRemainingTicks, "fireRaidRemainingTicks", 0, false);
		}

		// Token: 0x0400E6A6 RID: 59046
		private Site site;

		// Token: 0x0400E6A7 RID: 59047
		private Thing scanner;

		// Token: 0x0400E6A8 RID: 59048
		private int duration;

		// Token: 0x0400E6A9 RID: 59049
		private int endTick;

		// Token: 0x0400E6AA RID: 59050
		private int fireRaidTick;

		// Token: 0x0400E6AB RID: 59051
		private int fireRaidRemainingTicks;

		// Token: 0x0400E6AC RID: 59052
		private string completedSignal;

		// Token: 0x0400E6AD RID: 59053
		private string raidSignal;
	}
}
