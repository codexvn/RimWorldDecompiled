using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016F1 RID: 5873
	internal class LordToil_DefendTraderCaravan : LordToil_DefendPoint
	{
		// Token: 0x17001542 RID: 5442
		// (get) Token: 0x06008A87 RID: 35463 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001543 RID: 5443
		// (get) Token: 0x06008A88 RID: 35464 RVA: 0x00293245 File Offset: 0x00291445
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x06008A89 RID: 35465 RVA: 0x0029504C File Offset: 0x0029324C
		public LordToil_DefendTraderCaravan() : base(true)
		{
		}

		// Token: 0x06008A8A RID: 35466 RVA: 0x00295058 File Offset: 0x00293258
		public LordToil_DefendTraderCaravan(IntVec3 defendPoint) : base(defendPoint, null, null)
		{
		}

		// Token: 0x06008A8B RID: 35467 RVA: 0x00295080 File Offset: 0x00293280
		public override void UpdateAllDuties()
		{
			LordToilData_DefendPoint data = base.Data;
			Pawn pawn = TraderCaravanUtility.FindTrader(this.lord);
			if (pawn != null)
			{
				pawn.mindState.duty = new PawnDuty(DutyDefOf.Defend, data.defendPoint, data.defendRadius);
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn2 = this.lord.ownedPawns[i];
					switch (pawn2.GetTraderCaravanRole())
					{
					case TraderCaravanRole.Carrier:
						pawn2.mindState.duty = new PawnDuty(DutyDefOf.Follow, pawn, 5f);
						pawn2.mindState.duty.locomotion = LocomotionUrgency.Walk;
						break;
					case TraderCaravanRole.Guard:
						pawn2.mindState.duty = new PawnDuty(DutyDefOf.Defend, data.defendPoint, data.defendRadius);
						break;
					case TraderCaravanRole.Chattel:
						pawn2.mindState.duty = new PawnDuty(DutyDefOf.Escort, pawn, 5f);
						pawn2.mindState.duty.locomotion = LocomotionUrgency.Walk;
						break;
					}
				}
				return;
			}
		}
	}
}
