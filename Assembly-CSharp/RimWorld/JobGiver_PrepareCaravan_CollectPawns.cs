using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001427 RID: 5159
	public class JobGiver_PrepareCaravan_CollectPawns : JobGiver_PrepareCaravan_RopePawns
	{
		// Token: 0x17001399 RID: 5017
		// (get) Token: 0x06007D65 RID: 32101 RVA: 0x0025C31E File Offset: 0x0025A51E
		protected override JobDef RopeJobDef
		{
			get
			{
				return JobDefOf.PrepareCaravan_CollectAnimals;
			}
		}

		// Token: 0x06007D66 RID: 32102 RVA: 0x0025C325 File Offset: 0x0025A525
		protected override bool AnimalNeedsGathering(Pawn pawn, Pawn animal)
		{
			return JobGiver_PrepareCaravan_CollectPawns.DoesAnimalNeedGathering(pawn, animal);
		}

		// Token: 0x06007D67 RID: 32103 RVA: 0x0025C330 File Offset: 0x0025A530
		public static bool DoesAnimalNeedGathering(Pawn pawn, Pawn animal)
		{
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			return AnimalPenUtility.NeedsToBeManagedByRope(animal) && !GatherAnimalsAndSlavesForCaravanUtility.IsRopedByCaravanPawn(animal) && pawn.GetLord() == animal.GetLord() && GatherAnimalsAndSlavesForCaravanUtility.CanRoperTakeAnimalToDest(pawn, animal, cell) && pawn.CanReserve(animal, 1, -1, null, false);
		}
	}
}
