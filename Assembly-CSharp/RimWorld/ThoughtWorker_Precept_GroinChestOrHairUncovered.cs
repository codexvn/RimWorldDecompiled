using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017F7 RID: 6135
	public class ThoughtWorker_Precept_GroinChestOrHairUncovered : ThoughtWorker_Precept
	{
		// Token: 0x06008E74 RID: 36468 RVA: 0x002A31C8 File Offset: 0x002A13C8
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_GroinChestOrHairUncovered.HasUncoveredGroinChestOrHair(p);
		}

		// Token: 0x06008E75 RID: 36469 RVA: 0x002A31D8 File Offset: 0x002A13D8
		public static bool HasUncoveredGroinChestOrHair(Pawn p)
		{
			if (p.apparel == null)
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				Apparel apparel = wornApparel[i];
				for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
				{
					if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Torso)
					{
						flag = true;
					}
					else if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Legs)
					{
						flag2 = true;
					}
					else if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.UpperHead || apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.FullHead)
					{
						flag3 = true;
					}
				}
				if (flag && flag2 && flag3)
				{
					break;
				}
			}
			return !flag || !flag2 || !flag3;
		}
	}
}
