using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020004EC RID: 1260
	public sealed class Region
	{
		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06002084 RID: 8324 RVA: 0x000AA264 File Offset: 0x000A8464
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

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06002085 RID: 8325 RVA: 0x000AA281 File Offset: 0x000A8481
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				RegionGrid regions = this.Map.regionGrid;
				int num;
				for (int z = this.extentsClose.minZ; z <= this.extentsClose.maxZ; z = num + 1)
				{
					for (int x = this.extentsClose.minX; x <= this.extentsClose.maxX; x = num + 1)
					{
						IntVec3 intVec = new IntVec3(x, 0, z);
						if (regions.GetRegionAt_NoRebuild_InvalidAllowed(intVec) == this)
						{
							yield return intVec;
						}
						num = x;
					}
					num = z;
				}
				yield break;
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06002086 RID: 8326 RVA: 0x000AA294 File Offset: 0x000A8494
		public int CellCount
		{
			get
			{
				if (this.cachedCellCount == -1)
				{
					this.cachedCellCount = 0;
					RegionGrid regionGrid = this.Map.regionGrid;
					for (int i = this.extentsClose.minZ; i <= this.extentsClose.maxZ; i++)
					{
						for (int j = this.extentsClose.minX; j <= this.extentsClose.maxX; j++)
						{
							IntVec3 c = new IntVec3(j, 0, i);
							if (regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(c) == this)
							{
								this.cachedCellCount++;
							}
						}
					}
				}
				return this.cachedCellCount;
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06002087 RID: 8327 RVA: 0x000AA325 File Offset: 0x000A8525
		public IEnumerable<Region> Neighbors
		{
			get
			{
				int num;
				for (int li = 0; li < this.links.Count; li = num + 1)
				{
					RegionLink link = this.links[li];
					for (int ri = 0; ri < 2; ri = num + 1)
					{
						if (link.regions[ri] != null && link.regions[ri] != this && link.regions[ri].valid)
						{
							yield return link.regions[ri];
						}
						num = ri;
					}
					link = null;
					num = li;
				}
				yield break;
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06002088 RID: 8328 RVA: 0x000AA335 File Offset: 0x000A8535
		public IEnumerable<Region> NeighborsOfSameType
		{
			get
			{
				int num;
				for (int li = 0; li < this.links.Count; li = num + 1)
				{
					RegionLink link = this.links[li];
					for (int ri = 0; ri < 2; ri = num + 1)
					{
						if (link.regions[ri] != null && link.regions[ri] != this && link.regions[ri].type == this.type && link.regions[ri].valid)
						{
							yield return link.regions[ri];
						}
						num = ri;
					}
					link = null;
					num = li;
				}
				yield break;
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06002089 RID: 8329 RVA: 0x000AA345 File Offset: 0x000A8545
		public Room Room
		{
			get
			{
				District district = this.District;
				if (district == null)
				{
					return null;
				}
				return district.Room;
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x0600208A RID: 8330 RVA: 0x000AA358 File Offset: 0x000A8558
		// (set) Token: 0x0600208B RID: 8331 RVA: 0x000AA360 File Offset: 0x000A8560
		public District District
		{
			get
			{
				return this.districtInt;
			}
			set
			{
				if (value == this.districtInt)
				{
					return;
				}
				District district = this.districtInt;
				if (district != null)
				{
					district.RemoveRegion(this);
				}
				this.districtInt = value;
				District district2 = this.districtInt;
				if (district2 == null)
				{
					return;
				}
				district2.AddRegion(this);
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x0600208C RID: 8332 RVA: 0x000AA398 File Offset: 0x000A8598
		public IntVec3 RandomCell
		{
			get
			{
				Map map = this.Map;
				CellIndices cellIndices = map.cellIndices;
				Region[] directGrid = map.regionGrid.DirectGrid;
				for (int i = 0; i < 1000; i++)
				{
					IntVec3 randomCell = this.extentsClose.RandomCell;
					if (directGrid[cellIndices.CellToIndex(randomCell)] == this)
					{
						return randomCell;
					}
				}
				return this.AnyCell;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x0600208D RID: 8333 RVA: 0x000AA3F0 File Offset: 0x000A85F0
		public IntVec3 AnyCell
		{
			get
			{
				Map map = this.Map;
				CellIndices cellIndices = map.cellIndices;
				Region[] directGrid = map.regionGrid.DirectGrid;
				foreach (IntVec3 intVec in this.extentsClose)
				{
					if (directGrid[cellIndices.CellToIndex(intVec)] == this)
					{
						return intVec;
					}
				}
				Log.Error("Couldn't find any cell in region " + this.ToString());
				return this.extentsClose.RandomCell;
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x0600208E RID: 8334 RVA: 0x000AA48C File Offset: 0x000A868C
		public string DebugString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("id: " + this.id.ToString());
				stringBuilder.AppendLine("mapIndex: " + this.mapIndex.ToString());
				stringBuilder.AppendLine("links count: " + this.links.Count.ToString());
				stringBuilder.AppendLine("type: " + this.type.ToString());
				stringBuilder.AppendLine("touchesMapEdge: " + this.touchesMapEdge.ToString());
				foreach (RegionLink regionLink in this.links)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					string str = "  --";
					RegionLink regionLink2 = regionLink;
					stringBuilder2.AppendLine(str + ((regionLink2 != null) ? regionLink2.ToString() : null));
				}
				stringBuilder.AppendLine("valid: " + this.valid.ToString());
				stringBuilder.AppendLine("makeTick: " + this.debug_makeTick.ToString());
				stringBuilder.AppendLine("districtID: " + ((this.District != null) ? this.District.ID.ToString() : "null district!"));
				stringBuilder.AppendLine("roomID: " + ((this.Room != null) ? this.Room.ID.ToString() : "null room!"));
				StringBuilder stringBuilder3 = stringBuilder;
				string str2 = "extentsClose: ";
				CellRect cellRect = this.extentsClose;
				stringBuilder3.AppendLine(str2 + cellRect.ToString());
				StringBuilder stringBuilder4 = stringBuilder;
				string str3 = "extentsLimit: ";
				cellRect = this.extentsLimit;
				stringBuilder4.AppendLine(str3 + cellRect.ToString());
				stringBuilder.AppendLine("ListerThings:");
				if (this.listerThings.AllThings != null)
				{
					for (int i = 0; i < this.listerThings.AllThings.Count; i++)
					{
						StringBuilder stringBuilder5 = stringBuilder;
						string str4 = "  --";
						Thing thing = this.listerThings.AllThings[i];
						stringBuilder5.AppendLine(str4 + ((thing != null) ? thing.ToString() : null));
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x0600208F RID: 8335 RVA: 0x000AA6E8 File Offset: 0x000A88E8
		public bool DebugIsNew
		{
			get
			{
				return this.debug_makeTick > Find.TickManager.TicksGame - 60;
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06002090 RID: 8336 RVA: 0x000AA6FF File Offset: 0x000A88FF
		public ListerThings ListerThings
		{
			get
			{
				return this.listerThings;
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06002091 RID: 8337 RVA: 0x000AA707 File Offset: 0x000A8907
		public bool IsDoorway
		{
			get
			{
				return this.door != null;
			}
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x000AA714 File Offset: 0x000A8914
		private Region()
		{
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x000AA7A8 File Offset: 0x000A89A8
		public static Region MakeNewUnfilled(IntVec3 root, Map map)
		{
			Region region = new Region();
			region.debug_makeTick = Find.TickManager.TicksGame;
			region.id = Region.nextId;
			Region.nextId++;
			region.mapIndex = (sbyte)map.Index;
			region.precalculatedHashCode = Gen.HashCombineInt(region.id, 1295813358);
			region.extentsClose.minX = root.x;
			region.extentsClose.maxX = root.x;
			region.extentsClose.minZ = root.z;
			region.extentsClose.maxZ = root.z;
			region.extentsLimit.minX = root.x - root.x % 12;
			region.extentsLimit.maxX = root.x + 12 - (root.x + 12) % 12 - 1;
			region.extentsLimit.minZ = root.z - root.z % 12;
			region.extentsLimit.maxZ = root.z + 12 - (root.z + 12) % 12 - 1;
			region.extentsLimit.ClipInsideMap(map);
			return region;
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x000AA8D4 File Offset: 0x000A8AD4
		public bool Allows(TraverseParms tp, bool isDestination)
		{
			if (tp.mode != TraverseMode.PassAllDestroyableThings && tp.mode != TraverseMode.PassAllDestroyablePlayerOwnedThings && tp.mode != TraverseMode.PassAllDestroyableThingsNotWater && !this.type.Passable())
			{
				return false;
			}
			if (tp.maxDanger < Danger.Deadly && tp.pawn != null)
			{
				Danger danger = this.DangerFor(tp.pawn);
				if (isDestination || danger == Danger.Deadly)
				{
					Region region = tp.pawn.GetRegion(RegionType.Set_All);
					if ((region == null || danger > region.DangerFor(tp.pawn)) && danger > tp.maxDanger)
					{
						return false;
					}
				}
			}
			bool flag = this.type == RegionType.Fence && tp.fenceBlocked && !tp.canBashFences;
			switch (tp.mode)
			{
			case TraverseMode.ByPawn:
			{
				if (this.door == null)
				{
					return !flag;
				}
				if (tp.pawn == null)
				{
					return this.door.FreePassage || this.door.Open || tp.canBashDoors;
				}
				AvoidGrid avoidGrid;
				if (tp.pawn.TryGetAvoidGrid(out avoidGrid, true))
				{
					foreach (IntVec3 intVec in this.door.OccupiedRect())
					{
						if (avoidGrid.Grid[this.Map.cellIndices.CellToIndex(intVec)] == 255 && intVec.GetRegion(this.Map, RegionType.Set_Passable) == this)
						{
							return false;
						}
					}
				}
				if (tp.pawn.HostileTo(this.door))
				{
					return this.door.CanPhysicallyPass(tp.pawn) || tp.canBashDoors;
				}
				return this.door.CanPhysicallyPass(tp.pawn) && !this.door.IsForbiddenToPass(tp.pawn);
			}
			case TraverseMode.PassDoors:
				return !flag;
			case TraverseMode.NoPassClosedDoors:
			case TraverseMode.NoPassClosedDoorsOrWater:
				return (this.door == null || this.door.FreePassage) && !flag;
			case TraverseMode.PassAllDestroyableThings:
				return true;
			case TraverseMode.PassAllDestroyablePlayerOwnedThings:
				return this.door == null || this.door.Faction != Faction.OfPlayer;
			case TraverseMode.PassAllDestroyableThingsNotWater:
				return true;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x000AAB2C File Offset: 0x000A8D2C
		public IEnumerable<CellRect> EnumerateRectangleCover()
		{
			List<IntVec3> uncoveredCells = new List<IntVec3>(this.Cells);
			while (uncoveredCells.Count > 0)
			{
				IntVec3 intVec = uncoveredCells.PopFront<IntVec3>();
				int num = intVec.x;
				int num2 = intVec.z;
				while (uncoveredCells.Contains(new IntVec3(num + 1, 0, num2)))
				{
					num++;
				}
				for (;;)
				{
					bool flag = false;
					for (int i = intVec.x; i <= num; i++)
					{
						if (!uncoveredCells.Contains(new IntVec3(i, 0, num2 + 1)))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
					num2++;
				}
				CellRect cellRect = new CellRect(intVec.x, intVec.z, num - intVec.x + 1, num2 - intVec.z + 1);
				HashSet<IntVec3> toRemove = new HashSet<IntVec3>(cellRect.Cells);
				uncoveredCells.RemoveAll((IntVec3 x) => toRemove.Contains(x));
				yield return cellRect;
			}
			yield break;
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x000AAB3C File Offset: 0x000A8D3C
		public Danger DangerFor(Pawn p)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Danger result;
				if (this.cachedDangersForTick != GenTicks.TicksGame)
				{
					this.cachedDangers.Clear();
					this.cachedDangersForTick = GenTicks.TicksGame;
				}
				else if (this.cachedDangers.TryGetValue(p, out result))
				{
					return result;
				}
			}
			Room room = this.Room;
			float temperature = room.Temperature;
			FloatRange value;
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (Region.cachedSafeTemperatureRangesForTick != GenTicks.TicksGame)
				{
					Region.cachedSafeTemperatureRanges.Clear();
					Region.cachedSafeTemperatureRangesForTick = GenTicks.TicksGame;
				}
				if (!Region.cachedSafeTemperatureRanges.TryGetValue(p, out value))
				{
					value = p.SafeTemperatureRange();
					Region.cachedSafeTemperatureRanges.Add(p, value);
				}
			}
			else
			{
				value = p.SafeTemperatureRange();
			}
			Danger danger;
			if (value.Includes(temperature))
			{
				danger = Danger.None;
			}
			else
			{
				danger = (value.ExpandedBy(80f).Includes(temperature) ? Danger.Some : Danger.Deadly);
			}
			if (room.Vacuum > 0.5f && p.ConcernedByVacuum)
			{
				danger = Danger.Deadly;
			}
			this.cachedDangers[p] = danger;
			return danger;
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x000AAC38 File Offset: 0x000A8E38
		public float GetBaseDesiredPlantsCount(bool allowCache = true)
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (allowCache && ticksGame - this.cachedBaseDesiredPlantsCountForTick < 2500)
			{
				return this.cachedBaseDesiredPlantsCount;
			}
			this.cachedBaseDesiredPlantsCount = 0f;
			Map map = this.Map;
			foreach (IntVec3 c in this.Cells)
			{
				this.cachedBaseDesiredPlantsCount += map.wildPlantSpawner.GetBaseDesiredPlantsCountAt(c);
			}
			this.cachedBaseDesiredPlantsCountForTick = ticksGame;
			return this.cachedBaseDesiredPlantsCount;
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x000AACDC File Offset: 0x000A8EDC
		public AreaOverlap OverlapWith(Area a)
		{
			if (a.TrueCount == 0)
			{
				return AreaOverlap.None;
			}
			if (this.Map != a.Map)
			{
				return AreaOverlap.None;
			}
			if (this.cachedAreaOverlaps == null)
			{
				this.cachedAreaOverlaps = new Dictionary<Area, AreaOverlap>();
			}
			AreaOverlap areaOverlap;
			if (!this.cachedAreaOverlaps.TryGetValue(a, out areaOverlap))
			{
				int num = 0;
				int num2 = 0;
				foreach (IntVec3 c in this.Cells)
				{
					num2++;
					if (a[c])
					{
						num++;
					}
				}
				if (num == 0)
				{
					areaOverlap = AreaOverlap.None;
				}
				else if (num == num2)
				{
					areaOverlap = AreaOverlap.Entire;
				}
				else
				{
					areaOverlap = AreaOverlap.Partial;
				}
				this.cachedAreaOverlaps.Add(a, areaOverlap);
			}
			return areaOverlap;
		}

		// Token: 0x06002099 RID: 8345 RVA: 0x000AAD98 File Offset: 0x000A8F98
		public void Notify_AreaChanged(Area a)
		{
			if (this.cachedAreaOverlaps == null)
			{
				return;
			}
			if (this.cachedAreaOverlaps.ContainsKey(a))
			{
				this.cachedAreaOverlaps.Remove(a);
			}
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x000AADC0 File Offset: 0x000A8FC0
		public void DecrementMapIndex()
		{
			if (this.mapIndex <= 0)
			{
				Log.Warning("Tried to decrement map index for region " + this.id.ToString() + ", but mapIndex=" + this.mapIndex.ToString());
				return;
			}
			this.mapIndex -= 1;
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x000AAE10 File Offset: 0x000A9010
		public void Notify_MyMapRemoved()
		{
			this.listerThings.Clear();
			this.mapIndex = -1;
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x000AAE24 File Offset: 0x000A9024
		public static void ClearStaticData()
		{
			Region.cachedSafeTemperatureRanges.Clear();
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x000AAE30 File Offset: 0x000A9030
		public override string ToString()
		{
			string str;
			if (this.door != null)
			{
				str = this.door.ToString();
			}
			else
			{
				str = "null";
			}
			return string.Concat(new string[]
			{
				"Region(id=",
				this.id.ToString(),
				", mapIndex=",
				this.mapIndex.ToString(),
				", center=",
				this.extentsClose.CenterCell.ToString(),
				", links=",
				this.links.Count.ToString(),
				", cells=",
				this.CellCount.ToString(),
				", touchesMapEdge=",
				this.touchesMapEdge.ToString(),
				(this.door != null) ? (", portal=" + str) : null,
				")"
			});
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x000AAF28 File Offset: 0x000A9128
		public void DebugDraw()
		{
			if (DebugViewSettings.drawRegionTraversal && Find.TickManager.TicksGame < this.debug_lastTraverseTick + 60)
			{
				float a = 1f - (float)(Find.TickManager.TicksGame - this.debug_lastTraverseTick) / 60f;
				GenDraw.DrawFieldEdges(this.Cells.ToList<IntVec3>(), new Color(0f, 0f, 1f, a), null, null, 2900);
			}
		}

		// Token: 0x0600209F RID: 8351 RVA: 0x000AAFA4 File Offset: 0x000A91A4
		public void DebugDrawMouseover()
		{
			int num = Mathf.RoundToInt(Time.realtimeSinceStartup * 2f) % 2;
			if (DebugViewSettings.drawRegions)
			{
				GenDraw.DrawFieldEdges(this.Cells.ToList<IntVec3>(), this.DebugColor(), null, null, 2900);
				foreach (Region region in this.Neighbors)
				{
					GenDraw.DrawFieldEdges(region.Cells.ToList<IntVec3>(), Color.grey, null, null, 2900);
				}
			}
			if (DebugViewSettings.drawRegionLinks)
			{
				foreach (RegionLink regionLink in this.links)
				{
					if (num == 1)
					{
						List<IntVec3> list = regionLink.span.Cells.ToList<IntVec3>();
						Material mat = DebugSolidColorMats.MaterialOf(Color.magenta * new Color(1f, 1f, 1f, 0.25f));
						foreach (IntVec3 c in list)
						{
							CellRenderer.RenderCell(c, mat);
						}
						GenDraw.DrawFieldEdges(list, Color.white, null, null, 2900);
					}
				}
			}
			if (DebugViewSettings.drawRegionThings)
			{
				foreach (Thing thing in this.listerThings.AllThings)
				{
					CellRenderer.RenderSpot(thing.TrueCenter(), (float)(thing.thingIDNumber % 256) / 256f, 0.15f);
				}
			}
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x000AB1A4 File Offset: 0x000A93A4
		private Color DebugColor()
		{
			Color result;
			if (!this.valid)
			{
				result = Color.red;
			}
			else if (this.DebugIsNew)
			{
				result = Color.yellow;
			}
			else
			{
				result = Color.green;
			}
			return result;
		}

		// Token: 0x060020A1 RID: 8353 RVA: 0x000AB1D8 File Offset: 0x000A93D8
		public void Debug_Notify_Traversed()
		{
			this.debug_lastTraverseTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060020A2 RID: 8354 RVA: 0x000AB1EA File Offset: 0x000A93EA
		public override int GetHashCode()
		{
			return this.precalculatedHashCode;
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x000AB1F4 File Offset: 0x000A93F4
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			Region region = obj as Region;
			return region != null && region.id == this.id;
		}

		// Token: 0x04001BB8 RID: 7096
		public RegionType type = RegionType.Normal;

		// Token: 0x04001BB9 RID: 7097
		public int id = -1;

		// Token: 0x04001BBA RID: 7098
		public sbyte mapIndex = -1;

		// Token: 0x04001BBB RID: 7099
		private District districtInt;

		// Token: 0x04001BBC RID: 7100
		public List<RegionLink> links = new List<RegionLink>();

		// Token: 0x04001BBD RID: 7101
		public CellRect extentsClose;

		// Token: 0x04001BBE RID: 7102
		public CellRect extentsLimit;

		// Token: 0x04001BBF RID: 7103
		public Building_Door door;

		// Token: 0x04001BC0 RID: 7104
		private int precalculatedHashCode;

		// Token: 0x04001BC1 RID: 7105
		public bool touchesMapEdge;

		// Token: 0x04001BC2 RID: 7106
		private int cachedCellCount = -1;

		// Token: 0x04001BC3 RID: 7107
		public bool valid = true;

		// Token: 0x04001BC4 RID: 7108
		private readonly ListerThings listerThings = new ListerThings(ListerThingsUse.Region, null);

		// Token: 0x04001BC5 RID: 7109
		public readonly uint[] closedIndex = new uint[RegionTraverser.NumWorkers];

		// Token: 0x04001BC6 RID: 7110
		public uint reachedIndex;

		// Token: 0x04001BC7 RID: 7111
		public int newRegionGroupIndex = -1;

		// Token: 0x04001BC8 RID: 7112
		private Dictionary<Area, AreaOverlap> cachedAreaOverlaps;

		// Token: 0x04001BC9 RID: 7113
		public int mark;

		// Token: 0x04001BCA RID: 7114
		private readonly Dictionary<Pawn, Danger> cachedDangers = new Dictionary<Pawn, Danger>();

		// Token: 0x04001BCB RID: 7115
		private int cachedDangersForTick;

		// Token: 0x04001BCC RID: 7116
		private float cachedBaseDesiredPlantsCount;

		// Token: 0x04001BCD RID: 7117
		private int cachedBaseDesiredPlantsCountForTick = -999999;

		// Token: 0x04001BCE RID: 7118
		private static readonly Dictionary<Pawn, FloatRange> cachedSafeTemperatureRanges = new Dictionary<Pawn, FloatRange>();

		// Token: 0x04001BCF RID: 7119
		private static int cachedSafeTemperatureRangesForTick;

		// Token: 0x04001BD0 RID: 7120
		private int debug_makeTick = -1000;

		// Token: 0x04001BD1 RID: 7121
		private int debug_lastTraverseTick = -1000;

		// Token: 0x04001BD2 RID: 7122
		private static int nextId = 1;

		// Token: 0x04001BD3 RID: 7123
		public const int GridSize = 12;
	}
}
