using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017EF RID: 6127
	public class ThoughtWorker_Precept_AnyBodyPartButHairOrFaceCovered : ThoughtWorker_Precept
	{
		// Token: 0x06008E60 RID: 36448 RVA: 0x002A2EAF File Offset: 0x002A10AF
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_AnyBodyPartButHairOrFaceCovered.HasCoveredBodyPartsButHairOrFace(p);
		}

		// Token: 0x06008E61 RID: 36449 RVA: 0x002A2EBC File Offset: 0x002A10BC
		public static bool HasCoveredBodyPartsButHairOrFace(Pawn p)
		{
			if (p.apparel == null)
			{
				return false;
			}
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				Apparel apparel = wornApparel[i];
				if (apparel.def.apparel.countsAsClothingForNudity)
				{
					bool flag = false;
					bool flag2 = false;
					bool flag3 = false;
					for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
					{
						if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Torso)
						{
							flag = true;
						}
						else if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.UpperHead)
						{
							flag2 = true;
						}
						else if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.FullHead)
						{
							flag3 = true;
						}
					}
					if (flag || (!flag2 && !flag3))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
