using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BA6 RID: 15270
	public class QuestPart_RandomWaves : QuestPartActivable
	{
		// Token: 0x06014BC9 RID: 84937 RVA: 0x006361F5 File Offset: 0x006343F5
		public QuestPart_RandomWaves()
		{
		}

		// Token: 0x06014BCA RID: 84938 RVA: 0x00636208 File Offset: 0x00634408
		public QuestPart_RandomWaves(string inSignalEnable, string outSignal, float mtbWavesHours, float minRefireWavesHours, float maxRefireWavesHours = 3.4028235E+38f)
		{
			this.inSignalEnable = inSignalEnable;
			this.outSignal = outSignal;
			this.mtbWavesHours = mtbWavesHours;
			this.minRefireWavesHours = minRefireWavesHours;
			this.maxRefireWavesHours = maxRefireWavesHours;
		}

		// Token: 0x06014BCB RID: 84939 RVA: 0x00636240 File Offset: 0x00634440
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag != this.inSignalEnable)
			{
				return;
			}
			this.lastWaveTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06014BCC RID: 84940 RVA: 0x00636270 File Offset: 0x00634470
		public override void QuestPartTick()
		{
			if (((float)Find.TickManager.TicksGame > (float)this.lastWaveTick + this.minRefireWavesHours * 2500f && Rand.MTBEventOccurs(this.mtbWavesHours, 2500f, 1f)) || (float)Find.TickManager.TicksGame > (float)this.lastWaveTick + this.maxRefireWavesHours * 2500f)
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignal, false));
				this.lastWaveTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06014BCD RID: 84941 RVA: 0x006362FC File Offset: 0x006344FC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Values.Look<int>(ref this.lastWaveTick, "lastWaveTick", 0, false);
			Scribe_Values.Look<float>(ref this.mtbWavesHours, "mtbWavesHours", 40f, false);
			Scribe_Values.Look<float>(ref this.minRefireWavesHours, "minRefireWavesHours", 24f, false);
			Scribe_Values.Look<float>(ref this.maxRefireWavesHours, "maxRefireWavesHours", 72f, false);
		}

		// Token: 0x0400E6CF RID: 59087
		private string outSignal;

		// Token: 0x0400E6D0 RID: 59088
		private int lastWaveTick = -99999;

		// Token: 0x0400E6D1 RID: 59089
		private float mtbWavesHours;

		// Token: 0x0400E6D2 RID: 59090
		private float minRefireWavesHours;

		// Token: 0x0400E6D3 RID: 59091
		private float maxRefireWavesHours;
	}
}
