using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014E2 RID: 5346
	public class JobGiver_IdleJoy : JobGiver_GetJoy
	{
		// Token: 0x06007FB4 RID: 32692 RVA: 0x00266F0D File Offset: 0x0026510D
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.needs.joy == null)
			{
				return null;
			}
			if (Find.TickManager.TicksGame < 60000)
			{
				return null;
			}
			if (JoyUtility.LordPreventsGettingJoy(pawn) || JoyUtility.TimetablePreventsGettingJoy(pawn))
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x0400556B RID: 21867
		private const int GameStartNoIdleJoyTicks = 60000;
	}
}
