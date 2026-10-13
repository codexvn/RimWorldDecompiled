using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000523 RID: 1315
	public sealed class RoofGrid : IExposable, ICellBoolGiver
	{
		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x0600223F RID: 8767 RVA: 0x000B28DC File Offset: 0x000B0ADC
		public CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawerInt == null)
				{
					this.drawerInt = new CellBoolDrawer(this, this.map.Size.x, this.map.Size.z, 3620, 0.33f);
				}
				return this.drawerInt;
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06002240 RID: 8768 RVA: 0x000B292D File Offset: 0x000B0B2D
		public Color Color
		{
			get
			{
				return new Color(0.3f, 1f, 0.4f);
			}
		}

		// Token: 0x06002241 RID: 8769 RVA: 0x000B2943 File Offset: 0x000B0B43
		public RoofGrid(Map map)
		{
			this.map = map;
			this.roofGrid = new RoofDef[map.cellIndices.NumGridCells];
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x000B2968 File Offset: 0x000B0B68
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, delegate(IntVec3 c)
			{
				if (this.roofGrid[this.map.cellIndices.CellToIndex(c)] != null)
				{
					return this.roofGrid[this.map.cellIndices.CellToIndex(c)].shortHash;
				}
				return 0;
			}, delegate(IntVec3 c, ushort val)
			{
				this.SetRoof(c, DefDatabase<RoofDef>.GetByShortHash(val));
			}, "roofs");
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x000B2992 File Offset: 0x000B0B92
		public bool GetCellBool(int index)
		{
			return this.roofGrid[index] != null && !this.map.fogGrid.IsFogged(index);
		}

		// Token: 0x06002244 RID: 8772 RVA: 0x000B29B4 File Offset: 0x000B0BB4
		public Color GetCellExtraColor(int index)
		{
			if (RoofDefOf.RoofRockThick != null && this.roofGrid[index] == RoofDefOf.RoofRockThick)
			{
				return Color.gray;
			}
			return Color.white;
		}

		// Token: 0x06002245 RID: 8773 RVA: 0x000B29D7 File Offset: 0x000B0BD7
		public bool Roofed(int index)
		{
			return this.roofGrid[index] != null;
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x000B29E4 File Offset: 0x000B0BE4
		public bool Roofed(int x, int z)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(x, z)] != null;
		}

		// Token: 0x06002247 RID: 8775 RVA: 0x000B2A02 File Offset: 0x000B0C02
		public bool Roofed(IntVec3 c)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(c)] != null;
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x000B2A1F File Offset: 0x000B0C1F
		public RoofDef RoofAt(int index)
		{
			return this.roofGrid[index];
		}

		// Token: 0x06002249 RID: 8777 RVA: 0x000B2A29 File Offset: 0x000B0C29
		public RoofDef RoofAt(IntVec3 c)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x0600224A RID: 8778 RVA: 0x000B2A43 File Offset: 0x000B0C43
		public RoofDef RoofAt(int x, int z)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(x, z)];
		}

		// Token: 0x0600224B RID: 8779 RVA: 0x000B2A60 File Offset: 0x000B0C60
		public void SetRoof(IntVec3 c, RoofDef def)
		{
			if (this.roofGrid[this.map.cellIndices.CellToIndex(c)] == def)
			{
				return;
			}
			this.roofGrid[this.map.cellIndices.CellToIndex(c)] = def;
			Region validRegionAt_NoRebuild = this.map.regionGrid.GetValidRegionAt_NoRebuild(c);
			if (validRegionAt_NoRebuild != null)
			{
				validRegionAt_NoRebuild.District.Notify_RoofChanged();
			}
			if (this.drawerInt != null)
			{
				this.drawerInt.SetDirty();
			}
			this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Roofs);
			this.map.glowGrid.DirtyCell(c);
			this.map.events.Notify_RoofChanged(c);
		}

		// Token: 0x0600224C RID: 8780 RVA: 0x000B2B12 File Offset: 0x000B0D12
		public void RemoveRoofUnsafe(int index)
		{
			this.roofGrid[index] = null;
		}

		// Token: 0x0600224D RID: 8781 RVA: 0x000B2B1D File Offset: 0x000B0D1D
		public void RoofGridUpdate()
		{
			if (Find.PlaySettings.showRoofOverlay && !Find.ScreenshotModeHandler.Active)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x04001CD8 RID: 7384
		private Map map;

		// Token: 0x04001CD9 RID: 7385
		private RoofDef[] roofGrid;

		// Token: 0x04001CDA RID: 7386
		private CellBoolDrawer drawerInt;
	}
}
