using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014C7 RID: 5319
	public class JobGiver_SeekAllowedArea : ThinkNode_JobGiver
	{
		// Token: 0x06007F45 RID: 32581 RVA: 0x002652E0 File Offset: 0x002634E0
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.Position.IsForbidden(pawn))
			{
				return null;
			}
			if (this.HasJobWithSpawnedAllowedTarget(pawn))
			{
				return null;
			}
			Region region = pawn.GetRegion(RegionType.Set_Passable);
			if (region == null)
			{
				return null;
			}
			TraverseParms traverseParms = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(traverseParms, false);
			Region reg = null;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				if (r.IsDoorway)
				{
					return false;
				}
				if (!r.IsForbiddenEntirely(pawn))
				{
					reg = r;
					return true;
				}
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, 9999, RegionType.Set_Passable);
			if (reg == null)
			{
				return null;
			}
			IntVec3 c;
			if (!reg.TryFindRandomCellInRegionUnforbidden(pawn, null, out c))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Goto, c);
		}

		// Token: 0x06007F46 RID: 32582 RVA: 0x002653B4 File Offset: 0x002635B4
		private bool HasJobWithSpawnedAllowedTarget(Pawn pawn)
		{
			Job curJob = pawn.CurJob;
			return curJob != null && (this.IsSpawnedAllowedTarget(curJob.targetA, pawn) || this.IsSpawnedAllowedTarget(curJob.targetB, pawn) || this.IsSpawnedAllowedTarget(curJob.targetC, pawn));
		}

		// Token: 0x06007F47 RID: 32583 RVA: 0x002653FC File Offset: 0x002635FC
		private bool IsSpawnedAllowedTarget(LocalTargetInfo target, Pawn pawn)
		{
			if (!target.IsValid)
			{
				return false;
			}
			if (target.HasThing)
			{
				return target.Thing.Spawned && !target.Thing.Position.IsForbidden(pawn);
			}
			return target.Cell.InBounds(pawn.Map) && !target.Cell.IsForbidden(pawn);
		}
	}
}
