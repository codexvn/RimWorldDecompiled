using System;

namespace LudeonTK
{
	// Token: 0x0200006B RID: 107
	[Flags]
	public enum AllowedGameStates
	{
		// Token: 0x0400012F RID: 303
		Invalid = 0,
		// Token: 0x04000130 RID: 304
		Entry = 1,
		// Token: 0x04000131 RID: 305
		Playing = 2,
		// Token: 0x04000132 RID: 306
		WorldRenderedNow = 4,
		// Token: 0x04000133 RID: 307
		IsCurrentlyOnMap = 8,
		// Token: 0x04000134 RID: 308
		HasGameCondition = 16,
		// Token: 0x04000135 RID: 309
		PlayingOnMap = 10,
		// Token: 0x04000136 RID: 310
		PlayingOnWorld = 6
	}
}
