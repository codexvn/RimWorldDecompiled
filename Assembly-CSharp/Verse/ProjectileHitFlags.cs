using System;

namespace Verse
{
	// Token: 0x020008C4 RID: 2244
	[Flags]
	public enum ProjectileHitFlags
	{
		// Token: 0x040029F7 RID: 10743
		None = 0,
		// Token: 0x040029F8 RID: 10744
		IntendedTarget = 1,
		// Token: 0x040029F9 RID: 10745
		NonTargetPawns = 2,
		// Token: 0x040029FA RID: 10746
		NonTargetWorld = 4,
		// Token: 0x040029FB RID: 10747
		All = -1
	}
}
