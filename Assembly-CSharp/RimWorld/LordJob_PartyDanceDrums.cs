using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016BB RID: 5819
	public class LordJob_PartyDanceDrums : LordJob_Ritual
	{
		// Token: 0x06008922 RID: 35106 RVA: 0x0028E3E3 File Offset: 0x0028C5E3
		public LordJob_PartyDanceDrums()
		{
		}

		// Token: 0x06008923 RID: 35107 RVA: 0x0028E3EC File Offset: 0x0028C5EC
		public LordJob_PartyDanceDrums(TargetInfo selectedTarget, Precept_Ritual ritual, RitualObligation obligation, List<RitualStage> allStages, RitualRoleAssignments assignments, Pawn organizer = null) : base(selectedTarget, ritual, obligation, allStages, assignments, organizer, null)
		{
		}

		// Token: 0x06008924 RID: 35108 RVA: 0x0028E411 File Offset: 0x0028C611
		protected override LordToil_Ritual MakeToil(RitualStage stage)
		{
			if (!ModLister.CheckIdeology("Drum party toil"))
			{
				return null;
			}
			return new LordToil_PartyDanceDrums(this.spot, this, stage, this.organizer);
		}
	}
}
