using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200046B RID: 1131
	public sealed class MapDrawer
	{
		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001DA9 RID: 7593 RVA: 0x00097128 File Offset: 0x00095328
		private IntVec2 SectionCount
		{
			get
			{
				return new IntVec2
				{
					x = Mathf.CeilToInt((float)this.map.Size.x / 17f),
					z = Mathf.CeilToInt((float)this.map.Size.z / 17f)
				};
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001DAA RID: 7594 RVA: 0x00097184 File Offset: 0x00095384
		private CellRect VisibleSections
		{
			get
			{
				CellRect viewRect = this.ViewRect;
				IntVec2 intVec = this.SectionCoordsAt(viewRect.Min);
				IntVec2 intVec2 = this.SectionCoordsAt(viewRect.Max);
				if (intVec2.x < intVec.x || intVec2.z < intVec.z)
				{
					return CellRect.Empty;
				}
				return CellRect.FromLimits(intVec.x, intVec.z, intVec2.x, intVec2.z);
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06001DAB RID: 7595 RVA: 0x000971F4 File Offset: 0x000953F4
		private CellRect ViewRect
		{
			get
			{
				return Find.CameraDriver.CurrentViewRect.ExpandedBy(1).ClipInsideMap(this.map);
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06001DAC RID: 7596 RVA: 0x00097222 File Offset: 0x00095422
		public List<MapDrawLayer> GlobalLayers
		{
			get
			{
				return this.global;
			}
		}

		// Token: 0x06001DAD RID: 7597 RVA: 0x0009722A File Offset: 0x0009542A
		public MapDrawer(Map map)
		{
			this.map = map;
		}

		// Token: 0x06001DAE RID: 7598 RVA: 0x0009723C File Offset: 0x0009543C
		public T GetGlobalLayer<T>() where T : MapDrawLayer
		{
			for (int i = 0; i < this.global.Count; i++)
			{
				T t = this.global[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x0009728C File Offset: 0x0009548C
		public void MapMeshDirty(IntVec3 loc, ulong dirtyFlags)
		{
			bool regenAdjacentCells = (dirtyFlags & (MapMeshFlagDefOf.FogOfWar | MapMeshFlagDefOf.Buildings | MapMeshFlagDefOf.Roofs)) > 0UL;
			this.MapMeshDirty(loc, dirtyFlags, regenAdjacentCells, false);
		}

		// Token: 0x06001DB0 RID: 7600 RVA: 0x000972CC File Offset: 0x000954CC
		public void MapMeshDirty(IntVec3 loc, ulong dirtyFlags, bool regenAdjacentCells, bool regenAdjacentSections)
		{
			if (Current.ProgramState != ProgramState.Playing || this.sections == null)
			{
				return;
			}
			this.SectionAt(loc).dirtyFlags |= dirtyFlags;
			this.globalDirtyFlags |= dirtyFlags;
			if (regenAdjacentCells)
			{
				for (int i = 0; i < 8; i++)
				{
					IntVec3 intVec = loc + GenAdj.AdjacentCells[i];
					if (intVec.InBounds(this.map))
					{
						this.SectionAt(intVec).dirtyFlags |= dirtyFlags;
					}
				}
			}
			if (regenAdjacentSections)
			{
				IntVec2 a = this.SectionCoordsAt(loc);
				for (int j = 0; j < 8; j++)
				{
					IntVec3 intVec2 = GenAdj.AdjacentCells[j];
					IntVec2 intVec3 = a + new IntVec2(intVec2.x, intVec2.z);
					IntVec2 sectionCount = this.SectionCount;
					if (intVec3.x >= 0 && intVec3.z >= 0 && intVec3.x <= sectionCount.x - 1 && intVec3.z <= sectionCount.z - 1)
					{
						this.sections[intVec3.x, intVec3.z].dirtyFlags |= dirtyFlags;
					}
				}
			}
		}

		// Token: 0x06001DB1 RID: 7601 RVA: 0x00097400 File Offset: 0x00095600
		public void MapMeshDrawerUpdate_First()
		{
			if (this.globalDirtyFlags != 0UL)
			{
				for (int i = 0; i < this.global.Count; i++)
				{
					MapDrawLayer mapDrawLayer = this.global[i];
					mapDrawLayer.Dirty = (mapDrawLayer.Dirty || (this.globalDirtyFlags & mapDrawLayer.relevantChangeTypes) > 0UL);
					if (mapDrawLayer.Dirty)
					{
						mapDrawLayer.Regenerate();
					}
				}
			}
			CellRect viewRect = this.ViewRect;
			bool flag = false;
			Section[,] array = this.sections;
			int upperBound = array.GetUpperBound(0);
			int upperBound2 = array.GetUpperBound(1);
			for (int j = array.GetLowerBound(0); j <= upperBound; j++)
			{
				for (int k = array.GetLowerBound(1); k <= upperBound2; k++)
				{
					if (array[j, k].TryUpdate(viewRect))
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				return;
			}
			for (int l = 0; l < this.SectionCount.x; l++)
			{
				for (int m = 0; m < this.SectionCount.z; m++)
				{
					if (this.sections[l, m].TryUpdate(viewRect))
					{
						return;
					}
				}
			}
		}

		// Token: 0x06001DB2 RID: 7602 RVA: 0x00097524 File Offset: 0x00095724
		public void DrawMapMesh()
		{
			CellRect view = this.ViewRect;
			if (WorldComponent_GravshipController.GravshipRenderInProgess)
			{
				view = view.Encapsulate(WorldComponent_GravshipController.GravshipRenderBounds);
			}
			for (int i = 0; i < this.global.Count; i++)
			{
				MapDrawLayer mapDrawLayer = this.global[i];
				if (mapDrawLayer.Visible)
				{
					if (mapDrawLayer.Dirty)
					{
						mapDrawLayer.Regenerate();
						mapDrawLayer.RefreshSubMeshBounds();
					}
					mapDrawLayer.DrawLayer();
				}
			}
			for (int j = 0; j < this.SectionCount.x; j++)
			{
				for (int k = 0; k < this.SectionCount.z; k++)
				{
					Section section = this.sections[j, k];
					if (view.Overlaps(section.Bounds))
					{
						section.DrawSection();
					}
					else
					{
						section.DrawDynamicSections(view);
					}
				}
			}
		}

		// Token: 0x06001DB3 RID: 7603 RVA: 0x000975F2 File Offset: 0x000957F2
		private IntVec2 SectionCoordsAt(IntVec3 loc)
		{
			return new IntVec2(Mathf.FloorToInt((float)(loc.x / 17)), Mathf.FloorToInt((float)(loc.z / 17)));
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x00097618 File Offset: 0x00095818
		public Section SectionAt(IntVec3 loc)
		{
			IntVec2 intVec = this.SectionCoordsAt(loc);
			return this.sections[intVec.x, intVec.z];
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x00097644 File Offset: 0x00095844
		public void RegenerateLayerNow(Type type)
		{
			this.EnsureGlobalLayersInitialized();
			if (this.sections == null)
			{
				this.sections = new Section[this.SectionCount.x, this.SectionCount.z];
			}
			for (int i = 0; i < this.global.Count; i++)
			{
				MapDrawLayer mapDrawLayer = this.global[i];
				if (mapDrawLayer.GetType() == type && mapDrawLayer.Visible)
				{
					this.global[i].Regenerate();
					mapDrawLayer.RefreshSubMeshBounds();
				}
			}
			for (int j = 0; j < this.SectionCount.x; j++)
			{
				for (int k = 0; k < this.SectionCount.z; k++)
				{
					Section[,] array = this.sections;
					int num = j;
					int num2 = k;
					if (array[num, num2] == null)
					{
						array[num, num2] = new Section(new IntVec3(j, 0, k), this.map);
					}
					this.sections[j, k].RegenerateSingleLayer(this.sections[j, k].GetLayer(type));
				}
			}
		}

		// Token: 0x06001DB6 RID: 7606 RVA: 0x00097760 File Offset: 0x00095960
		public void RegenerateEverythingNow()
		{
			this.EnsureGlobalLayersInitialized();
			if (this.sections == null)
			{
				this.sections = new Section[this.SectionCount.x, this.SectionCount.z];
			}
			for (int i = 0; i < this.global.Count; i++)
			{
				MapDrawLayer mapDrawLayer = this.global[i];
				if (mapDrawLayer.Visible)
				{
					mapDrawLayer.Regenerate();
					mapDrawLayer.RefreshSubMeshBounds();
				}
			}
			for (int j = 0; j < this.SectionCount.x; j++)
			{
				for (int k = 0; k < this.SectionCount.z; k++)
				{
					Section[,] array = this.sections;
					int num = j;
					int num2 = k;
					if (array[num, num2] == null)
					{
						array[num, num2] = new Section(new IntVec3(j, 0, k), this.map);
					}
					this.sections[j, k].RegenerateAllLayers();
				}
			}
		}

		// Token: 0x06001DB7 RID: 7607 RVA: 0x0009784C File Offset: 0x00095A4C
		private void EnsureGlobalLayersInitialized()
		{
			if (this.global != null)
			{
				return;
			}
			this.global = new List<MapDrawLayer>();
			foreach (Type type in typeof(MapDrawLayer).AllSubclassesNonAbstract())
			{
				if (!typeof(SectionLayer).IsAssignableFrom(type))
				{
					this.global.Add((MapDrawLayer)Activator.CreateInstance(type, new object[]
					{
						this.map
					}));
				}
			}
		}

		// Token: 0x06001DB8 RID: 7608 RVA: 0x000978EC File Offset: 0x00095AEC
		public void WholeMapChanged(ulong change)
		{
			this.globalDirtyFlags |= change;
			for (int i = 0; i < this.SectionCount.x; i++)
			{
				for (int j = 0; j < this.SectionCount.z; j++)
				{
					this.sections[i, j].dirtyFlags |= change;
				}
			}
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x00097950 File Offset: 0x00095B50
		public void Dispose()
		{
			for (int i = 0; i < this.SectionCount.x; i++)
			{
				for (int j = 0; j < this.SectionCount.z; j++)
				{
					this.sections[i, j].Dispose();
				}
			}
			this.sections = null;
			foreach (MapDrawLayer mapDrawLayer in this.global)
			{
				mapDrawLayer.Dispose();
			}
		}

		// Token: 0x040019D0 RID: 6608
		private readonly Map map;

		// Token: 0x040019D1 RID: 6609
		private Section[,] sections;

		// Token: 0x040019D2 RID: 6610
		private List<MapDrawLayer> global;

		// Token: 0x040019D3 RID: 6611
		private ulong globalDirtyFlags;
	}
}
