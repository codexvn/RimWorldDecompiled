using System;

namespace Verse.AI
{
	// Token: 0x02001032 RID: 4146
	[Flags]
	public enum TargetScanFlags
	{
		// Token: 0x0400483A RID: 18490
		None = 0,
		// Token: 0x0400483B RID: 18491
		NeedLOSToPawns = 1,
		// Token: 0x0400483C RID: 18492
		NeedLOSToNonPawns = 2,
		// Token: 0x0400483D RID: 18493
		NeedLOSToAll = 3,
		// Token: 0x0400483E RID: 18494
		NeedReachable = 4,
		// Token: 0x0400483F RID: 18495
		NeedReachableIfCantHitFromMyPos = 8,
		// Token: 0x04004840 RID: 18496
		NeedNonBurning = 16,
		// Token: 0x04004841 RID: 18497
		NeedThreat = 32,
		// Token: 0x04004842 RID: 18498
		NeedActiveThreat = 64,
		// Token: 0x04004843 RID: 18499
		LOSBlockableByGas = 128,
		// Token: 0x04004844 RID: 18500
		NeedAutoTargetable = 256,
		// Token: 0x04004845 RID: 18501
		NeedNotUnderThickRoof = 512,
		// Token: 0x04004846 RID: 18502
		IgnoreNonCombatants = 1024
	}
}
