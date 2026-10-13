using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A73 RID: 10867
	public class Building_WorkTable_HeatPush : Building_WorkTable
	{
		// Token: 0x0600F1ED RID: 61933 RVA: 0x00462C8B File Offset: 0x00460E8B
		public override void UsedThisTick()
		{
			base.UsedThisTick();
			if (this.IsHashIntervalTick(30))
			{
				GenTemperature.PushHeat(this, this.def.building.heatPerTickWhileWorking * 30f);
			}
		}

		// Token: 0x04009F68 RID: 40808
		private const int HeatPushInterval = 30;
	}
}
