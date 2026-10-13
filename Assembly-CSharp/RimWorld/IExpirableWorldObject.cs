using System;

namespace RimWorld
{
	// Token: 0x0200283A RID: 10298
	public interface IExpirableWorldObject
	{
		// Token: 0x17002260 RID: 8800
		// (get) Token: 0x0600E162 RID: 57698
		// (set) Token: 0x0600E163 RID: 57699
		int ExpireAtTicks { get; set; }
	}
}
