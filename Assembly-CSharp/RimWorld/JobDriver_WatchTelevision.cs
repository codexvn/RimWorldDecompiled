using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011FB RID: 4603
	public class JobDriver_WatchTelevision : JobDriver_WatchBuilding
	{
		// Token: 0x06007078 RID: 28792 RVA: 0x0022E5B8 File Offset: 0x0022C7B8
		protected override void WatchTickAction(int delta)
		{
			if (!((Building)base.TargetA.Thing).TryGetComp<CompPowerTrader>().PowerOn)
			{
				base.EndJobWith(JobCondition.Incompletable);
				return;
			}
			base.WatchTickAction(delta);
		}
	}
}
