using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016D5 RID: 5845
	public class LordToil_PrepareCaravan_GatherDownedPawns : LordToil
	{
		// Token: 0x17001526 RID: 5414
		// (get) Token: 0x06008A0A RID: 35338 RVA: 0x00293245 File Offset: 0x00291445
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x17001527 RID: 5415
		// (get) Token: 0x06008A0B RID: 35339 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008A0C RID: 35340 RVA: 0x00293251 File Offset: 0x00291451
		public LordToil_PrepareCaravan_GatherDownedPawns(IntVec3 meetingPoint, IntVec3 exitSpot)
		{
			this.meetingPoint = meetingPoint;
			this.exitSpot = exitSpot;
		}

		// Token: 0x06008A0D RID: 35341 RVA: 0x00293268 File Offset: 0x00291468
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.IsColonist)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_GatherDownedPawns, this.meetingPoint, this.exitSpot, -1f);
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait, this.meetingPoint, -1f);
				}
			}
		}

		// Token: 0x06008A0E RID: 35342 RVA: 0x00293304 File Offset: 0x00291504
		private void CheckAllPawnsArrived()
		{
			bool flag = true;
			List<Pawn> downedPawns = ((LordJob_FormAndSendCaravan)this.lord.LordJob).downedPawns;
			for (int i = 0; i < downedPawns.Count; i++)
			{
				if (!JobGiver_PrepareCaravan_GatherDownedPawns.IsDownedPawnNearExitPoint(downedPawns[i], this.exitSpot))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				this.lord.ReceiveMemo("AllDownedPawnsGathered");
			}
		}

		// Token: 0x06008A0F RID: 35343 RVA: 0x00293365 File Offset: 0x00291565
		public override void Notify_PawnJobDone(Pawn p, JobCondition condition)
		{
			this.CheckAllPawnsArrived();
		}

		// Token: 0x06008A10 RID: 35344 RVA: 0x00293365 File Offset: 0x00291565
		public override void Notify_ReachedDutyLocation(Pawn pawn)
		{
			this.CheckAllPawnsArrived();
		}

		// Token: 0x04005A0F RID: 23055
		private IntVec3 meetingPoint;

		// Token: 0x04005A10 RID: 23056
		private IntVec3 exitSpot;
	}
}
