using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005C9 RID: 1481
	[BurstCompile(CompileSynchronously = true)]
	public struct PathGridJob : IJobParallelFor
	{
		// Token: 0x06002607 RID: 9735 RVA: 0x000C88AC File Offset: 0x000C6AAC
		[BurstCompile]
		public void Execute(int index)
		{
			this.passAllDestroyableThings = (this.traverseParams.mode == TraverseMode.PassAllDestroyableThings || this.traverseParams.mode == TraverseMode.PassAllDestroyableThingsNotWater);
			this.passWater = (this.traverseParams.mode != TraverseMode.NoPassClosedDoorsOrWater && this.traverseParams.mode != TraverseMode.PassAllDestroyableThingsNotWater);
			if (!this.CellIsPassable(index))
			{
				this.grid[index] = 10000;
				return;
			}
			this.grid[index] = this.CostForCell(index);
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x000C8934 File Offset: 0x000C6B34
		private bool DestroyableBuilding(int index)
		{
			return this.pathGridDirect[index] >= 10000 && ((this.traverseParams.fenceBlocked && this.traverseParams.canBashFences && this.fence.IsSet(index)) || (this.building.IsSet(index) && this.buildingDestroyable.IsSet(index) && ((this.traverseParams.mode == TraverseMode.PassAllDestroyablePlayerOwnedThings && this.player.IsSet(index)) || this.passAllDestroyableThings)));
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x000C89C4 File Offset: 0x000C6BC4
		private bool CellIsPassable(int index)
		{
			return (this.pathGridDirect[index] < 10000 || this.DestroyableBuilding(index)) && (this.custom.Length <= 0 || this.custom[index] < 10000) && (this.passWater || !this.water.IsSet(index)) && (!this.traverseParams.fenceBlocked || this.traverseParams.canBashFences || !this.fence.IsSet(index));
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x000C8A58 File Offset: 0x000C6C58
		private int CostForCell(int index)
		{
			int num;
			if (this.DestroyableBuilding(index))
			{
				num = this.tuning.costBlockedWallBase + Mathf.RoundToInt((float)this.buildingHitPoints[index] * this.tuning.costBlockedWallExtraPerHitPoint);
			}
			else if (this.tuning.costWater >= 0 && this.water.IsSet(index))
			{
				num = this.tuning.costWater;
			}
			else
			{
				num = this.pathGridDirect[index];
			}
			if (this.tuning.costWater <= 0 || !this.water.IsSet(index))
			{
				num += (int)this.perceptualCost[index];
			}
			if (this.avoidGrid.Length > 0)
			{
				num += (int)(this.avoidGrid[index] * 8);
			}
			if (this.allowedGrid.Length > 0 && !this.allowedGrid.IsSet(index))
			{
				num += 600;
			}
			if (this.traverseParams.fenceBlocked && this.fence.IsSet(index) && this.passAllDestroyableThings)
			{
				num += this.tuning.costBlockedDoor + Mathf.RoundToInt((float)this.buildingHitPoints[index] * this.tuning.costBlockedDoorPerHitPoint);
			}
			IntVec3 c = this.indicies[index];
			if (this.traverseParams.targetBuildable.Area == 0 || !this.traverseParams.targetBuildable.Contains(c))
			{
				num += (int)this.factionCosts[index];
			}
			if (this.traverseParams.avoidPersistentDanger && this.persistentDanger.IsSet(index))
			{
				num += this.tuning.costDanger;
			}
			else if (this.traverseParams.avoidDarknessDanger && this.darknessDanger.IsSet(index))
			{
				num += this.tuning.costDanger;
			}
			if (this.lordGrid.Length > 0 && !this.lordGrid.IsSet(index))
			{
				num += this.tuning.costOffLordWalkGrid;
			}
			if (this.traverseParams.avoidFog && this.fogged.IsSet(index))
			{
				num += 600;
			}
			if (this.custom.Length > 0)
			{
				num += (int)this.custom[index];
			}
			return num;
		}

		// Token: 0x04001F7E RID: 8062
		public const int Cost_OutsideAllowedArea = 600;

		// Token: 0x04001F7F RID: 8063
		public const int Cost_AvoidFog = 600;

		// Token: 0x04001F80 RID: 8064
		[ReadOnly]
		public PathFinderCostTuning tuning;

		// Token: 0x04001F81 RID: 8065
		[ReadOnly]
		public PathFinder.UnmanagedGridTraverseParams traverseParams;

		// Token: 0x04001F82 RID: 8066
		[ReadOnly]
		public CellIndices indicies;

		// Token: 0x04001F83 RID: 8067
		[ReadOnly]
		public NativeArray<int>.ReadOnly pathGridDirect;

		// Token: 0x04001F84 RID: 8068
		[ReadOnly]
		public NativeBitArray.ReadOnly allowedGrid;

		// Token: 0x04001F85 RID: 8069
		[ReadOnly]
		public NativeBitArray.ReadOnly building;

		// Token: 0x04001F86 RID: 8070
		[ReadOnly]
		public NativeBitArray.ReadOnly buildingDestroyable;

		// Token: 0x04001F87 RID: 8071
		[ReadOnly]
		public NativeBitArray.ReadOnly fence;

		// Token: 0x04001F88 RID: 8072
		[ReadOnly]
		public NativeBitArray.ReadOnly lordGrid;

		// Token: 0x04001F89 RID: 8073
		[ReadOnly]
		public NativeBitArray.ReadOnly water;

		// Token: 0x04001F8A RID: 8074
		[ReadOnly]
		public NativeBitArray.ReadOnly darknessDanger;

		// Token: 0x04001F8B RID: 8075
		[ReadOnly]
		public NativeBitArray.ReadOnly persistentDanger;

		// Token: 0x04001F8C RID: 8076
		[ReadOnly]
		public NativeBitArray.ReadOnly fogged;

		// Token: 0x04001F8D RID: 8077
		[ReadOnly]
		public NativeBitArray.ReadOnly player;

		// Token: 0x04001F8E RID: 8078
		[ReadOnly]
		public NativeArray<byte>.ReadOnly avoidGrid;

		// Token: 0x04001F8F RID: 8079
		[ReadOnly]
		public NativeArray<ushort>.ReadOnly factionCosts;

		// Token: 0x04001F90 RID: 8080
		[ReadOnly]
		public NativeArray<ushort>.ReadOnly buildingHitPoints;

		// Token: 0x04001F91 RID: 8081
		[ReadOnly]
		public NativeArray<ushort>.ReadOnly perceptualCost;

		// Token: 0x04001F92 RID: 8082
		[ReadOnly]
		public NativeArray<ushort>.ReadOnly custom;

		// Token: 0x04001F93 RID: 8083
		public NativeArray<int> grid;

		// Token: 0x04001F94 RID: 8084
		private bool passAllDestroyableThings;

		// Token: 0x04001F95 RID: 8085
		private bool passWater;
	}
}
