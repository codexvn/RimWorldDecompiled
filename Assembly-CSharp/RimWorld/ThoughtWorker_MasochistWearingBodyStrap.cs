using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018B6 RID: 6326
	public class ThoughtWorker_MasochistWearingBodyStrap : ThoughtWorker
	{
		// Token: 0x06009082 RID: 36994 RVA: 0x002A9728 File Offset: 0x002A7928
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (ModsConfig.IdeologyActive)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					if (wornApparel[i].def == ThingDefOf.Apparel_BodyStrap)
					{
						return true;
					}
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
