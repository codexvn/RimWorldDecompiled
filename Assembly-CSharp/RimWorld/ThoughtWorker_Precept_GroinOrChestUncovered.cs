using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017F5 RID: 6133
	public class ThoughtWorker_Precept_GroinOrChestUncovered : ThoughtWorker_Precept
	{
		// Token: 0x06008E6F RID: 36463 RVA: 0x002A30F1 File Offset: 0x002A12F1
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_GroinOrChestUncovered.HasUncoveredGroinOrChest(p);
		}

		// Token: 0x06008E70 RID: 36464 RVA: 0x002A3100 File Offset: 0x002A1300
		public static bool HasUncoveredGroinOrChest(Pawn p)
		{
			if (p.apparel == null)
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
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
				}
				if (flag && flag2)
				{
					break;
				}
			}
			return !flag || !flag2;
		}
	}
}
