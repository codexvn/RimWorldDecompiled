using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001765 RID: 5989
	public class TriggerData_PawnCycleInd : TriggerData
	{
		// Token: 0x06008C86 RID: 35974 RVA: 0x0029CA7A File Offset: 0x0029AC7A
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.pawnCycleInd, "pawnCycleInd", 0, false);
		}

		// Token: 0x04005B56 RID: 23382
		public int pawnCycleInd;
	}
}
