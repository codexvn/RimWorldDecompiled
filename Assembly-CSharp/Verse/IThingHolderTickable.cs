using System;

namespace Verse
{
	// Token: 0x020008DD RID: 2269
	public interface IThingHolderTickable : IThingHolder
	{
		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x060039F0 RID: 14832
		bool ShouldTickContents { get; }
	}
}
