using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016DA RID: 5850
	public abstract class LordToil_PrepareCaravan_RopeAnimals : LordToil
	{
		// Token: 0x1700152E RID: 5422
		// (get) Token: 0x06008A22 RID: 35362 RVA: 0x00293245 File Offset: 0x00291445
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x1700152F RID: 5423
		// (get) Token: 0x06008A23 RID: 35363 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008A24 RID: 35364 RVA: 0x002935C9 File Offset: 0x002917C9
		public LordToil_PrepareCaravan_RopeAnimals(IntVec3 destinationPoint, int? ropeeLimit)
		{
			this.destinationPoint = destinationPoint;
			this.ropeeLimit = ropeeLimit;
		}

		// Token: 0x06008A25 RID: 35365 RVA: 0x002935E0 File Offset: 0x002917E0
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.IsColonist || AnimalPenUtility.NeedsToBeManagedByRope(pawn))
				{
					pawn.mindState.duty = this.MakeRopeDuty();
					pawn.mindState.duty.ropeeLimit = this.ropeeLimit;
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait, this.destinationPoint, -1f);
				}
			}
		}

		// Token: 0x06008A26 RID: 35366
		protected abstract PawnDuty MakeRopeDuty();

		// Token: 0x04005A15 RID: 23061
		protected IntVec3 destinationPoint;

		// Token: 0x04005A16 RID: 23062
		protected int? ropeeLimit;
	}
}
