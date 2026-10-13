using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017E0 RID: 6112
	public abstract class ThoughtWorker_Precept : ThoughtWorker
	{
		// Token: 0x06008E2D RID: 36397
		protected abstract ThoughtState ShouldHaveThought(Pawn p);

		// Token: 0x06008E2E RID: 36398 RVA: 0x002A24F4 File Offset: 0x002A06F4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Ideo ideo = p.Ideo;
			if (ideo == null)
			{
				return ThoughtState.Inactive;
			}
			Precept firstPreceptAllowingSituationalThought = ideo.GetFirstPreceptAllowingSituationalThought(this.def);
			if (firstPreceptAllowingSituationalThought != null && !firstPreceptAllowingSituationalThought.def.enabledForNPCFactions && !p.CountsAsNonNPCForPrecepts())
			{
				return ThoughtState.Inactive;
			}
			if (!ideo.cachedPossibleSituationalThoughts.Contains(this.def))
			{
				return ThoughtState.Inactive;
			}
			return this.ShouldHaveThought(p);
		}
	}
}
