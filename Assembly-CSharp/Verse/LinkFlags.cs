using System;

namespace Verse
{
	// Token: 0x0200023E RID: 574
	[Flags]
	public enum LinkFlags
	{
		// Token: 0x040009B8 RID: 2488
		None = 0,
		// Token: 0x040009B9 RID: 2489
		MapEdge = 1,
		// Token: 0x040009BA RID: 2490
		Rock = 2,
		// Token: 0x040009BB RID: 2491
		Wall = 4,
		// Token: 0x040009BC RID: 2492
		Sandbags = 8,
		// Token: 0x040009BD RID: 2493
		PowerConduit = 16,
		// Token: 0x040009BE RID: 2494
		Barricades = 32,
		// Token: 0x040009BF RID: 2495
		Fences = 64,
		// Token: 0x040009C0 RID: 2496
		Fleshmass = 128,
		// Token: 0x040009C1 RID: 2497
		SolidIce = 256,
		// Token: 0x040009C2 RID: 2498
		BurrowWall = 512,
		// Token: 0x040009C3 RID: 2499
		Custom1 = 131072,
		// Token: 0x040009C4 RID: 2500
		Custom2 = 262144,
		// Token: 0x040009C5 RID: 2501
		Custom3 = 524288,
		// Token: 0x040009C6 RID: 2502
		Custom4 = 1048576,
		// Token: 0x040009C7 RID: 2503
		Custom5 = 2097152,
		// Token: 0x040009C8 RID: 2504
		Custom6 = 4194304,
		// Token: 0x040009C9 RID: 2505
		Custom7 = 8388608,
		// Token: 0x040009CA RID: 2506
		Custom8 = 16777216,
		// Token: 0x040009CB RID: 2507
		Custom9 = 33554432,
		// Token: 0x040009CC RID: 2508
		Custom10 = 67108864
	}
}
