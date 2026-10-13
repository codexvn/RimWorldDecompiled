using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001453 RID: 5203
	public class JobGiver_WanderInGatheringArea : JobGiver_Wander
	{
		// Token: 0x06007DDF RID: 32223 RVA: 0x0025E89C File Offset: 0x0025CA9C
		protected override IntVec3 GetExactWanderDest(Pawn pawn)
		{
			Predicate<IntVec3> cellValidator = (IntVec3 x) => this.allowUnroofed || !x.Roofed(pawn.Map);
			IntVec3 result;
			if (this.desiredRadius > 0f && GatheringsUtility.TryFindRandomCellInGatheringAreaWithRadius(pawn, this.desiredRadius, cellValidator, out result))
			{
				return result;
			}
			if (GatheringsUtility.TryFindRandomCellInGatheringArea(pawn, cellValidator, out result))
			{
				return result;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06007DE0 RID: 32224 RVA: 0x00066B11 File Offset: 0x00064D11
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007DE1 RID: 32225 RVA: 0x0025E906 File Offset: 0x0025CB06
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_WanderInGatheringArea jobGiver_WanderInGatheringArea = (JobGiver_WanderInGatheringArea)base.DeepCopy(resolve);
			jobGiver_WanderInGatheringArea.allowUnroofed = this.allowUnroofed;
			jobGiver_WanderInGatheringArea.desiredRadius = this.desiredRadius;
			return jobGiver_WanderInGatheringArea;
		}

		// Token: 0x04005487 RID: 21639
		public bool allowUnroofed = true;

		// Token: 0x04005488 RID: 21640
		public float desiredRadius = -1f;
	}
}
