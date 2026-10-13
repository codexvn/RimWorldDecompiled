using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200187D RID: 6269
	public class ThoughtWorker_HumanLeatherApparel : ThoughtWorker
	{
		// Token: 0x06008FE5 RID: 36837 RVA: 0x002A771C File Offset: 0x002A591C
		public static ThoughtState CurrentThoughtState(Pawn p)
		{
			string text = null;
			int num = 0;
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				if (wornApparel[i].Stuff == ThingDefOf.Human.race.leatherDef)
				{
					if (text == null)
					{
						text = wornApparel[i].def.label;
					}
					num++;
				}
			}
			if (num == 0)
			{
				return ThoughtState.Inactive;
			}
			if (num >= 5)
			{
				return ThoughtState.ActiveAtStage(4, text);
			}
			return ThoughtState.ActiveAtStage(num - 1, text);
		}

		// Token: 0x06008FE6 RID: 36838 RVA: 0x002A273E File Offset: 0x002A093E
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return ThoughtWorker_HumanLeatherApparel.CurrentThoughtState(p);
		}
	}
}
