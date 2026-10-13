using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002195 RID: 8597
	public class SubstructureGrid
	{
		// Token: 0x17001C88 RID: 7304
		// (get) Token: 0x0600BBC5 RID: 48069 RVA: 0x00366234 File Offset: 0x00364434
		public CellBoolDrawer Drawer
		{
			get
			{
				CellBoolDrawer result;
				if ((result = this.drawerInt) == null)
				{
					result = (this.drawerInt = new CellBoolDrawer(new Func<int, bool>(this.CellBoolDrawerGetBoolInt), new Func<Color>(this.CellBoolDrawerColorInt), new Func<int, Color>(this.CellBoolDrawerGetExtraColorInt), this.map.Size.x, this.map.Size.z, 3700, 0.33f));
				}
				return result;
			}
		}

		// Token: 0x0600BBC6 RID: 48070 RVA: 0x003662A7 File Offset: 0x003644A7
		public SubstructureGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600BBC7 RID: 48071 RVA: 0x000440FD File Offset: 0x000422FD
		private Color CellBoolDrawerColorInt()
		{
			return Color.white;
		}

		// Token: 0x0600BBC8 RID: 48072 RVA: 0x003662C8 File Offset: 0x003644C8
		private bool CellBoolDrawerGetBoolInt(int index)
		{
			TerrainDef terrainDef = this.map.terrainGrid.FoundationAt(index);
			return terrainDef != null && terrainDef.IsSubstructure;
		}

		// Token: 0x0600BBC9 RID: 48073 RVA: 0x003662E8 File Offset: 0x003644E8
		private Color CellBoolDrawerGetExtraColorInt(int index)
		{
			if (Find.TickManager.TicksGame != this.lastGotEnginesTick)
			{
				this.tmpEngines.Clear();
				this.tmpEngines.AddRange(this.map.listerBuildings.AllBuildingsColonistOfClass<Building_GravEngine>());
				this.lastGotEnginesTick = Find.TickManager.TicksGame;
			}
			TerrainDef terrainDef = this.map.terrainGrid.FoundationAt(index);
			if (terrainDef != null && terrainDef.IsSubstructure)
			{
				using (List<Building_GravEngine>.Enumerator enumerator = this.tmpEngines.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.ValidSubstructureAt(this.map.cellIndices.IndexToCell(index)))
						{
							return GravshipUtility.ConnectedSubstructureColor;
						}
					}
				}
			}
			return GravshipUtility.DisconnectedSubstructureColor;
		}

		// Token: 0x0600BBCA RID: 48074 RVA: 0x003663C4 File Offset: 0x003645C4
		public void DrawSubstructureGrid()
		{
			if (GravshipUtility.ShowConnectedSubstructure && !Find.ScreenshotModeHandler.Active)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x0600BBCB RID: 48075 RVA: 0x003663F0 File Offset: 0x003645F0
		public static void DrawSubstructureFootprint(List<IntVec3> extraCells = null, Thing thingToExclude = null)
		{
			if (Time.frameCount == SubstructureGrid.lastUpdateFrame)
			{
				return;
			}
			SubstructureGrid.lastUpdateFrame = Time.frameCount;
			foreach (Thing thing in Find.CurrentMap.listerThings.ThingsOfDef(ThingDefOf.GravEngine))
			{
				SubstructureGrid.tmpFootprintCells.Clear();
				SubstructureGrid.tmpFootprintCellsHashSet.Clear();
				SubstructureGrid.tmpGravEngineCells.Clear();
				SubstructureGrid.tmpGravEngineCellsHashSet.Clear();
				Building_GravEngine building_GravEngine = thing as Building_GravEngine;
				if (building_GravEngine != thingToExclude)
				{
					CompSubstructureFootprint compSubstructureFootprint = building_GravEngine.TryGetComp<CompSubstructureFootprint>();
					SubstructureGrid.tmpGravEngineCells.AddRange(GenRadial.RadialCellsAround(building_GravEngine.Position, compSubstructureFootprint.Props.radius, true));
					SubstructureGrid.tmpGravEngineCellsHashSet.AddRange(SubstructureGrid.tmpGravEngineCells);
					GenDraw.DrawFieldEdges(SubstructureGrid.tmpGravEngineCells, GravshipUtility.ConnectedSubstructureColor, null, null, 2900);
				}
				foreach (Thing thing2 in building_GravEngine.AffectedByFacilities.LinkedFacilitiesListForReading)
				{
					CompSubstructureFootprint compSubstructureFootprint2;
					if ((thingToExclude == null || thing2 != thingToExclude) && thing2.TryGetComp(out compSubstructureFootprint2) && compSubstructureFootprint2.Valid)
					{
						foreach (IntVec3 intVec in GenRadial.RadialCellsAround(thing2.Position, compSubstructureFootprint2.Props.radius, true))
						{
							if (intVec.InBounds(Find.CurrentMap) && !intVec.Fogged(Find.CurrentMap) && !SubstructureGrid.tmpGravEngineCellsHashSet.Contains(intVec) && !SubstructureGrid.tmpFootprintCellsHashSet.Contains(intVec))
							{
								SubstructureGrid.tmpFootprintCells.Add(intVec);
								SubstructureGrid.tmpFootprintCellsHashSet.Add(intVec);
							}
						}
					}
				}
				if (extraCells != null)
				{
					foreach (IntVec3 intVec2 in extraCells)
					{
						if (intVec2.InBounds(Find.CurrentMap) && !intVec2.Fogged(Find.CurrentMap) && !SubstructureGrid.tmpGravEngineCellsHashSet.Contains(intVec2) && !SubstructureGrid.tmpFootprintCellsHashSet.Contains(intVec2))
						{
							SubstructureGrid.tmpFootprintCells.Add(intVec2);
							SubstructureGrid.tmpFootprintCellsHashSet.Add(intVec2);
						}
					}
				}
				List<IntVec3> cells = SubstructureGrid.tmpFootprintCells;
				Color connectedSubstructureColor = GravshipUtility.ConnectedSubstructureColor;
				HashSet<IntVec3> ignoreBorderCells = SubstructureGrid.tmpGravEngineCellsHashSet;
				GenDraw.DrawFieldEdges(cells, connectedSubstructureColor, null, ignoreBorderCells, 2900);
			}
		}

		// Token: 0x0600BBCC RID: 48076 RVA: 0x003666DC File Offset: 0x003648DC
		public static void DrawSubstructureCountOnGUI()
		{
			if (Time.frameCount == SubstructureGrid.lastGUIFrame)
			{
				return;
			}
			foreach (Thing thing in Find.CurrentMap.listerThings.ThingsOfDef(ThingDefOf.GravEngine))
			{
				Building_GravEngine building_GravEngine = thing as Building_GravEngine;
				int count = building_GravEngine.AllConnectedSubstructure.Count;
				float statValue = building_GravEngine.GetStatValue(StatDefOf.SubstructureSupport, true, -1);
				Widgets.DrawStringOnMap(building_GravEngine.DrawPos.MapToUIPosition(), string.Format("{0}/{1}", count, statValue), Color.white);
			}
		}

		// Token: 0x0600BBCD RID: 48077 RVA: 0x0036678C File Offset: 0x0036498C
		public static void DrawSubstructureFootprintWithExtra(CompProperties_SubstructureFootprint footprint, IntVec3 center, Thing thing)
		{
			SubstructureGrid.tmpExtraFootprintCells.Clear();
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(center, footprint.radius, true))
			{
				if (intVec.InBounds(Find.CurrentMap) && !intVec.Fogged(Find.CurrentMap))
				{
					SubstructureGrid.tmpExtraFootprintCells.Add(intVec);
				}
			}
			SubstructureGrid.DrawSubstructureFootprint(SubstructureGrid.tmpExtraFootprintCells, thing);
		}

		// Token: 0x0600BBCE RID: 48078 RVA: 0x00366814 File Offset: 0x00364A14
		public void MarkDirty()
		{
			foreach (Building_GravEngine building_GravEngine in this.map.listerBuildings.AllBuildingsColonistOfClass<Building_GravEngine>())
			{
				building_GravEngine.substructureDirty = true;
			}
			this.Drawer.SetDirty();
		}

		// Token: 0x04008149 RID: 33097
		private Map map;

		// Token: 0x0400814A RID: 33098
		private CellBoolDrawer drawerInt;

		// Token: 0x0400814B RID: 33099
		private List<Building_GravEngine> tmpEngines = new List<Building_GravEngine>();

		// Token: 0x0400814C RID: 33100
		private int lastGotEnginesTick = -1;

		// Token: 0x0400814D RID: 33101
		private static readonly List<IntVec3> tmpFootprintCells = new List<IntVec3>();

		// Token: 0x0400814E RID: 33102
		private static readonly HashSet<IntVec3> tmpFootprintCellsHashSet = new HashSet<IntVec3>();

		// Token: 0x0400814F RID: 33103
		private static readonly List<IntVec3> tmpGravEngineCells = new List<IntVec3>();

		// Token: 0x04008150 RID: 33104
		private static readonly HashSet<IntVec3> tmpGravEngineCellsHashSet = new HashSet<IntVec3>();

		// Token: 0x04008151 RID: 33105
		private static int lastUpdateFrame = -1;

		// Token: 0x04008152 RID: 33106
		private static int lastGUIFrame = -1;

		// Token: 0x04008153 RID: 33107
		private static readonly List<IntVec3> tmpExtraFootprintCells = new List<IntVec3>();
	}
}
