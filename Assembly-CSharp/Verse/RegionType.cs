using System;

namespace Verse
{
	// Token: 0x0200050A RID: 1290
	[Flags]
	public enum RegionType
	{
		// Token: 0x04001C44 RID: 7236
		None = 0,
		// Token: 0x04001C45 RID: 7237
		ImpassableFreeAirExchange = 1,
		// Token: 0x04001C46 RID: 7238
		Normal = 2,
		// Token: 0x04001C47 RID: 7239
		Portal = 4,
		// Token: 0x04001C48 RID: 7240
		Fence = 8,
		// Token: 0x04001C49 RID: 7241
		Set_Passable = 14,
		// Token: 0x04001C4A RID: 7242
		Set_Impassable = 1,
		// Token: 0x04001C4B RID: 7243
		Set_All = 15
	}
}
