using System;

namespace Verse
{
	// Token: 0x020008DB RID: 2267
	public interface ISuspendableThingHolder : IThingHolder
	{
		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x060039EC RID: 14828
		bool IsContentsSuspended { get; }
	}
}
