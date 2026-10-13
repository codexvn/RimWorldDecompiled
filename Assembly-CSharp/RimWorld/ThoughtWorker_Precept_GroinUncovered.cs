using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017F3 RID: 6131
	public class ThoughtWorker_Precept_GroinUncovered : ThoughtWorker_Precept
	{
		// Token: 0x06008E6A RID: 36458 RVA: 0x002A3059 File Offset: 0x002A1259
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_GroinUncovered.HasUncoveredGroin(p);
		}

		// Token: 0x06008E6B RID: 36459 RVA: 0x002A3068 File Offset: 0x002A1268
		public static bool HasUncoveredGroin(Pawn p)
		{
			if (p.apparel == null)
			{
				return false;
			}
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				Apparel apparel = wornApparel[i];
				for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
				{
					if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Legs)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
