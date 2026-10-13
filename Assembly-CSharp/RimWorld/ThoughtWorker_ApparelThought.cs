using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200187A RID: 6266
	public abstract class ThoughtWorker_ApparelThought : ThoughtWorker
	{
		// Token: 0x06008FDD RID: 36829
		protected abstract bool ApparelCounts(Apparel apparel);

		// Token: 0x06008FDE RID: 36830 RVA: 0x002A7670 File Offset: 0x002A5870
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			string text = null;
			int num = 0;
			List<Apparel> wornApparel = p.apparel.WornApparel;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				if (this.ApparelCounts(wornApparel[i]))
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
	}
}
