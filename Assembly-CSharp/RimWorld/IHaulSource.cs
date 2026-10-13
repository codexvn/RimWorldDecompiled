using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020029A9 RID: 10665
	public interface IHaulSource : IStoreSettingsParent, IThingHolder
	{
		// Token: 0x17002419 RID: 9241
		// (get) Token: 0x0600EB0F RID: 60175
		Map Map { get; }

		// Token: 0x1700241A RID: 9242
		// (get) Token: 0x0600EB10 RID: 60176
		bool HaulSourceEnabled { get; }
	}
}
