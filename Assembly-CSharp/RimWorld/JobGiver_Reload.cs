using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Utility;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001581 RID: 5505
	public class JobGiver_Reload : ThinkNode_JobGiver
	{
		// Token: 0x06008287 RID: 33415 RVA: 0x002734DF File Offset: 0x002716DF
		public override float GetPriority(Pawn pawn)
		{
			return 5.9f;
		}

		// Token: 0x06008288 RID: 33416 RVA: 0x002734E8 File Offset: 0x002716E8
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return null;
			}
			IReloadableComp reloadableComp = ReloadableUtility.FindSomeReloadableComponent(pawn, false);
			if (reloadableComp == null)
			{
				return null;
			}
			if (pawn.carryTracker.AvailableStackSpace(reloadableComp.AmmoDef) < reloadableComp.MinAmmoNeeded(true))
			{
				return null;
			}
			List<Thing> list = ReloadableUtility.FindEnoughAmmo(pawn, pawn.Position, reloadableComp, false);
			if (list.NullOrEmpty<Thing>())
			{
				return null;
			}
			return JobGiver_Reload.MakeReloadJob(reloadableComp, list);
		}

		// Token: 0x06008289 RID: 33417 RVA: 0x00273558 File Offset: 0x00271758
		public static Job MakeReloadJob(IReloadableComp reloadable, List<Thing> chosenAmmo)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Reload, reloadable.ReloadableThing);
			job.targetQueueB = (from t in chosenAmmo
			select new LocalTargetInfo(t)).ToList<LocalTargetInfo>();
			job.count = chosenAmmo.Sum((Thing t) => t.stackCount);
			job.count = Math.Min(job.count, reloadable.MaxAmmoNeeded(true));
			return job;
		}

		// Token: 0x0400569C RID: 22172
		private const bool ForceReloadWhenLookingForWork = false;
	}
}
