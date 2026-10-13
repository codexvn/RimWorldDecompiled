using System;
using System.Collections.Generic;
using System.Text;
using LudeonTK;
using RimWorld;
using Unity.Collections;

namespace Verse.AI
{
	// Token: 0x02000FB5 RID: 4021
	public class PathGrid : IDisposable
	{
		// Token: 0x17000F87 RID: 3975
		// (get) Token: 0x060062BC RID: 25276 RVA: 0x001F1F3A File Offset: 0x001F013A
		public NativeArray<int> Grid_Unsafe
		{
			get
			{
				return this.pathGrid;
			}
		}

		// Token: 0x060062BD RID: 25277 RVA: 0x001F1F42 File Offset: 0x001F0142
		public PathGrid(Map map, PathGridDef def)
		{
			this.map = map;
			this.def = def;
			this.pathGrid = new NativeArray<int>(map.cellIndices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x060062BE RID: 25278 RVA: 0x001F1F70 File Offset: 0x001F0170
		public bool Walkable(IntVec3 loc)
		{
			return loc.InBounds(this.map) && this.Cost(loc) < 10000;
		}

		// Token: 0x060062BF RID: 25279 RVA: 0x001F1F90 File Offset: 0x001F0190
		public bool WalkableFast(IntVec3 loc)
		{
			return this.Cost(loc) < 10000;
		}

		// Token: 0x060062C0 RID: 25280 RVA: 0x001F1FA0 File Offset: 0x001F01A0
		public bool WalkableFast(int x, int z)
		{
			return this.pathGrid[this.map.cellIndices.CellToIndex(x, z)] < 10000;
		}

		// Token: 0x060062C1 RID: 25281 RVA: 0x001F1FC6 File Offset: 0x001F01C6
		public bool WalkableFast(int index)
		{
			return this.pathGrid[index] < 10000;
		}

		// Token: 0x060062C2 RID: 25282 RVA: 0x001F1FDB File Offset: 0x001F01DB
		public int Cost(IntVec3 loc)
		{
			return this.pathGrid[this.map.cellIndices.CellToIndex(loc)];
		}

		// Token: 0x060062C3 RID: 25283 RVA: 0x001F1FFC File Offset: 0x001F01FC
		private void RecalculatePerceivedPathCostAt(IntVec3 c)
		{
			bool flag = false;
			this.RecalculatePerceivedPathCostAt(c, ref flag);
		}

		// Token: 0x060062C4 RID: 25284 RVA: 0x001F2014 File Offset: 0x001F0214
		public void RecalculatePerceivedPathCostAt(IntVec3 c, ref bool haveNotified)
		{
			if (!c.InBounds(this.map))
			{
				return;
			}
			if (this.dirty.IsCreated)
			{
				this.dirty.Set(this.map.cellIndices.CellToIndex(c), true);
				return;
			}
			if (!haveNotified)
			{
				if (this.RecalcuateCellDirect(c))
				{
					this.NotifyCellDirtied(c);
					haveNotified = true;
					return;
				}
			}
			else
			{
				this.pathGrid[this.map.cellIndices.CellToIndex(c)] = this.CalculatedCostAt(c, true, IntVec3.Invalid, null);
			}
		}

		// Token: 0x060062C5 RID: 25285 RVA: 0x001F20A8 File Offset: 0x001F02A8
		private bool RecalcuateCellDirect(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				return false;
			}
			int index = this.map.cellIndices[c];
			bool flag = this.WalkableFast(index);
			this.pathGrid[index] = this.CalculatedCostAt(c, true, IntVec3.Invalid, null);
			return flag != this.WalkableFast(index);
		}

		// Token: 0x060062C6 RID: 25286 RVA: 0x001F210C File Offset: 0x001F030C
		private void NotifyCellDirtied(IntVec3 cell)
		{
			this.map.reachability.ClearCache();
			this.map.regionDirtyer.Notify_WalkabilityChanged(cell, this.WalkableFast(cell));
		}

		// Token: 0x060062C7 RID: 25287 RVA: 0x001F2138 File Offset: 0x001F0338
		public void RecalculateAllPerceivedPathCosts()
		{
			foreach (IntVec3 c in this.map.AllCells)
			{
				this.RecalculatePerceivedPathCostAt(c);
			}
			if (this.dirty.IsCreated)
			{
				this.dirty.Clear();
			}
		}

		// Token: 0x060062C8 RID: 25288 RVA: 0x001F21A4 File Offset: 0x001F03A4
		public virtual int CalculatedCostAt(IntVec3 c, bool perceivedStatic, IntVec3 prevCell, int? baseCostOverride = null)
		{
			int num = 0;
			bool flag = false;
			TerrainDef terrainDef = this.map.terrainGrid.TerrainAt(c);
			if (terrainDef == null || (terrainDef.passability == Traversability.Impassable && (!this.def.flying || !terrainDef.forcePassableByFlyingPawns)))
			{
				return 10000;
			}
			if (baseCostOverride != null)
			{
				num = baseCostOverride.Value;
			}
			else if (!this.def.flying)
			{
				num = terrainDef.pathCost;
			}
			List<Thing> list = this.map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.def.passability == Traversability.Impassable && (!this.def.flying || !thing.def.forcePassableByFlyingPawns))
				{
					return 10000;
				}
				if (!this.def.fencePassable && thing.def.building != null && thing.def.building.isFence)
				{
					return 10000;
				}
				if (!PathGrid.IsPathCostIgnoreRepeater(thing.def) || !prevCell.IsValid || !this.ContainsPathCostIgnoreRepeater(prevCell))
				{
					int pathCost = thing.def.pathCost;
					if (pathCost > num)
					{
						num = pathCost;
					}
				}
				Building_Door building_Door = thing as Building_Door;
				if (building_Door != null && prevCell.IsValid)
				{
					Building_Door building_Door2 = prevCell.GetEdifice(this.map) as Building_Door;
					if (building_Door2 != null && !building_Door.FreePassage && !building_Door2.FreePassage)
					{
						flag = true;
					}
				}
			}
			int num2 = WeatherBuildupUtility.MovementTicksAddOn(this.map.snowGrid.GetCategory(c));
			if (num2 > num)
			{
				num = num2;
			}
			if (ModsConfig.OdysseyActive)
			{
				int num3 = WeatherBuildupUtility.MovementTicksAddOn(this.map.sandGrid.GetCategory(c));
				if (num3 > num)
				{
					num = num3;
				}
			}
			if (flag)
			{
				num += 45;
			}
			if (perceivedStatic)
			{
				for (int j = 0; j < 9; j++)
				{
					IntVec3 intVec = GenAdj.AdjacentCellsAndInside[j];
					IntVec3 c2 = c + intVec;
					if (c2.InBounds(this.map))
					{
						Fire fire = null;
						list = this.map.thingGrid.ThingsListAtFast(c2);
						for (int k = 0; k < list.Count; k++)
						{
							fire = (list[k] as Fire);
							if (fire != null)
							{
								break;
							}
						}
						if (fire != null && fire.parent == null)
						{
							if (intVec.x == 0 && intVec.z == 0)
							{
								num += 1000;
							}
							else
							{
								num += 150;
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060062C9 RID: 25289 RVA: 0x001F242C File Offset: 0x001F062C
		private bool ContainsPathCostIgnoreRepeater(IntVec3 c)
		{
			List<Thing> list = this.map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (PathGrid.IsPathCostIgnoreRepeater(list[i].def))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060062CA RID: 25290 RVA: 0x001F2472 File Offset: 0x001F0672
		private static bool IsPathCostIgnoreRepeater(ThingDef def)
		{
			return def.pathCost >= 25 && def.pathCostIgnoreRepeat;
		}

		// Token: 0x060062CB RID: 25291 RVA: 0x001F2486 File Offset: 0x001F0686
		public void DisableIncrementalDirtying()
		{
			if (!this.dirty.IsCreated)
			{
				this.dirty = new NativeBitArray(this.map.cellIndices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			}
		}

		// Token: 0x060062CC RID: 25292 RVA: 0x001F24B8 File Offset: 0x001F06B8
		public void ReEnableIncrementalDirtying()
		{
			if (!this.dirty.IsCreated)
			{
				return;
			}
			for (int i = 0; i < this.dirty.Length; i++)
			{
				if (this.dirty.IsSet(i))
				{
					IntVec3 intVec = this.map.cellIndices[i];
					this.RecalcuateCellDirect(intVec);
					this.NotifyCellDirtied(intVec);
				}
			}
			this.dirty.Dispose();
		}

		// Token: 0x060062CD RID: 25293 RVA: 0x001F2523 File Offset: 0x001F0723
		public void Dispose()
		{
			ref this.pathGrid.EnsureDisposed<int>();
			ref this.dirty.EnsureDisposed();
		}

		// Token: 0x060062CE RID: 25294 RVA: 0x001F253C File Offset: 0x001F073C
		[DebugOutput]
		public static void ThingPathCostsIgnoreRepeaters()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("===============PATH COST IGNORE REPEATERS==============");
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (PathGrid.IsPathCostIgnoreRepeater(thingDef) && thingDef.passability != Traversability.Impassable)
				{
					stringBuilder.AppendLine(thingDef.defName + " " + thingDef.pathCost.ToString());
				}
			}
			stringBuilder.AppendLine("===============NON-PATH COST IGNORE REPEATERS that are buildings with >0 pathCost ==============");
			foreach (ThingDef thingDef2 in DefDatabase<ThingDef>.AllDefs)
			{
				if (!PathGrid.IsPathCostIgnoreRepeater(thingDef2) && thingDef2.passability != Traversability.Impassable && thingDef2.category == ThingCategory.Building && thingDef2.pathCost > 0)
				{
					stringBuilder.AppendLine(thingDef2.defName + " " + thingDef2.pathCost.ToString());
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04004695 RID: 18069
		public readonly Map map;

		// Token: 0x04004696 RID: 18070
		public readonly PathGridDef def;

		// Token: 0x04004697 RID: 18071
		private NativeArray<int> pathGrid;

		// Token: 0x04004698 RID: 18072
		private NativeBitArray dirty;

		// Token: 0x04004699 RID: 18073
		public const int ImpassableCost = 10000;
	}
}
