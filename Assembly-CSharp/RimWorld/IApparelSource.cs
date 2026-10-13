using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020029A6 RID: 10662
	public interface IApparelSource : IThingHolder
	{
		// Token: 0x17002413 RID: 9235
		// (get) Token: 0x0600EB06 RID: 60166
		Map Map { get; }

		// Token: 0x17002414 RID: 9236
		// (get) Token: 0x0600EB07 RID: 60167
		bool ApparelSourceEnabled { get; }

		// Token: 0x0600EB08 RID: 60168
		bool RemoveApparel(Apparel apparel);
	}
}
