using System;

namespace Verse.AI
{
	// Token: 0x02000E79 RID: 3705
	[Flags]
	public enum JobCondition : byte
	{
		// Token: 0x040043AF RID: 17327
		None = 0,
		// Token: 0x040043B0 RID: 17328
		Ongoing = 1,
		// Token: 0x040043B1 RID: 17329
		Succeeded = 2,
		// Token: 0x040043B2 RID: 17330
		Incompletable = 4,
		// Token: 0x040043B3 RID: 17331
		InterruptOptional = 8,
		// Token: 0x040043B4 RID: 17332
		InterruptForced = 16,
		// Token: 0x040043B5 RID: 17333
		QueuedNoLongerValid = 32,
		// Token: 0x040043B6 RID: 17334
		Errored = 64,
		// Token: 0x040043B7 RID: 17335
		ErroredPather = 128
	}
}
