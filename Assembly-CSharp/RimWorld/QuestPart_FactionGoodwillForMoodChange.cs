using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BC3 RID: 7107
	public class QuestPart_FactionGoodwillForMoodChange : QuestPartActivable
	{
		// Token: 0x17001847 RID: 6215
		// (get) Token: 0x06009E33 RID: 40499 RVA: 0x002DC694 File Offset: 0x002DA894
		public override string ExpiryInfoPart
		{
			get
			{
				return "QuestAveragePawnMood".Translate(120000.ToStringTicksToPeriodVerbose(true, true), this.cachedMovingAverage.ToStringPercent());
			}
		}

		// Token: 0x17001848 RID: 6216
		// (get) Token: 0x06009E34 RID: 40500 RVA: 0x002DC6C8 File Offset: 0x002DA8C8
		public override string ExpiryInfoPartTip
		{
			get
			{
				return "QuestAveragePawnMoodTargets".Translate((from p in this.pawns
				select p.LabelShort).ToCommaList(true, false), 120000.ToStringTicksToPeriodVerbose(true, true));
			}
		}

		// Token: 0x17001849 RID: 6217
		// (get) Token: 0x06009E35 RID: 40501 RVA: 0x002DC72C File Offset: 0x002DA92C
		private float AveragePawnMoodPercent
		{
			get
			{
				float num = 0f;
				int num2 = 0;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i].needs != null && this.pawns[i].needs.mood != null)
					{
						num += this.pawns[i].needs.mood.CurLevelPercentage;
						num2++;
					}
				}
				if (num2 == 0)
				{
					return 0f;
				}
				return num / (float)num2;
			}
		}

		// Token: 0x1700184A RID: 6218
		// (get) Token: 0x06009E36 RID: 40502 RVA: 0x002DC7B4 File Offset: 0x002DA9B4
		private float MovingAveragePawnMoodPercent
		{
			get
			{
				if (this.movingAverage.Count == 0)
				{
					return this.AveragePawnMoodPercent;
				}
				float num = 0f;
				for (int i = 0; i < this.movingAverage.Count; i++)
				{
					num += this.movingAverage[i];
				}
				return num / (float)this.movingAverage.Count;
			}
		}

		// Token: 0x1700184B RID: 6219
		// (get) Token: 0x06009E37 RID: 40503 RVA: 0x002DC80E File Offset: 0x002DAA0E
		public int SampleSize
		{
			get
			{
				return Mathf.FloorToInt(48f);
			}
		}

		// Token: 0x06009E38 RID: 40504 RVA: 0x002DC81C File Offset: 0x002DAA1C
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			this.currentInterval++;
			if (this.currentInterval >= 2500)
			{
				this.currentInterval = 0;
				while (this.movingAverage.Count >= this.SampleSize)
				{
					this.movingAverage.RemoveLast<float>();
				}
				this.movingAverage.Insert(0, this.AveragePawnMoodPercent);
				this.cachedMovingAverage = this.MovingAveragePawnMoodPercent;
			}
		}

		// Token: 0x06009E39 RID: 40505 RVA: 0x002DC890 File Offset: 0x002DAA90
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (!this.inSignal.NullOrEmpty() && signal.tag == this.inSignal)
			{
				float movingAveragePawnMoodPercent = this.MovingAveragePawnMoodPercent;
				int num = Mathf.RoundToInt(QuestPart_FactionGoodwillForMoodChange.GoodwillFromAverageMoodCurve.Evaluate(movingAveragePawnMoodPercent));
				SignalArgs args = new SignalArgs(num.Named("GOODWILL"), movingAveragePawnMoodPercent.ToStringPercent().Named("AVERAGEMOOD"));
				if (num > 0)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalSuccess, args, false));
					return;
				}
				Find.SignalManager.SendSignal(new Signal(this.outSignalFailed, args, false));
			}
		}

		// Token: 0x06009E3A RID: 40506 RVA: 0x002DC93C File Offset: 0x002DAB3C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalSuccess, "outSignalSuccess", null, false);
			Scribe_Values.Look<string>(ref this.outSignalFailed, "outSignalFailed", null, false);
			Scribe_Values.Look<int>(ref this.currentInterval, "currentInterval", 0, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<float>(ref this.movingAverage, "movingAverage", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
				if (this.movingAverage == null)
				{
					this.movingAverage = new List<float>();
				}
				this.cachedMovingAverage = this.MovingAveragePawnMoodPercent;
			}
		}

		// Token: 0x04006D85 RID: 28037
		public string inSignal;

		// Token: 0x04006D86 RID: 28038
		public string outSignalSuccess;

		// Token: 0x04006D87 RID: 28039
		public string outSignalFailed;

		// Token: 0x04006D88 RID: 28040
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04006D89 RID: 28041
		private int currentInterval = 2500;

		// Token: 0x04006D8A RID: 28042
		private List<float> movingAverage = new List<float>();

		// Token: 0x04006D8B RID: 28043
		private float cachedMovingAverage;

		// Token: 0x04006D8C RID: 28044
		private static readonly SimpleCurve GoodwillFromAverageMoodCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 20f),
				true
			}
		};

		// Token: 0x04006D8D RID: 28045
		public const int Interval = 2500;

		// Token: 0x04006D8E RID: 28046
		public const int Range = 120000;
	}
}
