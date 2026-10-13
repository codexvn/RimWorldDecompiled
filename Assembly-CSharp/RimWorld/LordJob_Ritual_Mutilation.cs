using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016CC RID: 5836
	public class LordJob_Ritual_Mutilation : LordJob_Ritual
	{
		// Token: 0x060089BB RID: 35259 RVA: 0x00291D48 File Offset: 0x0028FF48
		public LordJob_Ritual_Mutilation()
		{
		}

		// Token: 0x060089BC RID: 35260 RVA: 0x00291D5C File Offset: 0x0028FF5C
		public LordJob_Ritual_Mutilation(TargetInfo selectedTarget, Precept_Ritual ritual, RitualObligation obligation, List<RitualStage> allStages, RitualRoleAssignments assignments, Pawn organizer = null) : base(selectedTarget, ritual, obligation, allStages, assignments, organizer, null)
		{
		}

		// Token: 0x060089BD RID: 35261 RVA: 0x00291D8C File Offset: 0x0028FF8C
		protected override bool ShouldCallOffBecausePawnNoLongerOwned(Pawn p)
		{
			return base.ShouldCallOffBecausePawnNoLongerOwned(p) && !this.mutilatedPawns.Contains(p);
		}

		// Token: 0x060089BE RID: 35262 RVA: 0x00291DA8 File Offset: 0x0028FFA8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.mutilatedPawns, "mutilatedPawns", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x04005A00 RID: 23040
		public List<Pawn> mutilatedPawns = new List<Pawn>();
	}
}
