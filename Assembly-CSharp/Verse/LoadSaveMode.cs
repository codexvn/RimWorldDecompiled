using System;

namespace Verse
{
	// Token: 0x02000858 RID: 2136
	public enum LoadSaveMode : byte
	{
		// Token: 0x040028A5 RID: 10405
		Inactive,
		// Token: 0x040028A6 RID: 10406
		Saving,
		// Token: 0x040028A7 RID: 10407
		LoadingVars,
		// Token: 0x040028A8 RID: 10408
		ResolvingCrossRefs,
		// Token: 0x040028A9 RID: 10409
		PostLoadInit
	}
}
