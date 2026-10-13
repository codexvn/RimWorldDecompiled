using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200050C RID: 1292
	public class Room
	{
		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x0600216A RID: 8554 RVA: 0x000AE662 File Offset: 0x000AC862
		public List<District> Districts
		{
			get
			{
				return this.districts;
			}
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x0600216B RID: 8555 RVA: 0x000AE66A File Offset: 0x000AC86A
		public Map Map
		{
			get
			{
				if (!this.districts.Any<District>())
				{
					return null;
				}
				return this.districts[0].Map;
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x0600216C RID: 8556 RVA: 0x000AE68C File Offset: 0x000AC88C
		public int DistrictCount
		{
			get
			{
				return this.districts.Count;
			}
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x0600216D RID: 8557 RVA: 0x000AE699 File Offset: 0x000AC899
		public RoomTempTracker TempTracker
		{
			get
			{
				return this.tempTracker;
			}
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x0600216E RID: 8558 RVA: 0x000AE6A1 File Offset: 0x000AC8A1
		// (set) Token: 0x0600216F RID: 8559 RVA: 0x000AE6AE File Offset: 0x000AC8AE
		public float Temperature
		{
			get
			{
				return this.tempTracker.Temperature;
			}
			set
			{
				this.tempTracker.Temperature = value;
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06002170 RID: 8560 RVA: 0x000AE6BC File Offset: 0x000AC8BC
		public bool UsesOutdoorTemperature
		{
			get
			{
				return this.TouchesMapEdge || this.OpenRoofCount >= Mathf.CeilToInt((float)this.CellCount * 0.25f);
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06002171 RID: 8561 RVA: 0x000AE6E5 File Offset: 0x000AC8E5
		public bool ExposedToSpace
		{
			get
			{
				return this.Map.Biome.inVacuum && (this.TouchesMapEdge || this.ExposedCountStopAt(1) > 0);
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06002172 RID: 8562 RVA: 0x000AE70F File Offset: 0x000AC90F
		public bool Dereferenced
		{
			get
			{
				return this.RegionCount == 0;
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06002173 RID: 8563 RVA: 0x000AE71A File Offset: 0x000AC91A
		public bool IsHuge
		{
			get
			{
				return this.RegionCount > 60;
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06002174 RID: 8564 RVA: 0x000AE726 File Offset: 0x000AC926
		public bool IsPrisonCell
		{
			get
			{
				return this.isPrisonCell;
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06002175 RID: 8565 RVA: 0x000AE730 File Offset: 0x000AC930
		// (set) Token: 0x06002176 RID: 8566 RVA: 0x000AE864 File Offset: 0x000ACA64
		public float Vacuum
		{
			get
			{
				if (!this.Map.Biome.inVacuum)
				{
					return 0f;
				}
				if (this.ExposedToSpace)
				{
					return this.vacuum = 1f;
				}
				if (this.IsDoorway)
				{
					if (!this.Map.regionAndRoomUpdater.Enabled)
					{
						return 0f;
					}
					float num = 0f;
					Region region = this.Regions[0];
					foreach (RegionLink regionLink in region.links)
					{
						Region otherRegion = regionLink.GetOtherRegion(region);
						Room room = (otherRegion != null) ? otherRegion.Room : null;
						if (room != null)
						{
							num += room.UnsanitizedVacuum;
						}
					}
					if (Mathf.Approximately(num, 0f) || region.links.Count == 0)
					{
						return 0f;
					}
					num /= (float)region.links.Count;
					return this.vacuum = ((num <= 0.01f) ? 0f : num);
				}
				else
				{
					if (this.vacuum > 0.01f)
					{
						return this.vacuum;
					}
					return 0f;
				}
			}
			set
			{
				this.vacuum = Mathf.Clamp(value, -0.1f, 1f);
				foreach (Thing thing in this.uniqueContainedThings)
				{
					ThingWithComps thingWithComps = thing as ThingWithComps;
					if (thingWithComps != null)
					{
						thingWithComps.BroadcastCompSignal("Vacuum");
					}
				}
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06002177 RID: 8567 RVA: 0x000AE8DC File Offset: 0x000ACADC
		internal float UnsanitizedVacuum
		{
			get
			{
				return this.vacuum;
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06002178 RID: 8568 RVA: 0x000AE8E4 File Offset: 0x000ACAE4
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				int num;
				for (int i = 0; i < this.districts.Count; i = num + 1)
				{
					foreach (IntVec3 intVec in this.districts[i].Cells)
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

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06002179 RID: 8569 RVA: 0x000AE8F4 File Offset: 0x000ACAF4
		public int CellCount
		{
			get
			{
				if (this.cachedCellCount == -1)
				{
					this.cachedCellCount = 0;
					for (int i = 0; i < this.districts.Count; i++)
					{
						this.cachedCellCount += this.districts[i].CellCount;
					}
				}
				return this.cachedCellCount;
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x0600217A RID: 8570 RVA: 0x000AE94C File Offset: 0x000ACB4C
		public Region FirstRegion
		{
			get
			{
				for (int i = 0; i < this.districts.Count; i++)
				{
					List<Region> regions = this.districts[i].Regions;
					if (regions.Count > 0)
					{
						return regions[0];
					}
				}
				return null;
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x0600217B RID: 8571 RVA: 0x000AE994 File Offset: 0x000ACB94
		public List<Region> Regions
		{
			get
			{
				this.tmpRegions.Clear();
				for (int i = 0; i < this.districts.Count; i++)
				{
					List<Region> regions = this.districts[i].Regions;
					for (int j = 0; j < regions.Count; j++)
					{
						this.tmpRegions.Add(regions[j]);
					}
				}
				return this.tmpRegions;
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x0600217C RID: 8572 RVA: 0x000AEA00 File Offset: 0x000ACC00
		public int RegionCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.districts.Count; i++)
				{
					num += this.districts[i].RegionCount;
				}
				return num;
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x0600217D RID: 8573 RVA: 0x000AEA3C File Offset: 0x000ACC3C
		public CellRect ExtentsClose
		{
			get
			{
				CellRect cellRect = new CellRect(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
				foreach (Region region in this.Regions)
				{
					if (region.extentsClose.minX < cellRect.minX)
					{
						cellRect.minX = region.extentsClose.minX;
					}
					if (region.extentsClose.minZ < cellRect.minZ)
					{
						cellRect.minZ = region.extentsClose.minZ;
					}
					if (region.extentsClose.maxX > cellRect.maxX)
					{
						cellRect.maxX = region.extentsClose.maxX;
					}
					if (region.extentsClose.maxZ > cellRect.maxZ)
					{
						cellRect.maxZ = region.extentsClose.maxZ;
					}
				}
				return cellRect;
			}
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x000AEB40 File Offset: 0x000ACD40
		private int OpenRoofCountStopAt(int threshold)
		{
			if (this.cachedOpenRoofCount != -1)
			{
				return this.cachedOpenRoofCount;
			}
			int num = 0;
			for (int i = 0; i < this.districts.Count; i++)
			{
				num += this.districts[i].OpenRoofCountStopAt(threshold);
				if (num >= threshold)
				{
					return num;
				}
				threshold -= num;
			}
			return num;
		}

		// Token: 0x0600217F RID: 8575 RVA: 0x000AEB98 File Offset: 0x000ACD98
		private int ExposedCountStopAt(int threshold)
		{
			if (this.cachedExposedThreshold > 0 && this.cachedExposedThreshold <= threshold)
			{
				return this.cachedExposedCount;
			}
			this.cachedExposedThreshold = threshold;
			int num = 0;
			for (int i = 0; i < this.districts.Count; i++)
			{
				num += this.districts[i].ExposedVacuumCount(threshold);
				if (num >= threshold)
				{
					return this.cachedExposedCount = num;
				}
				threshold -= num;
			}
			return this.cachedExposedCount = num;
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06002180 RID: 8576 RVA: 0x000AEC10 File Offset: 0x000ACE10
		public int OpenRoofCount
		{
			get
			{
				if (this.cachedOpenRoofCount == -1)
				{
					this.cachedOpenRoofCount = this.OpenRoofCountStopAt(int.MaxValue);
				}
				return this.cachedOpenRoofCount;
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06002181 RID: 8577 RVA: 0x000AEC32 File Offset: 0x000ACE32
		public IEnumerable<IntVec3> BorderCells
		{
			get
			{
				foreach (IntVec3 c in this.Cells)
				{
					int num;
					for (int i = 0; i < 8; i = num)
					{
						IntVec3 intVec = c + GenAdj.AdjacentCells[i];
						Region region = (c + GenAdj.AdjacentCells[i]).GetRegion(this.Map, RegionType.Set_Passable);
						if (region == null || region.Room != this)
						{
							yield return intVec;
						}
						num = i + 1;
					}
				}
				IEnumerator<IntVec3> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06002182 RID: 8578 RVA: 0x000AEC42 File Offset: 0x000ACE42
		public IEnumerable<IntVec3> BorderCellsCardinal
		{
			get
			{
				foreach (IntVec3 c in this.Cells)
				{
					int num;
					for (int i = 0; i < 4; i = num)
					{
						IntVec3 intVec = c + GenAdj.CardinalDirections[i];
						Region region = intVec.GetRegion(this.Map, RegionType.Set_Passable);
						if (region == null || region.Room != this)
						{
							yield return intVec;
						}
						num = i + 1;
					}
				}
				IEnumerator<IntVec3> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06002183 RID: 8579 RVA: 0x000AEC54 File Offset: 0x000ACE54
		public IEnumerable<IntVec3> BorderCellsCached
		{
			get
			{
				if ((long)GenTicks.TicksGame - this.cachedBorderCellsTick > 120L)
				{
					this.cachedBorderCellsTick = (long)GenTicks.TicksGame;
					this.cachedBorderCells.Clear();
					this.cachedBorderCells.AddRange(this.BorderCells);
				}
				return this.cachedBorderCells;
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06002184 RID: 8580 RVA: 0x000AECA4 File Offset: 0x000ACEA4
		public bool TouchesMapEdge
		{
			get
			{
				for (int i = 0; i < this.districts.Count; i++)
				{
					if (this.districts[i].TouchesMapEdge)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06002185 RID: 8581 RVA: 0x000AECDD File Offset: 0x000ACEDD
		public bool PsychologicallyOutdoors
		{
			get
			{
				return this.OpenRoofCountStopAt(300) >= 300 || (this.TouchesMapEdge && (float)this.OpenRoofCount / (float)this.CellCount >= 0.5f);
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06002186 RID: 8582 RVA: 0x000AED14 File Offset: 0x000ACF14
		public bool OutdoorsForWork
		{
			get
			{
				return this.OpenRoofCountStopAt(101) > 100 || (float)this.OpenRoofCount > (float)this.CellCount * 0.25f;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06002187 RID: 8583 RVA: 0x000AED3B File Offset: 0x000ACF3B
		public IEnumerable<Pawn> Owners
		{
			get
			{
				if (this.TouchesMapEdge)
				{
					yield break;
				}
				if (this.IsHuge)
				{
					yield break;
				}
				if (this.Role != RoomRoleDefOf.Bedroom && this.Role != RoomRoleDefOf.PrisonCell && this.Role != RoomRoleDefOf.Barracks && this.Role != RoomRoleDefOf.PrisonBarracks)
				{
					yield break;
				}
				IEnumerable<Building_Bed> enumerable = from x in this.ContainedBeds
				where x.def.building.bed_humanlike
				select x;
				if (enumerable.Count<Building_Bed>() > 1 && (this.Role == RoomRoleDefOf.Barracks || this.Role == RoomRoleDefOf.PrisonBarracks))
				{
					if ((from b in enumerable
					where b.OwnersForReading.Any<Pawn>()
					select b).Count<Building_Bed>() > 1)
					{
						yield break;
					}
				}
				foreach (Building_Bed building_Bed in enumerable)
				{
					List<Pawn> assignedPawns = building_Bed.OwnersForReading;
					int num;
					for (int i = 0; i < assignedPawns.Count; i = num + 1)
					{
						yield return assignedPawns[i];
						num = i;
					}
					assignedPawns = null;
				}
				IEnumerator<Building_Bed> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06002188 RID: 8584 RVA: 0x000AED4B File Offset: 0x000ACF4B
		public IEnumerable<Building_Bed> ContainedBeds
		{
			get
			{
				List<Thing> things = this.ContainedAndAdjacentThings;
				int num;
				for (int i = 0; i < things.Count; i = num + 1)
				{
					Building_Bed building_Bed = things[i] as Building_Bed;
					if (building_Bed != null)
					{
						yield return building_Bed;
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06002189 RID: 8585 RVA: 0x000AED5B File Offset: 0x000ACF5B
		public bool Fogged
		{
			get
			{
				return this.RegionCount != 0 && this.FirstRegion.AnyCell.Fogged(this.Map);
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x0600218A RID: 8586 RVA: 0x000AED7D File Offset: 0x000ACF7D
		public bool IsDoorway
		{
			get
			{
				return this.districts.Count == 1 && this.districts[0].IsDoorway;
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x0600218B RID: 8587 RVA: 0x000AEDA0 File Offset: 0x000ACFA0
		public Building_Door Door
		{
			get
			{
				if (!this.IsDoorway)
				{
					return null;
				}
				return this.districts[0].Regions[0].door;
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x0600218C RID: 8588 RVA: 0x000AEDC8 File Offset: 0x000ACFC8
		public List<Thing> ContainedAndAdjacentThings
		{
			get
			{
				this.uniqueContainedThingsSet.Clear();
				this.uniqueContainedThings.Clear();
				List<Region> regions = this.Regions;
				for (int i = 0; i < regions.Count; i++)
				{
					List<Thing> allThings = regions[i].ListerThings.AllThings;
					if (allThings != null)
					{
						for (int j = 0; j < allThings.Count; j++)
						{
							Thing item = allThings[j];
							if (this.uniqueContainedThingsSet.Add(item))
							{
								this.uniqueContainedThings.Add(item);
							}
						}
					}
				}
				this.uniqueContainedThingsSet.Clear();
				return this.uniqueContainedThings;
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x0600218D RID: 8589 RVA: 0x000AEE5F File Offset: 0x000AD05F
		public RoomRoleDef Role
		{
			get
			{
				if (this.statsAndRoleDirty)
				{
					this.UpdateRoomStatsAndRole();
				}
				return this.role;
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x0600218E RID: 8590 RVA: 0x000AEE78 File Offset: 0x000AD078
		public bool AnyPassable
		{
			get
			{
				for (int i = 0; i < this.districts.Count; i++)
				{
					if (this.districts[i].Passable)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x0600218F RID: 8591 RVA: 0x000AEEB4 File Offset: 0x000AD0B4
		public bool ProperRoom
		{
			get
			{
				if (this.TouchesMapEdge)
				{
					return false;
				}
				for (int i = 0; i < this.districts.Count; i++)
				{
					if (this.districts[i].RegionType == RegionType.Normal)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x000AEEF8 File Offset: 0x000AD0F8
		public static Room MakeNew(Map map)
		{
			Room room = new Room();
			room.ID = Room.nextRoomID;
			room.tempTracker = new RoomTempTracker(room, map);
			map.regionAndRoomUpdater.roomLookup[room.ID] = room;
			Room.nextRoomID++;
			return room;
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x000AEF48 File Offset: 0x000AD148
		public void AddDistrict(District district)
		{
			if (this.districts.Contains(district))
			{
				Log.Error("Tried to add the same district twice to Room. district=" + ((district != null) ? district.ToString() : null) + ", room=" + ((this != null) ? this.ToString() : null));
				return;
			}
			this.districts.Add(district);
			if (this.districts.Count == 1)
			{
				this.Map.regionGrid.RoomAdded(this);
			}
		}

		// Token: 0x06002192 RID: 8594 RVA: 0x000AEFC0 File Offset: 0x000AD1C0
		public void RemoveDistrict(District district)
		{
			if (!this.districts.Contains(district))
			{
				Log.Error("Tried to remove district from Room but this district is not here. district=" + ((district != null) ? district.ToString() : null) + ", room=" + ((this != null) ? this.ToString() : null));
				return;
			}
			Map map = this.Map;
			this.districts.Remove(district);
			this.tmpRegions.Clear();
			if (this.districts.Count == 0 && map != null)
			{
				map.regionGrid.RoomRemoved(this);
			}
			this.statsAndRoleDirty = true;
		}

		// Token: 0x06002193 RID: 8595 RVA: 0x000AF04C File Offset: 0x000AD24C
		public bool PushHeat(float energy)
		{
			if (this.UsesOutdoorTemperature)
			{
				return false;
			}
			this.Temperature += energy / (float)this.CellCount;
			return true;
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x000AF070 File Offset: 0x000AD270
		public void Notify_ContainedThingSpawnedOrDespawned(Thing th)
		{
			if (th.def.category != ThingCategory.Mote && th.def.category != ThingCategory.Projectile && th.def.category != ThingCategory.Ethereal && th.def.category != ThingCategory.Pawn)
			{
				if (this.IsDoorway)
				{
					List<Region> regions = this.districts[0].Regions;
					for (int i = 0; i < regions[0].links.Count; i++)
					{
						Region otherRegion = regions[0].links[i].GetOtherRegion(regions[0]);
						if (otherRegion != null && !otherRegion.IsDoorway)
						{
							otherRegion.Room.Notify_ContainedThingSpawnedOrDespawned(th);
						}
					}
				}
				this.statsAndRoleDirty = true;
			}
		}

		// Token: 0x06002195 RID: 8597 RVA: 0x000AF133 File Offset: 0x000AD333
		public void Notify_TerrainChanged()
		{
			this.cachedExposedCount = -1;
			this.cachedExposedThreshold = 0;
			this.statsAndRoleDirty = true;
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x000AF14A File Offset: 0x000AD34A
		public void Notify_BedTypeChanged()
		{
			this.statsAndRoleDirty = true;
		}

		// Token: 0x06002197 RID: 8599 RVA: 0x000AF153 File Offset: 0x000AD353
		public void Notify_RoofChanged()
		{
			this.cachedOpenRoofCount = -1;
			this.cachedExposedCount = -1;
			this.cachedExposedThreshold = 0;
			this.tempTracker.RoofChanged();
		}

		// Token: 0x06002198 RID: 8600 RVA: 0x000AF178 File Offset: 0x000AD378
		public void Notify_RoomShapeChanged()
		{
			this.cachedCellCount = -1;
			this.cachedOpenRoofCount = -1;
			this.cachedExposedCount = -1;
			this.cachedExposedThreshold = 0;
			if (this.Dereferenced)
			{
				this.isPrisonCell = false;
				this.statsAndRoleDirty = true;
				return;
			}
			this.tempTracker.RoomChanged();
			if (Current.ProgramState == ProgramState.Playing && !this.Fogged && !GravshipPlacementUtility.placingGravship)
			{
				this.Map.autoBuildRoofAreaSetter.TryGenerateAreaFor(this);
			}
			this.isPrisonCell = false;
			if (Building_Bed.RoomCanBePrisonCell(this))
			{
				List<Thing> containedAndAdjacentThings = this.ContainedAndAdjacentThings;
				for (int i = 0; i < containedAndAdjacentThings.Count; i++)
				{
					Building_Bed building_Bed = containedAndAdjacentThings[i] as Building_Bed;
					if (building_Bed != null && building_Bed.ForPrisoners)
					{
						this.isPrisonCell = true;
						break;
					}
				}
			}
			List<Thing> list = this.Map.listerThings.ThingsOfDef(ThingDefOf.NutrientPasteDispenser);
			for (int j = 0; j < list.Count; j++)
			{
				list[j].Notify_ColorChanged();
			}
			if (Current.ProgramState == ProgramState.Playing && this.isPrisonCell)
			{
				foreach (Building_Bed building_Bed2 in this.ContainedBeds)
				{
					building_Bed2.ForPrisoners = true;
				}
			}
			this.statsAndRoleDirty = true;
		}

		// Token: 0x06002199 RID: 8601 RVA: 0x000AF2C8 File Offset: 0x000AD4C8
		public bool ContainsCell(IntVec3 cell)
		{
			return this.Map != null && cell.GetRoom(this.Map) == this;
		}

		// Token: 0x0600219A RID: 8602 RVA: 0x000AF2E4 File Offset: 0x000AD4E4
		public bool ContainsThing(ThingDef def)
		{
			List<Region> regions = this.Regions;
			for (int i = 0; i < regions.Count; i++)
			{
				if (regions[i].ListerThings.ThingsOfDef(def).Any<Thing>())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x000AF325 File Offset: 0x000AD525
		public IEnumerable<Thing> ContainedThings(ThingDef def)
		{
			this.uniqueContainedThingsOfDef.Clear();
			List<Region> regions = this.Regions;
			int num;
			for (int i = 0; i < regions.Count; i = num)
			{
				List<Thing> things = regions[i].ListerThings.ThingsOfDef(def);
				for (int j = 0; j < things.Count; j = num)
				{
					if (this.uniqueContainedThingsOfDef.Add(things[j]))
					{
						yield return things[j];
					}
					num = j + 1;
				}
				things = null;
				num = i + 1;
			}
			this.uniqueContainedThingsOfDef.Clear();
			yield break;
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x000AF33C File Offset: 0x000AD53C
		public IEnumerable<T> ContainedThings<T>() where T : Thing
		{
			this.uniqueContainedThingsOfDef.Clear();
			foreach (Region region in this.Regions)
			{
				foreach (T t in region.ListerThings.GetThingsOfType<T>())
				{
					if (this.uniqueContainedThingsOfDef.Add(t))
					{
						yield return t;
					}
				}
				IEnumerator<T> enumerator2 = null;
			}
			List<Region>.Enumerator enumerator = default(List<Region>.Enumerator);
			this.uniqueContainedThingsOfDef.Clear();
			yield break;
			yield break;
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x000AF34C File Offset: 0x000AD54C
		public IEnumerable<Thing> ContainedThingsList(IEnumerable<ThingDef> thingDefs)
		{
			foreach (ThingDef def in thingDefs)
			{
				foreach (Thing thing in this.ContainedThings(def))
				{
					yield return thing;
				}
				IEnumerator<Thing> enumerator2 = null;
			}
			IEnumerator<ThingDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x000AF364 File Offset: 0x000AD564
		public int ThingCount(ThingDef def)
		{
			this.uniqueContainedThingsOfDef.Clear();
			List<Region> regions = this.Regions;
			int num = 0;
			for (int i = 0; i < regions.Count; i++)
			{
				List<Thing> list = regions[i].ListerThings.ThingsOfDef(def);
				for (int j = 0; j < list.Count; j++)
				{
					if (this.uniqueContainedThingsOfDef.Add(list[j]))
					{
						num += list[j].stackCount;
					}
				}
			}
			this.uniqueContainedThingsOfDef.Clear();
			return num;
		}

		// Token: 0x0600219F RID: 8607 RVA: 0x000AF3EF File Offset: 0x000AD5EF
		public float GetStat(RoomStatDef roomStat)
		{
			if (this.statsAndRoleDirty)
			{
				this.UpdateRoomStatsAndRole();
			}
			if (this.stats == null)
			{
				return roomStat.roomlessScore;
			}
			return this.stats[roomStat];
		}

		// Token: 0x060021A0 RID: 8608 RVA: 0x000AF41C File Offset: 0x000AD61C
		public void DrawFieldEdges()
		{
			Room.fields.Clear();
			Room.fields.AddRange(this.Cells);
			Color color = this.isPrisonCell ? Room.PrisonFieldColor : Room.NonPrisonFieldColor;
			color.a = Pulser.PulseBrightness(1f, 0.6f);
			GenDraw.DrawFieldEdges(Room.fields, color, null, null, 2900);
			Room.fields.Clear();
		}

		// Token: 0x060021A1 RID: 8609 RVA: 0x000AF494 File Offset: 0x000AD694
		public RoomRoleDef GetRoomRoleIfBuildingPlaced(ThingDef buildingDef)
		{
			if (this.statsAndRoleDirty)
			{
				this.UpdateRoomStatsAndRole();
			}
			if (!this.ProperRoom || this.RegionCount > 60)
			{
				return RoomRoleDefOf.None;
			}
			return DefDatabase<RoomRoleDef>.AllDefs.MaxBy((RoomRoleDef x) => x.Worker.GetScore(this) + x.Worker.GetScoreDeltaIfBuildingPlaced(this, buildingDef));
		}

		// Token: 0x060021A2 RID: 8610 RVA: 0x000AF4F4 File Offset: 0x000AD6F4
		private void UpdateRoomStatsAndRole()
		{
			this.statsAndRoleDirty = false;
			if (this.ProperRoom && this.RegionCount <= 60)
			{
				if (this.stats == null)
				{
					this.stats = new DefMap<RoomStatDef, float>();
				}
				foreach (RoomStatDef roomStatDef in from x in DefDatabase<RoomStatDef>.AllDefs
				orderby x.updatePriority descending
				select x)
				{
					this.stats[roomStatDef] = roomStatDef.Worker.GetScore(this);
				}
				this.role = DefDatabase<RoomRoleDef>.AllDefs.MaxBy((RoomRoleDef x) => x.Worker.GetScore(this));
				return;
			}
			this.stats = null;
			this.role = RoomRoleDefOf.None;
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x000AF5D8 File Offset: 0x000AD7D8
		public string GetRoomRoleLabel()
		{
			Pawn pawn = null;
			Pawn pawn2 = null;
			foreach (Pawn pawn3 in this.Owners)
			{
				if (pawn == null)
				{
					pawn = pawn3;
				}
				else
				{
					pawn2 = pawn3;
				}
			}
			TaggedString taggedString;
			if (pawn == null)
			{
				taggedString = this.Role.PostProcessedLabel(this);
			}
			else if (pawn2 == null || pawn2 == pawn)
			{
				taggedString = "SomeonesRoom".Translate(pawn.LabelShort, this.Role.label, pawn.Named("PAWN"));
			}
			else
			{
				taggedString = "CouplesRoom".Translate(pawn.LabelShort, pawn2.LabelShort, this.Role.label, pawn.Named("PAWN1"), pawn2.Named("PAWN2"));
			}
			return taggedString;
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x000AF6D0 File Offset: 0x000AD8D0
		public string DebugString()
		{
			return string.Concat(new string[]
			{
				"Room ID=",
				this.ID.ToString(),
				"\n  first cell=",
				this.Cells.FirstOrDefault<IntVec3>().ToString(),
				"\n  DistrictCount=",
				this.DistrictCount.ToString(),
				"\n  RegionCount=",
				this.RegionCount.ToString(),
				"\n  CellCount=",
				this.CellCount.ToString(),
				"\n  OpenRoofCount=",
				this.OpenRoofCount.ToString(),
				"\n  PsychologicallyOutdoors=",
				this.PsychologicallyOutdoors.ToString(),
				"\n  OutdoorsForWork=",
				this.OutdoorsForWork.ToString(),
				"\n  WellEnclosed=",
				this.ProperRoom.ToString(),
				"\n  ",
				this.tempTracker.DebugString(),
				DebugViewSettings.writeRoomRoles ? ("\n" + this.DebugRolesString()) : ""
			});
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x000AF818 File Offset: 0x000ADA18
		private string DebugRolesString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ValueTuple<float, RoomRoleDef> valueTuple in from x in DefDatabase<RoomRoleDef>.AllDefs
			select new ValueTuple<float, RoomRoleDef>(x.Worker.GetScore(this), x) into tuple
			orderby tuple.Item1 descending
			select tuple)
			{
				float item = valueTuple.Item1;
				RoomRoleDef item2 = valueTuple.Item2;
				stringBuilder.AppendLine(string.Format("{0}: {1}", item, item2));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x000AF8C4 File Offset: 0x000ADAC4
		internal void DebugDraw()
		{
			int num = Gen.HashCombineInt(this.GetHashCode(), 1948571531);
			foreach (IntVec3 c in this.Cells)
			{
				CellRenderer.RenderCell(c, (float)num * 0.01f);
			}
			this.tempTracker.DebugDraw();
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x000AF934 File Offset: 0x000ADB34
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Room(roomID=",
				this.ID.ToString(),
				", first=",
				this.Cells.FirstOrDefault<IntVec3>().ToString(),
				", RegionsCount=",
				this.RegionCount.ToString(),
				")"
			});
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x000AF9A7 File Offset: 0x000ADBA7
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.ID, 1538478891);
		}

		// Token: 0x04001C4C RID: 7244
		public int ID = -1;

		// Token: 0x04001C4D RID: 7245
		private List<District> districts = new List<District>();

		// Token: 0x04001C4E RID: 7246
		private RoomTempTracker tempTracker;

		// Token: 0x04001C4F RID: 7247
		private int cachedOpenRoofCount = -1;

		// Token: 0x04001C50 RID: 7248
		private int cachedExposedCount = -1;

		// Token: 0x04001C51 RID: 7249
		private int cachedExposedThreshold;

		// Token: 0x04001C52 RID: 7250
		private int cachedCellCount = -1;

		// Token: 0x04001C53 RID: 7251
		private bool isPrisonCell;

		// Token: 0x04001C54 RID: 7252
		private bool statsAndRoleDirty = true;

		// Token: 0x04001C55 RID: 7253
		private DefMap<RoomStatDef, float> stats = new DefMap<RoomStatDef, float>();

		// Token: 0x04001C56 RID: 7254
		private RoomRoleDef role;

		// Token: 0x04001C57 RID: 7255
		private List<IntVec3> cachedBorderCells = new List<IntVec3>();

		// Token: 0x04001C58 RID: 7256
		private long cachedBorderCellsTick = -1L;

		// Token: 0x04001C59 RID: 7257
		private float vacuum;

		// Token: 0x04001C5A RID: 7258
		public bool vacuumOverlayDrawn;

		// Token: 0x04001C5B RID: 7259
		public int vacuumOverlayRects;

		// Token: 0x04001C5C RID: 7260
		private static int nextRoomID;

		// Token: 0x04001C5D RID: 7261
		private const int RegionCountHuge = 60;

		// Token: 0x04001C5E RID: 7262
		private const float UseOutdoorTemperatureUnroofedFraction = 0.25f;

		// Token: 0x04001C5F RID: 7263
		public const float MinVaccum = -0.1f;

		// Token: 0x04001C60 RID: 7264
		private const int MaxRegionsToAssignRoomRole = 60;

		// Token: 0x04001C61 RID: 7265
		private static readonly Color PrisonFieldColor = new Color(1f, 0.7f, 0.2f);

		// Token: 0x04001C62 RID: 7266
		private static readonly Color NonPrisonFieldColor = new Color(0.3f, 0.3f, 1f);

		// Token: 0x04001C63 RID: 7267
		public const string VacuumChangedSignal = "Vacuum";

		// Token: 0x04001C64 RID: 7268
		private List<Region> tmpRegions = new List<Region>();

		// Token: 0x04001C65 RID: 7269
		private readonly HashSet<Thing> uniqueContainedThingsSet = new HashSet<Thing>();

		// Token: 0x04001C66 RID: 7270
		private readonly List<Thing> uniqueContainedThings = new List<Thing>();

		// Token: 0x04001C67 RID: 7271
		private readonly HashSet<Thing> uniqueContainedThingsOfDef = new HashSet<Thing>();

		// Token: 0x04001C68 RID: 7272
		private static List<IntVec3> fields = new List<IntVec3>();
	}
}
