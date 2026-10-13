using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001483 RID: 5251
	public class JobGiver_FleeAllPawns : ThinkNode_JobGiver
	{
		// Token: 0x06007E6A RID: 32362 RVA: 0x00260C1C File Offset: 0x0025EE1C
		protected override Job TryGiveJob(Pawn pawn)
		{
			Region region = pawn.GetRegion(RegionType.Set_Passable);
			if (region == null)
			{
				return null;
			}
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region reg) => reg.door == null || reg.door.Open, delegate(Region reg)
			{
				List<Thing> list = reg.ListerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] != pawn)
					{
						IAttackTarget attackTarget = list[i] as IAttackTarget;
						if (attackTarget != null && !attackTarget.ThreatDisabled(null))
						{
							Pawn pawn2 = list[i] as Pawn;
							if (pawn2 != null && (pawn2.HostileTo(pawn) || pawn2.RaceProps.Humanlike) && GenSight.LineOfSightToThing(pawn.Position, pawn2, pawn.Map, true, null))
							{
								JobGiver_FleeAllPawns.tmpPawns.Add(pawn2);
							}
						}
					}
				}
				return false;
			}, 9, RegionType.Set_Passable);
			if (JobGiver_FleeAllPawns.tmpPawns.Any<Thing>())
			{
				IntVec3 fleeDest = CellFinderLoose.GetFleeDest(pawn, JobGiver_FleeAllPawns.tmpPawns, 23f);
				JobGiver_FleeAllPawns.tmpPawns.Clear();
				if (fleeDest.IsValid && fleeDest != pawn.Position)
				{
					Job job = JobMaker.MakeJob(JobDefOf.FleeAndCowerShort, fleeDest);
					job.checkOverrideOnExpire = true;
					job.expiryInterval = 65;
					return job;
				}
			}
			return null;
		}

		// Token: 0x040054C4 RID: 21700
		private static readonly List<Thing> tmpPawns = new List<Thing>();
	}
}
