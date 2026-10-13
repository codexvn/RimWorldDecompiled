using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020026D6 RID: 9942
	public class RitualBehaviorWorker_PrisonerSacrifice : RitualBehaviorWorker
	{
		// Token: 0x0600D9EA RID: 55786 RVA: 0x00406F23 File Offset: 0x00405123
		public RitualBehaviorWorker_PrisonerSacrifice()
		{
		}

		// Token: 0x0600D9EB RID: 55787 RVA: 0x00406F2B File Offset: 0x0040512B
		public RitualBehaviorWorker_PrisonerSacrifice(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x0600D9EC RID: 55788 RVA: 0x00407A94 File Offset: 0x00405C94
		public override void PostCleanup(LordJob_Ritual ritual)
		{
			Pawn warden = ritual.PawnWithRole("moralist");
			Pawn pawn = ritual.PawnWithRole("prisoner");
			if (pawn.IsPrisonerOfColony)
			{
				WorkGiver_Warden_TakeToBed.TryTakePrisonerToBed(pawn, warden);
				pawn.guest.WaitInsteadOfEscapingFor(1250);
			}
		}
	}
}
