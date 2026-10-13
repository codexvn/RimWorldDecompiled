using System;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B9E RID: 15262
	public class QuestPart_SurveyScanner : QuestPartActivable
	{
		// Token: 0x06014B9B RID: 84891 RVA: 0x002D9B28 File Offset: 0x002D7D28
		public QuestPart_SurveyScanner()
		{
		}

		// Token: 0x06014B9C RID: 84892 RVA: 0x00634FB5 File Offset: 0x006331B5
		public QuestPart_SurveyScanner(Site site, string inSignalEnable, string scannerTag, int duration)
		{
			this.site = site;
			this.inSignalEnable = inSignalEnable;
			this.scannerTag = scannerTag;
			this.duration = duration;
		}

		// Token: 0x06014B9D RID: 84893 RVA: 0x00634FDC File Offset: 0x006331DC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
			Scribe_References.Look<Thing>(ref this.scanner, "scanner", false);
			Scribe_Values.Look<string>(ref this.scannerTag, "scannerTag", null, false);
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
		}

		// Token: 0x06014B9E RID: 84894 RVA: 0x00635038 File Offset: 0x00633238
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignalEnable)
			{
				this.scanner = this.site.Map.listerThings.ThingsOfDef(ThingDefOf.SurveyScanner).FirstOrDefault<Thing>();
				this.scanner.TryGetComp<CompCountdown>().endTick = GenTicks.TicksGame + this.duration;
				QuestUtility.AddQuestTag(ref this.scanner.questTags, this.scannerTag);
				this.scanner.SetFactionDirect(Faction.OfPlayer);
				Find.LetterStack.ReceiveLetter("LetterLabelSurveyScannerArrived".Translate(), "LetterSurveyScannerArrived".Translate(this.duration.ToStringTicksToPeriod(true, false, true, true, false).Named("DURATION")), LetterDefOf.NeutralEvent, this.scanner, null, null, null, null, 0, true);
			}
		}

		// Token: 0x0400E6A2 RID: 59042
		private Site site;

		// Token: 0x0400E6A3 RID: 59043
		private Thing scanner;

		// Token: 0x0400E6A4 RID: 59044
		private string scannerTag;

		// Token: 0x0400E6A5 RID: 59045
		private int duration;
	}
}
