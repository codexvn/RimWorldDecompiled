using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014D2 RID: 5330
	public class JobGiver_TakeCountToInventory : ThinkNode_JobGiver
	{
		// Token: 0x06007F71 RID: 32625 RVA: 0x00265CB4 File Offset: 0x00263EB4
		protected override Job TryGiveJob(Pawn pawn)
		{
			int toTake = Math.Max(this.count - pawn.inventory.Count(this.def), 0);
			if (toTake == 0)
			{
				return null;
			}
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(this.def), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing x) => x.stackCount >= toTake && !x.IsForbidden(pawn) && pawn.CanReserve(x, 10, toTake, null, false), null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (thing != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.TakeCountToInventory, thing);
				job.count = toTake;
				return job;
			}
			return null;
		}

		// Token: 0x06007F72 RID: 32626 RVA: 0x00265D74 File Offset: 0x00263F74
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_TakeCountToInventory jobGiver_TakeCountToInventory = (JobGiver_TakeCountToInventory)base.DeepCopy(resolve);
			jobGiver_TakeCountToInventory.def = this.def;
			jobGiver_TakeCountToInventory.count = this.count;
			return jobGiver_TakeCountToInventory;
		}

		// Token: 0x04005549 RID: 21833
		public ThingDef def;

		// Token: 0x0400554A RID: 21834
		public int count = 1;
	}
}
