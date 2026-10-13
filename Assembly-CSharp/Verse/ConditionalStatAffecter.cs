using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020001B8 RID: 440
	public abstract class ConditionalStatAffecter
	{
		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000C44 RID: 3140
		public abstract string Label { get; }

		// Token: 0x06000C45 RID: 3141
		public abstract bool Applies(StatRequest req);

		// Token: 0x0400070A RID: 1802
		public List<StatModifier> statFactors;

		// Token: 0x0400070B RID: 1803
		public List<StatModifier> statOffsets;
	}
}
