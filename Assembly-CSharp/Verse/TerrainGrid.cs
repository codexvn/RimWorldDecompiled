using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000439 RID: 1081
	public sealed class TerrainGrid : IExposable
	{
		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06001BCB RID: 7115 RVA: 0x0008B85C File Offset: 0x00089A5C
		public CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawerInt == null)
				{
					this.drawerInt = new CellBoolDrawer(new Func<int, bool>(this.CellBoolDrawerGetBoolInt), new Func<Color>(this.CellBoolDrawerColorInt), new Func<int, Color>(this.CellBoolDrawerGetExtraColorInt), this.map.Size.x, this.map.Size.z, 3600, 0.33f);
				}
				return this.drawerInt;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06001BCC RID: 7116 RVA: 0x0008B8D0 File Offset: 0x00089AD0
		public bool AnyWaterCells
		{
			get
			{
				return this.waterCells.Count > 0;
			}
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x0008B8E0 File Offset: 0x00089AE0
		public TerrainGrid(Map map)
		{
			this.map = map;
			this.ResetGrids();
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x0008B900 File Offset: 0x00089B00
		public void ResetGrids()
		{
			this.topGrid = new TerrainDef[this.map.cellIndices.NumGridCells];
			this.underGrid = new TerrainDef[this.map.cellIndices.NumGridCells];
			this.foundationGrid = new TerrainDef[this.map.cellIndices.NumGridCells];
			this.tempGrid = new TerrainDef[this.map.cellIndices.NumGridCells];
			this.colorGrid = new ColorDef[this.map.cellIndices.NumGridCells];
			this.waterCells = new HashSet<IntVec3>();
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x0008B9A0 File Offset: 0x00089BA0
		public TerrainDef TerrainAt(int ind)
		{
			if (this.tempGrid[ind] != null)
			{
				return this.tempGrid[ind];
			}
			if (this.underGrid[ind] == null && this.foundationGrid[ind] != null)
			{
				return this.foundationGrid[ind];
			}
			if (this.topGrid[ind] == null)
			{
				return TerrainDefOf.Soil;
			}
			return this.topGrid[ind];
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x0008B9F5 File Offset: 0x00089BF5
		public TerrainDef TerrainAtIgnoreTemp(int ind)
		{
			if (this.underGrid[ind] == null && this.foundationGrid[ind] != null)
			{
				return this.foundationGrid[ind];
			}
			if (this.topGrid[ind] == null)
			{
				return TerrainDefOf.Soil;
			}
			return this.topGrid[ind];
		}

		// Token: 0x06001BD1 RID: 7121 RVA: 0x0008BA2C File Offset: 0x00089C2C
		public TerrainDef TerrainAt(IntVec3 c)
		{
			return this.TerrainAt(this.map.cellIndices.CellToIndex(c));
		}

		// Token: 0x06001BD2 RID: 7122 RVA: 0x0008BA48 File Offset: 0x00089C48
		public TerrainDef BaseTerrainAt(IntVec3 c)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			if (this.underGrid[num] != null)
			{
				return this.underGrid[num];
			}
			if (this.topGrid[num] == null)
			{
				return TerrainDefOf.Soil;
			}
			return this.topGrid[num];
		}

		// Token: 0x06001BD3 RID: 7123 RVA: 0x0008BA92 File Offset: 0x00089C92
		public TerrainDef TopTerrainAt(int ind)
		{
			if (this.topGrid[ind] == null)
			{
				return TerrainDefOf.Soil;
			}
			return this.topGrid[ind];
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x0008BAAC File Offset: 0x00089CAC
		public TerrainDef TopTerrainAt(IntVec3 c)
		{
			if (this.topGrid[this.map.cellIndices.CellToIndex(c)] == null)
			{
				return TerrainDefOf.Soil;
			}
			return this.topGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06001BD5 RID: 7125 RVA: 0x0008BAE6 File Offset: 0x00089CE6
		public TerrainDef UnderTerrainAt(int ind)
		{
			return this.underGrid[ind];
		}

		// Token: 0x06001BD6 RID: 7126 RVA: 0x0008BAF0 File Offset: 0x00089CF0
		public TerrainDef UnderTerrainAt(IntVec3 c)
		{
			return this.underGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x0008BB0A File Offset: 0x00089D0A
		public TerrainDef FoundationAt(int ind)
		{
			return this.foundationGrid[ind];
		}

		// Token: 0x06001BD8 RID: 7128 RVA: 0x0008BB14 File Offset: 0x00089D14
		public TerrainDef FoundationAt(IntVec3 c)
		{
			return this.foundationGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x0008BB2E File Offset: 0x00089D2E
		public TerrainDef TempTerrainAt(int ind)
		{
			return this.tempGrid[ind];
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x0008BB38 File Offset: 0x00089D38
		public TerrainDef TempTerrainAt(IntVec3 c)
		{
			return this.tempGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06001BDB RID: 7131 RVA: 0x0008BB52 File Offset: 0x00089D52
		public ColorDef ColorAt(IntVec3 c)
		{
			return this.ColorAt(this.map.cellIndices.CellToIndex(c));
		}

		// Token: 0x06001BDC RID: 7132 RVA: 0x0008BB6B File Offset: 0x00089D6B
		public bool WaterAt(IntVec3 c)
		{
			return this.waterCells.Contains(c);
		}

		// Token: 0x06001BDD RID: 7133 RVA: 0x0008BB79 File Offset: 0x00089D79
		public ColorDef ColorAt(int ind)
		{
			if (this.tempGrid[ind] != null)
			{
				return null;
			}
			return this.colorGrid[ind];
		}

		// Token: 0x06001BDE RID: 7134 RVA: 0x0008BB90 File Offset: 0x00089D90
		public Material GetMaterial(TerrainDef def, bool polluted, ColorDef color)
		{
			ValueTuple<TerrainDef, bool, ColorDef> key = new ValueTuple<TerrainDef, bool, ColorDef>(def, polluted, color);
			if (!this.terrainMatCache.ContainsKey(key))
			{
				Graphic graphic = polluted ? def.graphicPolluted : def.graphic;
				if (color != null)
				{
					this.terrainMatCache[key] = graphic.GetColoredVersion(def.graphic.Shader, color.color, Color.white).MatSingle;
				}
				else
				{
					this.terrainMatCache[key] = (polluted ? def.DrawMatPolluted : def.DrawMatSingle);
				}
			}
			return this.terrainMatCache[key];
		}

		// Token: 0x06001BDF RID: 7135 RVA: 0x0008BC24 File Offset: 0x00089E24
		public void SetTerrain(IntVec3 c, TerrainDef newTerr)
		{
			if (newTerr == null)
			{
				Log.Error(string.Format("Tried to set terrain at {0} to null.", c));
				return;
			}
			if (!c.InBounds(this.map))
			{
				Log.Error(string.Format("Tried to set terrain in out of bounds cell {0} (bounds: {1})", c, this.map.Size));
				return;
			}
			if (newTerr.temporary)
			{
				this.SetTempTerrain(c, newTerr);
				return;
			}
			if (newTerr.isFoundation)
			{
				this.SetFoundation(c, newTerr);
				return;
			}
			int num = this.map.cellIndices.CellToIndex(c);
			if (newTerr.layerable)
			{
				if (this.underGrid[num] == null)
				{
					if (this.topGrid[num] != null && this.topGrid[num].passability != Traversability.Impassable)
					{
						this.underGrid[num] = this.topGrid[num];
					}
					else
					{
						this.underGrid[num] = (this.map.generatorDef.defaultUnderGridTerrain ?? TerrainDefOf.Sand);
					}
				}
			}
			else
			{
				this.underGrid[num] = null;
			}
			TerrainDef oldTerr = null;
			if (this.topGrid[num] != null)
			{
				if (this.topGrid[num].glowRadius > 0f)
				{
					this.map.glowGrid.DeregisterTerrain(c);
				}
				if (this.topGrid[num].IsWater)
				{
					this.waterCells.Remove(c);
				}
				oldTerr = this.topGrid[num];
			}
			this.topGrid[num] = newTerr;
			this.colorGrid[num] = null;
			if (newTerr.glowRadius > 0f)
			{
				this.map.glowGrid.RegisterTerrain(c);
			}
			if (newTerr.IsWater)
			{
				this.waterCells.Add(c);
			}
			this.DoTerrainChangedEffects(c, oldTerr, newTerr, null);
		}

		// Token: 0x06001BE0 RID: 7136 RVA: 0x0008BDC4 File Offset: 0x00089FC4
		public void SetUnderTerrain(IntVec3 c, TerrainDef newTerr)
		{
			if (!c.InBounds(this.map))
			{
				Log.Error(string.Format("Tried to set terrain out of bounds at {0}", c));
				return;
			}
			int num = this.map.cellIndices.CellToIndex(c);
			if (this.underGrid[num] != null && this.underGrid[num].glowRadius > 0f)
			{
				this.map.glowGrid.DeregisterTerrain(c);
			}
			this.underGrid[num] = newTerr;
			if (newTerr.glowRadius > 0f)
			{
				this.map.glowGrid.RegisterTerrain(c);
			}
			this.map.events.Notify_TerrainChanged(c);
		}

		// Token: 0x06001BE1 RID: 7137 RVA: 0x0008BE70 File Offset: 0x0008A070
		public void RemoveTopLayer(IntVec3 c, bool doLeavings = true)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			TerrainDef oldTerr = this.TerrainAt(c);
			if (this.tempGrid[num] != null)
			{
				this.RemoveTempTerrain(c, doLeavings, false);
				return;
			}
			if (this.foundationGrid[num] != null && this.underGrid[num] == null)
			{
				this.RemoveFoundation(c, doLeavings);
				return;
			}
			if (doLeavings)
			{
				GenLeaving.DoLeavingsFor(this.topGrid[num], c, this.map);
			}
			if (this.underGrid[num] == null)
			{
				return;
			}
			if (this.topGrid[num].glowRadius > 0f)
			{
				this.map.glowGrid.DeregisterTerrain(c);
			}
			this.topGrid[num] = this.underGrid[num];
			this.underGrid[num] = null;
			this.colorGrid[num] = null;
			if (this.foundationGrid[num] != null && this.foundationGrid[num].glowRadius > 0f)
			{
				this.map.glowGrid.RegisterTerrain(c);
			}
			else if (this.topGrid[num].glowRadius > 0f)
			{
				this.map.glowGrid.RegisterTerrain(c);
			}
			this.DoTerrainChangedEffects(c, oldTerr, this.TerrainAt(c), null);
		}

		// Token: 0x06001BE2 RID: 7138 RVA: 0x0008BF98 File Offset: 0x0008A198
		public void SetFoundation(IntVec3 c, TerrainDef newTerr)
		{
			if (newTerr == null)
			{
				Log.Error(string.Format("Tried to set terrain at {0} to null.", c));
				return;
			}
			if (!newTerr.isFoundation)
			{
				Log.Error(string.Format("Tried to set foundation terrain at {0} to a type which is not a foundation ({1}).", c, newTerr.defName));
				return;
			}
			int num = this.map.cellIndices.CellToIndex(c);
			TerrainDef oldTerr = this.TerrainAt(num);
			if (this.TempTerrainAt(num) != null)
			{
				this.RemoveTempTerrain(c, false, true);
			}
			if (this.underGrid[num] != null)
			{
				Log.Error(string.Format("Tried to set foundation at {0} to {1} but there is an under terrain there.", c, newTerr));
				return;
			}
			if (this.topGrid[num] != null && this.topGrid[num].glowRadius > 0f)
			{
				this.map.glowGrid.DeregisterTerrain(c);
			}
			this.foundationGrid[num] = newTerr;
			if (newTerr.glowRadius > 0f)
			{
				this.map.glowGrid.RegisterTerrain(c);
			}
			this.DoTerrainChangedEffects(c, oldTerr, newTerr, null);
		}

		// Token: 0x06001BE3 RID: 7139 RVA: 0x0008C090 File Offset: 0x0008A290
		public void RemoveFoundation(IntVec3 c, bool doLeavings = true)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			TerrainDef oldTerr = this.TerrainAt(num);
			TerrainDef oldFoundation = this.FoundationAt(num);
			if (doLeavings)
			{
				GenLeaving.DoLeavingsFor(this.foundationGrid[num], c, this.map);
			}
			if (this.foundationGrid[num].glowRadius > 0f)
			{
				this.map.glowGrid.DeregisterTerrain(c);
			}
			this.foundationGrid[num] = null;
			this.colorGrid[num] = null;
			if (this.topGrid[num].glowRadius > 0f)
			{
				this.map.glowGrid.RegisterTerrain(c);
			}
			this.DoTerrainChangedEffects(c, oldTerr, this.TerrainAt(num), oldFoundation);
		}

		// Token: 0x06001BE4 RID: 7140 RVA: 0x0008C144 File Offset: 0x0008A344
		public void RemoveGravshipTerrainUnsafe(IntVec3 cell, int index)
		{
			TerrainDef terrainDef = this.TerrainAt(cell);
			TerrainDef terrainDef2 = this.underGrid[index] ?? this.topGrid[index];
			if (terrainDef2 == null)
			{
				return;
			}
			if (terrainDef.glowRadius > 0f)
			{
				this.map.glowGrid.DeregisterTerrain(cell);
			}
			this.tempGrid[index] = null;
			this.foundationGrid[index] = null;
			this.colorGrid[index] = null;
			this.underGrid[index] = null;
			this.topGrid[index] = terrainDef2;
			if (terrainDef2.glowRadius > 0f)
			{
				this.map.glowGrid.RegisterTerrain(cell);
			}
		}

		// Token: 0x06001BE5 RID: 7141 RVA: 0x0008C1DC File Offset: 0x0008A3DC
		public void SetTempTerrain(IntVec3 c, TerrainDef newTerr)
		{
			if (newTerr == null)
			{
				Log.Error(string.Format("Tried to set terrain at {0} to null.", c));
				return;
			}
			if (!newTerr.temporary)
			{
				Log.Error(string.Format("Tried to set temp terrain at {0} to a type which is not temporary ({1}).", c, newTerr.defName));
				return;
			}
			TerrainDef oldTerr = this.TerrainAt(c);
			if (Current.ProgramState == ProgramState.Playing)
			{
				Designation designation = this.map.designationManager.DesignationAt(c, DesignationDefOf.SmoothFloor);
				if (designation != null)
				{
					designation.Delete();
				}
			}
			int num = this.map.cellIndices.CellToIndex(c);
			if (this.topGrid[num] != null && this.topGrid[num].glowRadius > 0f)
			{
				this.map.glowGrid.DeregisterTerrain(c);
			}
			TempTerrainProps tempTerrain = newTerr.tempTerrain;
			if (tempTerrain != null && tempTerrain.destroysFloors && this.underGrid[num] != null && !this.topGrid[num].natural)
			{
				this.topGrid[num] = this.underGrid[num];
				this.underGrid[num] = null;
				this.colorGrid[num] = null;
			}
			this.tempGrid[num] = newTerr;
			if (newTerr.glowRadius > 0f)
			{
				this.map.glowGrid.RegisterTerrain(c);
			}
			this.DoTerrainChangedEffects(c, oldTerr, newTerr, null);
		}

		// Token: 0x06001BE6 RID: 7142 RVA: 0x0008C318 File Offset: 0x0008A518
		public void RemoveTempTerrain(IntVec3 c, bool doLeavings = false, bool preventDestroyEffects = false)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			if (doLeavings)
			{
				GenLeaving.DoLeavingsFor(this.tempGrid[num], c, this.map);
			}
			if (this.tempGrid[num] == null)
			{
				return;
			}
			TerrainDef terrainDef = this.tempGrid[num];
			if (terrainDef.glowRadius > 0f)
			{
				this.map.glowGrid.DeregisterTerrain(c);
			}
			this.tempGrid[num] = null;
			if (!preventDestroyEffects)
			{
				TempTerrainProps tempTerrain = terrainDef.tempTerrain;
				if (((tempTerrain != null) ? tempTerrain.terrainOnRemoved : null) != null)
				{
					if (terrainDef.tempTerrain.terrainOnRemoved.temporary)
					{
						this.SetTempTerrain(c, terrainDef.tempTerrain.terrainOnRemoved);
						goto IL_DA;
					}
					this.SetTerrain(c, terrainDef.tempTerrain.terrainOnRemoved);
					goto IL_DA;
				}
			}
			if (this.topGrid[num].glowRadius > 0f)
			{
				this.map.glowGrid.RegisterTerrain(c);
			}
			IL_DA:
			this.DoTerrainChangedEffects(c, terrainDef, this.TerrainAt(c), null);
		}

		// Token: 0x06001BE7 RID: 7143 RVA: 0x0008C410 File Offset: 0x0008A610
		public void SetTerrainColor(IntVec3 c, ColorDef color)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			this.colorGrid[num] = color;
			this.DoTerrainChangedEffects(c, this.TerrainAt(c), this.TerrainAt(c), null);
		}

		// Token: 0x06001BE8 RID: 7144 RVA: 0x0008C450 File Offset: 0x0008A650
		public bool CanRemoveTopLayerAt(IntVec3 c)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			TerrainDef terrainDef = this.tempGrid[num];
			return (terrainDef != null && terrainDef.Removable) || (this.topGrid[num].Removable && this.underGrid[num] != null);
		}

		// Token: 0x06001BE9 RID: 7145 RVA: 0x0008C4A4 File Offset: 0x0008A6A4
		public bool CanRemoveFoundationAt(IntVec3 c)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			return this.foundationGrid[num] != null && this.foundationGrid[num].Removable;
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x0008C4DC File Offset: 0x0008A6DC
		private void DoTerrainChangedEffects(IntVec3 c, TerrainDef oldTerr, TerrainDef newTerr, TerrainDef oldFoundation = null)
		{
			this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Terrain, true, false);
			List<Thing> thingList = c.GetThingList(this.map);
			int i = thingList.Count - 1;
			while (i >= 0)
			{
				if (thingList[i].def.category != ThingCategory.Plant)
				{
					goto IL_150;
				}
				if (this.map.fertilityGrid.FertilityAt(c) < thingList[i].def.plant.fertilityMin)
				{
					if (newTerr.IsFlood && !thingList[i].def.plant.destroyedByFlooding)
					{
						goto IL_20C;
					}
					if (!thingList[i].def.plant.completelyIgnoreFertility)
					{
						thingList[i].Destroy(DestroyMode.Vanish);
						goto IL_20C;
					}
				}
				if (thingList[i].def.plant.WildTerrainTags.Count > 0 && !thingList[i].def.plant.WildTerrainTags.Overlaps(newTerr.tags.OrElseEmptyEnumerable<string>()))
				{
					thingList[i].Destroy(DestroyMode.Vanish);
				}
				else
				{
					if (!thingList[i].def.plant.showInFrozenWater)
					{
						this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Things);
						goto IL_150;
					}
					goto IL_150;
				}
				IL_20C:
				i--;
				continue;
				IL_150:
				if (thingList[i].def.category == ThingCategory.Filth && !FilthMaker.TerrainAcceptsFilth(newTerr, thingList[i].def, FilthSourceFlags.None))
				{
					thingList[i].Destroy(DestroyMode.Vanish);
					goto IL_20C;
				}
				if ((thingList[i].def.IsBlueprint || thingList[i].def.IsFrame) && !GenConstruct.CanBuildOnTerrain(thingList[i].def.entityDefToBuild, thingList[i].Position, this.map, thingList[i].Rotation, thingList[i], ((IConstructible)thingList[i]).EntityToBuildStuff()))
				{
					thingList[i].Destroy(DestroyMode.Cancel);
					goto IL_20C;
				}
				goto IL_20C;
			}
			if (!newTerr.holdSnowOrSand)
			{
				this.map.snowGrid.SetDepth(c, 0f);
				SandGrid sandGrid = this.map.sandGrid;
				if (sandGrid != null)
				{
					sandGrid.SetDepth(c, 0f);
				}
			}
			Designation designation = this.map.designationManager.DesignationAt(c, DesignationDefOf.SmoothFloor);
			if (designation != null)
			{
				designation.Delete();
			}
			Designation designation2 = this.map.designationManager.DesignationAt(c, DesignationDefOf.RemoveFloor);
			if (designation2 != null)
			{
				designation2.Delete();
			}
			Designation designation3 = this.map.designationManager.DesignationAt(c, DesignationDefOf.PaintFloor);
			if (designation3 != null)
			{
				designation3.Delete();
			}
			Designation designation4 = this.map.designationManager.DesignationAt(c, DesignationDefOf.RemovePaintFloor);
			if (designation4 != null)
			{
				designation4.Delete();
			}
			this.map.pathing.RecalculatePerceivedPathCostAt(c);
			CellBoolDrawer cellBoolDrawer = this.drawerInt;
			if (cellBoolDrawer != null)
			{
				cellBoolDrawer.SetDirty();
			}
			this.map.fertilityGrid.Drawer.SetDirty();
			if (ModsConfig.OdysseyActive && ((oldFoundation != null && oldFoundation.IsSubstructure) || newTerr.IsSubstructure))
			{
				SubstructureGrid substructureGrid = this.map.substructureGrid;
				if (substructureGrid != null)
				{
					substructureGrid.MarkDirty();
				}
			}
			Region regionAt_NoRebuild_InvalidAllowed = this.map.regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(c);
			if (regionAt_NoRebuild_InvalidAllowed != null)
			{
				District district = regionAt_NoRebuild_InvalidAllowed.District;
				if (district != null)
				{
					district.Notify_TerrainChanged();
				}
			}
			if (ModsConfig.OdysseyActive)
			{
				using (new ProfilerBlock("FishPopulationTracker.Notify_TerrainChanged"))
				{
					WaterBodyTracker waterBodyTracker = this.map.waterBodyTracker;
					if (waterBodyTracker != null)
					{
						waterBodyTracker.Notify_TerrainChanged(c, oldTerr, newTerr);
					}
				}
			}
			this.map.events.Notify_TerrainChanged(c);
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x0008C8AC File Offset: 0x0008AAAC
		public void ExposeData()
		{
			this.ExposeTerrainGrid(this.topGrid, "topGrid", TerrainDefOf.Soil);
			this.ExposeTerrainGrid(this.underGrid, "underGrid", null);
			this.ExposeTerrainGrid(this.foundationGrid, "foundationGrid", null);
			this.ExposeTerrainGrid(this.tempGrid, "tempGrid", null);
			this.ExposeColorGrid();
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				foreach (IntVec3 intVec in this.map.AllCells)
				{
					int num = this.map.cellIndices.CellToIndex(intVec);
					if (this.TerrainAt(num).glowRadius > 0f)
					{
						this.map.glowGrid.RegisterTerrain(intVec);
					}
					if (this.topGrid[num].IsWater)
					{
						this.waterCells.Add(intVec);
					}
					if (this.topGrid[num].isFoundation)
					{
						this.foundationGrid[num] = this.topGrid[num];
						this.topGrid[num] = this.underGrid[num];
						this.underGrid[num] = null;
					}
				}
			}
		}

		// Token: 0x06001BEC RID: 7148 RVA: 0x0008C9E4 File Offset: 0x0008ABE4
		public void Notify_TerrainBurned(IntVec3 c)
		{
			TerrainDef terrain = c.GetTerrain(this.map);
			TerrainDef terrainDef = this.FoundationAt(c);
			if (terrainDef != null && terrainDef.Flammable())
			{
				this.Notify_FoundationDestroyed(c);
				return;
			}
			this.Notify_TerrainDestroyed(c);
			if (terrain.burnedDef != null)
			{
				this.SetTerrain(c, terrain.burnedDef);
			}
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x0008CA38 File Offset: 0x0008AC38
		public void Notify_TerrainDestroyed(IntVec3 c)
		{
			if (!this.CanRemoveTopLayerAt(c))
			{
				return;
			}
			TerrainDef terrainDef = this.TerrainAt(c);
			this.RemoveTopLayer(c, false);
			if (terrainDef.destroyBuildingsOnDestroyed)
			{
				Building firstBuilding = c.GetFirstBuilding(this.map);
				if (firstBuilding != null)
				{
					firstBuilding.Kill(null, null);
				}
			}
			if (terrainDef.destroyEffectWater != null && this.TerrainAt(c) != null && this.TerrainAt(c).IsWater)
			{
				Effecter effecter = terrainDef.destroyEffectWater.Spawn();
				effecter.Trigger(new TargetInfo(c, this.map, false), new TargetInfo(c, this.map, false), -1);
				effecter.Cleanup();
			}
			else if (terrainDef.destroyEffect != null)
			{
				Effecter effecter2 = terrainDef.destroyEffect.Spawn();
				effecter2.Trigger(new TargetInfo(c, this.map, false), new TargetInfo(c, this.map, false), -1);
				effecter2.Cleanup();
			}
			ThingUtility.CheckAutoRebuildTerrainOnDestroyed(terrainDef, c, this.map);
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x0008CB24 File Offset: 0x0008AD24
		public void Notify_FoundationDestroyed(IntVec3 c)
		{
			TerrainDef terrainDef = this.FoundationAt(c);
			if (terrainDef == null)
			{
				return;
			}
			if (this.CanRemoveTopLayerAt(c))
			{
				this.Notify_TerrainDestroyed(c);
			}
			this.RemoveFoundation(c, false);
			if (terrainDef.destroyBuildingsOnDestroyed)
			{
				Building firstBuilding = c.GetFirstBuilding(this.map);
				if (firstBuilding != null)
				{
					firstBuilding.Kill(null, null);
				}
			}
			if (terrainDef.destroyEffectWater != null && this.TerrainAt(c) != null && this.TerrainAt(c).IsWater)
			{
				Effecter effecter = terrainDef.destroyEffectWater.Spawn();
				effecter.Trigger(new TargetInfo(c, this.map, false), new TargetInfo(c, this.map, false), -1);
				effecter.Cleanup();
			}
			else if (terrainDef.destroyEffect != null)
			{
				Effecter effecter2 = terrainDef.destroyEffect.Spawn();
				effecter2.Trigger(new TargetInfo(c, this.map, false), new TargetInfo(c, this.map, false), -1);
				effecter2.Cleanup();
			}
			ThingUtility.CheckAutoRebuildTerrainOnDestroyed(terrainDef, c, this.map);
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x0008CC18 File Offset: 0x0008AE18
		private void ExposeTerrainGrid(TerrainDef[] grid, string label, TerrainDef fallbackTerrain)
		{
			TerrainGrid.<>c__DisplayClass47_0 CS$<>8__locals1 = new TerrainGrid.<>c__DisplayClass47_0();
			CS$<>8__locals1.grid = grid;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.fallbackTerrain = fallbackTerrain;
			CS$<>8__locals1.terrainDefsByShortHash = new Dictionary<ushort, TerrainDef>();
			foreach (TerrainDef terrainDef in DefDatabase<TerrainDef>.AllDefs)
			{
				CS$<>8__locals1.terrainDefsByShortHash.Add(terrainDef.shortHash, terrainDef);
			}
			MapExposeUtility.ExposeUshort(this.map, new Func<IntVec3, ushort>(CS$<>8__locals1.<ExposeTerrainGrid>g__Read|0), new Action<IntVec3, ushort>(CS$<>8__locals1.<ExposeTerrainGrid>g__Write|1), label);
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x0008CCBC File Offset: 0x0008AEBC
		private void ExposeColorGrid()
		{
			TerrainGrid.<>c__DisplayClass48_0 CS$<>8__locals1 = new TerrainGrid.<>c__DisplayClass48_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.colorDefsByShortHash = new Dictionary<ushort, ColorDef>();
			foreach (ColorDef colorDef in DefDatabase<ColorDef>.AllDefs)
			{
				CS$<>8__locals1.colorDefsByShortHash.Add(colorDef.shortHash, colorDef);
			}
			MapExposeUtility.ExposeUshort(this.map, new Func<IntVec3, ushort>(CS$<>8__locals1.<ExposeColorGrid>g__Read|0), new Action<IntVec3, ushort>(CS$<>8__locals1.<ExposeColorGrid>g__Write|1), "colorGrid");
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x0008CD54 File Offset: 0x0008AF54
		public string DebugStringAt(IntVec3 c)
		{
			if (c.InBounds(this.map))
			{
				TerrainDef terrain = c.GetTerrain(this.map);
				TerrainDef terrainDef = this.underGrid[this.map.cellIndices.CellToIndex(c)];
				return "top: " + ((terrain != null) ? terrain.defName : "null") + ", under=" + ((terrainDef != null) ? terrainDef.defName : "null");
			}
			return "out of bounds";
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x0008CDCA File Offset: 0x0008AFCA
		public void TerrainGridUpdate()
		{
			if (Find.PlaySettings.showTerrainAffordanceOverlay && !Find.ScreenshotModeHandler.Active)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x000440FD File Offset: 0x000422FD
		private Color CellBoolDrawerColorInt()
		{
			return Color.white;
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x0008CDFC File Offset: 0x0008AFFC
		private bool CellBoolDrawerGetBoolInt(int index)
		{
			IntVec3 intVec = CellIndicesUtility.IndexToCell(index, this.map.Size.x);
			TerrainAffordanceDef terrainAffordanceDef;
			return !intVec.Filled(this.map) && !intVec.Fogged(this.map) && this.TryGetAffordanceDefToDraw(intVec, out terrainAffordanceDef);
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x0008CE48 File Offset: 0x0008B048
		private bool TryGetAffordanceDefToDraw(IntVec3 cell, out TerrainAffordanceDef affordance)
		{
			if (cell.GetAffordances(this.map).NullOrEmpty<TerrainAffordanceDef>())
			{
				affordance = null;
				return true;
			}
			TerrainAffordanceDef terrainAffordanceDef = null;
			int num = int.MinValue;
			foreach (TerrainAffordanceDef terrainAffordanceDef2 in cell.GetAffordances(this.map))
			{
				if (terrainAffordanceDef2.visualizeOnAffordanceOverlay)
				{
					if (num < terrainAffordanceDef2.order)
					{
						num = terrainAffordanceDef2.order;
						terrainAffordanceDef = terrainAffordanceDef2;
					}
				}
				else if (terrainAffordanceDef2.blockAffordanceOverlay)
				{
					affordance = null;
					return false;
				}
			}
			affordance = terrainAffordanceDef;
			return true;
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x0008CEF0 File Offset: 0x0008B0F0
		private Color CellBoolDrawerGetExtraColorInt(int index)
		{
			IntVec3 cell = CellIndicesUtility.IndexToCell(index, this.map.Size.x);
			TerrainAffordanceDef terrainAffordanceDef;
			this.TryGetAffordanceDefToDraw(cell, out terrainAffordanceDef);
			if (terrainAffordanceDef == null)
			{
				return TerrainGrid.NoAffordanceColor;
			}
			return terrainAffordanceDef.affordanceOverlayColor;
		}

		// Token: 0x04001807 RID: 6151
		private Map map;

		// Token: 0x04001808 RID: 6152
		private TerrainDef[] tempGrid;

		// Token: 0x04001809 RID: 6153
		public TerrainDef[] topGrid;

		// Token: 0x0400180A RID: 6154
		public TerrainDef[] foundationGrid;

		// Token: 0x0400180B RID: 6155
		private TerrainDef[] underGrid;

		// Token: 0x0400180C RID: 6156
		public ColorDef[] colorGrid;

		// Token: 0x0400180D RID: 6157
		private CellBoolDrawer drawerInt;

		// Token: 0x0400180E RID: 6158
		private HashSet<IntVec3> waterCells;

		// Token: 0x0400180F RID: 6159
		private Dictionary<ValueTuple<TerrainDef, bool, ColorDef>, Material> terrainMatCache = new Dictionary<ValueTuple<TerrainDef, bool, ColorDef>, Material>();

		// Token: 0x04001810 RID: 6160
		private static readonly Color NoAffordanceColor = Color.red;
	}
}
