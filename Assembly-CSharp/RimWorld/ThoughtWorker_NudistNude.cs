using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001887 RID: 6279
	public class ThoughtWorker_NudistNude : ThoughtWorker
	{
		// Token: 0x06008FFD RID: 36861 RVA: 0x002A7CCC File Offset: 0x002A5ECC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				Apparel apparel = wornApparel[i];
				if (apparel.def.apparel.countsAsClothingForNudity)
				{
					for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
					{
						if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Torso)
						{
							return false;
						}
						if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Legs)
						{
							return false;
						}
					}
				}
			}
			return true;
		}
	}
}
