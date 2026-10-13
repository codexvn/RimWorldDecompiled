using System;

namespace Verse.AI
{
	// Token: 0x02001016 RID: 4118
	public class JobGiver_WanderNearConnectedTree : JobGiver_Wander
	{
		// Token: 0x060064ED RID: 25837 RVA: 0x001FEBC4 File Offset: 0x001FCDC4
		public JobGiver_WanderNearConnectedTree()
		{
			this.wanderRadius = 12f;
			this.ticksBetweenWandersRange = new IntRange(130, 250);
		}

		// Token: 0x060064EE RID: 25838 RVA: 0x001FEBEC File Offset: 0x001FCDEC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.connections.ConnectedThings.FirstOrDefault((Thing x) => x.Spawned && x.Map == pawn.Map && pawn.CanReach(x, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn)) == null)
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x060064EF RID: 25839 RVA: 0x001FEC37 File Offset: 0x001FCE37
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.connections.ConnectedThings[0].Position;
		}
	}
}
