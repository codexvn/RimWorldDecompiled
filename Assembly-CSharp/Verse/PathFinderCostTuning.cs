using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020005BE RID: 1470
	public struct PathFinderCostTuning : IEquatable<PathFinderCostTuning>
	{
		// Token: 0x060025E0 RID: 9696 RVA: 0x000C724C File Offset: 0x000C544C
		public static PathFinderCostTuning For(Pawn pawn)
		{
			PathFinderCostTuning defaultTuning = PathFinderCostTuning.DefaultTuning;
			if (pawn.WaterCellCost != null)
			{
				defaultTuning.costWater = pawn.WaterCellCost.Value;
			}
			if (!pawn.RaceProps.ToolUser || pawn.HostileTo(Faction.OfPlayer))
			{
				defaultTuning.costBlueprintFrame = 0;
			}
			return defaultTuning;
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x000C72A8 File Offset: 0x000C54A8
		public PathFinderCostTuning(int costBlockedWallBase, float costBlockedWallExtraPerHitPoint, int costBlockedWallExtraForNaturalWalls, int costBlockedDoor, float costBlockedDoorPerHitPoint, int costOffLordWalkGrid, int costDanger, int costBlueprintFrame, int costWater = -1)
		{
			this.costBlockedWallBase = costBlockedWallBase;
			this.costBlockedWallExtraPerHitPoint = costBlockedWallExtraPerHitPoint;
			this.costBlockedWallExtraForNaturalWalls = costBlockedWallExtraForNaturalWalls;
			this.costBlockedDoor = costBlockedDoor;
			this.costBlockedDoorPerHitPoint = costBlockedDoorPerHitPoint;
			this.costOffLordWalkGrid = costOffLordWalkGrid;
			this.costDanger = costDanger;
			this.costBlueprintFrame = costBlueprintFrame;
			this.costWater = costWater;
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x000C72FC File Offset: 0x000C54FC
		public bool Equals(PathFinderCostTuning other)
		{
			return this.costBlockedWallBase == other.costBlockedWallBase && this.costBlockedWallExtraPerHitPoint.Equals(other.costBlockedWallExtraPerHitPoint) && this.costBlockedWallExtraForNaturalWalls == other.costBlockedWallExtraForNaturalWalls && this.costBlockedDoor == other.costBlockedDoor && this.costBlockedDoorPerHitPoint.Equals(other.costBlockedDoorPerHitPoint) && this.costOffLordWalkGrid == other.costOffLordWalkGrid && this.costDanger == other.costDanger && this.costBlueprintFrame == other.costBlueprintFrame && this.costWater == other.costWater;
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x000C7394 File Offset: 0x000C5594
		public override bool Equals(object obj)
		{
			if (obj is PathFinderCostTuning)
			{
				PathFinderCostTuning other = (PathFinderCostTuning)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x000C73BC File Offset: 0x000C55BC
		public override int GetHashCode()
		{
			return HashCode.Combine<int, int>(HashCode.Combine<int, float, int, int, float, int, int, int>(this.costBlockedWallBase, this.costBlockedWallExtraPerHitPoint, this.costBlockedWallExtraForNaturalWalls, this.costBlockedDoor, this.costBlockedDoorPerHitPoint, this.costOffLordWalkGrid, this.costDanger, this.costBlueprintFrame), this.costWater);
		}

		// Token: 0x04001F24 RID: 7972
		private const int Cost_BlockedWallBase = 70;

		// Token: 0x04001F25 RID: 7973
		private const float Cost_BlockedWallExtraPerHitPoint = 0.2f;

		// Token: 0x04001F26 RID: 7974
		private const int Cost_BlockedWallExtraForNaturalWalls = 0;

		// Token: 0x04001F27 RID: 7975
		private const int Cost_BlockedDoor = 50;

		// Token: 0x04001F28 RID: 7976
		private const float Cost_BlockedDoorPerHitPoint = 0.2f;

		// Token: 0x04001F29 RID: 7977
		private const int Cost_OffLordWalkGrid = 70;

		// Token: 0x04001F2A RID: 7978
		private const int Cost_Danger = 300;

		// Token: 0x04001F2B RID: 7979
		private const int Cost_BlueprintFrame = 800;

		// Token: 0x04001F2C RID: 7980
		public int costBlockedWallBase;

		// Token: 0x04001F2D RID: 7981
		public float costBlockedWallExtraPerHitPoint;

		// Token: 0x04001F2E RID: 7982
		public int costBlockedWallExtraForNaturalWalls;

		// Token: 0x04001F2F RID: 7983
		public int costBlockedDoor;

		// Token: 0x04001F30 RID: 7984
		public float costBlockedDoorPerHitPoint;

		// Token: 0x04001F31 RID: 7985
		public int costOffLordWalkGrid;

		// Token: 0x04001F32 RID: 7986
		public int costDanger;

		// Token: 0x04001F33 RID: 7987
		public int costWater;

		// Token: 0x04001F34 RID: 7988
		public int costBlueprintFrame;

		// Token: 0x04001F35 RID: 7989
		public static readonly PathFinderCostTuning DefaultTuning = new PathFinderCostTuning(70, 0.2f, 0, 50, 0.2f, 70, 300, 800, -1);
	}
}
