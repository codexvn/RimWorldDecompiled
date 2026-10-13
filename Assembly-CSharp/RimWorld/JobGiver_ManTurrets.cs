using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001463 RID: 5219
	public abstract class JobGiver_ManTurrets : ThinkNode_JobGiver
	{
		// Token: 0x06007E0A RID: 32266 RVA: 0x0025F418 File Offset: 0x0025D618
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_ManTurrets jobGiver_ManTurrets = (JobGiver_ManTurrets)base.DeepCopy(resolve);
			jobGiver_ManTurrets.maxDistFromPoint = this.maxDistFromPoint;
			return jobGiver_ManTurrets;
		}

		// Token: 0x06007E0B RID: 32267 RVA: 0x0025F434 File Offset: 0x0025D634
		protected override Job TryGiveJob(Pawn pawn)
		{
			JobGiver_ManTurrets.<>c__DisplayClass2_0 CS$<>8__locals1 = new JobGiver_ManTurrets.<>c__DisplayClass2_0();
			CS$<>8__locals1.pawn = pawn;
			Thing thing = GenClosest.ClosestThingReachable(this.GetRoot(CS$<>8__locals1.pawn), CS$<>8__locals1.pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.InteractionCell, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), this.maxDistFromPoint, new Predicate<Thing>(CS$<>8__locals1.<TryGiveJob>g__Validator|0), null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (thing != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.ManTurret, thing);
				job.expiryInterval = 2000;
				job.checkOverrideOnExpire = true;
				return job;
			}
			return null;
		}

		// Token: 0x06007E0C RID: 32268
		protected abstract IntVec3 GetRoot(Pawn pawn);

		// Token: 0x0400549E RID: 21662
		public float maxDistFromPoint = -1f;
	}
}
