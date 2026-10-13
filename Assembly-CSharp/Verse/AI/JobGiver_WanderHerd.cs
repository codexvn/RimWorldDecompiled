using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200101F RID: 4127
	public class JobGiver_WanderHerd : JobGiver_Wander
	{
		// Token: 0x06006505 RID: 25861 RVA: 0x001FF0FC File Offset: 0x001FD2FC
		public JobGiver_WanderHerd()
		{
			this.wanderRadius = 5f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06006506 RID: 25862 RVA: 0x001FF124 File Offset: 0x001FD324
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			Predicate<Thing> isHerdValidator = delegate(Thing t)
			{
				if (((Pawn)t).RaceProps != pawn.RaceProps || t == pawn)
				{
					return false;
				}
				if (t.Faction != pawn.Faction)
				{
					return false;
				}
				IReadOnlyList<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					Pawn pawn2 = allPawnsSpawned[i];
					if (pawn2.RaceProps.Humanlike && (pawn2.Position - t.Position).LengthHorizontalSquared < 225)
					{
						return false;
					}
				}
				return true;
			};
			return WanderUtility.GetHerdWanderRoot(pawn, isHerdValidator);
		}

		// Token: 0x0400482A RID: 18474
		private const int MinDistToHumanlike = 15;
	}
}
