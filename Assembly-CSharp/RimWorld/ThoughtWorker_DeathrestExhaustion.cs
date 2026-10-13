using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C9 RID: 6345
	public class ThoughtWorker_DeathrestExhaustion : ThoughtWorker
	{
		// Token: 0x060090B6 RID: 37046 RVA: 0x002A9FA0 File Offset: 0x002A81A0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			Need_Deathrest need_Deathrest;
			if (p.needs == null || !p.needs.TryGetNeed<Need_Deathrest>(out need_Deathrest))
			{
				return ThoughtState.Inactive;
			}
			return need_Deathrest.CurLevel == 0f;
		}
	}
}
