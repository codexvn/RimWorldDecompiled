using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001487 RID: 5255
	public class JobGiver_AnimalFlee : ThinkNode_JobGiver
	{
		// Token: 0x06007E75 RID: 32373 RVA: 0x00260E88 File Offset: 0x0025F088
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.playerSettings != null && pawn.playerSettings.UsesConfigurableHostilityResponse)
			{
				return null;
			}
			if (ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn))
			{
				return null;
			}
			if (pawn.Faction == null)
			{
				List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.AlwaysFlee);
				for (int i = 0; i < list.Count; i++)
				{
					if (pawn.Position.InHorDistOf(list[i].Position, 18f) && FleeUtility.ShouldFleeFrom(list[i], pawn, false, false))
					{
						Job job = FleeUtility.FleeJob(pawn, list[i], 24);
						if (job != null)
						{
							return job;
						}
					}
				}
				Job job2 = FleeUtility.FleeLargeFireJob(pawn, 60, 10, 24);
				if (job2 != null)
				{
					return job2;
				}
			}
			else if (FleeUtility.ShouldAnimalFleeDanger(pawn))
			{
				List<IAttackTarget> potentialTargetsFor = pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn);
				for (int j = 0; j < potentialTargetsFor.Count; j++)
				{
					Thing thing = potentialTargetsFor[j].Thing;
					if (pawn.Position.InHorDistOf(thing.Position, 18f) && FleeUtility.ShouldFleeFrom(thing, pawn, false, true))
					{
						Pawn pawn2 = thing as Pawn;
						if (pawn2 == null || !pawn2.AnimalOrWildMan() || pawn2.Faction != null)
						{
							Job job3 = FleeUtility.FleeJob(pawn, thing, 24);
							if (job3 != null)
							{
								return job3;
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x040054CB RID: 21707
		private const int FleeDistance = 24;

		// Token: 0x040054CC RID: 21708
		private const int DistToDangerToFlee = 18;

		// Token: 0x040054CD RID: 21709
		private const int DistToFireToFlee = 10;

		// Token: 0x040054CE RID: 21710
		private const int MinFiresNearbyToFlee = 60;
	}
}
