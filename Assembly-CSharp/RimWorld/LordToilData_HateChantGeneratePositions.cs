using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001700 RID: 5888
	public class LordToilData_HateChantGeneratePositions : LordToilData
	{
		// Token: 0x06008ACF RID: 35535 RVA: 0x002967AA File Offset: 0x002949AA
		public override void ExposeData()
		{
			Scribe_Collections.Look<PsychicRitualParticipant>(ref this.chanters, "chanters", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<IntVec3>(ref this.foundPositions, "foundPositions", LookMode.Deep);
		}

		// Token: 0x04005A53 RID: 23123
		public List<PsychicRitualParticipant> chanters;

		// Token: 0x04005A54 RID: 23124
		public HashSet<IntVec3> foundPositions;
	}
}
