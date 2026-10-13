using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004F5 RID: 1269
	public sealed class RegionGrid : IDisposable
	{
		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x060020EC RID: 8428 RVA: 0x000AC95C File Offset: 0x000AAB5C
		public IReadOnlyList<Room> AllRooms
		{
			get
			{
				return this.allRooms;
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x060020ED RID: 8429 RVA: 0x000AC964 File Offset: 0x000AAB64
		public IReadOnlyDictionary<int, Room> RoomLookup
		{
			get
			{
				return this.roomLookup;
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x060020EE RID: 8430 RVA: 0x000AC96C File Offset: 0x000AAB6C
		public Region[] DirectGrid
		{
			get
			{
				if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
				{
					Log.Warning("Trying to get the region grid but RegionAndRoomUpdater is disabled. The result may be incorrect.");
				}
				this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
				return this.regionGrid;
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x060020EF RID: 8431 RVA: 0x000AC9BD File Offset: 0x000AABBD
		public IEnumerable<Region> AllRegions_NoRebuild_InvalidAllowed
		{
			get
			{
				if (this.disposed)
				{
					yield break;
				}
				RegionGrid.allRegionsYielded.Clear();
				try
				{
					int count = this.map.cellIndices.NumGridCells;
					int num;
					for (int i = 0; i < count; i = num + 1)
					{
						if (this.regionGrid[i] != null && !RegionGrid.allRegionsYielded.Contains(this.regionGrid[i]))
						{
							yield return this.regionGrid[i];
							RegionGrid.allRegionsYielded.Add(this.regionGrid[i]);
						}
						num = i;
					}
				}
				finally
				{
					RegionGrid.allRegionsYielded.Clear();
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x060020F0 RID: 8432 RVA: 0x000AC9CD File Offset: 0x000AABCD
		public IEnumerable<Region> AllRegions
		{
			get
			{
				if (this.disposed)
				{
					yield break;
				}
				if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
				{
					Log.Warning("Trying to get all valid regions but RegionAndRoomUpdater is disabled. The result may be incorrect.");
				}
				this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
				RegionGrid.allRegionsYielded.Clear();
				try
				{
					int count = this.map.cellIndices.NumGridCells;
					int num;
					for (int i = 0; i < count; i = num + 1)
					{
						if (this.regionGrid[i] != null && this.regionGrid[i].valid && !RegionGrid.allRegionsYielded.Contains(this.regionGrid[i]))
						{
							yield return this.regionGrid[i];
							RegionGrid.allRegionsYielded.Add(this.regionGrid[i]);
						}
						num = i;
					}
				}
				finally
				{
					RegionGrid.allRegionsYielded.Clear();
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x060020F1 RID: 8433 RVA: 0x000AC9E0 File Offset: 0x000AABE0
		public RegionGrid(Map map)
		{
			this.map = map;
			this.regionGrid = new Region[map.cellIndices.NumGridCells];
		}

		// Token: 0x060020F2 RID: 8434 RVA: 0x000ACA3C File Offset: 0x000AAC3C
		public Region GetValidRegionAt(IntVec3 c)
		{
			if (this.disposed)
			{
				return null;
			}
			if (!c.InBounds(this.map))
			{
				string str = "Tried to get valid region out of bounds at ";
				IntVec3 intVec = c;
				Log.Error(str + intVec.ToString());
				return null;
			}
			if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
			{
				string str2 = "Trying to get valid region at ";
				IntVec3 intVec = c;
				Log.Warning(str2 + intVec.ToString() + " but RegionAndRoomUpdater is disabled. The result may be incorrect.");
			}
			this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
			Region region = this.regionGrid[this.map.cellIndices.CellToIndex(c)];
			if (region != null && region.valid)
			{
				return region;
			}
			return null;
		}

		// Token: 0x060020F3 RID: 8435 RVA: 0x000ACB00 File Offset: 0x000AAD00
		public Region GetValidRegionAt_NoRebuild(IntVec3 c)
		{
			if (this.disposed)
			{
				return null;
			}
			if (!c.InBounds(this.map))
			{
				string str = "Tried to get valid region out of bounds at ";
				IntVec3 intVec = c;
				Log.Error(str + intVec.ToString());
				return null;
			}
			Region region = this.regionGrid[this.map.cellIndices.CellToIndex(c)];
			if (region != null && region.valid)
			{
				return region;
			}
			return null;
		}

		// Token: 0x060020F4 RID: 8436 RVA: 0x000ACB6C File Offset: 0x000AAD6C
		public Region GetRegionAt_NoRebuild_InvalidAllowed(IntVec3 c)
		{
			if (this.disposed)
			{
				return null;
			}
			return this.regionGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x000ACB90 File Offset: 0x000AAD90
		public void SetRegionAt(IntVec3 c, Region reg)
		{
			this.regionGrid[this.map.cellIndices.CellToIndex(c)] = reg;
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x000ACBAB File Offset: 0x000AADAB
		public void RoomAdded(Room room)
		{
			this.allRooms.Add(room);
			this.roomLookup[room.ID] = room;
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x000ACBCB File Offset: 0x000AADCB
		public void RoomRemoved(Room room)
		{
			this.allRooms.Remove(room);
			this.roomLookup.Remove(room.ID);
		}

		// Token: 0x060020F8 RID: 8440 RVA: 0x000ACBEC File Offset: 0x000AADEC
		public void UpdateClean()
		{
			if (this.disposed)
			{
				return;
			}
			for (int i = 0; i < 16; i++)
			{
				if (this.curCleanIndex >= this.regionGrid.Length)
				{
					this.curCleanIndex = 0;
				}
				Region region = this.regionGrid[this.curCleanIndex];
				if (region != null && !region.valid)
				{
					this.regionGrid[this.curCleanIndex] = null;
				}
				this.curCleanIndex++;
			}
		}

		// Token: 0x060020F9 RID: 8441 RVA: 0x000ACC5C File Offset: 0x000AAE5C
		public void DebugDraw()
		{
			if (this.map != Find.CurrentMap || this.disposed)
			{
				return;
			}
			if (DebugViewSettings.drawRegionTraversal)
			{
				CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
				currentViewRect.ClipInsideMap(this.map);
				foreach (IntVec3 c in currentViewRect)
				{
					Region validRegionAt = this.GetValidRegionAt(c);
					if (validRegionAt != null && !this.drawnRegions.Contains(validRegionAt))
					{
						validRegionAt.DebugDraw();
						this.drawnRegions.Add(validRegionAt);
					}
				}
				this.drawnRegions.Clear();
			}
			IntVec3 intVec = UI.MouseCell();
			if (intVec.InBounds(this.map))
			{
				if (DebugViewSettings.drawDistricts)
				{
					District district = intVec.GetDistrict(this.map, RegionType.Set_Passable);
					if (district != null)
					{
						district.DebugDraw();
					}
				}
				if (DebugViewSettings.drawRooms)
				{
					Room room = intVec.GetRoom(this.map);
					if (room != null)
					{
						room.DebugDraw();
					}
				}
				if (DebugViewSettings.drawRegions || DebugViewSettings.drawRegionLinks || DebugViewSettings.drawRegionThings)
				{
					Region regionAt_NoRebuild_InvalidAllowed = this.GetRegionAt_NoRebuild_InvalidAllowed(intVec);
					if (regionAt_NoRebuild_InvalidAllowed != null)
					{
						regionAt_NoRebuild_InvalidAllowed.DebugDrawMouseover();
					}
				}
			}
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x000ACD98 File Offset: 0x000AAF98
		public void Dispose()
		{
			this.regionGrid = null;
			this.allDistricts.Clear();
			this.allRooms.Clear();
			this.roomLookup.Clear();
			RegionGrid.allRegionsYielded.Clear();
			this.disposed = true;
		}

		// Token: 0x04001C00 RID: 7168
		private readonly Map map;

		// Token: 0x04001C01 RID: 7169
		private Region[] regionGrid;

		// Token: 0x04001C02 RID: 7170
		private int curCleanIndex;

		// Token: 0x04001C03 RID: 7171
		private bool disposed;

		// Token: 0x04001C04 RID: 7172
		public List<District> allDistricts = new List<District>();

		// Token: 0x04001C05 RID: 7173
		private List<Room> allRooms = new List<Room>();

		// Token: 0x04001C06 RID: 7174
		private Dictionary<int, Room> roomLookup = new Dictionary<int, Room>();

		// Token: 0x04001C07 RID: 7175
		private static HashSet<Region> allRegionsYielded = new HashSet<Region>();

		// Token: 0x04001C08 RID: 7176
		private const int CleanSquaresPerFrame = 16;

		// Token: 0x04001C09 RID: 7177
		private HashSet<Region> drawnRegions = new HashSet<Region>();
	}
}
