using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02001018 RID: 4120
	public class JobGiver_FightFiresNearConnectedTree : ThinkNode_JobGiver
	{
		// Token: 0x060064F2 RID: 25842 RVA: 0x001FEC84 File Offset: 0x001FCE84
		protected override Job TryGiveJob(Pawn pawn)
		{
			Thing thing = pawn.connections.ConnectedThings.FirstOrDefault((Thing x) => x.Spawned && x.Map == pawn.Map && pawn.CanReach(x, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn));
			if (thing == null)
			{
				return null;
			}
			Thing thing2 = GenClosest.ClosestThingReachable(thing.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.Fire), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 10f, JobGiver_FightFiresNearPoint.FireValidator(pawn), null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (thing2 != null)
			{
				return JobMaker.MakeJob(JobDefOf.BeatFire, thing2);
			}
			return null;
		}

		// Token: 0x0400481B RID: 18459
		private const float FightFireDistance = 10f;
	}
}
