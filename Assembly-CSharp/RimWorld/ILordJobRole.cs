using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020027AC RID: 10156
	public interface ILordJobRole
	{
		// Token: 0x17002205 RID: 8709
		// (get) Token: 0x0600DE93 RID: 56979
		int MaxCount { get; }

		// Token: 0x17002206 RID: 8710
		// (get) Token: 0x0600DE94 RID: 56980
		int MinCount { get; }

		// Token: 0x17002207 RID: 8711
		// (get) Token: 0x0600DE95 RID: 56981
		TaggedString Label { get; }

		// Token: 0x17002208 RID: 8712
		// (get) Token: 0x0600DE96 RID: 56982
		TaggedString LabelCap { get; }

		// Token: 0x17002209 RID: 8713
		// (get) Token: 0x0600DE97 RID: 56983
		TaggedString CategoryLabel { get; }

		// Token: 0x1700220A RID: 8714
		// (get) Token: 0x0600DE98 RID: 56984
		TaggedString CategoryLabelCap { get; }
	}
}
