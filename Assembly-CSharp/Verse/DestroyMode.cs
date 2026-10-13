using System;

namespace Verse
{
	// Token: 0x020008CB RID: 2251
	public enum DestroyMode : byte
	{
		// Token: 0x04002A13 RID: 10771
		Vanish,
		// Token: 0x04002A14 RID: 10772
		WillReplace,
		// Token: 0x04002A15 RID: 10773
		KillFinalize,
		// Token: 0x04002A16 RID: 10774
		KillFinalizeLeavingsOnly,
		// Token: 0x04002A17 RID: 10775
		Deconstruct,
		// Token: 0x04002A18 RID: 10776
		FailConstruction,
		// Token: 0x04002A19 RID: 10777
		Cancel,
		// Token: 0x04002A1A RID: 10778
		Refund,
		// Token: 0x04002A1B RID: 10779
		QuestLogic
	}
}
