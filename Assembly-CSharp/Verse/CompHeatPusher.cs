using System;

namespace Verse
{
	// Token: 0x02000900 RID: 2304
	public class CompHeatPusher : ThingComp
	{
		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x06003B7F RID: 15231 RVA: 0x0013A304 File Offset: 0x00138504
		public CompProperties_HeatPusher Props
		{
			get
			{
				return (CompProperties_HeatPusher)this.props;
			}
		}

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x06003B80 RID: 15232 RVA: 0x0013A314 File Offset: 0x00138514
		public virtual bool ShouldPushHeatNow
		{
			get
			{
				if (!this.parent.SpawnedOrAnyParentSpawned)
				{
					return false;
				}
				CompProperties_HeatPusher props = this.Props;
				float ambientTemperature = this.parent.AmbientTemperature;
				return this.enabled && ambientTemperature < props.heatPushMaxTemperature && ambientTemperature > props.heatPushMinTemperature;
			}
		}

		// Token: 0x06003B81 RID: 15233 RVA: 0x0013A360 File Offset: 0x00138560
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(60) && this.ShouldPushHeatNow)
			{
				GenTemperature.PushHeat(this.parent.PositionHeld, this.parent.MapHeld, this.Props.heatPerSecond);
			}
		}

		// Token: 0x06003B82 RID: 15234 RVA: 0x0013A3B1 File Offset: 0x001385B1
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.ShouldPushHeatNow)
			{
				GenTemperature.PushHeat(this.parent.PositionHeld, this.parent.MapHeld, this.Props.heatPerSecond * 4.1666665f);
			}
		}

		// Token: 0x06003B83 RID: 15235 RVA: 0x0013A3EE File Offset: 0x001385EE
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.enabled, "enabled", true, false);
		}

		// Token: 0x04002AF3 RID: 10995
		private const int HeatPushInterval = 60;

		// Token: 0x04002AF4 RID: 10996
		public bool enabled = true;
	}
}
