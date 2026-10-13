using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001849 RID: 6217
	public class ThoughtWorker_NeedOutdoors : ThoughtWorker
	{
		// Token: 0x06008F73 RID: 36723 RVA: 0x002A5E54 File Offset: 0x002A4054
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.needs.outdoors == null)
			{
				return ThoughtState.Inactive;
			}
			if (p.HostFaction != null)
			{
				return ThoughtState.Inactive;
			}
			switch (p.needs.outdoors.CurCategory)
			{
			case OutdoorsCategory.Entombed:
				return ThoughtState.ActiveAtStage(0);
			case OutdoorsCategory.Trapped:
				return ThoughtState.ActiveAtStage(1);
			case OutdoorsCategory.CabinFeverSevere:
				return ThoughtState.ActiveAtStage(2);
			case OutdoorsCategory.CabinFeverLight:
				return ThoughtState.ActiveAtStage(3);
			case OutdoorsCategory.NeedFreshAir:
				return ThoughtState.ActiveAtStage(4);
			case OutdoorsCategory.Free:
				return ThoughtState.Inactive;
			default:
				throw new InvalidOperationException("Unknown OutdoorsCategory");
			}
		}
	}
}
