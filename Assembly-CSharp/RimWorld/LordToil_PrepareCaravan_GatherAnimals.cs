using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020016DB RID: 5851
	public class LordToil_PrepareCaravan_GatherAnimals : LordToil_PrepareCaravan_RopeAnimals
	{
		// Token: 0x06008A27 RID: 35367 RVA: 0x0029367C File Offset: 0x0029187C
		public LordToil_PrepareCaravan_GatherAnimals(IntVec3 destinationPoint) : base(destinationPoint, null)
		{
		}

		// Token: 0x06008A28 RID: 35368 RVA: 0x00293699 File Offset: 0x00291899
		protected override PawnDuty MakeRopeDuty()
		{
			return new PawnDuty(DutyDefOf.PrepareCaravan_GatherAnimals, this.destinationPoint, -1f);
		}

		// Token: 0x06008A29 RID: 35369 RVA: 0x002936B8 File Offset: 0x002918B8
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0)
			{
				GatherAnimalsAndSlavesForCaravanUtility.CheckArrived(this.lord, this.lord.ownedPawns, this.destinationPoint, "AllAnimalsGathered", new Predicate<Pawn>(AnimalPenUtility.NeedsToBeManagedByRope), (Pawn x) => x.roping.RopedToSpot == this.destinationPoint);
			}
		}
	}
}
