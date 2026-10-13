using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A96 RID: 10902
	public abstract class Flood : Thing
	{
		// Token: 0x170025EC RID: 9708
		// (get) Token: 0x0600F2B6 RID: 62134 RVA: 0x004679C7 File Offset: 0x00465BC7
		private int ExpandIntervalTicks
		{
			get
			{
				return this.MaxFloodDurationTicks / this.estimatedFloodedTiles;
			}
		}

		// Token: 0x170025ED RID: 9709
		// (get) Token: 0x0600F2B7 RID: 62135 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual int StartDelayTicks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170025EE RID: 9710
		// (get) Token: 0x0600F2B8 RID: 62136 RVA: 0x004679D6 File Offset: 0x00465BD6
		protected int FloodingTicks
		{
			get
			{
				return this.MaxFloodDurationTicks + this.StartDelayTicks;
			}
		}

		// Token: 0x170025EF RID: 9711
		// (get) Token: 0x0600F2B9 RID: 62137 RVA: 0x004679E5 File Offset: 0x00465BE5
		protected virtual int MaxFloodDurationTicks
		{
			get
			{
				return 120000;
			}
		}

		// Token: 0x0600F2BA RID: 62138 RVA: 0x004679EC File Offset: 0x00465BEC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.estimatedFloodedTiles, "estimatedFloodedTiles", 0, false);
			Scribe_Values.Look<int>(ref this.floodedTileCount, "floodedTileCount", 0, false);
			Dictionary<IntVec3, int> dictionary = new Dictionary<IntVec3, int>();
			for (int i = 0; i < this.openCellWeights.Length; i++)
			{
				HashSet<IntVec3>[] array = this.openCellWeights;
				int num = i;
				if (array[num] == null)
				{
					array[num] = new HashSet<IntVec3>();
				}
				foreach (IntVec3 key in this.openCellWeights[i])
				{
					dictionary[key] = i;
				}
			}
			Scribe_Collections.Look<IntVec3, int>(ref dictionary, "openCellWeights", LookMode.Value, LookMode.Value, ref this.tmpOpenCells, ref this.tmpFloodedWeights, true, false, false);
			this.openCellWeights = new HashSet<IntVec3>[Flood.FloodWidthRange.max];
			for (int j = 0; j < this.openCellWeights.Length; j++)
			{
				this.openCellWeights[j] = new HashSet<IntVec3>();
			}
			foreach (KeyValuePair<IntVec3, int> keyValuePair in dictionary)
			{
				int num;
				IntVec3 intVec;
				keyValuePair.Deconstruct(out intVec, out num);
				IntVec3 item = intVec;
				int num2 = num;
				this.openCellWeights[num2].Add(item);
			}
		}

		// Token: 0x0600F2BB RID: 62139 RVA: 0x00467B50 File Offset: 0x00465D50
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!ModLister.CheckOdyssey("Flood"))
			{
				return;
			}
			map.events.TerrainChanged += this.Notify_OnTerrainChanged;
			map.events.BuildingSpawned += this.Notify_BuildingChanged;
			map.events.BuildingDespawned += this.Notify_BuildingChanged;
			if (respawningAfterLoad)
			{
				return;
			}
			for (int i = 0; i < this.openCellWeights.Length; i++)
			{
				this.openCellWeights[i] = new HashSet<IntVec3>();
			}
			foreach (ValueTuple<IntVec3, int> valueTuple in this.GetInitialCells(map))
			{
				IntVec3 item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				this.openCellWeights[item2 - 1].Add(item);
			}
			if (!this.AnyCellOpen())
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			this.estimatedFloodedTiles = this.openCellWeights.Sum((HashSet<IntVec3> c) => c.Count) * Flood.FloodWidthRange.max;
		}

		// Token: 0x0600F2BC RID: 62140 RVA: 0x00467C80 File Offset: 0x00465E80
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Map.events.TerrainChanged -= this.Notify_OnTerrainChanged;
			base.Map.events.BuildingSpawned -= this.Notify_BuildingChanged;
			base.Map.events.BuildingDespawned -= this.Notify_BuildingChanged;
			base.DeSpawn(mode);
		}

		// Token: 0x0600F2BD RID: 62141 RVA: 0x00467CE8 File Offset: 0x00465EE8
		private void Notify_BuildingChanged(Building building)
		{
			if (this.CanFloodSpreadInto(building.Position))
			{
				this.noPossibleCell = false;
			}
		}

		// Token: 0x0600F2BE RID: 62142 RVA: 0x00467CFF File Offset: 0x00465EFF
		private void Notify_OnTerrainChanged(IntVec3 cell)
		{
			if (this.CanFloodSpreadInto(cell))
			{
				this.noPossibleCell = false;
			}
		}

		// Token: 0x0600F2BF RID: 62143
		protected abstract IEnumerable<ValueTuple<IntVec3, int>> GetInitialCells(Map map);

		// Token: 0x0600F2C0 RID: 62144 RVA: 0x00467D11 File Offset: 0x00465F11
		protected override void Tick()
		{
			if (!this.noPossibleCell && this.IsHashIntervalTick(this.ExpandIntervalTicks) && Find.TickManager.TicksGame > this.spawnedTick + this.StartDelayTicks)
			{
				this.SpreadFlood();
			}
		}

		// Token: 0x0600F2C1 RID: 62145 RVA: 0x00467D48 File Offset: 0x00465F48
		private void SpreadFlood()
		{
			if (this.tempAdjCells == null)
			{
				this.tempAdjCells = GenAdj.CardinalDirections.ToList<IntVec3>();
			}
			this.tempAdjCells.Shuffle<IntVec3>();
			bool flag = false;
			while (!flag)
			{
				ValueTuple<IntVec3, int> nextCell = this.GetNextCell();
				IntVec3 item = nextCell.Item1;
				int item2 = nextCell.Item2;
				if (!item.IsValid)
				{
					this.noPossibleCell = true;
					return;
				}
				bool flag2 = true;
				TerrainDef terrainDef = base.Map.terrainGrid.TempTerrainAt(item);
				if (terrainDef == null || terrainDef.floodTerrain == null)
				{
					terrainDef = base.Map.terrainGrid.BaseTerrainAt(item);
				}
				for (int i = 0; i < 4; i++)
				{
					IntVec3 intVec = item + GenAdj.CardinalDirections[i];
					if (this.CanFloodPotentiallySpreadInto(intVec))
					{
						flag2 = false;
					}
					if (this.CanFloodSpreadInto(intVec))
					{
						int num = item2 - 1;
						if (num > 0)
						{
							this.openCellWeights[num - 1].Add(intVec);
						}
						this.SpreadFlood(intVec, terrainDef);
						this.floodedTileCount++;
						flag = true;
						break;
					}
				}
				if (flag2)
				{
					this.openCellWeights[item2 - 1].Remove(item);
				}
			}
		}

		// Token: 0x0600F2C2 RID: 62146
		protected abstract void SpreadFlood(IntVec3 cell, TerrainDef sourceTerrain);

		// Token: 0x0600F2C3 RID: 62147 RVA: 0x00467E65 File Offset: 0x00466065
		private bool CanFloodPotentiallySpreadInto(IntVec3 cell)
		{
			return cell.InBounds(base.Map) && !this.CellOpen(cell) && !base.Map.terrainGrid.TerrainAt(cell).IsWater;
		}

		// Token: 0x0600F2C4 RID: 62148 RVA: 0x00467E9D File Offset: 0x0046609D
		private bool CanFloodSpreadInto(IntVec3 cell)
		{
			return this.CanFloodPotentiallySpreadInto(cell) && base.Map.terrainGrid.FoundationAt(cell) == null && cell.GetEdifice(base.Map) == null;
		}

		// Token: 0x0600F2C5 RID: 62149 RVA: 0x00467ED0 File Offset: 0x004660D0
		private ValueTuple<IntVec3, int> GetNextCell()
		{
			ValueTuple<IntVec3, int> result;
			using (ProfilerBlock.Scope("GetNextCell"))
			{
				for (int i = this.openCellWeights.Length - 1; i >= 0; i--)
				{
					if (this.openCellWeights[i].Count != 0)
					{
						for (int j = 0; j < 10; j++)
						{
							IntVec3 intVec;
							if (this.openCellWeights[i].TryRandomElement(out intVec) && this.CellCanSpreadFlood(intVec))
							{
								return new ValueTuple<IntVec3, int>(intVec, i + 1);
							}
							if (!this.CellCanPotentiallySpreadFlood(intVec))
							{
								this.openCellWeights[i].Remove(intVec);
							}
						}
						for (int k = this.openCellWeights[i].Count - 1; k >= 0; k--)
						{
							IntVec3 intVec2;
							if (this.openCellWeights[i].TryRandomElement(out intVec2) && this.CellCanSpreadFlood(intVec2))
							{
								return new ValueTuple<IntVec3, int>(intVec2, i + 1);
							}
							if (!this.CellCanPotentiallySpreadFlood(intVec2))
							{
								this.openCellWeights[i].Remove(intVec2);
							}
						}
					}
				}
				result = new ValueTuple<IntVec3, int>(IntVec3.Invalid, -1);
			}
			return result;
		}

		// Token: 0x0600F2C6 RID: 62150 RVA: 0x00467FF4 File Offset: 0x004661F4
		private bool CellCanSpreadFlood(IntVec3 cell)
		{
			for (int i = 0; i < GenAdj.CardinalDirections.Length; i++)
			{
				IntVec3 b = GenAdj.CardinalDirections[i];
				if (this.CanFloodSpreadInto(cell + b))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600F2C7 RID: 62151 RVA: 0x00468034 File Offset: 0x00466234
		private bool CellCanPotentiallySpreadFlood(IntVec3 cell)
		{
			for (int i = 0; i < GenAdj.CardinalDirections.Length; i++)
			{
				IntVec3 b = GenAdj.CardinalDirections[i];
				if (this.CanFloodPotentiallySpreadInto(cell + b))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600F2C8 RID: 62152 RVA: 0x00468074 File Offset: 0x00466274
		private bool CellOpen(IntVec3 cell)
		{
			for (int i = 0; i < this.openCellWeights.Length; i++)
			{
				if (this.openCellWeights[i].Contains(cell))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600F2C9 RID: 62153 RVA: 0x004680A8 File Offset: 0x004662A8
		private bool AnyCellOpen()
		{
			for (int i = 0; i < this.openCellWeights.Length; i++)
			{
				if (this.openCellWeights[i].Count > 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04009FE7 RID: 40935
		protected static readonly IntRange FloodWidthRange = new IntRange(10, 12);

		// Token: 0x04009FE8 RID: 40936
		private HashSet<IntVec3>[] openCellWeights = new HashSet<IntVec3>[Flood.FloodWidthRange.max];

		// Token: 0x04009FE9 RID: 40937
		protected int estimatedFloodedTiles;

		// Token: 0x04009FEA RID: 40938
		protected int floodedTileCount;

		// Token: 0x04009FEB RID: 40939
		private bool noPossibleCell;

		// Token: 0x04009FEC RID: 40940
		private List<IntVec3> tmpOpenCells = new List<IntVec3>();

		// Token: 0x04009FED RID: 40941
		private List<int> tmpFloodedWeights = new List<int>();

		// Token: 0x04009FEE RID: 40942
		private List<IntVec3> tempAdjCells;
	}
}
