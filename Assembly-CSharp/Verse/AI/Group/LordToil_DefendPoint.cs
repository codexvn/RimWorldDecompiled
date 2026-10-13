using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200109D RID: 4253
	public class LordToil_DefendPoint : LordToil
	{
		// Token: 0x17001027 RID: 4135
		// (get) Token: 0x06006887 RID: 26759 RVA: 0x00210B9D File Offset: 0x0020ED9D
		protected LordToilData_DefendPoint Data
		{
			get
			{
				return (LordToilData_DefendPoint)this.data;
			}
		}

		// Token: 0x17001028 RID: 4136
		// (get) Token: 0x06006888 RID: 26760 RVA: 0x00210BAA File Offset: 0x0020EDAA
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.defendPoint;
			}
		}

		// Token: 0x17001029 RID: 4137
		// (get) Token: 0x06006889 RID: 26761 RVA: 0x00210BB7 File Offset: 0x0020EDB7
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return this.allowSatisfyLongNeeds;
			}
		}

		// Token: 0x0600688A RID: 26762 RVA: 0x00210BBF File Offset: 0x0020EDBF
		public LordToil_DefendPoint(bool canSatisfyLongNeeds = true)
		{
			this.allowSatisfyLongNeeds = canSatisfyLongNeeds;
			this.data = new LordToilData_DefendPoint();
		}

		// Token: 0x0600688B RID: 26763 RVA: 0x00210BE0 File Offset: 0x0020EDE0
		public LordToil_DefendPoint(IntVec3 defendPoint, float? defendRadius = null, float? wanderRadius = null) : this(true)
		{
			this.Data.defendPoint = defendPoint;
			this.Data.defendRadius = (defendRadius ?? 28f);
			this.Data.wanderRadius = wanderRadius;
		}

		// Token: 0x0600688C RID: 26764 RVA: 0x00210C30 File Offset: 0x0020EE30
		public override void UpdateAllDuties()
		{
			LordToilData_DefendPoint data = this.Data;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (((pawn != null) ? pawn.mindState : null) != null)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.Defend, data.defendPoint, -1f);
					pawn.mindState.duty.focusSecond = data.defendPoint;
					pawn.mindState.duty.radius = ((pawn.kindDef.defendPointRadius >= 0f) ? pawn.kindDef.defendPointRadius : data.defendRadius);
					pawn.mindState.duty.wanderRadius = data.wanderRadius;
				}
			}
		}

		// Token: 0x0600688D RID: 26765 RVA: 0x00210D13 File Offset: 0x0020EF13
		public void SetDefendPoint(IntVec3 defendPoint)
		{
			this.Data.defendPoint = defendPoint;
		}

		// Token: 0x04004A30 RID: 18992
		private bool allowSatisfyLongNeeds = true;
	}
}
