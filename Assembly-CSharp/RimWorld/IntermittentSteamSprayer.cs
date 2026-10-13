using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B08 RID: 11016
	public class IntermittentSteamSprayer
	{
		// Token: 0x17002678 RID: 9848
		// (get) Token: 0x0600F5F7 RID: 62967 RVA: 0x00476D38 File Offset: 0x00474F38
		// (set) Token: 0x0600F5F8 RID: 62968 RVA: 0x00476D40 File Offset: 0x00474F40
		public int MinTicksBetweenSprays { get; set; } = 500;

		// Token: 0x17002679 RID: 9849
		// (get) Token: 0x0600F5F9 RID: 62969 RVA: 0x00476D49 File Offset: 0x00474F49
		// (set) Token: 0x0600F5FA RID: 62970 RVA: 0x00476D51 File Offset: 0x00474F51
		public int MaxTicksBetweenSprays { get; set; } = 2000;

		// Token: 0x1700267A RID: 9850
		// (get) Token: 0x0600F5FB RID: 62971 RVA: 0x00476D5A File Offset: 0x00474F5A
		// (set) Token: 0x0600F5FC RID: 62972 RVA: 0x00476D62 File Offset: 0x00474F62
		public int MinSprayDuration { get; set; } = 200;

		// Token: 0x1700267B RID: 9851
		// (get) Token: 0x0600F5FD RID: 62973 RVA: 0x00476D6B File Offset: 0x00474F6B
		// (set) Token: 0x0600F5FE RID: 62974 RVA: 0x00476D73 File Offset: 0x00474F73
		public int MaxSprayDuration { get; set; } = 500;

		// Token: 0x1700267C RID: 9852
		// (get) Token: 0x0600F5FF RID: 62975 RVA: 0x00476D7C File Offset: 0x00474F7C
		// (set) Token: 0x0600F600 RID: 62976 RVA: 0x00476D84 File Offset: 0x00474F84
		public int PushHeatInterval { get; set; } = 20;

		// Token: 0x1700267D RID: 9853
		// (get) Token: 0x0600F601 RID: 62977 RVA: 0x00476D8D File Offset: 0x00474F8D
		// (set) Token: 0x0600F602 RID: 62978 RVA: 0x00476D95 File Offset: 0x00474F95
		public float SprayThickness { get; set; } = 0.6f;

		// Token: 0x0600F603 RID: 62979 RVA: 0x00476DA0 File Offset: 0x00474FA0
		public IntermittentSteamSprayer(Thing parent)
		{
			this.parent = parent;
			this.ticksUntilSpray = this.MinTicksBetweenSprays;
		}

		// Token: 0x0600F604 RID: 62980 RVA: 0x00476E08 File Offset: 0x00475008
		public void SteamSprayerTick()
		{
			if (this.sprayTicksLeft > 0)
			{
				this.sprayTicksLeft--;
				if (Rand.Value < this.SprayThickness)
				{
					FleckMaker.ThrowAirPuffUp(this.parent.TrueCenter(), this.parent.Map);
				}
				if (this.parent.IsHashIntervalTick(this.PushHeatInterval))
				{
					GenTemperature.PushHeat(this.parent, 40f);
				}
				if (this.sprayTicksLeft <= 0)
				{
					if (this.endSprayCallback != null)
					{
						this.endSprayCallback();
					}
					this.ticksUntilSpray = Rand.RangeInclusive(this.MinTicksBetweenSprays, this.MaxTicksBetweenSprays);
					return;
				}
			}
			else
			{
				this.ticksUntilSpray--;
				if (this.ticksUntilSpray <= 0)
				{
					if (this.startSprayCallback != null)
					{
						this.startSprayCallback();
					}
					this.sprayTicksLeft = Rand.RangeInclusive(this.MinSprayDuration, this.MaxSprayDuration);
				}
			}
		}

		// Token: 0x0400A1E0 RID: 41440
		private readonly Thing parent;

		// Token: 0x0400A1E1 RID: 41441
		private int ticksUntilSpray;

		// Token: 0x0400A1E2 RID: 41442
		private int sprayTicksLeft;

		// Token: 0x0400A1E3 RID: 41443
		public Action startSprayCallback;

		// Token: 0x0400A1E4 RID: 41444
		public Action endSprayCallback;
	}
}
