using System;

namespace RimWorld
{
	// Token: 0x0200296E RID: 10606
	[Flags]
	public enum OverlayTypes
	{
		// Token: 0x04009B11 RID: 39697
		None = 0,
		// Token: 0x04009B12 RID: 39698
		NeedsPower = 1,
		// Token: 0x04009B13 RID: 39699
		PowerOff = 2,
		// Token: 0x04009B14 RID: 39700
		BurningWick = 4,
		// Token: 0x04009B15 RID: 39701
		Forbidden = 8,
		// Token: 0x04009B16 RID: 39702
		ForbiddenBig = 16,
		// Token: 0x04009B17 RID: 39703
		QuestionMark = 32,
		// Token: 0x04009B18 RID: 39704
		BrokenDown = 64,
		// Token: 0x04009B19 RID: 39705
		OutOfFuel = 128,
		// Token: 0x04009B1A RID: 39706
		ForbiddenRefuel = 256,
		// Token: 0x04009B1B RID: 39707
		SelfShutdown = 512,
		// Token: 0x04009B1C RID: 39708
		ForbiddenAtomizer = 1024
	}
}
