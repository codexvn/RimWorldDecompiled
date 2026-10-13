using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B5D RID: 11101
	public class ThingSetMaker_Conditional_MakingFaction : ThingSetMaker_Conditional
	{
		// Token: 0x0600F840 RID: 63552 RVA: 0x00483414 File Offset: 0x00481614
		protected override bool Condition(ThingSetMakerParams parms)
		{
			return (!this.requireNonNull || parms.makingFaction != null) && (this.makingFaction == null || (parms.makingFaction != null && parms.makingFaction.def == this.makingFaction)) && (this.makingFactionCategories.NullOrEmpty<string>() || (parms.makingFaction != null && this.makingFactionCategories.Contains(parms.makingFaction.def.categoryTag)));
		}

		// Token: 0x0400A33C RID: 41788
		public FactionDef makingFaction;

		// Token: 0x0400A33D RID: 41789
		public List<string> makingFactionCategories;

		// Token: 0x0400A33E RID: 41790
		public bool requireNonNull;
	}
}
