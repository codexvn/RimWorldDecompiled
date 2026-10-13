using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C6 RID: 6342
	public class ThoughtWorker_RotStinkLingering : ThoughtWorker
	{
		// Token: 0x060090B0 RID: 37040 RVA: 0x002A9E60 File Offset: 0x002A8060
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.Spawned || p.mindState == null)
			{
				return ThoughtState.Inactive;
			}
			if (p.Map.gasGrid.DensityAt(p.Position, GasType.RotStink) > 0)
			{
				return ThoughtState.Inactive;
			}
			if (Find.TickManager.TicksGame > p.mindState.lastRotStinkTick + 1800)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(0);
		}

		// Token: 0x04005C4C RID: 23628
		private const int LingerDuration = 1800;
	}
}
