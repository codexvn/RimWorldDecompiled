using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BD2 RID: 7122
	public class QuestPart_MergeOutcomes : QuestPart
	{
		// Token: 0x06009E93 RID: 40595 RVA: 0x002DDADC File Offset: 0x002DBCDC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			int num = this.inSignals.IndexOf(signal.tag);
			if (num >= 0)
			{
				while (this.signalsReceived.Count <= num)
				{
					this.signalsReceived.Add(null);
				}
				this.signalsReceived[num] = new QuestEndOutcome?(this.GetOutcome(signal.args));
				this.CheckEnd();
			}
		}

		// Token: 0x06009E94 RID: 40596 RVA: 0x002DDB50 File Offset: 0x002DBD50
		private QuestEndOutcome GetOutcome(SignalArgs args)
		{
			QuestEndOutcome result;
			if (args.TryGetArg<QuestEndOutcome>("OUTCOME", out result))
			{
				return result;
			}
			return QuestEndOutcome.Unknown;
		}

		// Token: 0x06009E95 RID: 40597 RVA: 0x002DDB70 File Offset: 0x002DBD70
		private void CheckEnd()
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = this.inSignals.Count == this.signalsReceived.Count;
			for (int i = 0; i < this.signalsReceived.Count; i++)
			{
				if (this.signalsReceived[i] == null)
				{
					flag3 = false;
				}
				else if (this.signalsReceived[i].Value == QuestEndOutcome.Success)
				{
					flag = true;
				}
				else if (this.signalsReceived[i].Value == QuestEndOutcome.Fail)
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignal, QuestEndOutcome.Fail.Named("OUTCOME")));
				return;
			}
			if (flag3)
			{
				if (flag)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignal, QuestEndOutcome.Success.Named("OUTCOME")));
					return;
				}
				Find.SignalManager.SendSignal(new Signal(this.outSignal, QuestEndOutcome.Unknown.Named("OUTCOME")));
			}
		}

		// Token: 0x06009E96 RID: 40598 RVA: 0x002DDC7C File Offset: 0x002DBE7C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Collections.Look<QuestEndOutcome?>(ref this.signalsReceived, "signalsReceived", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06009E97 RID: 40599 RVA: 0x002DDCD0 File Offset: 0x002DBED0
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int.ToString());
			}
			this.outSignal = "DebugSignal" + Rand.Int.ToString();
		}

		// Token: 0x04006DC2 RID: 28098
		public List<string> inSignals = new List<string>();

		// Token: 0x04006DC3 RID: 28099
		public string outSignal;

		// Token: 0x04006DC4 RID: 28100
		private List<QuestEndOutcome?> signalsReceived = new List<QuestEndOutcome?>();
	}
}
