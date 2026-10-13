using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020004F3 RID: 1267
	public class RegionAndRoomUpdater
	{
		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x060020D1 RID: 8401 RVA: 0x000ABA22 File Offset: 0x000A9C22
		// (set) Token: 0x060020D2 RID: 8402 RVA: 0x000ABA2A File Offset: 0x000A9C2A
		public bool Enabled
		{
			get
			{
				return this.enabledInt;
			}
			set
			{
				this.enabledInt = value;
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x060020D3 RID: 8403 RVA: 0x000ABA33 File Offset: 0x000A9C33
		public bool AnythingToRebuild
		{
			get
			{
				return this.map.regionDirtyer.AnyDirty || !this.initialized;
			}
		}

		// Token: 0x060020D4 RID: 8404 RVA: 0x000ABA54 File Offset: 0x000A9C54
		public RegionAndRoomUpdater(Map map)
		{
			this.map = map;
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x000ABAE3 File Offset: 0x000A9CE3
		public void RebuildAllRegionsAndRooms()
		{
			if (!this.Enabled)
			{
				Log.Warning("Called RebuildAllRegionsAndRooms() but RegionAndRoomUpdater is disabled. Regions won't be rebuilt.");
			}
			this.map.TemperatureVacuumCache.ResetTemperatureCache();
			this.map.regionDirtyer.SetAllDirty();
			this.TryRebuildDirtyRegionsAndRooms();
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x000ABB20 File Offset: 0x000A9D20
		public void TryRebuildDirtyRegionsAndRooms()
		{
			if (this.working || !this.Enabled)
			{
				return;
			}
			this.working = true;
			if (!this.initialized)
			{
				this.RebuildAllRegionsAndRooms();
			}
			if (!this.map.regionDirtyer.AnyDirty)
			{
				this.working = false;
				return;
			}
			try
			{
				this.RegenerateNewRegionsFromDirtyCells();
				this.CreateOrUpdateRooms();
			}
			catch (Exception ex)
			{
				string str = "Exception while rebuilding dirty regions: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
			this.newRegions.Clear();
			this.map.regionDirtyer.SetAllClean();
			this.initialized = true;
			this.working = false;
			if (DebugSettings.detectRegionListersBugs)
			{
				Autotests_RegionListers.CheckBugs(this.map);
			}
			this.map.events.Notify_RegionsRoomsChanged();
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x000ABBF8 File Offset: 0x000A9DF8
		private void RegenerateNewRegionsFromDirtyCells()
		{
			this.newRegions.Clear();
			foreach (IntVec3 intVec in this.map.regionDirtyer.DirtyCells)
			{
				if (intVec.GetRegion(this.map, RegionType.Set_All) == null)
				{
					Region region = this.map.regionMaker.TryGenerateRegionFrom(intVec);
					if (region != null)
					{
						this.newRegions.Add(region);
					}
				}
			}
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x000ABC8C File Offset: 0x000A9E8C
		private void CreateOrUpdateRooms()
		{
			this.newDistricts.Clear();
			this.reusedOldDistricts.Clear();
			this.newRooms.Clear();
			this.reusedOldRooms.Clear();
			int numRegionGroups = this.CombineNewRegionsIntoContiguousGroups();
			this.CreateOrAttachToExistingDistricts(numRegionGroups);
			int numRooms = this.CombineNewAndReusedDistrictsIntoContiguousRooms();
			this.CreateOrAttachToExistingRooms(numRooms);
			this.NotifyAffectedDistrictsAndRoomsAndUpdateTemperatureVacuum();
			this.newDistricts.Clear();
			this.reusedOldDistricts.Clear();
			this.newRooms.Clear();
			this.reusedOldRooms.Clear();
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x000ABD14 File Offset: 0x000A9F14
		private int CombineNewRegionsIntoContiguousGroups()
		{
			int num = 0;
			for (int i = 0; i < this.newRegions.Count; i++)
			{
				if (this.newRegions[i].newRegionGroupIndex < 0)
				{
					RegionTraverser.FloodAndSetNewRegionIndex(this.newRegions[i], num);
					num++;
				}
			}
			return num;
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x000ABD64 File Offset: 0x000A9F64
		private void CreateOrAttachToExistingDistricts(int numRegionGroups)
		{
			for (int i = 0; i < numRegionGroups; i++)
			{
				this.currentRegionGroup.Clear();
				for (int j = 0; j < this.newRegions.Count; j++)
				{
					if (this.newRegions[j].newRegionGroupIndex == i)
					{
						this.currentRegionGroup.Add(this.newRegions[j]);
					}
				}
				if (!this.currentRegionGroup[0].type.AllowsMultipleRegionsPerDistrict())
				{
					if (this.currentRegionGroup.Count != 1)
					{
						Log.Error("Region type doesn't allow multiple regions per room but there are >1 regions in this group.");
					}
					District district = District.MakeNew(this.map);
					this.currentRegionGroup[0].District = district;
					this.newDistricts.Add(district);
				}
				else
				{
					bool flag;
					District district2 = this.FindCurrentRegionGroupNeighborWithMostRegions(out flag);
					if (district2 == null)
					{
						District item = RegionTraverser.FloodAndSetDistricts(this.currentRegionGroup[0], this.map, null);
						this.newDistricts.Add(item);
					}
					else if (!flag)
					{
						for (int k = 0; k < this.currentRegionGroup.Count; k++)
						{
							this.currentRegionGroup[k].District = district2;
						}
						this.reusedOldDistricts.Add(district2);
					}
					else
					{
						RegionTraverser.FloodAndSetDistricts(this.currentRegionGroup[0], this.map, district2);
						this.reusedOldDistricts.Add(district2);
					}
				}
			}
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x000ABECC File Offset: 0x000AA0CC
		private int CombineNewAndReusedDistrictsIntoContiguousRooms()
		{
			int num = 0;
			foreach (District district in this.reusedOldDistricts)
			{
				district.newOrReusedRoomIndex = -1;
			}
			foreach (District district2 in this.reusedOldDistricts.Concat(this.newDistricts))
			{
				if (district2.newOrReusedRoomIndex < 0)
				{
					this.tmpDistrictStack.Clear();
					this.tmpDistrictStack.Push(district2);
					district2.newOrReusedRoomIndex = num;
					while (this.tmpDistrictStack.Count != 0)
					{
						District district3 = this.tmpDistrictStack.Pop();
						foreach (District district4 in district3.Neighbors)
						{
							if (district4.newOrReusedRoomIndex < 0 && this.ShouldBeInTheSameRoom(district3, district4))
							{
								district4.newOrReusedRoomIndex = num;
								this.tmpDistrictStack.Push(district4);
							}
						}
					}
					this.tmpDistrictStack.Clear();
					num++;
				}
			}
			return num;
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x000AC024 File Offset: 0x000AA224
		private void CreateOrAttachToExistingRooms(int numRooms)
		{
			for (int i = 0; i < numRooms; i++)
			{
				this.currentDistrictGroup.Clear();
				foreach (District district in this.reusedOldDistricts)
				{
					if (district.newOrReusedRoomIndex == i)
					{
						this.currentDistrictGroup.Add(district);
					}
				}
				for (int j = 0; j < this.newDistricts.Count; j++)
				{
					if (this.newDistricts[j].newOrReusedRoomIndex == i)
					{
						this.currentDistrictGroup.Add(this.newDistricts[j]);
					}
				}
				bool flag;
				Room room = this.FindCurrentRoomNeighborWithMostRegions(out flag);
				if (room == null)
				{
					Room room2 = Room.MakeNew(this.map);
					this.roomLookup[room2.ID] = room2;
					this.FloodAndSetRooms(this.currentDistrictGroup[0], room2);
					this.newRooms.Add(room2);
				}
				else if (!flag)
				{
					for (int k = 0; k < this.currentDistrictGroup.Count; k++)
					{
						this.currentDistrictGroup[k].Room = room;
					}
					this.reusedOldRooms.Add(room);
				}
				else
				{
					this.FloodAndSetRooms(this.currentDistrictGroup[0], room);
					this.reusedOldRooms.Add(room);
				}
			}
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x000AC19C File Offset: 0x000AA39C
		private void FloodAndSetRooms(District start, Room room)
		{
			this.tmpDistrictStack.Clear();
			this.tmpDistrictStack.Push(start);
			this.tmpVisitedDistricts.Clear();
			this.tmpVisitedDistricts.Add(start);
			while (this.tmpDistrictStack.Count != 0)
			{
				District district = this.tmpDistrictStack.Pop();
				district.Room = room;
				foreach (District district2 in district.Neighbors)
				{
					if (!this.tmpVisitedDistricts.Contains(district2) && this.ShouldBeInTheSameRoom(district, district2))
					{
						this.tmpDistrictStack.Push(district2);
						this.tmpVisitedDistricts.Add(district2);
					}
				}
			}
			this.tmpVisitedDistricts.Clear();
			this.tmpDistrictStack.Clear();
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x000AC280 File Offset: 0x000AA480
		private void NotifyAffectedDistrictsAndRoomsAndUpdateTemperatureVacuum()
		{
			foreach (District district in this.reusedOldDistricts)
			{
				district.Notify_RoomShapeOrContainedBedsChanged();
			}
			for (int i = 0; i < this.newDistricts.Count; i++)
			{
				this.newDistricts[i].Notify_RoomShapeOrContainedBedsChanged();
			}
			foreach (Room room in this.reusedOldRooms)
			{
				room.Notify_RoomShapeChanged();
			}
			for (int j = 0; j < this.newRooms.Count; j++)
			{
				Room room2 = this.newRooms[j];
				room2.Notify_RoomShapeChanged();
				float temperature;
				float vacuum;
				if (this.map.TemperatureVacuumCache.TryGetAverageCachedRoomTempVacuum(room2, out temperature, out vacuum))
				{
					room2.Temperature = temperature;
					room2.Vacuum = vacuum;
				}
				else if (this.map.Biome.inVacuum)
				{
					room2.Vacuum = 1f;
				}
			}
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x000AC3AC File Offset: 0x000AA5AC
		private District FindCurrentRegionGroupNeighborWithMostRegions(out bool multipleOldNeighborDistricts)
		{
			multipleOldNeighborDistricts = false;
			District district = null;
			for (int i = 0; i < this.currentRegionGroup.Count; i++)
			{
				foreach (Region region in this.currentRegionGroup[i].NeighborsOfSameType)
				{
					if (region.District != null && !this.reusedOldDistricts.Contains(region.District))
					{
						if (district == null)
						{
							district = region.District;
						}
						else if (region.District != district)
						{
							multipleOldNeighborDistricts = true;
							if (region.District.RegionCount > district.RegionCount)
							{
								district = region.District;
							}
						}
					}
				}
			}
			return district;
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x000AC46C File Offset: 0x000AA66C
		private Room FindCurrentRoomNeighborWithMostRegions(out bool multipleOldNeighborRooms)
		{
			multipleOldNeighborRooms = false;
			Room room = null;
			for (int i = 0; i < this.currentDistrictGroup.Count; i++)
			{
				foreach (District district in this.currentDistrictGroup[i].Neighbors)
				{
					if (district.Room != null && this.ShouldBeInTheSameRoom(this.currentDistrictGroup[i], district) && !this.reusedOldRooms.Contains(district.Room))
					{
						if (room == null)
						{
							room = district.Room;
						}
						else if (district.Room != room)
						{
							multipleOldNeighborRooms = true;
							if (district.Room.RegionCount > room.RegionCount)
							{
								room = district.Room;
							}
						}
					}
				}
			}
			return room;
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x000AC548 File Offset: 0x000AA748
		private bool ShouldBeInTheSameRoom(District a, District b)
		{
			RegionType regionType = a.RegionType;
			RegionType regionType2 = b.RegionType;
			return (regionType == RegionType.Normal || regionType == RegionType.ImpassableFreeAirExchange || regionType == RegionType.Fence) && (regionType2 == RegionType.Normal || regionType2 == RegionType.ImpassableFreeAirExchange || regionType2 == RegionType.Fence);
		}

		// Token: 0x04001BEF RID: 7151
		private Map map;

		// Token: 0x04001BF0 RID: 7152
		private List<Region> newRegions = new List<Region>();

		// Token: 0x04001BF1 RID: 7153
		private List<District> newDistricts = new List<District>();

		// Token: 0x04001BF2 RID: 7154
		private HashSet<District> reusedOldDistricts = new HashSet<District>();

		// Token: 0x04001BF3 RID: 7155
		private List<Room> newRooms = new List<Room>();

		// Token: 0x04001BF4 RID: 7156
		private HashSet<Room> reusedOldRooms = new HashSet<Room>();

		// Token: 0x04001BF5 RID: 7157
		private List<Region> currentRegionGroup = new List<Region>();

		// Token: 0x04001BF6 RID: 7158
		private List<District> currentDistrictGroup = new List<District>();

		// Token: 0x04001BF7 RID: 7159
		private Stack<District> tmpDistrictStack = new Stack<District>();

		// Token: 0x04001BF8 RID: 7160
		private HashSet<District> tmpVisitedDistricts = new HashSet<District>();

		// Token: 0x04001BF9 RID: 7161
		private bool initialized;

		// Token: 0x04001BFA RID: 7162
		private bool working;

		// Token: 0x04001BFB RID: 7163
		private bool enabledInt = true;

		// Token: 0x04001BFC RID: 7164
		public Dictionary<int, Room> roomLookup = new Dictionary<int, Room>();
	}
}
