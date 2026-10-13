using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010A7 RID: 4263
	public class LordToil_Travel : LordToil
	{
		// Token: 0x17001034 RID: 4148
		// (get) Token: 0x060068A9 RID: 26793 RVA: 0x002110DF File Offset: 0x0020F2DF
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.dest;
			}
		}

		// Token: 0x17001035 RID: 4149
		// (get) Token: 0x060068AA RID: 26794 RVA: 0x002110EC File Offset: 0x0020F2EC
		protected LordToilData_Travel Data
		{
			get
			{
				return (LordToilData_Travel)this.data;
			}
		}

		// Token: 0x17001036 RID: 4150
		// (get) Token: 0x060068AB RID: 26795 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001037 RID: 4151
		// (get) Token: 0x060068AC RID: 26796 RVA: 0x0019D0E8 File Offset: 0x0019B2E8
		protected virtual float AllArrivedCheckRadius
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x060068AD RID: 26797 RVA: 0x002110F9 File Offset: 0x0020F2F9
		public LordToil_Travel(IntVec3 dest)
		{
			this.data = new LordToilData_Travel();
			this.Data.dest = dest;
		}

		// Token: 0x060068AE RID: 26798 RVA: 0x00211118 File Offset: 0x0020F318
		public override void UpdateAllDuties()
		{
			LordToilData_Travel data = this.Data;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.TravelOrLeave, data.dest, -1f);
				pawnDuty.maxDanger = this.maxDanger;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x060068AF RID: 26799 RVA: 0x0021118C File Offset: 0x0020F38C
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 205 == 0)
			{
				LordToilData_Travel data = this.Data;
				bool flag = true;
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = this.lord.ownedPawns[i];
					if (!pawn.Position.InHorDistOf(data.dest, this.AllArrivedCheckRadius) || !pawn.CanReach(data.dest, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					this.lord.ReceiveMemo("TravelArrived");
				}
			}
		}

		// Token: 0x060068B0 RID: 26800 RVA: 0x00211230 File Offset: 0x0020F430
		public bool HasDestination()
		{
			return this.Data.dest.IsValid;
		}

		// Token: 0x060068B1 RID: 26801 RVA: 0x00211242 File Offset: 0x0020F442
		public void SetDestination(IntVec3 dest)
		{
			this.Data.dest = dest;
		}

		// Token: 0x04004A3B RID: 19003
		public Danger maxDanger;
	}
}
