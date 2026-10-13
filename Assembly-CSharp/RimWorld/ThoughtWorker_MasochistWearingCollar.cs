using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018B5 RID: 6325
	public class ThoughtWorker_MasochistWearingCollar : ThoughtWorker
	{
		// Token: 0x06009080 RID: 36992 RVA: 0x002A96D8 File Offset: 0x002A78D8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (ModsConfig.IdeologyActive)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					if (wornApparel[i].def == ThingDefOf.Apparel_Collar)
					{
						return true;
					}
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
