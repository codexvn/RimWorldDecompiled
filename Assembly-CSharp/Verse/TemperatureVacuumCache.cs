using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200053A RID: 1338
	public sealed class TemperatureVacuumCache : IExposable
	{
		// Token: 0x060022C2 RID: 8898 RVA: 0x000B4D68 File Offset: 0x000B2F68
		public TemperatureVacuumCache(Map map)
		{
			this.map = map;
			this.tempCache = new CachedTempInfo[map.cellIndices.NumGridCells];
			this.TemperatureVacuumSaveLoad = new TemperatureVacuumSaveLoad(map);
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x000B4DBC File Offset: 0x000B2FBC
		public void ResetTemperatureCache()
		{
			int numGridCells = this.map.cellIndices.NumGridCells;
			for (int i = 0; i < numGridCells; i++)
			{
				this.tempCache[i].Reset();
			}
		}

		// Token: 0x060022C4 RID: 8900 RVA: 0x000B4DF7 File Offset: 0x000B2FF7
		public void ExposeData()
		{
			this.TemperatureVacuumSaveLoad.DoExposeWork();
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x000B4E04 File Offset: 0x000B3004
		public void ResetCachedCellInfo(IntVec3 c)
		{
			this.tempCache[this.map.cellIndices.CellToIndex(c)].Reset();
		}

		// Token: 0x060022C6 RID: 8902 RVA: 0x000B4E27 File Offset: 0x000B3027
		private void SetCachedCellInfo(IntVec3 c, CachedTempInfo info)
		{
			this.tempCache[this.map.cellIndices.CellToIndex(c)] = info;
		}

		// Token: 0x060022C7 RID: 8903 RVA: 0x000B4E48 File Offset: 0x000B3048
		public void TryCacheRegionTempInfo(IntVec3 c, Region reg)
		{
			Room room = reg.Room;
			if (room != null)
			{
				this.SetCachedCellInfo(c, new CachedTempInfo(room.ID, room.CellCount, room.Temperature, room.Vacuum));
			}
		}

		// Token: 0x060022C8 RID: 8904 RVA: 0x000B4E84 File Offset: 0x000B3084
		public bool TryGetAverageCachedRoomTempVacuum(Room r, out float temperature, out float vacuum)
		{
			CellIndices cellIndices = this.map.cellIndices;
			foreach (IntVec3 c in r.Cells)
			{
				CachedTempInfo cachedTempInfo = this.map.TemperatureVacuumCache.tempCache[cellIndices.CellToIndex(c)];
				if (cachedTempInfo.numCells > 0 && !this.processedRoomIDs.Contains(cachedTempInfo.roomID))
				{
					this.relevantTempInfoList.Add(cachedTempInfo);
					this.processedRoomIDs.Add(cachedTempInfo.roomID);
				}
			}
			int num = 0;
			float num2 = 0f;
			float num3 = 0f;
			foreach (CachedTempInfo cachedTempInfo2 in this.relevantTempInfoList)
			{
				num += cachedTempInfo2.numCells;
				num2 += cachedTempInfo2.temperature * (float)cachedTempInfo2.numCells;
				num3 += cachedTempInfo2.vacuum * (float)cachedTempInfo2.numCells;
			}
			temperature = num2 / (float)num;
			vacuum = num3 / (float)num;
			bool result = !this.relevantTempInfoList.NullOrEmpty<CachedTempInfo>();
			this.processedRoomIDs.Clear();
			this.relevantTempInfoList.Clear();
			return result;
		}

		// Token: 0x04001D1C RID: 7452
		private readonly Map map;

		// Token: 0x04001D1D RID: 7453
		internal TemperatureVacuumSaveLoad TemperatureVacuumSaveLoad;

		// Token: 0x04001D1E RID: 7454
		private readonly CachedTempInfo[] tempCache;

		// Token: 0x04001D1F RID: 7455
		private readonly HashSet<int> processedRoomIDs = new HashSet<int>();

		// Token: 0x04001D20 RID: 7456
		private readonly List<CachedTempInfo> relevantTempInfoList = new List<CachedTempInfo>();
	}
}
