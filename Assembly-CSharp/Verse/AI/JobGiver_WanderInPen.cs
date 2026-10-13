using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02001021 RID: 4129
	public class JobGiver_WanderInPen : JobGiver_Wander
	{
		// Token: 0x06006509 RID: 25865 RVA: 0x001FF1FC File Offset: 0x001FD3FC
		public JobGiver_WanderInPen()
		{
			this.wanderRadius = 10f;
		}

		// Token: 0x0600650A RID: 25866 RVA: 0x001FF210 File Offset: 0x001FD410
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			if (pawn.GetDistrict(RegionType.Set_Passable).TouchesMapEdge)
			{
				CompAnimalPenMarker compAnimalPenMarker = AnimalPenUtility.ClosestSuitablePen(pawn, true);
				if (compAnimalPenMarker != null)
				{
					return compAnimalPenMarker.parent.Position;
				}
			}
			return pawn.Position;
		}
	}
}
