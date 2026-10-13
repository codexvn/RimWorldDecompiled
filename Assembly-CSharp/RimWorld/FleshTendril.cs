using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CC5 RID: 11461
	internal class FleshTendril : IExposable
	{
		// Token: 0x0601016C RID: 65900 RVA: 0x004A781C File Offset: 0x004A5A1C
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.sourceNode, "sourceNode", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.currentPos, "currentPos", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.length, "length", 0, false);
		}

		// Token: 0x0400A813 RID: 43027
		public IntVec3 sourceNode;

		// Token: 0x0400A814 RID: 43028
		public IntVec3 currentPos;

		// Token: 0x0400A815 RID: 43029
		public int length;
	}
}
