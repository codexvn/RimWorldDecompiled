using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001529 RID: 5417
	internal class JobGiver_FireStartingSpree : ThinkNode_JobGiver
	{
		// Token: 0x060080E1 RID: 32993 RVA: 0x0026D007 File Offset: 0x0026B207
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_FireStartingSpree jobGiver_FireStartingSpree = (JobGiver_FireStartingSpree)base.DeepCopy(resolve);
			jobGiver_FireStartingSpree.waitTicks = this.waitTicks;
			return jobGiver_FireStartingSpree;
		}

		// Token: 0x060080E2 RID: 32994 RVA: 0x0026D024 File Offset: 0x0026B224
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.mindState.nextMoveOrderIsWait)
			{
				Job job = JobMaker.MakeJob(JobDefOf.Wait_Wander);
				job.expiryInterval = this.waitTicks.RandomInRange;
				pawn.mindState.nextMoveOrderIsWait = false;
				return job;
			}
			if (Rand.Value < 0.75f)
			{
				Thing thing = this.TryFindRandomIgniteTarget(pawn);
				if (thing != null)
				{
					pawn.mindState.nextMoveOrderIsWait = true;
					return JobMaker.MakeJob(JobDefOf.Ignite, thing);
				}
			}
			IntVec3 c = RCellFinder.RandomWanderDestFor(pawn, pawn.Position, 10f, null, Danger.Deadly, false);
			if (c.IsValid)
			{
				pawn.mindState.nextMoveOrderIsWait = true;
				return JobMaker.MakeJob(JobDefOf.GotoWander, c);
			}
			return null;
		}

		// Token: 0x060080E3 RID: 32995 RVA: 0x0026D0D8 File Offset: 0x0026B2D8
		private Thing TryFindRandomIgniteTarget(Pawn pawn)
		{
			Region region;
			if (!CellFinder.TryFindClosestRegionWith(pawn.GetRegion(RegionType.Set_Passable), TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), (Region candidateRegion) => !candidateRegion.IsForbiddenEntirely(pawn), 100, out region, RegionType.Set_Passable))
			{
				return null;
			}
			JobGiver_FireStartingSpree.potentialTargets.Clear();
			List<Thing> allThings = region.ListerThings.AllThings;
			for (int i = 0; i < allThings.Count; i++)
			{
				Thing thing = allThings[i];
				if ((thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Plant) && thing.FlammableNow && !thing.IsBurning() && !thing.OccupiedRect().Contains(pawn.Position))
				{
					JobGiver_FireStartingSpree.potentialTargets.Add(thing);
				}
			}
			if (JobGiver_FireStartingSpree.potentialTargets.NullOrEmpty<Thing>())
			{
				return null;
			}
			return JobGiver_FireStartingSpree.potentialTargets.RandomElement<Thing>();
		}

		// Token: 0x04005612 RID: 22034
		private IntRange waitTicks = new IntRange(80, 140);

		// Token: 0x04005613 RID: 22035
		private const float FireStartChance = 0.75f;

		// Token: 0x04005614 RID: 22036
		private static List<Thing> potentialTargets = new List<Thing>();
	}
}
