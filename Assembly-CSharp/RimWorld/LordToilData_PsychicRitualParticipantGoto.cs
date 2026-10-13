using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001714 RID: 5908
	public class LordToilData_PsychicRitualParticipantGoto : LordToilData
	{
		// Token: 0x06008B24 RID: 35620 RVA: 0x00297520 File Offset: 0x00295720
		public override void ExposeData()
		{
			Scribe_Deep.Look<JobSyncTracker>(ref this.finalGoto, "finalGoto", Array.Empty<object>());
			Scribe_Collections.Look<PsychicRitualParticipant>(ref this.participants, "participants", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.ticksElapsed, "ticksElapsed", 0, false);
			Scribe_Values.Look<int>(ref this.timeoutTicks, "timeoutTicks", 0, false);
		}

		// Token: 0x04005A68 RID: 23144
		public List<PsychicRitualParticipant> participants;

		// Token: 0x04005A69 RID: 23145
		public JobSyncTracker finalGoto;

		// Token: 0x04005A6A RID: 23146
		public int ticksElapsed;

		// Token: 0x04005A6B RID: 23147
		public int timeoutTicks;
	}
}
