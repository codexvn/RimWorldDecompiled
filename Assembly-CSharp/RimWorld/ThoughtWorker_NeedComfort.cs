using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001846 RID: 6214
	public class ThoughtWorker_NeedComfort : ThoughtWorker
	{
		// Token: 0x06008F6D RID: 36717 RVA: 0x002A5C8C File Offset: 0x002A3E8C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.needs.comfort == null)
			{
				return ThoughtState.Inactive;
			}
			switch (p.needs.comfort.CurCategory)
			{
			case ComfortCategory.Uncomfortable:
				return ThoughtState.ActiveAtStage(0);
			case ComfortCategory.Normal:
				return ThoughtState.Inactive;
			case ComfortCategory.Comfortable:
				return ThoughtState.ActiveAtStage(1);
			case ComfortCategory.VeryComfortable:
				return ThoughtState.ActiveAtStage(2);
			case ComfortCategory.ExtremelyComfortable:
				return ThoughtState.ActiveAtStage(3);
			case ComfortCategory.LuxuriantlyComfortable:
				return ThoughtState.ActiveAtStage(4);
			default:
				throw new NotImplementedException();
			}
		}
	}
}
