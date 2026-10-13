using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D33 RID: 7475
	public class GameCondition_ClimateCycle : GameCondition
	{
		// Token: 0x0600A769 RID: 42857 RVA: 0x002FFB63 File Offset: 0x002FDD63
		public override void Init()
		{
			this.ticksOffset = ((Rand.Value < 0.5f) ? 0 : 7200000);
		}

		// Token: 0x0600A76A RID: 42858 RVA: 0x002FFB7F File Offset: 0x002FDD7F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksOffset, "ticksOffset", 0, false);
		}

		// Token: 0x0600A76B RID: 42859 RVA: 0x002FFB99 File Offset: 0x002FDD99
		public override float TemperatureOffset()
		{
			return Mathf.Sin((GenDate.YearsPassedFloat + (float)this.ticksOffset / 3600000f) / 4f * 3.1415927f * 2f) * 20f;
		}

		// Token: 0x040073EA RID: 29674
		private int ticksOffset;

		// Token: 0x040073EB RID: 29675
		private const float PeriodYears = 4f;

		// Token: 0x040073EC RID: 29676
		private const float MaxTempOffset = 20f;
	}
}
