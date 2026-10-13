using System;

namespace RimWorld
{
	// Token: 0x020023A9 RID: 9129
	[Flags]
	public enum PawnPosture : byte
	{
		// Token: 0x040086EB RID: 34539
		Standing = 0,
		// Token: 0x040086EC RID: 34540
		LayingOnGroundFaceUp = 3,
		// Token: 0x040086ED RID: 34541
		LayingOnGroundNormal = 1,
		// Token: 0x040086EE RID: 34542
		LayingInBed = 5,
		// Token: 0x040086EF RID: 34543
		LayingInBedFaceUp = 7,
		// Token: 0x040086F0 RID: 34544
		LayingMask = 1,
		// Token: 0x040086F1 RID: 34545
		FaceUpMask = 2,
		// Token: 0x040086F2 RID: 34546
		InBedMask = 4
	}
}
