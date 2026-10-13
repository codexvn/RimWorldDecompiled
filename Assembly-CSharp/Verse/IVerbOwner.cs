using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000D6D RID: 3437
	public interface IVerbOwner
	{
		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x0600577A RID: 22394
		VerbTracker VerbTracker { get; }

		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x0600577B RID: 22395
		List<VerbProperties> VerbProperties { get; }

		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x0600577C RID: 22396
		List<Tool> Tools { get; }

		// Token: 0x17000DC3 RID: 3523
		// (get) Token: 0x0600577D RID: 22397
		ImplementOwnerTypeDef ImplementOwnerTypeDef { get; }

		// Token: 0x0600577E RID: 22398
		string UniqueVerbOwnerID();

		// Token: 0x0600577F RID: 22399
		bool VerbsStillUsableBy(Pawn p);

		// Token: 0x17000DC4 RID: 3524
		// (get) Token: 0x06005780 RID: 22400
		Thing ConstantCaster { get; }
	}
}
