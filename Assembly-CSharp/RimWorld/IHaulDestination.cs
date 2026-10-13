using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020029A7 RID: 10663
	public interface IHaulDestination : IStoreSettingsParent
	{
		// Token: 0x17002415 RID: 9237
		// (get) Token: 0x0600EB09 RID: 60169
		IntVec3 Position { get; }

		// Token: 0x17002416 RID: 9238
		// (get) Token: 0x0600EB0A RID: 60170
		Map Map { get; }

		// Token: 0x17002417 RID: 9239
		// (get) Token: 0x0600EB0B RID: 60171
		bool HaulDestinationEnabled { get; }

		// Token: 0x0600EB0C RID: 60172
		bool Accepts(Thing t);
	}
}
