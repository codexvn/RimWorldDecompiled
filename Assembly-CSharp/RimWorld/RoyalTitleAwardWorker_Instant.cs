using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003798 RID: 14232
	public class RoyalTitleAwardWorker_Instant : RoyalTitleAwardWorker
	{
		// Token: 0x06013B50 RID: 80720 RVA: 0x005DAE33 File Offset: 0x005D9033
		public override void DoAward(Pawn pawn, Faction faction, RoyalTitleDef currentTitle, RoyalTitleDef newTitle)
		{
			pawn.royalty.TryUpdateTitle(faction, true, this.def);
		}
	}
}
