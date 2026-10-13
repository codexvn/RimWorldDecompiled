using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011E4 RID: 4580
	public class JobDriver_PlayHoopstone : JobDriver_WatchBuilding
	{
		// Token: 0x06006FEC RID: 28652 RVA: 0x0022CB3C File Offset: 0x0022AD3C
		protected override void WatchTickAction(int delta)
		{
			if (this.pawn.IsHashIntervalTick(400, delta))
			{
				FleckMaker.ThrowStone(this.pawn, base.TargetA.Cell);
			}
			base.WatchTickAction(delta);
		}

		// Token: 0x04004DF8 RID: 19960
		private const int StoneThrowInterval = 400;
	}
}
