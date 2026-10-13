using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020026CE RID: 9934
	public class RitualBehaviorWorker_ThroneSpeech : RitualBehaviorWorker
	{
		// Token: 0x0600D9C7 RID: 55751 RVA: 0x00406F23 File Offset: 0x00405123
		public RitualBehaviorWorker_ThroneSpeech()
		{
		}

		// Token: 0x0600D9C8 RID: 55752 RVA: 0x00406F2B File Offset: 0x0040512B
		public RitualBehaviorWorker_ThroneSpeech(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x0600D9C9 RID: 55753 RVA: 0x00407367 File Offset: 0x00405567
		protected override LordJob CreateLordJob(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			return new LordJob_Joinable_Speech(target, organizer, ritual, this.def.stages, assignments, true);
		}
	}
}
