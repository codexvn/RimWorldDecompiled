using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000436 RID: 1078
	public class PollutionGrid : IExposable
	{
		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06001B99 RID: 7065 RVA: 0x0008AC17 File Offset: 0x00088E17
		public int TotalPollution
		{
			get
			{
				return this.grid.TrueCount;
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06001B9A RID: 7066 RVA: 0x0008AC24 File Offset: 0x00088E24
		public List<IntVec3> AllPollutableCells
		{
			get
			{
				this.allPollutableCells.Clear();
				this.allPollutableCells.AddRange(this.map.AllCells.Where(new Func<IntVec3, bool>(this.EverPollutable)));
				return this.allPollutableCells;
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06001B9B RID: 7067 RVA: 0x0008AC5E File Offset: 0x00088E5E
		public float TotalPollutionPercent
		{
			get
			{
				return (float)this.grid.TrueCount / (float)this.AllPollutableCells.Count;
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06001B9C RID: 7068 RVA: 0x0008AC7C File Offset: 0x00088E7C
		public CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawerInt == null)
				{
					this.drawerInt = new CellBoolDrawer(new Func<int, bool>(this.CellBoolDrawerGetBoolInt), new Func<Color>(this.CellBoolDrawerColorInt), new Func<int, Color>(this.CellBoolDrawerGetExtraColorInt), this.map.Size.x, this.map.Size.z, 3650, 0.33f);
				}
				return this.drawerInt;
			}
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x0008ACF0 File Offset: 0x00088EF0
		public PollutionGrid(Map map)
		{
			this.grid = new BoolGrid(map);
			this.map = map;
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x0008AD21 File Offset: 0x00088F21
		public bool IsPolluted(IntVec3 cell)
		{
			return ModsConfig.BiotechActive && cell.InBounds(this.map) && this.grid[cell];
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x0008AD48 File Offset: 0x00088F48
		public bool EverPollutable(IntVec3 cell)
		{
			if (!cell.InBounds(this.map))
			{
				return false;
			}
			Building edifice = cell.GetEdifice(this.map);
			return (edifice == null || edifice.def.EverPollutable) && cell.GetTerrain(this.map).canBePolluted;
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x0008AD98 File Offset: 0x00088F98
		public void SetPolluted(IntVec3 cell, bool isPolluted, bool silent = false)
		{
			if (!ModLister.CheckBiotech("Set pollution"))
			{
				return;
			}
			if (!cell.InBounds(this.map) || this.grid[cell] == isPolluted)
			{
				return;
			}
			this.grid.Set(cell, isPolluted);
			this.dirty = true;
			this.map.mapDrawer.MapMeshDirty(cell, MapMeshFlagDefOf.Terrain, true, false);
			this.map.mapDrawer.MapMeshDirty(cell, MapMeshFlagDefOf.Pollution, false, false);
			this.Drawer.SetDirty();
			this.map.fertilityGrid.Drawer.SetDirty();
			WaterBodyTracker waterBodyTracker = this.map.waterBodyTracker;
			if (waterBodyTracker != null)
			{
				waterBodyTracker.Notify_PollutionChanged(cell, isPolluted);
			}
			if (!silent && isPolluted && MapGenerator.mapBeingGenerated != this.map && !this.pollutedCellsThisTick.Contains(cell))
			{
				this.pollutedCellsThisTick.Add(cell);
			}
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x0008AE85 File Offset: 0x00089085
		public bool CanPollute(IntVec3 c)
		{
			return this.EverPollutable(c) && !this.IsPolluted(c);
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x0008AE9C File Offset: 0x0008909C
		public bool CanUnpollute(IntVec3 c)
		{
			return this.IsPolluted(c);
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x0008AEA8 File Offset: 0x000890A8
		public void PollutionTick()
		{
			if (this.dirty)
			{
				if (this.map.Tile.Valid)
				{
					Find.WorldGrid[this.map.Tile].pollution = this.TotalPollutionPercent;
					Find.World.renderer.Notify_TilePollutionChanged(this.map.Tile);
				}
				this.dirty = false;
			}
			if (this.map.IsHashIntervalTick(200) && this.TotalPollution > 0)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.PollutedTerrain, OpportunityType.Important);
			}
			if (this.pollutedCellsThisTick.Count > 0)
			{
				EffecterDef effecterDef = (this.pollutedCellsThisTick.Count > 10) ? EffecterDefOf.CellPollution_Performant : EffecterDefOf.CellPollution;
				for (int i = 0; i < this.pollutedCellsThisTick.Count; i++)
				{
					IntVec3 intVec = this.pollutedCellsThisTick[i];
					Effecter eff = effecterDef.Spawn(intVec, this.map, Vector3.zero, 1f);
					this.map.effecterMaintainer.AddEffecterToMaintain(eff, intVec, 45);
				}
			}
			this.pollutedCellsThisTick.Clear();
		}

		// Token: 0x06001BA4 RID: 7076 RVA: 0x0008AFBF File Offset: 0x000891BF
		public void PollutionGridUpdate()
		{
			if (Find.PlaySettings.showPollutionOverlay && !Find.ScreenshotModeHandler.Active)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x0008AFF0 File Offset: 0x000891F0
		private bool CellBoolDrawerGetBoolInt(int index)
		{
			IntVec3 intVec = CellIndicesUtility.IndexToCell(index, this.map.Size.x);
			return intVec.InBounds(this.map) && !intVec.Filled(this.map) && !intVec.Fogged(this.map) && this.EverPollutable(intVec) && this.IsPolluted(intVec);
		}

		// Token: 0x06001BA6 RID: 7078 RVA: 0x000440FD File Offset: 0x000422FD
		private Color CellBoolDrawerColorInt()
		{
			return Color.white;
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x0008B054 File Offset: 0x00089254
		private Color CellBoolDrawerGetExtraColorInt(int index)
		{
			IntVec3 cell = CellIndicesUtility.IndexToCell(index, this.map.Size.x);
			if (!this.IsPolluted(cell))
			{
				return Color.white;
			}
			return Color.red;
		}

		// Token: 0x06001BA8 RID: 7080 RVA: 0x0008B08C File Offset: 0x0008928C
		public void ExposeData()
		{
			Scribe_Deep.Look<BoolGrid>(ref this.grid, "grid", Array.Empty<object>());
		}

		// Token: 0x040017F6 RID: 6134
		private const int LessonPollutionTicks = 200;

		// Token: 0x040017F7 RID: 6135
		private const int PerformantPollutionEffectThreshold = 10;

		// Token: 0x040017F8 RID: 6136
		public const int DissolutionEffecterDuration = 45;

		// Token: 0x040017F9 RID: 6137
		private BoolGrid grid;

		// Token: 0x040017FA RID: 6138
		private Map map;

		// Token: 0x040017FB RID: 6139
		private bool dirty;

		// Token: 0x040017FC RID: 6140
		private CellBoolDrawer drawerInt;

		// Token: 0x040017FD RID: 6141
		private List<IntVec3> allPollutableCells = new List<IntVec3>();

		// Token: 0x040017FE RID: 6142
		private List<IntVec3> pollutedCellsThisTick = new List<IntVec3>();
	}
}
