using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020029A8 RID: 10664
	public interface IHaulEnroute : ILoadReferenceable
	{
		// Token: 0x17002418 RID: 9240
		// (get) Token: 0x0600EB0D RID: 60173
		Map Map { get; }

		// Token: 0x0600EB0E RID: 60174
		int SpaceRemainingFor(ThingDef stuff);
	}
}
