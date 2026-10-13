using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001445 RID: 5189
	public class JobGiver_GotoAndStandSociallyActive : ThinkNode_JobGiver
	{
		// Token: 0x06007DBC RID: 32188 RVA: 0x0025DF74 File Offset: 0x0025C174
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 dest = this.GetDest(pawn);
			Job job = JobMaker.MakeJob(JobDefOf.GotoAndBeSociallyActive, dest, dest + pawn.Rotation.FacingCell);
			job.locomotionUrgency = this.locomotionUrgency;
			job.expiryInterval = this.expiryInterval;
			job.checkOverrideOnExpire = true;
			return job;
		}

		// Token: 0x06007DBD RID: 32189 RVA: 0x0025DFD4 File Offset: 0x0025C1D4
		public IntVec3 GetDest(Pawn pawn)
		{
			Predicate<IntVec3> validatorRelaxed = (IntVec3 x) => this.allowUnroofed || !x.Roofed(pawn.Map);
			Predicate<IntVec3> cellValidator = delegate(IntVec3 x)
			{
				if (!RitualUtility.GoodSpectateCellForRitual(x, pawn, pawn.Map))
				{
					return false;
				}
				if (this.minDistanceToOtherReservedCell > 0)
				{
					foreach (IntVec3 c in GenRadial.RadialCellsAround(x, (float)this.minDistanceToOtherReservedCell, true))
					{
						if (!pawn.CanReserveAndReach(c, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false))
						{
							return false;
						}
					}
				}
				return validatorRelaxed(x);
			};
			IntVec3 result;
			if (this.desiredRadius > 0f && GatheringsUtility.TryFindRandomCellInGatheringAreaWithRadius(pawn, this.desiredRadius, cellValidator, out result))
			{
				return result;
			}
			if (GatheringsUtility.TryFindRandomCellInGatheringArea(pawn, validatorRelaxed, out result))
			{
				return result;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06007DBE RID: 32190 RVA: 0x0025E058 File Offset: 0x0025C258
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GotoAndStandSociallyActive jobGiver_GotoAndStandSociallyActive = (JobGiver_GotoAndStandSociallyActive)base.DeepCopy(resolve);
			jobGiver_GotoAndStandSociallyActive.locomotionUrgency = this.locomotionUrgency;
			jobGiver_GotoAndStandSociallyActive.allowUnroofed = this.allowUnroofed;
			jobGiver_GotoAndStandSociallyActive.desiredRadius = this.desiredRadius;
			jobGiver_GotoAndStandSociallyActive.minDistanceToOtherReservedCell = this.minDistanceToOtherReservedCell;
			jobGiver_GotoAndStandSociallyActive.expiryInterval = this.expiryInterval;
			return jobGiver_GotoAndStandSociallyActive;
		}

		// Token: 0x04005474 RID: 21620
		protected LocomotionUrgency locomotionUrgency = LocomotionUrgency.Walk;

		// Token: 0x04005475 RID: 21621
		public bool allowUnroofed = true;

		// Token: 0x04005476 RID: 21622
		protected int expiryInterval = -1;

		// Token: 0x04005477 RID: 21623
		public float desiredRadius = -1f;

		// Token: 0x04005478 RID: 21624
		public int minDistanceToOtherReservedCell = -1;
	}
}
