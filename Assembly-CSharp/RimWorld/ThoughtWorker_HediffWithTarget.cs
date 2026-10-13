using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001872 RID: 6258
	public class ThoughtWorker_HediffWithTarget : ThoughtWorker_Hediff
	{
		// Token: 0x06008FCB RID: 36811 RVA: 0x002A72C8 File Offset: 0x002A54C8
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			HediffWithTarget hediffWithTarget = (HediffWithTarget)p.health.hediffSet.GetFirstHediffOfDef(this.def.hediff, false);
			if (hediffWithTarget == null || hediffWithTarget.target != other)
			{
				return ThoughtState.Inactive;
			}
			return this.CurrentStateInternal(p);
		}
	}
}
