using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200184A RID: 6218
	public class ThoughtWorker_NeedIndoors : ThoughtWorker
	{
		// Token: 0x06008F75 RID: 36725 RVA: 0x002A5EE8 File Offset: 0x002A40E8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.needs.indoors == null)
			{
				return ThoughtState.Inactive;
			}
			if (p.HostFaction != null)
			{
				return ThoughtState.Inactive;
			}
			switch (p.needs.indoors.CurCategory)
			{
			case IndoorsCategory.ComfortablyIndoors:
				return ThoughtState.Inactive;
			case IndoorsCategory.JustOutdoors:
				return ThoughtState.ActiveAtStage(0);
			case IndoorsCategory.Outdoors:
				return ThoughtState.ActiveAtStage(1);
			case IndoorsCategory.LongOutdoors:
				return ThoughtState.ActiveAtStage(2);
			case IndoorsCategory.VeryLongOutdoors:
				return ThoughtState.ActiveAtStage(3);
			case IndoorsCategory.BrutalOutdoors:
				return ThoughtState.ActiveAtStage(4);
			default:
				throw new InvalidOperationException("Unknown IndoorsCategory");
			}
		}
	}
}
