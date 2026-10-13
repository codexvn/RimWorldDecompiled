using System;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x02001007 RID: 4103
	public class JobGiver_ExitMapFollowOverseer : JobGiver_ExitMapBest
	{
		// Token: 0x060064A7 RID: 25767 RVA: 0x001FE075 File Offset: 0x001FC275
		public JobGiver_ExitMapFollowOverseer()
		{
			this.failIfCantJoinOrCreateCaravan = true;
		}

		// Token: 0x060064A8 RID: 25768 RVA: 0x001FE084 File Offset: 0x001FC284
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(pawn))
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}
	}
}
