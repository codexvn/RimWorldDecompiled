using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BDE RID: 7134
	public class QuestPart_StartDetectionRaids : QuestPart
	{
		// Token: 0x06009EE8 RID: 40680 RVA: 0x002DEC60 File Offset: 0x002DCE60
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				TimedDetectionRaids component = this.worldObject.GetComponent<TimedDetectionRaids>();
				int ticks = 240000;
				if (this.delayRangeHours != null)
				{
					ticks = (int)(this.delayRangeHours.Value.RandomInRange * 2500f);
					component.delayRangeHours = this.delayRangeHours.Value;
				}
				if (this.firstRaidDelayTicks != null)
				{
					ticks = this.firstRaidDelayTicks.Value;
				}
				component.StartDetectionCountdown(ticks, -1);
				component.SetNotifiedSilently();
			}
		}

		// Token: 0x06009EE9 RID: 40681 RVA: 0x002DECFC File Offset: 0x002DCEFC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
			Scribe_Values.Look<FloatRange?>(ref this.delayRangeHours, "delayRangeHours", null, false);
			Scribe_Values.Look<int?>(ref this.firstRaidDelayTicks, "firstRaidDelayTicks", null, false);
		}

		// Token: 0x04006DEE RID: 28142
		public string inSignal;

		// Token: 0x04006DEF RID: 28143
		public FloatRange? delayRangeHours;

		// Token: 0x04006DF0 RID: 28144
		public WorldObject worldObject;

		// Token: 0x04006DF1 RID: 28145
		public int? firstRaidDelayTicks;
	}
}
