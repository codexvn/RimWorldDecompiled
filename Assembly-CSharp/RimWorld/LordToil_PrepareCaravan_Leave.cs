using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016D7 RID: 5847
	public class LordToil_PrepareCaravan_Leave : LordToil
	{
		// Token: 0x1700152A RID: 5418
		// (get) Token: 0x06008A16 RID: 35350 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700152B RID: 5419
		// (get) Token: 0x06008A17 RID: 35351 RVA: 0x00293245 File Offset: 0x00291445
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x1700152C RID: 5420
		// (get) Token: 0x06008A18 RID: 35352 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700152D RID: 5421
		// (get) Token: 0x06008A19 RID: 35353 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008A1A RID: 35354 RVA: 0x00293490 File Offset: 0x00291690
		public LordToil_PrepareCaravan_Leave(IntVec3 exitSpot)
		{
			this.exitSpot = exitSpot;
		}

		// Token: 0x06008A1B RID: 35355 RVA: 0x002934A0 File Offset: 0x002916A0
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				pawn.mindState.duty = new PawnDuty(DutyDefOf.TravelOrWait, this.exitSpot, -1f);
				pawn.mindState.duty.locomotion = LocomotionUrgency.Jog;
			}
		}

		// Token: 0x06008A1C RID: 35356 RVA: 0x00293510 File Offset: 0x00291710
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0)
			{
				GatherAnimalsAndSlavesForCaravanUtility.CheckArrived(this.lord, this.lord.ownedPawns, this.exitSpot, "ReadyToExitMap", (Pawn x) => true, null);
			}
		}

		// Token: 0x04005A12 RID: 23058
		private IntVec3 exitSpot;
	}
}
