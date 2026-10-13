using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003797 RID: 14231
	public class RoyalTitleAwardWorker
	{
		// Token: 0x06013B4D RID: 80717 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void OnPreAward(Pawn pawn, Faction faction, RoyalTitleDef currentTitle, RoyalTitleDef newTitle)
		{
		}

		// Token: 0x06013B4E RID: 80718 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void DoAward(Pawn pawn, Faction faction, RoyalTitleDef currentTitle, RoyalTitleDef newTitle)
		{
		}

		// Token: 0x0400DA49 RID: 55881
		public RoyalTitleDef def;
	}
}
