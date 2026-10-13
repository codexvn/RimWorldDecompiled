using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ACB RID: 6859
	public class RaidAgeRestrictionWorker_Children : RaidAgeRestrictionWorker
	{
		// Token: 0x0600998E RID: 39310 RVA: 0x002CB87F File Offset: 0x002C9A7F
		public override bool CanUseWith(IncidentParms parms)
		{
			return Find.Storyteller.difficulty.ChildRaidersAllowed && !Find.Storyteller.difficulty.babiesAreHealthy && base.CanUseWith(parms);
		}
	}
}
