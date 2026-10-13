using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013D5 RID: 5077
	public class JobGiver_Mine : ThinkNode_JobGiver
	{
		// Token: 0x06007C2D RID: 31789 RVA: 0x00256590 File Offset: 0x00254790
		protected override Job TryGiveJob(Pawn pawn)
		{
			JobGiver_Mine.<>c__DisplayClass0_0 CS$<>8__locals1 = new JobGiver_Mine.<>c__DisplayClass0_0();
			CS$<>8__locals1.pawn = pawn;
			IEnumerable<Thing> enumerable = MineAIUtility.PotentialMineables(CS$<>8__locals1.pawn);
			Thing thing = GenClosest.ClosestThingReachable(CS$<>8__locals1.pawn.Position, CS$<>8__locals1.pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Undefined), PathEndMode.Touch, TraverseParms.For(CS$<>8__locals1.pawn, CS$<>8__locals1.pawn.NormalMaxDanger(), TraverseMode.ByPawn, false, false, false, true), 9999f, new Predicate<Thing>(CS$<>8__locals1.<TryGiveJob>g__Validator|0), enumerable, 0, -1, enumerable != null, RegionType.Set_Passable, false, false);
			if (thing == null)
			{
				return null;
			}
			return MineAIUtility.JobOnThing(CS$<>8__locals1.pawn, thing, false);
		}
	}
}
