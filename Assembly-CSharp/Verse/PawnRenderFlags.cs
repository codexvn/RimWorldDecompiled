using System;

namespace Verse
{
	// Token: 0x020005FB RID: 1531
	[Flags]
	public enum PawnRenderFlags : uint
	{
		// Token: 0x0400208E RID: 8334
		None = 0U,
		// Token: 0x0400208F RID: 8335
		Portrait = 1U,
		// Token: 0x04002090 RID: 8336
		HeadStump = 2U,
		// Token: 0x04002091 RID: 8337
		Invisible = 4U,
		// Token: 0x04002092 RID: 8338
		DrawNow = 8U,
		// Token: 0x04002093 RID: 8339
		Cache = 16U,
		// Token: 0x04002094 RID: 8340
		Headgear = 32U,
		// Token: 0x04002095 RID: 8341
		Clothes = 64U,
		// Token: 0x04002096 RID: 8342
		NeverAimWeapon = 128U,
		// Token: 0x04002097 RID: 8343
		StylingStation = 256U,
		// Token: 0x04002098 RID: 8344
		NoBody = 512U,
		// Token: 0x04002099 RID: 8345
		Statue = 1024U
	}
}
