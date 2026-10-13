using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001132 RID: 4402
	public class JobDriver_PrepareCaravan_CollectAnimals : JobDriver_RopeToDestination
	{
		// Token: 0x06006B6A RID: 27498 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool HasRopeeArrived(Pawn ropee, bool roperWaitingAtDest)
		{
			return false;
		}

		// Token: 0x06006B6B RID: 27499 RVA: 0x000026EA File Offset: 0x000008EA
		protected override void ProcessArrivedRopee(Pawn ropee)
		{
		}

		// Token: 0x06006B6C RID: 27500 RVA: 0x0021D0B1 File Offset: 0x0021B2B1
		protected override bool ShouldOpportunisticallyRopeAnimal(Pawn animal)
		{
			return JobGiver_PrepareCaravan_CollectPawns.DoesAnimalNeedGathering(this.pawn, animal);
		}

		// Token: 0x06006B6D RID: 27501 RVA: 0x0021D0C0 File Offset: 0x0021B2C0
		protected override Thing FindDistantAnimalToRope()
		{
			Lord lord = this.pawn.GetLord();
			if (lord == null)
			{
				return null;
			}
			return GenClosest.ClosestThing_Global(this.pawn.Position, lord.ownedPawns, 99999f, (Thing t) => this.ShouldOpportunisticallyRopeAnimal(t as Pawn), null, false);
		}
	}
}
