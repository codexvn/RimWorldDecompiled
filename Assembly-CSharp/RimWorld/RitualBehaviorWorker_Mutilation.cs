using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020026C9 RID: 9929
	public class RitualBehaviorWorker_Mutilation : RitualBehaviorWorker
	{
		// Token: 0x0600D9B2 RID: 55730 RVA: 0x00406F23 File Offset: 0x00405123
		public RitualBehaviorWorker_Mutilation()
		{
		}

		// Token: 0x0600D9B3 RID: 55731 RVA: 0x00406F2B File Offset: 0x0040512B
		public RitualBehaviorWorker_Mutilation(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x0600D9B4 RID: 55732 RVA: 0x00406F34 File Offset: 0x00405134
		protected override LordJob CreateLordJob(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			return new LordJob_Ritual_Mutilation(target, ritual, obligation, this.def.stages, assignments, organizer);
		}
	}
}
