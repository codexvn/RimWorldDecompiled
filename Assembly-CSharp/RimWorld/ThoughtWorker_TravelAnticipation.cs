using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C8 RID: 6344
	public class ThoughtWorker_TravelAnticipation : ThoughtWorker
	{
		// Token: 0x060090B4 RID: 37044 RVA: 0x002A9F34 File Offset: 0x002A8134
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.IsColonist || p.jobs == null || p.jobs.StartedFormingCaravanTick < 0)
			{
				return ThoughtState.Inactive;
			}
			int num = (Find.TickManager.TicksGame - p.jobs.StartedFormingCaravanTick) / 2500;
			if (num < 8)
			{
				return ThoughtState.Inactive;
			}
			if (num < 16)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return ThoughtState.ActiveAtStage(1);
		}

		// Token: 0x04005C4F RID: 23631
		private const int stage0Hours = 8;

		// Token: 0x04005C50 RID: 23632
		private const int stage1Hours = 16;
	}
}
