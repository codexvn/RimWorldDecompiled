using System;
using System.Collections.Generic;
using LudeonTK;
using RimWorld;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005C8 RID: 1480
	public class PathGridDoorsBlockedJob : IJob
	{
		// Token: 0x06002604 RID: 9732 RVA: 0x000C8568 File Offset: 0x000C6768
		public void Execute()
		{
			this.providerCost.Clear<ushort>();
			this.blocked.Clear<bool>();
			foreach (Thing thing in this.doors)
			{
				Building_Door building_Door = thing as Building_Door;
				if (building_Door != null)
				{
					foreach (IntVec3 c in building_Door.OccupiedRect())
					{
						int index = this.map.cellIndices.CellToIndex(c);
						this.providerCost[index] = PathUtility.GetDoorCost(building_Door, this.traverseParams, this.pawn, new PathFinderCostTuning?(this.tuning));
					}
				}
			}
			if (this.pawn != null)
			{
				foreach (IPathFindCostProvider pathFindCostProvider in this.providers)
				{
					foreach (IntVec3 c2 in pathFindCostProvider.GetOccupiedRect())
					{
						int num = this.map.cellIndices.CellToIndex(c2);
						ref NativeArray<ushort> ptr = ref this.providerCost;
						int index2 = num;
						ptr[index2] += pathFindCostProvider.PathFindCostFor(this.pawn);
					}
				}
			}
			if (!PawnUtility.ShouldCollideWithPawns(this.pawn))
			{
				return;
			}
			foreach (Pawn pawn in this.pawns)
			{
				if (pawn != this.pawn && this.CanBlockEver(pawn))
				{
					bool collideWithNonHostile = this.pawn.CurJob != null && (this.pawn.CurJob.collideWithPawns || this.pawn.CurJob.def.collideWithPawns || this.pawn.jobs.curDriver.collideWithPawns);
					if (PawnUtility.PawnBlockedBy(this.pawn, pawn, false, collideWithNonHostile, true, false))
					{
						this.blocked[this.map.cellIndices.CellToIndex(pawn.Position)] = true;
					}
				}
			}
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x000C8808 File Offset: 0x000C6A08
		private bool CanBlockEver(Pawn other)
		{
			if (other.Destroyed || other.Discarded || !other.Spawned)
			{
				return false;
			}
			if (this.blocked[this.map.cellIndices.CellToIndex(other.Position)])
			{
				return false;
			}
			if (!this.pawn.pather.BestPathHadPawnsInTheWayRecently())
			{
				int lengthManhattan = (other.Position - this.start).LengthManhattan;
				int lengthManhattan2 = (other.Position - this.dest).LengthManhattan;
				if (Mathf.Min(lengthManhattan, lengthManhattan2) > 32)
				{
					return false;
				}
			}
			return PawnUtility.ShouldCollideWithPawns(other);
		}

		// Token: 0x04001F70 RID: 8048
		private const int MinDistToCheckBlocked = 32;

		// Token: 0x04001F71 RID: 8049
		public Map map;

		// Token: 0x04001F72 RID: 8050
		public Pawn pawn;

		// Token: 0x04001F73 RID: 8051
		public IntVec3 start;

		// Token: 0x04001F74 RID: 8052
		public IntVec3 dest;

		// Token: 0x04001F75 RID: 8053
		public TraverseParms traverseParams;

		// Token: 0x04001F76 RID: 8054
		public PathFinderCostTuning tuning;

		// Token: 0x04001F77 RID: 8055
		public IReadOnlyList<Thing> doors;

		// Token: 0x04001F78 RID: 8056
		public IReadOnlyList<IPathFindCostProvider> providers;

		// Token: 0x04001F79 RID: 8057
		public IReadOnlyList<Pawn> pawns;

		// Token: 0x04001F7A RID: 8058
		public NativeArray<ushort> providerCost;

		// Token: 0x04001F7B RID: 8059
		public NativeArray<bool> blocked;

		// Token: 0x04001F7C RID: 8060
		private bool passAllDestroyableThings;

		// Token: 0x04001F7D RID: 8061
		private bool passWater;
	}
}
