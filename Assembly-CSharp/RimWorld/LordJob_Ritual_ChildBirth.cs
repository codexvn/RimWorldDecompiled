using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016C9 RID: 5833
	public class LordJob_Ritual_ChildBirth : LordJob_Ritual
	{
		// Token: 0x060089A4 RID: 35236 RVA: 0x0028E3E3 File Offset: 0x0028C5E3
		public LordJob_Ritual_ChildBirth()
		{
		}

		// Token: 0x060089A5 RID: 35237 RVA: 0x002918E7 File Offset: 0x0028FAE7
		public LordJob_Ritual_ChildBirth(TargetInfo selectedTarget, Precept_Ritual ritual, RitualObligation obligation, List<RitualStage> allStages, RitualRoleAssignments assignments, Pawn organizer = null, IntVec3? spotOverride = null) : base(selectedTarget, ritual, obligation, allStages, assignments, organizer, spotOverride)
		{
		}

		// Token: 0x060089A6 RID: 35238 RVA: 0x002918FA File Offset: 0x0028FAFA
		protected override bool RitualFinished(float progress, bool cancelled)
		{
			return progress == 1f;
		}
	}
}
