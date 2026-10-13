using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016FE RID: 5886
	public class LordToilData_HateChant : LordToilData
	{
		// Token: 0x06008AC4 RID: 35524 RVA: 0x00296220 File Offset: 0x00294420
		public override void ExposeData()
		{
			Scribe_References.Look<GameCondition_HateChantDrone>(ref this.condition, "condition", false);
			Scribe_Values.Look<int>(ref this.lastDroneUpdate, "lastDroneUpdate", 0, false);
			Scribe_Collections.Look<PsychicRitualParticipant>(ref this.chanters, "chanters", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x04005A4B RID: 23115
		public List<PsychicRitualParticipant> chanters;

		// Token: 0x04005A4C RID: 23116
		public GameCondition_HateChantDrone condition;

		// Token: 0x04005A4D RID: 23117
		public int lastDroneUpdate;
	}
}
