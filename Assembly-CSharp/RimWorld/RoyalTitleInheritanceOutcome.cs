using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003795 RID: 14229
	public struct RoyalTitleInheritanceOutcome
	{
		// Token: 0x1700325F RID: 12895
		// (get) Token: 0x06013B49 RID: 80713 RVA: 0x005DADB4 File Offset: 0x005D8FB4
		public bool FoundHeir
		{
			get
			{
				return this.heir != null;
			}
		}

		// Token: 0x17003260 RID: 12896
		// (get) Token: 0x06013B4A RID: 80714 RVA: 0x005DADBF File Offset: 0x005D8FBF
		public bool HeirHasTitle
		{
			get
			{
				return this.heirCurrentTitle != null;
			}
		}

		// Token: 0x0400DA43 RID: 55875
		public Pawn heir;

		// Token: 0x0400DA44 RID: 55876
		public RoyalTitleDef heirCurrentTitle;

		// Token: 0x0400DA45 RID: 55877
		public bool heirTitleHigher;
	}
}
