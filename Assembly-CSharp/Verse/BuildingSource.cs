using System;
using System.Collections.Generic;
using LudeonTK;
using RimWorld;
using Unity.Collections;

namespace Verse
{
	// Token: 0x020005CE RID: 1486
	public class BuildingSource : IPathFinderDataSource, IDisposable
	{
		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x0600262D RID: 9773 RVA: 0x000C975C File Offset: 0x000C795C
		public NativeBitArray.ReadOnly Buildings
		{
			get
			{
				return this.buildings.AsReadOnly();
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x0600262E RID: 9774 RVA: 0x000C9769 File Offset: 0x000C7969
		public NativeBitArray.ReadOnly Destroyable
		{
			get
			{
				return this.destroyable.AsReadOnly();
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x0600262F RID: 9775 RVA: 0x000C9776 File Offset: 0x000C7976
		public NativeBitArray.ReadOnly Player
		{
			get
			{
				return this.player.AsReadOnly();
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06002630 RID: 9776 RVA: 0x000C9783 File Offset: 0x000C7983
		public NativeArray<ushort>.ReadOnly Hitpoints
		{
			get
			{
				return this.hitpoints.AsReadOnly();
			}
		}

		// Token: 0x06002631 RID: 9777 RVA: 0x000C9790 File Offset: 0x000C7990
		public BuildingSource(Map map)
		{
			this.map = map;
			this.cellCount = map.cellIndices.NumGridCells;
			this.buildings = new NativeBitArray(this.cellCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.destroyable = new NativeBitArray(this.cellCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.player = new NativeBitArray(this.cellCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.hitpoints = new NativeArray<ushort>(this.cellCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06002632 RID: 9778 RVA: 0x000C9816 File Offset: 0x000C7A16
		public void Dispose()
		{
			this.buildings.Dispose();
			this.destroyable.Dispose();
			this.player.Dispose();
			this.hitpoints.Dispose();
		}

		// Token: 0x06002633 RID: 9779 RVA: 0x000C9844 File Offset: 0x000C7A44
		public void ComputeAll(IEnumerable<PathRequest> _)
		{
			this.buildings.Clear();
			this.destroyable.Clear();
			this.player.Clear();
			this.hitpoints.Clear<ushort>();
			Building[] innerArray = this.map.edificeGrid.InnerArray;
			for (int i = 0; i < this.cellCount; i++)
			{
				if (innerArray[i] != null)
				{
					this.SetBuildingData(i);
				}
			}
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x000C98AC File Offset: 0x000C7AAC
		public bool UpdateIncrementally(IEnumerable<PathRequest> _, List<IntVec3> cellDeltas)
		{
			CellIndices cellIndices = this.map.cellIndices;
			Building[] innerArray = this.map.edificeGrid.InnerArray;
			foreach (IntVec3 c in cellDeltas)
			{
				int num = cellIndices.CellToIndex(c);
				if (innerArray[num] != null)
				{
					this.SetBuildingData(num);
				}
				else
				{
					this.buildings.Set(num, false);
					this.destroyable.Set(num, false);
					this.player.Set(num, false);
					this.hitpoints[num] = 0;
				}
			}
			return false;
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x000C9964 File Offset: 0x000C7B64
		private void SetBuildingData(int index)
		{
			Building building = this.map.edificeGrid[index];
			if (building == null)
			{
				return;
			}
			if (building.def.Fillage == FillCategory.Full)
			{
				this.buildings.Set(index, true);
				this.destroyable.Set(index, building.def.destroyable);
				this.hitpoints[index] = (ushort)Math.Clamp(building.HitPoints, 0, 65535);
			}
			else
			{
				this.buildings.Set(index, false);
				this.destroyable.Set(index, false);
				this.hitpoints[index] = 0;
			}
			Faction faction = building.Faction;
			this.player.Set(index, faction != null && faction.IsPlayer);
		}

		// Token: 0x04001FAD RID: 8109
		private readonly Map map;

		// Token: 0x04001FAE RID: 8110
		private readonly int cellCount;

		// Token: 0x04001FAF RID: 8111
		private NativeBitArray buildings;

		// Token: 0x04001FB0 RID: 8112
		private NativeBitArray destroyable;

		// Token: 0x04001FB1 RID: 8113
		private NativeBitArray player;

		// Token: 0x04001FB2 RID: 8114
		private NativeArray<ushort> hitpoints;
	}
}
