using System;

namespace RimWorld
{
	// Token: 0x02001995 RID: 6549
	[Flags]
	public enum FilthSourceFlags
	{
		// Token: 0x04005FFF RID: 24575
		None = 0,
		// Token: 0x04006000 RID: 24576
		Terrain = 1,
		// Token: 0x04006001 RID: 24577
		Natural = 2,
		// Token: 0x04006002 RID: 24578
		Unnatural = 4,
		// Token: 0x04006003 RID: 24579
		Pawn = 8,
		// Token: 0x04006004 RID: 24580
		Any = 15
	}
}
