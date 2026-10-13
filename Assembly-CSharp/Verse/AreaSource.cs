using System;
using System.Collections.Generic;
using Unity.Collections;

namespace Verse
{
	// Token: 0x020005CD RID: 1485
	public class AreaSource : IPathFinderDataSource, IDisposable
	{
		// Token: 0x06002626 RID: 9766 RVA: 0x000C9380 File Offset: 0x000C7580
		public AreaSource(Map map)
		{
			this.map = map;
			this.cellCount = map.cellIndices.NumGridCells;
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x000C93CC File Offset: 0x000C75CC
		public void Dispose()
		{
			foreach (NativeBitArray nativeBitArray in this.areaToBits.Values)
			{
				nativeBitArray.Dispose();
			}
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x000C9424 File Offset: 0x000C7624
		public NativeBitArray DataForArea(Area area)
		{
			return this.areaToBits[area];
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x000C9434 File Offset: 0x000C7634
		public void ComputeAll(IEnumerable<PathRequest> _)
		{
			this.Prune();
			foreach (Area area in this.map.areaManager.AllAreas)
			{
				NativeBitArray value;
				if (!this.areaToBits.TryGetValue(area, out value))
				{
					value = new NativeBitArray(this.cellCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
					this.areaToBits[area] = value;
				}
				BoolGrid innerGrid = area.InnerGrid;
				for (int i = 0; i < this.cellCount; i++)
				{
					value.Set(i, innerGrid[i]);
				}
			}
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x000C94F0 File Offset: 0x000C76F0
		public bool UpdateIncrementally(IEnumerable<PathRequest> _, List<IntVec3> __)
		{
			this.Prune();
			bool result = false;
			CellIndices cellIndices = this.map.cellIndices;
			foreach (Area area in this.map.areaManager.AllAreas)
			{
				NativeBitArray value;
				if (!this.areaToBits.TryGetValue(area, out value))
				{
					value = new NativeBitArray(this.cellCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
					this.areaToBits[area] = value;
					BoolGrid innerGrid = area.InnerGrid;
					for (int i = 0; i < this.cellCount; i++)
					{
						value.Set(i, innerGrid[i]);
					}
					result = true;
				}
				List<IntVec3> list;
				if (this.areaToDeltas.TryGetValue(area, out list))
				{
					BoolGrid innerGrid2 = area.InnerGrid;
					foreach (IntVec3 c in list)
					{
						int num = cellIndices.CellToIndex(c);
						value.Set(num, innerGrid2[num]);
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x000C9634 File Offset: 0x000C7834
		public void Notify_AreaDelta(Area area, IntVec3 cell)
		{
			List<IntVec3> list;
			if (!this.areaToDeltas.TryGetValue(area, out list))
			{
				list = new List<IntVec3>();
				this.areaToDeltas[area] = list;
			}
			list.Add(cell);
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x000C966C File Offset: 0x000C786C
		private void Prune()
		{
			List<Area> allAreas = this.map.areaManager.AllAreas;
			foreach (Area item in this.areaToBits.Keys)
			{
				if (!allAreas.Contains(item))
				{
					this.tmpToPrune.Add(item);
				}
			}
			foreach (Area key in this.tmpToPrune)
			{
				this.areaToBits[key].Dispose();
				this.areaToBits.Remove(key);
				this.areaToDeltas.Remove(key);
			}
			this.tmpToPrune.Clear();
		}

		// Token: 0x04001FA8 RID: 8104
		private readonly Map map;

		// Token: 0x04001FA9 RID: 8105
		private readonly int cellCount;

		// Token: 0x04001FAA RID: 8106
		private readonly Dictionary<Area, NativeBitArray> areaToBits = new Dictionary<Area, NativeBitArray>();

		// Token: 0x04001FAB RID: 8107
		private readonly Dictionary<Area, List<IntVec3>> areaToDeltas = new Dictionary<Area, List<IntVec3>>();

		// Token: 0x04001FAC RID: 8108
		private readonly List<Area> tmpToPrune = new List<Area>();
	}
}
