using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001711 RID: 5905
	public class LordToil_PrisonerEscape : LordToil_Travel
	{
		// Token: 0x1700155D RID: 5469
		// (get) Token: 0x06008B0C RID: 35596 RVA: 0x002970B8 File Offset: 0x002952B8
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.dest;
			}
		}

		// Token: 0x1700155E RID: 5470
		// (get) Token: 0x06008B0D RID: 35597 RVA: 0x002110EC File Offset: 0x0020F2EC
		private new LordToilData_Travel Data
		{
			get
			{
				return (LordToilData_Travel)this.data;
			}
		}

		// Token: 0x1700155F RID: 5471
		// (get) Token: 0x06008B0E RID: 35598 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001560 RID: 5472
		// (get) Token: 0x06008B0F RID: 35599 RVA: 0x002970C5 File Offset: 0x002952C5
		protected override float AllArrivedCheckRadius
		{
			get
			{
				return 14f;
			}
		}

		// Token: 0x06008B10 RID: 35600 RVA: 0x002970CC File Offset: 0x002952CC
		public LordToil_PrisonerEscape(IntVec3 dest, int sapperThingID) : base(dest)
		{
			this.sapperThingID = sapperThingID;
		}

		// Token: 0x06008B11 RID: 35601 RVA: 0x002970DC File Offset: 0x002952DC
		public override void UpdateAllDuties()
		{
			LordToilData_Travel data = this.Data;
			Pawn leader = this.GetLeader();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (this.IsSapper(pawn))
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrisonerEscapeSapper, data.dest, -1f);
				}
				else if (leader == null || pawn == leader)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrisonerEscape, data.dest, -1f);
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrisonerEscape, leader, 10f);
				}
			}
		}

		// Token: 0x06008B12 RID: 35602 RVA: 0x002971A8 File Offset: 0x002953A8
		public override void LordToilTick()
		{
			base.LordToilTick();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].guilt.Notify_Guilty(60000);
			}
		}

		// Token: 0x06008B13 RID: 35603 RVA: 0x002971F8 File Offset: 0x002953F8
		private Pawn GetLeader()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				if (!this.lord.ownedPawns[i].Downed && this.IsSapper(this.lord.ownedPawns[i]))
				{
					return this.lord.ownedPawns[i];
				}
			}
			for (int j = 0; j < this.lord.ownedPawns.Count; j++)
			{
				if (!this.lord.ownedPawns[j].Downed)
				{
					return this.lord.ownedPawns[j];
				}
			}
			return null;
		}

		// Token: 0x06008B14 RID: 35604 RVA: 0x002972A9 File Offset: 0x002954A9
		private bool IsSapper(Pawn p)
		{
			return p.thingIDNumber == this.sapperThingID;
		}

		// Token: 0x04005A64 RID: 23140
		private int sapperThingID;
	}
}
