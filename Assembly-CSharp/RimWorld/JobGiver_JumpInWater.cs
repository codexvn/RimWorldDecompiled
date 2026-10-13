using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014A5 RID: 5285
	public class JobGiver_JumpInWater : ThinkNode_JobGiver
	{
		// Token: 0x06007EDA RID: 32474 RVA: 0x00262AAC File Offset: 0x00260CAC
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (Rand.Value < 1f && RCellFinder.TryFindRandomCellNearWith(pawn.Position, (IntVec3 pos) => pos.GetTerrain(pawn.Map).extinguishesFire, pawn.Map, out c, 5, this.MaxDistance.RandomInRange))
			{
				return JobMaker.MakeJob(JobDefOf.Goto, c);
			}
			return null;
		}

		// Token: 0x04005502 RID: 21762
		private const float ActivateChance = 1f;

		// Token: 0x04005503 RID: 21763
		private readonly IntRange MaxDistance = new IntRange(10, 16);
	}
}
