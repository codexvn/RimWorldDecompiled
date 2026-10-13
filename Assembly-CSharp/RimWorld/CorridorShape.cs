using System;

namespace RimWorld
{
	// Token: 0x02001F43 RID: 8003
	[Flags]
	public enum CorridorShape : byte
	{
		// Token: 0x04007936 RID: 31030
		Straight = 1,
		// Token: 0x04007937 RID: 31031
		Cross = 2,
		// Token: 0x04007938 RID: 31032
		T = 4,
		// Token: 0x04007939 RID: 31033
		H = 8,
		// Token: 0x0400793A RID: 31034
		AsymmetricCross = 16,
		// Token: 0x0400793B RID: 31035
		All = 31
	}
}
