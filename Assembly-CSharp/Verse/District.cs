using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020004DD RID: 1245
	public sealed class District
	{
		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x0600201C RID: 8220 RVA: 0x000A7EF0 File Offset: 0x000A60F0
		public Map Map
		{
			get
			{
				if (this.mapIndex >= 0)
				{
					return Find.Maps[(int)this.mapIndex];
				}
				return null;
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x0600201D RID: 8221 RVA: 0x000A7F0D File Offset: 0x000A610D
		public RegionType RegionType
		{
			get
			{
				if (!this.regions.Any<Region>())
				{
					return RegionType.None;
				}
				return this.regions[0].type;
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x0600201E RID: 8222 RVA: 0x000A7F2F File Offset: 0x000A612F
		public List<Region> Regions
		{
			get
			{
				return this.regions;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x0600201F RID: 8223 RVA: 0x000A7F37 File Offset: 0x000A6137
		public int RegionCount
		{
			get
			{
				return this.regions.Count;
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06002020 RID: 8224 RVA: 0x000A7F44 File Offset: 0x000A6144
		public bool TouchesMapEdge
		{
			get
			{
				return this.numRegionsTouchingMapEdge > 0;
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06002021 RID: 8225 RVA: 0x000A7F4F File Offset: 0x000A614F
		public bool Passable
		{
			get
			{
				return this.RegionType.Passable();
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06002022 RID: 8226 RVA: 0x000A7F5C File Offset: 0x000A615C
		public bool IsDoorway
		{
			get
			{
				return this.regions.Count == 1 && this.regions[0].IsDoorway;
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06002023 RID: 8227 RVA: 0x000A7F7F File Offset: 0x000A617F
		// (set) Token: 0x06002024 RID: 8228 RVA: 0x000A7F87 File Offset: 0x000A6187
		public Room Room
		{
			get
			{
				return this.roomInt;
			}
			set
			{
				if (value == this.roomInt)
				{
					return;
				}
				if (this.roomInt != null)
				{
					this.roomInt.RemoveDistrict(this);
				}
				this.roomInt = value;
				if (this.roomInt != null)
				{
					this.roomInt.AddDistrict(this);
				}
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06002025 RID: 8229 RVA: 0x000A7FC4 File Offset: 0x000A61C4
		public int CellCount
		{
			get
			{
				if (this.cachedCellCount == -1)
				{
					this.cachedCellCount = 0;
					for (int i = 0; i < this.regions.Count; i++)
					{
						this.cachedCellCount += this.regions[i].CellCount;
					}
				}
				return this.cachedCellCount;
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06002026 RID: 8230 RVA: 0x000A801C File Offset: 0x000A621C
		public List<District> Neighbors
		{
			get
			{
				this.uniqueNeighborsSet.Clear();
				this.uniqueNeighbors.Clear();
				for (int i = 0; i < this.regions.Count; i++)
				{
					foreach (Region region in this.regions[i].Neighbors)
					{
						if (this.uniqueNeighborsSet.Add(region.District) && region.District != this)
						{
							this.uniqueNeighbors.Add(region.District);
						}
					}
				}
				this.uniqueNeighborsSet.Clear();
				return this.uniqueNeighbors;
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06002027 RID: 8231 RVA: 0x000A80D8 File Offset: 0x000A62D8
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				int num;
				for (int i = 0; i < this.regions.Count; i = num + 1)
				{
					foreach (IntVec3 intVec in this.regions[i].Cells)
					{
						yield return intVec;
					}
					IEnumerator<IntVec3> enumerator = null;
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x000A80E8 File Offset: 0x000A62E8
		public static District MakeNew(Map map)
		{
			District district = new District();
			district.mapIndex = (sbyte)map.Index;
			district.ID = District.nextDistrictID;
			District.nextDistrictID++;
			return district;
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x000A8114 File Offset: 0x000A6314
		public void AddRegion(Region r)
		{
			if (this.regions.Contains(r))
			{
				Log.Error("Tried to add the same region twice to District. region=" + ((r != null) ? r.ToString() : null) + ", district=" + ((this != null) ? this.ToString() : null));
				return;
			}
			this.regions.Add(r);
			if (r.touchesMapEdge)
			{
				this.numRegionsTouchingMapEdge++;
			}
			if (this.regions.Count == 1)
			{
				this.Map.regionGrid.allDistricts.Add(this);
			}
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x000A81A4 File Offset: 0x000A63A4
		public void RemoveRegion(Region r)
		{
			if (!this.regions.Contains(r))
			{
				Log.Error("Tried to remove region from District but this region is not here. region=" + ((r != null) ? r.ToString() : null) + ", district=" + ((this != null) ? this.ToString() : null));
				return;
			}
			this.regions.Remove(r);
			if (r.touchesMapEdge)
			{
				this.numRegionsTouchingMapEdge--;
			}
			if (this.regions.Count == 0)
			{
				this.Room = null;
				this.cachedOpenRoofCount = -1;
				this.cachedExposedCount = -1;
				IEnumerator<IntVec3> enumerator = this.cachedOpenRoofState;
				if (enumerator != null)
				{
					enumerator.Dispose();
				}
				this.cachedOpenRoofState = null;
				IEnumerator<IntVec3> enumerator2 = this.cachedExposedState;
				if (enumerator2 != null)
				{
					enumerator2.Dispose();
				}
				this.cachedExposedState = null;
				Map map = this.Map;
				if (map == null)
				{
					return;
				}
				map.regionGrid.allDistricts.Remove(this);
			}
		}

		// Token: 0x0600202B RID: 8235 RVA: 0x000A827F File Offset: 0x000A647F
		public void Notify_MyMapRemoved()
		{
			this.mapIndex = -1;
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x000A8288 File Offset: 0x000A6488
		public void Notify_TerrainChanged()
		{
			this.cachedExposedCount = -1;
			IEnumerator<IntVec3> enumerator = this.cachedExposedState;
			if (enumerator != null)
			{
				enumerator.Dispose();
			}
			this.cachedExposedState = null;
			this.Room.Notify_TerrainChanged();
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x000A82B4 File Offset: 0x000A64B4
		public void Notify_RoofChanged()
		{
			this.cachedOpenRoofCount = -1;
			this.cachedExposedCount = -1;
			IEnumerator<IntVec3> enumerator = this.cachedOpenRoofState;
			if (enumerator != null)
			{
				enumerator.Dispose();
			}
			this.cachedOpenRoofState = null;
			IEnumerator<IntVec3> enumerator2 = this.cachedExposedState;
			if (enumerator2 != null)
			{
				enumerator2.Dispose();
			}
			this.cachedExposedState = null;
			this.Room.Notify_RoofChanged();
		}

		// Token: 0x0600202E RID: 8238 RVA: 0x000A830C File Offset: 0x000A650C
		public void Notify_RoomShapeOrContainedBedsChanged()
		{
			this.cachedCellCount = -1;
			this.cachedOpenRoofCount = -1;
			this.cachedExposedCount = -1;
			IEnumerator<IntVec3> enumerator = this.cachedOpenRoofState;
			if (enumerator != null)
			{
				enumerator.Dispose();
			}
			this.cachedOpenRoofState = null;
			IEnumerator<IntVec3> enumerator2 = this.cachedExposedState;
			if (enumerator2 != null)
			{
				enumerator2.Dispose();
			}
			this.cachedExposedState = null;
			AnimalPenConnectedDistrictsCalculator.InvalidateDistrictCache(this);
			this.lastChangeTick = Find.TickManager.TicksGame;
			FacilitiesUtility.NotifyFacilitiesAboutChangedLOSBlockers(this.regions);
		}

		// Token: 0x0600202F RID: 8239 RVA: 0x000A8380 File Offset: 0x000A6580
		public void DecrementMapIndex()
		{
			if (this.mapIndex <= 0)
			{
				Log.Warning("Tried to decrement map index for district " + this.ID.ToString() + ", but mapIndex=" + this.mapIndex.ToString());
				return;
			}
			this.mapIndex -= 1;
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x000A83D0 File Offset: 0x000A65D0
		public int OpenRoofCountStopAt(int threshold)
		{
			if (this.cachedOpenRoofCount == -1 && this.cachedOpenRoofState == null)
			{
				this.cachedOpenRoofCount = 0;
				this.cachedOpenRoofState = this.Cells.GetEnumerator();
			}
			if (this.cachedOpenRoofCount < threshold && this.cachedOpenRoofState != null)
			{
				RoofGrid roofGrid = this.Map.roofGrid;
				while (this.cachedOpenRoofCount < threshold && this.cachedOpenRoofState.MoveNext())
				{
					if (!roofGrid.Roofed(this.cachedOpenRoofState.Current))
					{
						this.cachedOpenRoofCount++;
					}
				}
				if (this.cachedOpenRoofCount < threshold)
				{
					this.cachedOpenRoofState.Dispose();
					this.cachedOpenRoofState = null;
				}
			}
			return this.cachedOpenRoofCount;
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x000A8480 File Offset: 0x000A6680
		public int ExposedVacuumCount(int threshold)
		{
			if (this.cachedExposedCount == -1 && this.cachedExposedState == null)
			{
				this.cachedExposedCount = 0;
				this.cachedExposedState = this.Cells.GetEnumerator();
			}
			if (this.cachedExposedCount < threshold && this.cachedExposedState != null)
			{
				RoofGrid roofGrid = this.Map.roofGrid;
				TerrainGrid terrainGrid = this.Map.terrainGrid;
				while (this.cachedExposedCount < threshold && this.cachedExposedState.MoveNext())
				{
					IntVec3 c = this.cachedExposedState.Current;
					if (!roofGrid.Roofed(c) || terrainGrid.TerrainAt(c).exposesToVacuum)
					{
						this.cachedExposedCount++;
					}
				}
				if (this.cachedExposedCount < threshold)
				{
					this.cachedExposedState.Dispose();
					this.cachedExposedState = null;
				}
			}
			return this.cachedExposedCount;
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x000A854C File Offset: 0x000A674C
		internal void DebugDraw()
		{
			int hashCode = this.GetHashCode();
			foreach (IntVec3 c in this.Cells)
			{
				CellRenderer.RenderCell(c, (float)hashCode * 0.01f);
			}
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x000A85A8 File Offset: 0x000A67A8
		internal string DebugString()
		{
			return string.Concat(new string[]
			{
				"District ID=",
				this.ID.ToString(),
				"\n  first cell=",
				this.Cells.FirstOrDefault<IntVec3>().ToString(),
				"\n  RegionCount=",
				this.RegionCount.ToString(),
				"\n  RegionType=",
				this.RegionType.ToString(),
				"\n  CellCount=",
				this.CellCount.ToString(),
				"\n  OpenRoofCount=",
				this.OpenRoofCountStopAt(int.MaxValue).ToString(),
				"\n  numRegionsTouchingMapEdge=",
				this.numRegionsTouchingMapEdge.ToString(),
				"\n  lastChangeTick=",
				this.lastChangeTick.ToString(),
				"\n  Room=",
				(this.Room != null) ? this.Room.ID.ToString() : "null"
			});
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x000A86CC File Offset: 0x000A68CC
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"District(districtID=",
				this.ID.ToString(),
				", first=",
				this.Cells.FirstOrDefault<IntVec3>().ToString(),
				", RegionsCount=",
				this.RegionCount.ToString(),
				", lastChangeTick=",
				this.lastChangeTick.ToString(),
				")"
			});
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x000A8756 File Offset: 0x000A6956
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.ID, 1538478890);
		}

		// Token: 0x04001B77 RID: 7031
		public sbyte mapIndex = -1;

		// Token: 0x04001B78 RID: 7032
		private Room roomInt;

		// Token: 0x04001B79 RID: 7033
		private List<Region> regions = new List<Region>();

		// Token: 0x04001B7A RID: 7034
		public int ID = -16161616;

		// Token: 0x04001B7B RID: 7035
		public int lastChangeTick = -1;

		// Token: 0x04001B7C RID: 7036
		private int numRegionsTouchingMapEdge;

		// Token: 0x04001B7D RID: 7037
		private int cachedOpenRoofCount = -1;

		// Token: 0x04001B7E RID: 7038
		private int cachedExposedCount = -1;

		// Token: 0x04001B7F RID: 7039
		private IEnumerator<IntVec3> cachedOpenRoofState;

		// Token: 0x04001B80 RID: 7040
		private IEnumerator<IntVec3> cachedExposedState;

		// Token: 0x04001B81 RID: 7041
		private int cachedCellCount = -1;

		// Token: 0x04001B82 RID: 7042
		public int newOrReusedRoomIndex = -1;

		// Token: 0x04001B83 RID: 7043
		private static int nextDistrictID;

		// Token: 0x04001B84 RID: 7044
		private HashSet<District> uniqueNeighborsSet = new HashSet<District>();

		// Token: 0x04001B85 RID: 7045
		private List<District> uniqueNeighbors = new List<District>();
	}
}
