using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000E80 RID: 3712
	public class JobDriver_FollowClose : JobDriver
	{
		// Token: 0x17000F04 RID: 3844
		// (get) Token: 0x06005D8C RID: 23948 RVA: 0x001DD7F8 File Offset: 0x001DB9F8
		private Pawn Followee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000F05 RID: 3845
		// (get) Token: 0x06005D8D RID: 23949 RVA: 0x001DD81E File Offset: 0x001DBA1E
		private bool CurrentlyWalkingToFollowee
		{
			get
			{
				return this.pawn.pather.Moving && this.pawn.pather.Destination == this.Followee;
			}
		}

		// Token: 0x06005D8E RID: 23950 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06005D8F RID: 23951 RVA: 0x001DD854 File Offset: 0x001DBA54
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			if (this.job.followRadius <= 0f)
			{
				Log.Error("Follow radius is <= 0. pawn=" + this.pawn.ToStringSafe<Pawn>());
				this.job.followRadius = 10f;
			}
		}

		// Token: 0x06005D90 RID: 23952 RVA: 0x001DD8A3 File Offset: 0x001DBAA3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.tickIntervalAction = delegate(int delta)
			{
				Pawn followee = this.Followee;
				float followRadius = this.job.followRadius;
				if (this.pawn.pather.Moving && !this.pawn.IsHashIntervalTick(30, delta))
				{
					return;
				}
				bool flag = false;
				if (this.CurrentlyWalkingToFollowee)
				{
					if (JobDriver_FollowClose.NearFollowee(this.pawn, followee, followRadius))
					{
						flag = true;
					}
				}
				else
				{
					float radius = followRadius * 1.2f;
					if (JobDriver_FollowClose.NearFollowee(this.pawn, followee, radius))
					{
						flag = true;
					}
					else
					{
						if (!this.pawn.CanReach(followee, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
						{
							base.EndJobWith(JobCondition.Incompletable);
							return;
						}
						this.pawn.pather.StartPath(followee, PathEndMode.Touch);
						this.locomotionUrgencySameAs = null;
					}
				}
				if (flag)
				{
					if (JobDriver_FollowClose.NearDestinationOrNotMoving(this.pawn, followee, followRadius))
					{
						base.EndJobWith(JobCondition.Succeeded);
						return;
					}
					IntVec3 lastPassableCellInPath = followee.pather.LastPassableCellInPath;
					if (!this.pawn.pather.Moving || this.pawn.pather.Destination.HasThing || !this.pawn.pather.Destination.Cell.InHorDistOf(lastPassableCellInPath, followRadius))
					{
						IntVec3 intVec = CellFinder.RandomClosewalkCellNear(lastPassableCellInPath, base.Map, Mathf.FloorToInt(followRadius), null);
						if (intVec == this.pawn.Position)
						{
							base.EndJobWith(JobCondition.Succeeded);
							return;
						}
						if (intVec.IsValid && this.pawn.CanReach(intVec, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
						{
							this.pawn.pather.StartPath(intVec, PathEndMode.OnCell);
							this.locomotionUrgencySameAs = followee;
							return;
						}
						base.EndJobWith(JobCondition.Incompletable);
						return;
					}
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			yield return toil;
			yield break;
		}

		// Token: 0x06005D91 RID: 23953 RVA: 0x001D7A1B File Offset: 0x001D5C1B
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x06005D92 RID: 23954 RVA: 0x001DD8B4 File Offset: 0x001DBAB4
		public static bool FarEnoughAndPossibleToStartJob(Pawn follower, Pawn followee, float radius)
		{
			if (radius <= 0f)
			{
				string text = "Checking follow job with radius <= 0. pawn=" + follower.ToStringSafe<Pawn>();
				if (follower.mindState != null && follower.mindState.duty != null)
				{
					string str = text;
					string str2 = " duty=";
					DutyDef def = follower.mindState.duty.def;
					text = str + str2 + ((def != null) ? def.ToString() : null);
				}
				Log.ErrorOnce(text, follower.thingIDNumber ^ 843254009);
				return false;
			}
			if (!follower.CanReach(followee, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return false;
			}
			float radius2 = radius * 1.2f;
			return !JobDriver_FollowClose.NearFollowee(follower, followee, radius2) || (!JobDriver_FollowClose.NearDestinationOrNotMoving(follower, followee, radius2) && follower.CanReach(followee.pather.LastPassableCellInPath, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn));
		}

		// Token: 0x06005D93 RID: 23955 RVA: 0x001DD97C File Offset: 0x001DBB7C
		private static bool NearFollowee(Pawn follower, Pawn followee, float radius)
		{
			return follower.Position.AdjacentTo8WayOrInside(followee.Position) || (follower.Position.InHorDistOf(followee.Position, radius) && GenSight.LineOfSight(follower.Position, followee.Position, follower.Map));
		}

		// Token: 0x06005D94 RID: 23956 RVA: 0x001DD9D0 File Offset: 0x001DBBD0
		private static bool NearDestinationOrNotMoving(Pawn follower, Pawn followee, float radius)
		{
			if (!followee.pather.Moving)
			{
				return true;
			}
			IntVec3 lastPassableCellInPath = followee.pather.LastPassableCellInPath;
			return !lastPassableCellInPath.IsValid || follower.Position.AdjacentTo8WayOrInside(lastPassableCellInPath) || follower.Position.InHorDistOf(lastPassableCellInPath, radius);
		}

		// Token: 0x040043C9 RID: 17353
		private const TargetIndex FolloweeInd = TargetIndex.A;

		// Token: 0x040043CA RID: 17354
		private const int CheckPathIntervalTicks = 30;
	}
}
