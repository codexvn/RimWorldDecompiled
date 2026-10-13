using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F2A RID: 12074
	public abstract class CompTargetEffect : ThingComp
	{
		// Token: 0x060110C3 RID: 69827
		public abstract void DoEffectOn(Pawn user, Thing target);

		// Token: 0x060110C4 RID: 69828 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanApplyOn(Thing target)
		{
			return true;
		}
	}
}
