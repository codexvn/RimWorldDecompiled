using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B45 RID: 11077
	public interface IActiveTransporter : IThingHolder
	{
		// Token: 0x170026DD RID: 9949
		// (get) Token: 0x0600F7A5 RID: 63397
		ActiveTransporterInfo Contents { get; }
	}
}
