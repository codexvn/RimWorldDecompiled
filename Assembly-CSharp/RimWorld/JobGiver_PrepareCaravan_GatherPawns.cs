using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001426 RID: 5158
	public class JobGiver_PrepareCaravan_GatherPawns : JobGiver_PrepareCaravan_RopePawns
	{
		// Token: 0x17001398 RID: 5016
		// (get) Token: 0x06007D61 RID: 32097 RVA: 0x0025C284 File Offset: 0x0025A484
		protected override JobDef RopeJobDef
		{
			get
			{
				return JobDefOf.PrepareCaravan_GatherAnimals;
			}
		}

		// Token: 0x06007D62 RID: 32098 RVA: 0x0025C28B File Offset: 0x0025A48B
		protected override bool AnimalNeedsGathering(Pawn pawn, Pawn animal)
		{
			return JobGiver_PrepareCaravan_GatherPawns.DoesAnimalNeedGathering(pawn, animal);
		}

		// Token: 0x06007D63 RID: 32099 RVA: 0x0025C294 File Offset: 0x0025A494
		public static bool DoesAnimalNeedGathering(Pawn pawn, Pawn animal)
		{
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			return AnimalPenUtility.NeedsToBeManagedByRope(animal) && !animal.roping.IsRopedByPawn && (!animal.roping.IsRopedToSpot || !(animal.roping.RopedToSpot == cell)) && pawn.GetLord() == animal.GetLord() && GatherAnimalsAndSlavesForCaravanUtility.CanRoperTakeAnimalToDest(pawn, animal, cell) && pawn.CanReserve(animal, 1, -1, null, false);
		}
	}
}
