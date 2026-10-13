using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011E5 RID: 4581
	public class JobDriver_PlayHorseshoes : JobDriver_WatchBuilding
	{
		// Token: 0x06006FEE RID: 28654 RVA: 0x0022CB84 File Offset: 0x0022AD84
		protected override void WatchTickAction(int delta)
		{
			if (this.pawn.IsHashIntervalTick(400, delta))
			{
				FleckMaker.ThrowHorseshoe(this.pawn, base.TargetA.Cell);
			}
			base.WatchTickAction(delta);
		}

		// Token: 0x04004DF9 RID: 19961
		private const int HorseshoeThrowInterval = 400;
	}
}
