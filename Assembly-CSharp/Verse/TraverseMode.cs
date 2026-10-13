using System;

namespace Verse
{
	// Token: 0x020001CD RID: 461
	public enum TraverseMode : byte
	{
		// Token: 0x04000730 RID: 1840
		ByPawn,
		// Token: 0x04000731 RID: 1841
		PassDoors,
		// Token: 0x04000732 RID: 1842
		NoPassClosedDoors,
		// Token: 0x04000733 RID: 1843
		PassAllDestroyableThings,
		// Token: 0x04000734 RID: 1844
		PassAllDestroyablePlayerOwnedThings,
		// Token: 0x04000735 RID: 1845
		NoPassClosedDoorsOrWater,
		// Token: 0x04000736 RID: 1846
		PassAllDestroyableThingsNotWater
	}
}
