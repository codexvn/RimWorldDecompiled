using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001844 RID: 6212
	public class ThoughtWorker_NeedRest : ThoughtWorker
	{
		// Token: 0x06008F69 RID: 36713 RVA: 0x002A5BA0 File Offset: 0x002A3DA0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.needs.rest == null)
			{
				return ThoughtState.Inactive;
			}
			switch (p.needs.rest.CurCategory)
			{
			case RestCategory.Rested:
				return ThoughtState.Inactive;
			case RestCategory.Tired:
				return ThoughtState.ActiveAtStage(0);
			case RestCategory.VeryTired:
				return ThoughtState.ActiveAtStage(1);
			case RestCategory.Exhausted:
				return ThoughtState.ActiveAtStage(2);
			default:
				throw new NotImplementedException();
			}
		}
	}
}
