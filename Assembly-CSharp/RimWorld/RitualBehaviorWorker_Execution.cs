using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020026D7 RID: 9943
	public class RitualBehaviorWorker_Execution : RitualBehaviorWorker
	{
		// Token: 0x0600D9ED RID: 55789 RVA: 0x00406F23 File Offset: 0x00405123
		public RitualBehaviorWorker_Execution()
		{
		}

		// Token: 0x0600D9EE RID: 55790 RVA: 0x00406F2B File Offset: 0x0040512B
		public RitualBehaviorWorker_Execution(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x0600D9EF RID: 55791 RVA: 0x00407AD8 File Offset: 0x00405CD8
		public override void PostCleanup(LordJob_Ritual ritual)
		{
			Pawn warden = ritual.PawnWithRole("executioner");
			Pawn pawn = ritual.PawnWithRole("prisoner");
			if (pawn != null && pawn.IsPrisonerOfColony)
			{
				WorkGiver_Warden_TakeToBed.TryTakePrisonerToBed(pawn, warden);
				pawn.guest.WaitInsteadOfEscapingFor(1250);
			}
		}
	}
}
