using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200311E RID: 12574
	public interface ILordJobOutcomePossibility
	{
		// Token: 0x17002DE9 RID: 11753
		// (get) Token: 0x06011A95 RID: 72341
		TaggedString Label { get; }

		// Token: 0x17002DEA RID: 11754
		// (get) Token: 0x06011A96 RID: 72342
		TaggedString ToolTip { get; }

		// Token: 0x06011A97 RID: 72343
		float Weight(FloatRange qualityRange);
	}
}
