using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010A4 RID: 4260
	public class LordToil_ExitMapTraderFighting : LordToil
	{
		// Token: 0x17001030 RID: 4144
		// (get) Token: 0x0600689F RID: 26783 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001031 RID: 4145
		// (get) Token: 0x060068A0 RID: 26784 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060068A1 RID: 26785 RVA: 0x00210F6C File Offset: 0x0020F16C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				TraderCaravanRole traderCaravanRole = pawn.GetTraderCaravanRole();
				if (traderCaravanRole == TraderCaravanRole.Carrier || traderCaravanRole == TraderCaravanRole.Chattel)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBest);
					pawn.mindState.duty.locomotion = LocomotionUrgency.Jog;
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBestAndDefendSelf);
				}
			}
		}
	}
}
