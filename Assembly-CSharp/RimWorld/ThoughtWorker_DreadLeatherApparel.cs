using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200187C RID: 6268
	public class ThoughtWorker_DreadLeatherApparel : ThoughtWorker_ApparelThought
	{
		// Token: 0x06008FE2 RID: 36834 RVA: 0x002A76F4 File Offset: 0x002A58F4
		protected override bool ApparelCounts(Apparel apparel)
		{
			return apparel.Stuff == ThingDefOf.Leather_Dread;
		}

		// Token: 0x06008FE3 RID: 36835 RVA: 0x002A7703 File Offset: 0x002A5903
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			return base.CurrentStateInternal(p);
		}
	}
}
