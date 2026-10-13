using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014C3 RID: 5315
	public class JobGiver_RevenantEscape : ThinkNode_JobGiver
	{
		// Token: 0x06007F3C RID: 32572 RVA: 0x0026512C File Offset: 0x0026332C
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c = RevenantUtility.FindEscapeCell(pawn);
			if (!c.IsValid)
			{
				return null;
			}
			using (PawnPath pawnPath = pawn.Map.pathFinder.FindPathNow(pawn.Position, c, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassDoors, false, false, false, true), null, PathEndMode.OnCell, null))
			{
				if (!pawnPath.Found)
				{
					using (PawnPath pawnPath2 = pawn.Map.pathFinder.FindPathNow(pawn.Position, c, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassAllDestroyableThings, false, false, false, true), null, PathEndMode.OnCell, null))
					{
						IntVec3 cellBeforeBlocker;
						Thing thing = pawnPath2.FirstBlockingBuilding(out cellBeforeBlocker, pawn);
						if (thing != null)
						{
							Job job = DigUtility.PassBlockerJob(pawn, thing, cellBeforeBlocker, true, true);
							if (job != null)
							{
								return job;
							}
						}
					}
				}
			}
			Job job2 = JobMaker.MakeJob(JobDefOf.RevenantEscape, c);
			job2.locomotionUrgency = LocomotionUrgency.Sprint;
			job2.canBashDoors = true;
			job2.canBashFences = true;
			return job2;
		}
	}
}
