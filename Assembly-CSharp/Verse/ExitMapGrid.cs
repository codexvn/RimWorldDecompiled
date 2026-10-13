using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000428 RID: 1064
	public sealed class ExitMapGrid : ICellBoolGiver
	{
		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06001B04 RID: 6916 RVA: 0x0008857D File Offset: 0x0008677D
		public bool MapUsesExitGrid
		{
			get
			{
				if (Find.TickManager.TicksGame == this.lastCheckedMapTick)
				{
					return this.mapUsesExitGrid;
				}
				this.lastCheckedMapTick = Find.TickManager.TicksGame;
				this.mapUsesExitGrid = this.MapUsesExitGridNow;
				return this.mapUsesExitGrid;
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06001B05 RID: 6917 RVA: 0x000885BC File Offset: 0x000867BC
		private bool MapUsesExitGridNow
		{
			get
			{
				if (this.map.IsPlayerHome)
				{
					return false;
				}
				if (this.map.IsPocketMap)
				{
					return false;
				}
				if (this.map.Biome.inVacuum)
				{
					return false;
				}
				CaravansBattlefield caravansBattlefield = this.map.Parent as CaravansBattlefield;
				if (caravansBattlefield != null && caravansBattlefield.def.blockExitGridUntilBattleIsWon && !caravansBattlefield.WonBattle)
				{
					return false;
				}
				MapParent parent = this.map.Parent;
				FormCaravanComp formCaravanComp = (parent != null) ? parent.GetComponent<FormCaravanComp>() : null;
				return formCaravanComp == null || !formCaravanComp.CanFormOrReformCaravanNow;
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001B06 RID: 6918 RVA: 0x0008864C File Offset: 0x0008684C
		public CellBoolDrawer Drawer
		{
			get
			{
				if (!this.MapUsesExitGrid)
				{
					return null;
				}
				if (this.dirty)
				{
					this.Rebuild();
				}
				if (this.drawerInt == null)
				{
					this.drawerInt = new CellBoolDrawer(this, this.map.Size.x, this.map.Size.z, 3690, 0.33f);
				}
				return this.drawerInt;
			}
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001B07 RID: 6919 RVA: 0x000886B5 File Offset: 0x000868B5
		public BoolGrid Grid
		{
			get
			{
				if (!this.MapUsesExitGrid)
				{
					return null;
				}
				if (this.dirty)
				{
					this.Rebuild();
				}
				return this.exitMapGrid;
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001B08 RID: 6920 RVA: 0x000886D5 File Offset: 0x000868D5
		public Color Color
		{
			get
			{
				return new Color(0.35f, 1f, 0.35f, 0.12f);
			}
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x000886F0 File Offset: 0x000868F0
		public ExitMapGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x00088711 File Offset: 0x00086911
		public bool GetCellBool(int index)
		{
			return this.Grid[index] && !this.map.fogGrid.IsFogged(index);
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x000440FD File Offset: 0x000422FD
		public Color GetCellExtraColor(int index)
		{
			return Color.white;
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x00088737 File Offset: 0x00086937
		public bool IsExitCell(IntVec3 c)
		{
			return this.MapUsesExitGrid && c.InBounds(this.map) && this.Grid[c];
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x0008875F File Offset: 0x0008695F
		public void ExitMapGridUpdate()
		{
			if (!this.MapUsesExitGrid || Find.ScreenshotModeHandler.Active)
			{
				return;
			}
			this.Drawer.MarkForDraw();
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x0008878C File Offset: 0x0008698C
		public void Notify_LOSBlockerSpawned()
		{
			this.dirty = true;
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x0008878C File Offset: 0x0008698C
		public void Notify_LOSBlockerDespawned()
		{
			this.dirty = true;
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x00088798 File Offset: 0x00086998
		private void Rebuild()
		{
			this.dirty = false;
			if (this.exitMapGrid == null)
			{
				this.exitMapGrid = new BoolGrid(this.map);
			}
			else
			{
				this.exitMapGrid.Clear();
			}
			CellRect cellRect = CellRect.WholeMap(this.map);
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					if (i > 1 && i < cellRect.maxZ - 2 + 1 && j > 1 && j < cellRect.maxX - 2 + 1)
					{
						j = cellRect.maxX - 2 + 1;
					}
					IntVec3 intVec = new IntVec3(j, 0, i);
					if (this.IsGoodExitCell(intVec))
					{
						this.exitMapGrid[intVec] = true;
					}
				}
			}
			if (this.drawerInt != null)
			{
				this.drawerInt.SetDirty();
			}
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x0008886C File Offset: 0x00086A6C
		private bool IsGoodExitCell(IntVec3 cell)
		{
			if (!cell.CanBeSeenOver(this.map))
			{
				return false;
			}
			int num = GenRadial.NumCellsInRadius(2f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = cell + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.map) && intVec.OnEdge(this.map) && intVec.CanBeSeenOverFast(this.map) && GenSight.LineOfSight(cell, intVec, this.map))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040017AB RID: 6059
		private Map map;

		// Token: 0x040017AC RID: 6060
		private bool dirty = true;

		// Token: 0x040017AD RID: 6061
		private BoolGrid exitMapGrid;

		// Token: 0x040017AE RID: 6062
		private CellBoolDrawer drawerInt;

		// Token: 0x040017AF RID: 6063
		private const int MaxDistToEdge = 2;

		// Token: 0x040017B0 RID: 6064
		[Unsaved(false)]
		private bool mapUsesExitGrid;

		// Token: 0x040017B1 RID: 6065
		[Unsaved(false)]
		private int lastCheckedMapTick = -99999;
	}
}
