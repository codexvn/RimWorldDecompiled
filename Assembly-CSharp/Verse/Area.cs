using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003C2 RID: 962
	public abstract class Area : IExposable, ILoadReferenceable, ICellBoolGiver, IRenameable
	{
		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x060018A1 RID: 6305 RVA: 0x0007CE64 File Offset: 0x0007B064
		// (set) Token: 0x060018A2 RID: 6306 RVA: 0x0007CE6C File Offset: 0x0007B06C
		public virtual string RenamableLabel { get; set; }

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x060018A3 RID: 6307 RVA: 0x0007CE75 File Offset: 0x0007B075
		// (set) Token: 0x060018A4 RID: 6308 RVA: 0x0007CE7D File Offset: 0x0007B07D
		public virtual string BaseLabel { get; set; }

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x060018A5 RID: 6309 RVA: 0x0007CE86 File Offset: 0x0007B086
		public virtual string InspectLabel
		{
			get
			{
				return this.RenamableLabel;
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x060018A6 RID: 6310 RVA: 0x0007CE8E File Offset: 0x0007B08E
		public Map Map
		{
			get
			{
				return this.areaManager.map;
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x060018A7 RID: 6311 RVA: 0x0007CE9B File Offset: 0x0007B09B
		public BoolGrid InnerGrid
		{
			get
			{
				return this.innerGrid;
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x060018A8 RID: 6312 RVA: 0x0007CEA3 File Offset: 0x0007B0A3
		public int TrueCount
		{
			get
			{
				return this.innerGrid.TrueCount;
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x060018A9 RID: 6313
		public abstract string Label { get; }

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x060018AA RID: 6314
		public abstract Color Color { get; }

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x060018AB RID: 6315
		public abstract int ListPriority { get; }

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x060018AC RID: 6316 RVA: 0x0007CEB0 File Offset: 0x0007B0B0
		public Texture2D ColorTexture
		{
			get
			{
				if (this.colorTextureInt == null)
				{
					this.colorTextureInt = SolidColorMaterials.NewSolidColorTexture(this.Color);
				}
				return this.colorTextureInt;
			}
		}

		// Token: 0x17000481 RID: 1153
		public bool this[int index]
		{
			get
			{
				return this.innerGrid[index];
			}
			set
			{
				this.Set(this.Map.cellIndices.IndexToCell(index), value);
			}
		}

		// Token: 0x17000482 RID: 1154
		public bool this[IntVec3 c]
		{
			get
			{
				return this.innerGrid[this.Map.cellIndices.CellToIndex(c)];
			}
			set
			{
				this.Set(c, value);
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x060018B1 RID: 6321 RVA: 0x0007CF28 File Offset: 0x0007B128
		private CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawer == null)
				{
					this.drawer = new CellBoolDrawer(this, this.Map.Size.x, this.Map.Size.z, 3650, 0.33f);
				}
				return this.drawer;
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x060018B2 RID: 6322 RVA: 0x0007CF79 File Offset: 0x0007B179
		public IEnumerable<IntVec3> ActiveCells
		{
			get
			{
				return this.innerGrid.ActiveCells;
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x060018B3 RID: 6323 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool Mutable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x0007CF86 File Offset: 0x0007B186
		public Area()
		{
		}

		// Token: 0x060018B5 RID: 6325 RVA: 0x0007CF95 File Offset: 0x0007B195
		public Area(AreaManager areaManager)
		{
			this.areaManager = areaManager;
			this.innerGrid = new BoolGrid(areaManager.map);
			this.ID = Find.UniqueIDsManager.GetNextAreaID();
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x0007CFCC File Offset: 0x0007B1CC
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Deep.Look<BoolGrid>(ref this.innerGrid, "innerGrid", Array.Empty<object>());
		}

		// Token: 0x060018B7 RID: 6327 RVA: 0x0007CED7 File Offset: 0x0007B0D7
		public bool GetCellBool(int index)
		{
			return this.innerGrid[index];
		}

		// Token: 0x060018B8 RID: 6328 RVA: 0x000440FD File Offset: 0x000422FD
		public Color GetCellExtraColor(int index)
		{
			return Color.white;
		}

		// Token: 0x060018B9 RID: 6329 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool AssignableAsAllowed()
		{
			return false;
		}

		// Token: 0x060018BA RID: 6330 RVA: 0x0007CFF8 File Offset: 0x0007B1F8
		protected virtual void Set(IntVec3 c, bool val)
		{
			int index = this.Map.cellIndices.CellToIndex(c);
			if (this.innerGrid[index] == val)
			{
				return;
			}
			this.innerGrid[index] = val;
			this.MarkDirty(c);
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x0007D03C File Offset: 0x0007B23C
		private void MarkDirty(IntVec3 c)
		{
			this.Drawer.SetDirty();
			this.Map.pathFinder.MapData.Notify_AreaDelta(this, c);
			Region region = c.GetRegion(this.Map, RegionType.Set_All);
			if (region != null)
			{
				region.Notify_AreaChanged(this);
			}
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x0007D084 File Offset: 0x0007B284
		public void Delete()
		{
			this.areaManager.Remove(this);
		}

		// Token: 0x060018BD RID: 6333 RVA: 0x0007D092 File Offset: 0x0007B292
		public void MarkForDraw()
		{
			if (this.Map == Find.CurrentMap && !Find.ScreenshotModeHandler.Active)
			{
				this.Drawer.MarkForDraw();
			}
		}

		// Token: 0x060018BE RID: 6334 RVA: 0x0007D0B8 File Offset: 0x0007B2B8
		public void AreaUpdate()
		{
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x060018BF RID: 6335 RVA: 0x0007D0C5 File Offset: 0x0007B2C5
		public void Invert()
		{
			this.innerGrid.Invert();
			this.Drawer.SetDirty();
		}

		// Token: 0x060018C0 RID: 6336 RVA: 0x0007D0DD File Offset: 0x0007B2DD
		public void Clear()
		{
			this.innerGrid.Clear();
			this.Drawer.SetDirty();
		}

		// Token: 0x060018C1 RID: 6337 RVA: 0x0007D0F5 File Offset: 0x0007B2F5
		protected void InvalidateColorTexture()
		{
			UnityEngine.Object.Destroy(this.colorTextureInt);
			this.colorTextureInt = null;
			CellBoolDrawer cellBoolDrawer = this.drawer;
			if (cellBoolDrawer == null)
			{
				return;
			}
			cellBoolDrawer.Notify_ColorChanged();
		}

		// Token: 0x060018C2 RID: 6338
		public abstract string GetUniqueLoadID();

		// Token: 0x040015E0 RID: 5600
		public AreaManager areaManager;

		// Token: 0x040015E1 RID: 5601
		public int ID = -1;

		// Token: 0x040015E2 RID: 5602
		private BoolGrid innerGrid;

		// Token: 0x040015E3 RID: 5603
		private CellBoolDrawer drawer;

		// Token: 0x040015E4 RID: 5604
		private Texture2D colorTextureInt;
	}
}
