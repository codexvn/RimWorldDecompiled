using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017ED RID: 6125
	public class ThoughtWorker_Precept_AnyBodyPartButGroinCovered : ThoughtWorker_Precept
	{
		// Token: 0x06008E5B RID: 36443 RVA: 0x002A2D78 File Offset: 0x002A0F78
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_AnyBodyPartButGroinCovered.HasCoveredBodyPartsButGroin(p) && GenTemperature.SafeTemperatureRange(p.def, null).Includes(p.AmbientTemperature);
		}

		// Token: 0x06008E5C RID: 36444 RVA: 0x002A2DB0 File Offset: 0x002A0FB0
		public static bool HasCoveredBodyPartsButGroin(Pawn p)
		{
			if (p.apparel == null)
			{
				return false;
			}
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				Apparel apparel = wornApparel[i];
				if ((p.kindDef.apparelRequired == null || !p.kindDef.apparelRequired.Contains(apparel.def)) && apparel.def.apparel.countsAsClothingForNudity)
				{
					bool flag = false;
					bool flag2 = false;
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
					if (flag || !flag2)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
