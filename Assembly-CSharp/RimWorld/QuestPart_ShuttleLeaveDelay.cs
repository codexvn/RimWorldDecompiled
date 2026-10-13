using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BDC RID: 7132
	public class QuestPart_ShuttleLeaveDelay : QuestPart_Delay
	{
		// Token: 0x17001867 RID: 6247
		// (get) Token: 0x06009ED4 RID: 40660 RVA: 0x002DE8CF File Offset: 0x002DCACF
		public override AlertReport AlertReport
		{
			get
			{
				if (this.shuttle == null || !this.shuttle.Spawned)
				{
					return false;
				}
				return AlertReport.CulpritIs(this.shuttle);
			}
		}

		// Token: 0x17001868 RID: 6248
		// (get) Token: 0x06009ED5 RID: 40661 RVA: 0x002DE4E2 File Offset: 0x002DC6E2
		public override bool AlertCritical
		{
			get
			{
				return base.TicksLeft < 60000;
			}
		}

		// Token: 0x17001869 RID: 6249
		// (get) Token: 0x06009ED6 RID: 40662 RVA: 0x002DE8FD File Offset: 0x002DCAFD
		public override string AlertLabel
		{
			get
			{
				return "QuestPartShuttleLeaveDelay".Translate(base.TicksLeft.ToStringTicksToPeriodVerbose(true, true));
			}
		}

		// Token: 0x1700186A RID: 6250
		// (get) Token: 0x06009ED7 RID: 40663 RVA: 0x002DE920 File Offset: 0x002DCB20
		public override string AlertExplanation
		{
			get
			{
				if (this.quest.hidden)
				{
					return "QuestPartShuttleLeaveDelayDescHidden".Translate(base.TicksLeft.ToStringTicksToPeriodVerbose(true, true).Colorize(ColoredText.DateTimeColor));
				}
				return "QuestPartShuttleLeaveDelayDesc".Translate(this.quest.name, base.TicksLeft.ToStringTicksToPeriodVerbose(true, true).Colorize(ColoredText.DateTimeColor), this.shuttle.TryGetComp<CompShuttle>().RequiredThingsLabel);
			}
		}

		// Token: 0x1700186B RID: 6251
		// (get) Token: 0x06009ED8 RID: 40664 RVA: 0x002DE9B6 File Offset: 0x002DCBB6
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.shuttle != null)
				{
					yield return this.shuttle;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06009ED9 RID: 40665 RVA: 0x002DE9C6 File Offset: 0x002DCBC6
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (base.State == QuestPartState.Enabled && this.inSignalsDisable.Contains(signal.tag))
			{
				this.Disable();
			}
		}

		// Token: 0x06009EDA RID: 40666 RVA: 0x002DE9F1 File Offset: 0x002DCBF1
		public override string ExtraInspectString(ISelectable target)
		{
			if (target == this.shuttle)
			{
				return "ShuttleLeaveDelayInspectString".Translate(base.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false));
			}
			return null;
		}

		// Token: 0x06009EDB RID: 40667 RVA: 0x002DEA24 File Offset: 0x002DCC24
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Collections.Look<string>(ref this.inSignalsDisable, "inSignalsDisable", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.inSignalsDisable == null)
			{
				this.inSignalsDisable = new List<string>();
			}
		}

		// Token: 0x06009EDC RID: 40668 RVA: 0x002DEA79 File Offset: 0x002DCC79
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.shuttle != null)
			{
				this.shuttle.TryGetComp<CompShuttle>().requiredPawns.Replace(replace, with);
			}
		}

		// Token: 0x04006DE7 RID: 28135
		public Thing shuttle;

		// Token: 0x04006DE8 RID: 28136
		public List<string> inSignalsDisable = new List<string>();
	}
}
