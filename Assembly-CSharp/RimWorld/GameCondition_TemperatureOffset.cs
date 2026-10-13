using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D2E RID: 7470
	public class GameCondition_TemperatureOffset : GameCondition
	{
		// Token: 0x0600A755 RID: 42837 RVA: 0x002FF7E8 File Offset: 0x002FD9E8
		public override void Init()
		{
			base.Init();
			this.tempOffset = this.def.temperatureOffset;
		}

		// Token: 0x0600A756 RID: 42838 RVA: 0x002FF801 File Offset: 0x002FDA01
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.tempOffset, "tempOffset", 0f, false);
		}

		// Token: 0x0600A757 RID: 42839 RVA: 0x002FF81F File Offset: 0x002FDA1F
		public override float TemperatureOffset()
		{
			return this.tempOffset;
		}

		// Token: 0x040073E6 RID: 29670
		public float tempOffset;
	}
}
