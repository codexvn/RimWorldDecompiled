using System;
using System.Collections.Generic;
using LudeonTK;
using Unity.Collections;

namespace Verse
{
	// Token: 0x020005D5 RID: 1493
	public class PerceptualSource : IPathFinderDataSource, IDisposable
	{
		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06002652 RID: 9810 RVA: 0x000CA840 File Offset: 0x000C8A40
		public NativeArray<ushort>.ReadOnly CostDrafted
		{
			get
			{
				return this.costDrafted.AsReadOnly();
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06002653 RID: 9811 RVA: 0x000CA84D File Offset: 0x000C8A4D
		public NativeArray<ushort>.ReadOnly CostUndrafted
		{
			get
			{
				return this.costUndrafted.AsReadOnly();
			}
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x000CA85C File Offset: 0x000C8A5C
		public PerceptualSource(Map map)
		{
			this.map = map;
			int numGridCells = map.cellIndices.NumGridCells;
			this.costDrafted = new NativeArray<ushort>(numGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.costUndrafted = new NativeArray<ushort>(numGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06002655 RID: 9813 RVA: 0x000CA89E File Offset: 0x000C8A9E
		public void Dispose()
		{
			this.costDrafted.Dispose();
			this.costUndrafted.Dispose();
		}

		// Token: 0x06002656 RID: 9814 RVA: 0x000CA8B8 File Offset: 0x000C8AB8
		public void ComputeAll(IEnumerable<PathRequest> _)
		{
			this.costDrafted.Clear<ushort>();
			this.costUndrafted.Clear<ushort>();
			TerrainGrid terrainGrid = this.map.terrainGrid;
			for (int i = 0; i < this.map.cellIndices.NumGridCells; i++)
			{
				TerrainDef terrainDef = terrainGrid.TerrainAt(i);
				if (terrainDef != null)
				{
					this.costUndrafted[i] = (ushort)Math.Clamp(terrainDef.extraNonDraftedPerceivedPathCost, 0, 65535);
					this.costDrafted[i] = (ushort)Math.Clamp(terrainDef.extraDraftedPerceivedPathCost, 0, 65535);
				}
			}
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x000CA94C File Offset: 0x000C8B4C
		public bool UpdateIncrementally(IEnumerable<PathRequest> _, List<IntVec3> cellDeltas)
		{
			CellIndices cellIndices = this.map.cellIndices;
			TerrainGrid terrainGrid = this.map.terrainGrid;
			foreach (IntVec3 c in cellDeltas)
			{
				int num = cellIndices.CellToIndex(c);
				TerrainDef terrainDef = terrainGrid.TerrainAt(num);
				if (terrainDef != null)
				{
					this.costUndrafted[num] = (ushort)Math.Clamp(terrainDef.extraNonDraftedPerceivedPathCost, 0, 65535);
					this.costDrafted[num] = (ushort)Math.Clamp(terrainDef.extraDraftedPerceivedPathCost, 0, 65535);
				}
				else
				{
					this.costUndrafted[num] = 0;
					this.costDrafted[num] = 0;
				}
			}
			return false;
		}

		// Token: 0x04001FBD RID: 8125
		private readonly Map map;

		// Token: 0x04001FBE RID: 8126
		private NativeArray<ushort> costDrafted;

		// Token: 0x04001FBF RID: 8127
		private NativeArray<ushort> costUndrafted;
	}
}
