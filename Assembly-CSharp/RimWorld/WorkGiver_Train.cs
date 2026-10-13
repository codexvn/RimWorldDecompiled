using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001557 RID: 5463
	public class WorkGiver_Train : WorkGiver_InteractAnimal
	{
		// Token: 0x060081AA RID: 33194 RVA: 0x0026F7EF File Offset: 0x0026D9EF
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
		}

		// Token: 0x060081AB RID: 33195 RVA: 0x00270838 File Offset: 0x0026EA38
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !pawn2.IsAnimal)
			{
				return null;
			}
			if (pawn2.RaceProps.animalType == AnimalType.Dryad)
			{
				return null;
			}
			if (pawn2.Faction != pawn.Faction)
			{
				return null;
			}
			if (pawn2.training == null)
			{
				return null;
			}
			if (pawn2.training.NextTrainableToTrain() == null)
			{
				return null;
			}
			if (!this.CanInteractWithAnimal(pawn, pawn2, forced))
			{
				return null;
			}
			if (pawn2.RaceProps.EatsFood)
			{
				Pawn_NeedsTracker needs = pawn2.needs;
				if (((needs != null) ? needs.food : null) != null && !base.HasFoodToInteractAnimal(pawn, pawn2))
				{
					Job job = base.TakeFoodForAnimalInteractJob(pawn, pawn2);
					if (job == null)
					{
						JobFailReason.Is(WorkGiver_InteractAnimal.NoUsableFoodTrans, null);
					}
					return job;
				}
			}
			if (TrainableUtility.TrainedTooRecently(pawn2))
			{
				JobFailReason.Is(WorkGiver_InteractAnimal.AnimalInteractedTooRecentlyTrans, null);
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Train, t);
		}
	}
}
