using System;

namespace Verse
{
	// Token: 0x0200054E RID: 1358
	public interface IRenameable
	{
		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06002332 RID: 9010
		// (set) Token: 0x06002333 RID: 9011
		string RenamableLabel { get; set; }

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06002334 RID: 9012
		string BaseLabel { get; }

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06002335 RID: 9013
		string InspectLabel { get; }
	}
}
