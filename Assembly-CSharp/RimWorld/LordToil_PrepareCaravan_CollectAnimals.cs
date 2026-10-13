using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020016DC RID: 5852
	public class LordToil_PrepareCaravan_CollectAnimals : LordToil_PrepareCaravan_RopeAnimals
	{
		// Token: 0x06008A2B RID: 35371 RVA: 0x00293725 File Offset: 0x00291925
		public LordToil_PrepareCaravan_CollectAnimals(IntVec3 destinationPoint) : base(destinationPoint, new int?(int.MaxValue))
		{
		}

		// Token: 0x06008A2C RID: 35372 RVA: 0x00293738 File Offset: 0x00291938
		protected override PawnDuty MakeRopeDuty()
		{
			return new PawnDuty(DutyDefOf.PrepareCaravan_CollectAnimals, this.destinationPoint, -1f);
		}

		// Token: 0x06008A2D RID: 35373 RVA: 0x00293754 File Offset: 0x00291954
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0)
			{
				bool flag = true;
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = this.lord.ownedPawns[i];
					if (AnimalPenUtility.NeedsToBeManagedByRope(pawn) && !GatherAnimalsAndSlavesForCaravanUtility.IsRopedByCaravanPawn(pawn))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					this.lord.ReceiveMemo("AllAnimalsCollected");
				}
			}
		}
	}
}
