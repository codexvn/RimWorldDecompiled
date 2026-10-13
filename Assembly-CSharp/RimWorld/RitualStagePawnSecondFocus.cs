using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016BC RID: 5820
	public class RitualStagePawnSecondFocus : IExposable
	{
		// Token: 0x06008925 RID: 35109 RVA: 0x0028E434 File Offset: 0x0028C634
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.stageIndex, "stageIndex", 0, false);
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_TargetInfo.Look(ref this.target, "target");
		}

		// Token: 0x040059A1 RID: 22945
		public int stageIndex;

		// Token: 0x040059A2 RID: 22946
		public Pawn pawn;

		// Token: 0x040059A3 RID: 22947
		public TargetInfo target;
	}
}
