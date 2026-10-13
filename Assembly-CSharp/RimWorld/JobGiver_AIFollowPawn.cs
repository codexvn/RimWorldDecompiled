using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001406 RID: 5126
	public abstract class JobGiver_AIFollowPawn : ThinkNode_JobGiver
	{
		// Token: 0x06007CEF RID: 31983
		protected abstract Pawn GetFollowee(Pawn pawn);

		// Token: 0x06007CF0 RID: 31984
		protected abstract float GetRadius(Pawn pawn);

		// Token: 0x17001393 RID: 5011
		// (get) Token: 0x06007CF1 RID: 31985 RVA: 0x00259F6D File Offset: 0x0025816D
		protected virtual int FollowJobExpireInterval
		{
			get
			{
				return 140;
			}
		}

		// Token: 0x06007CF2 RID: 31986 RVA: 0x00259F74 File Offset: 0x00258174
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn followee = this.GetFollowee(pawn);
			if (followee == null)
			{
				Type type = base.GetType();
				Log.Error(((type != null) ? type.ToString() : null) + " has null followee. pawn=" + pawn.ToStringSafe<Pawn>());
				return null;
			}
			if (!followee.Spawned || !pawn.CanReach(followee, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			float radius = this.GetRadius(pawn);
			if (!JobDriver_FollowClose.FarEnoughAndPossibleToStartJob(pawn, followee, radius))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.FollowClose, followee);
			job.expiryInterval = this.FollowJobExpireInterval;
			job.checkOverrideOnExpire = true;
			job.followRadius = radius;
			return job;
		}
	}
}
