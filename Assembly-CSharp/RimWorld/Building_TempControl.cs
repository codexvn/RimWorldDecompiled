using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020029C5 RID: 10693
	public class Building_TempControl : Building
	{
		// Token: 0x0600EBBB RID: 60347 RVA: 0x0044790B File Offset: 0x00445B0B
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.compTempControl = base.GetComp<CompTempControl>();
			this.compPowerTrader = base.GetComp<CompPowerTrader>();
		}

		// Token: 0x04009C6F RID: 40047
		public CompTempControl compTempControl;

		// Token: 0x04009C70 RID: 40048
		public CompPowerTrader compPowerTrader;
	}
}
